using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System;
using VRage.Collections;
using VRage.Game.Components;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame.Utilities;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Game;
using VRage;
using VRageMath;

namespace IngameScript {
	partial class Program : MyGridProgram {
		//// Config ////
		// Blocks must contain their respective strings.
		private const string PistonName = "OutriggerPiston";
		private const string LockName = "OutriggerLock";
		private const string FootName = "OutriggerFoot";

		private const string StatusLCDsName = "OutriggerStatusLCD"; //Debug LCD
		private const string ConsoleLCDsName = "OutriggerConsoleLCD"; //Control LCD

		private const bool ExtendMatch = true;    //Outrigger Extend Direction Matches Piston Extend Direction
		private const float MoveDectect = 0.75f;     //Ship-Speed compared to X*Percent of piston velocity
		//// End of config ////









		// Magic Numbers
		private static float GravAccel = 9.81f;
		//Persistent Variables
		private static int Operation = Operations.Unknown;
		StringGraphics.ArrowSelector OperationGraphic = new StringGraphics.ArrowSelector();
		StringGraphics.ArrowSelector CommandGraphic = new StringGraphics.ArrowSelector();
		private static IMyCockpit Cockpit;
		private static IMyExtendedPistonBase Piston;
		private static IMyLandingGear Lock;
		private static IMyLandingGear Foot;
		private static List<IMyTextPanel> StatusLCDs = new List<IMyTextPanel>();
		private static List<IMyTextPanel> ConsoleLCDs = new List<IMyTextPanel>();
		// Functions
		private static Func<IMyTerminalBlock, bool> IsType(string TypeIdString) {
			return (Block) => Block.BlockDefinition.TypeIdString.Contains(TypeIdString);
		}
		private static Func<IMyTerminalBlock, bool> IsName(string CustNameString) {
			return (Block) => Block.CustomName.Contains(CustNameString);
		}
		private void LCDInit() {
			List<IMyTerminalBlock> StatusDubious = new List<IMyTerminalBlock>();
			List<IMyTerminalBlock> ConsoleDubious = new List<IMyTerminalBlock>();
			GridTerminalSystem.GetBlocksOfType<IMyTextPanel>(StatusDubious, IsName(StatusLCDsName));
			GridTerminalSystem.GetBlocksOfType<IMyTextPanel>(ConsoleDubious, IsName(ConsoleLCDsName));

			foreach (IMyTerminalBlock TextPanel in StatusDubious) {
				if (IsType("TextPanel")(TextPanel)) {
					IMyTextPanel LCD = (IMyTextPanel)TextPanel;
					StatusLCDs.Add(LCD);
					LCD.ContentType = ContentType.TEXT_AND_IMAGE;
					LCD.Font = "Monospace";
					LCD.WritePublicTitle("Outrigger Status");
				};
			};

			foreach (IMyTerminalBlock TextPanel in ConsoleDubious) {
				if (IsType("TextPanel")(TextPanel)) {
					IMyTextPanel LCD = (IMyTextPanel)TextPanel;
					ConsoleLCDs.Add(LCD);
					LCD.ContentType = ContentType.TEXT_AND_IMAGE;
					LCD.Font = "Monospace";
					LCD.WritePublicTitle("Outrigger Console");
				};
			};
		}
		private void LCDWriteText(string Value, bool append = false) {
			if (StatusLCDs.Count < 1) {
				Echo("LCD Not Found!");
				Echo(Value);
				return; 
			};
			foreach (IMyTextPanel LCD in StatusLCDs) {
				LCD.ContentType = ContentType.TEXT_AND_IMAGE;
				LCD.Font = "Monospace";
				LCD.WritePublicTitle("Outrigger Status");
				LCD.WriteText(Value, append);
			}
		}
		public Program() {
			Runtime.UpdateFrequency = UpdateFrequency.Once | UpdateFrequency.Update100;

			OperationGraphic.ArrowText = "Op.=> ";
			OperationGraphic.Options = Operations.ToArray();
			OperationGraphic.Selection = (UInt16)Operations.ToArrayIndex(-1);

			CommandGraphic.ArrowText = "==> ";
			CommandGraphic.Options = Commands.ToArray();
			CommandGraphic.Selection = (UInt16)Commands.ToArrayIndex(-1);

			List<IMyTerminalBlock> PistonsDubious = new List<IMyTerminalBlock>();
			List<IMyTerminalBlock> LocksDubious = new List<IMyTerminalBlock>();
			List<IMyTerminalBlock> FeetDubious = new List<IMyTerminalBlock>();
			List<IMyCockpit> Cockpits = new List<IMyCockpit>();
			GridTerminalSystem.GetBlocksOfType<IMyPistonBase>(PistonsDubious, IsName(PistonName));
			GridTerminalSystem.GetBlocksOfType<IMyLandingGear>(LocksDubious, IsName(LockName));
			GridTerminalSystem.GetBlocksOfType<IMyLandingGear>(FeetDubious, IsName(FootName));
			GridTerminalSystem.GetBlocksOfType<IMyCockpit>(Cockpits);

			foreach (IMyTerminalBlock PistonBase in PistonsDubious) {
				if (!IsType("PistonBase")(PistonBase)) continue;
				Piston = (IMyExtendedPistonBase)PistonBase;
				break;
			};
			foreach (IMyTerminalBlock LandingGear in LocksDubious) {
				if (!IsType("LandingGear")(LandingGear)) continue;
				Lock = (IMyLandingGear)LandingGear;
				break;
			};
			foreach (IMyTerminalBlock LandingGear in FeetDubious) {
				if (!IsType("LandingGear")(LandingGear)) continue;
				Foot = (IMyLandingGear)LandingGear;
				break;
			};
			foreach (IMyCockpit X in Cockpits) {
				if (!X.IsMainCockpit) continue;
				Cockpit = X;
				GravAccel = (float)Cockpit.GetTotalGravity().Length();
				break;
			};
			LCDInit();
		}
		private static void ResolveOperation() {
			if (Piston == null || Cockpit == null || Foot == null) {
				Operation = Operations.Broken;
				return;
			}
			if (Piston.Velocity == 0 || !Piston.Enabled) {
				if (Piston.Status == OutrigStatus.Extended) {
					Operation = Operations.Extended;
				} else if (Piston.Status == OutrigStatus.Retracted) {
					Operation = Operations.Retracted;
				} else {
					Operation = Operations.Stabilized;
				};
			} else if (Piston.Velocity > 0 && ExtendMatch) {
				if (Piston.Status == PistonStatus.Stopped || Piston.Status == OutrigStatus.Extended) {
					Operation = Operations.Extended;
				} else if (Foot.IsLocked) {
					Operation = Operations.Extending;
				} else {
					Operation = Operations.Stabilizing;
				};
			} else {
				if (Piston.Status == PistonStatus.Stopped || Piston.Status == OutrigStatus.Retracted) {
					Operation = Operations.Retracted;
				} else if (Cockpit.GetShipSpeed() < Math.Abs(Piston.Velocity) * MoveDectect && Foot.IsLocked) {
					Operation = Operations.StabilizeCorrecting;
				} else {
					Operation = Operations.Retracting;
				};
			};
		}
		private static void Governor() {
			if (Operation == Operations.Broken) {
				return;
			}
			if (Operation == Operations.StabilizeCorrecting) {	// Elastic Wheel Compression
				Piston.Enabled = false;
			};
			if (Operation != Operations.Extended && Operation != Operations.Retracted && Operation != Operations.Stabilized && Lock.IsLocked) {
				Lock.Enabled = true;							// Klang-tom Forces (Piston Force on Self Locked Landing Gear)
				Lock.Unlock();
				Lock.Enabled = false;
			};
			if (Operation != Operations.Retracted) {			// Driving or movement confusing ResolveOperation
				Cockpit.HandBrake = true;
			};
			if ((Cockpit.GetShipSpeed() > Math.Abs(Piston.Velocity) * MoveDectect && Operation != Operations.Retracted) || Operation == Operations.Extended) {	// Falling over while extended
				Foot.Enabled = true;
				Foot.Lock();
				Foot.Enabled = false;
			};
		}

		private static string RenderMaxImpulseAxis() {
			double RequiredNewtons = Math.Round(Cockpit.CalculateShipMass().TotalMass * GravAccel, 0);
			int UnitPower = (int)Math.Floor(Math.Log10(RequiredNewtons) / 3);
			double Shortened = RequiredNewtons / Math.Pow(10,UnitPower*3);		//Inverse square faster than division
			string Rendered = Shortened.ToString("N1");
			string Unit;
			switch (UnitPower) {
				case 0: Unit = "N";     //			10^0	One
					break;
				case 1: Unit = "KN";    // kilo		10^3	Thousand
					break;
				case 2:	Unit = "MN";    // mega		10^6	Million
					break;
				case 3:	Unit = "GN";    // giga		10^9	Billion
					break;
				case 4: Unit = "TN";    // tera		10^12	Trillion
					break;
				case 5:	Unit = "PN";    // peta		10^15	Quadrillion
					break;
				case 6:	Unit = "EN";    // exa		10^18	Quintillion
					break;
				case 7: Unit = "ZN";    // zetta	10^21	Sextillion
					break;
				case 8:	Unit = "YN";    // yotta	10^24	Septillion, Yo mama so fat...
					break;
				default: Unit = "N"; Rendered = RequiredNewtons.ToString("E3"); // Oh f*ck me thats big, or small...
					break;
			}

			return "Set MaxImpulseAxis to " + Rendered + Unit + '\n';
		}

		public void Save() {
		}

		public void Main(string argument, UpdateType updateSource) {
			ResolveOperation();
			switch (argument) {
				case "1":
					break;
				case "2":
					break;
				case "3":
					break;
				default:
					break;
			}


			OperationGraphic.Selection = (UInt16)Operations.ToArrayIndex(Operation);
			LCDWriteText(OperationGraphic.Render()+RenderMaxImpulseAxis());
			Echo(
				RenderMaxImpulseAxis() + 
				"Argument: " + (argument ?? "Null") + '\n' +
				"UpdateType: " + updateSource.ToString() + '\n'
			); ;
			Governor();
		}
	}
}
