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
		//Config
		//Blocks must contain their respective strings.
		private const bool ExtendMatch = true;    //Outrigger Extend Direction Matches Piston Extend Direction

		private const string OutriggerPistonName = "OutriggerPiston";
		private const string OutriggerLockName = "OutriggerLock";
		private const string OutriggerFootName = "OutriggerFoot";

		private const string OutriggerLCDsName = "OutriggerLCD"; //Debug LCD
		//End of config










		//Persistent Variables
		private static int Operation = Operations.Unknown;
		StringGraphics.ArrowSelector OperationGraphic = new StringGraphics.ArrowSelector();
		private static IMyCockpit Cockpit;
		private static IMyExtendedPistonBase Piston;
		private static IMyLandingGear Lock;
		private static IMyLandingGear Foot;
		private static List<IMyTextPanel> LCDs = new List<IMyTextPanel>();
		//Delta Variables
		private static float PistonLastLength = -1;
		//Functions
		private static Func<IMyTerminalBlock, bool> IsType(string TypeIdString) {
			return (Block) => Block.BlockDefinition.TypeIdString.Contains(TypeIdString);
		}
		private static Func<IMyTerminalBlock, bool> IsName(string CustNameString) {
			return (Block) => Block.CustomName.Contains(CustNameString);
		}
		private static void UpdateDeltaVariables() {
			if (Piston != null) { PistonLastLength = Piston.CurrentPosition; };
		}
		private void LCDInit() {
			List<IMyTerminalBlock> LCDDubious = new List<IMyTerminalBlock>();
			GridTerminalSystem.GetBlocksOfType<IMyTextPanel>(LCDDubious, IsName(OutriggerLCDsName));

			foreach (IMyTerminalBlock TextPanel in LCDDubious) {
				if (IsType("TextPanel")(TextPanel)) {
					LCDs.Add((IMyTextPanel)TextPanel);
				};
			};

			foreach (IMyTextPanel LCD in LCDs) {
				LCD.ContentType = ContentType.TEXT_AND_IMAGE;
				LCD.Font = "Monospace";
				LCD.WritePublicTitle("Outrigger Status");
			}
		}
		private void LCDWriteText(string Value, bool append = false) {
			if (LCDs.Count < 1) {
				Echo("LCD Not Found!");
				Echo(Value);
				return; 
			};
			foreach (IMyTextPanel LCD in LCDs) {
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

			List<IMyTerminalBlock> PistonsDubious = new List<IMyTerminalBlock>();
			List<IMyTerminalBlock> LocksDubious = new List<IMyTerminalBlock>();
			List<IMyTerminalBlock> FeetDubious = new List<IMyTerminalBlock>();
			List<IMyCockpit> Cockpits = new List<IMyCockpit>();
			GridTerminalSystem.GetBlocksOfType<IMyPistonBase>(PistonsDubious, IsName(OutriggerPistonName));
			GridTerminalSystem.GetBlocksOfType<IMyLandingGear>(LocksDubious, IsName(OutriggerLockName));
			GridTerminalSystem.GetBlocksOfType<IMyLandingGear>(FeetDubious, IsName(OutriggerFootName));
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
				break;
			};
			LCDInit();
			UpdateDeltaVariables();
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
				} else if (Cockpit.GetShipSpeed() < Math.Abs(Piston.Velocity) * 0.50 && Foot.IsLocked) {
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
			if (Operation == Operations.StabilizeCorrecting) {	// Wheel Compression
				Piston.Enabled = false;
			};
			if (Operation != Operations.Extended && Operation != Operations.Retracted && Operation != Operations.Stabilized && Lock.IsLocked) {
				Lock.Enabled = true;							// Klang-tom Forces (Piston Force on Self Locked Landing Gear)
				Lock.Unlock();
				Lock.Enabled = false;
			};
			if (Operation != Operations.Retracted) {            // Driving or movement confusing ResolveOperation
				Cockpit.HandBrake = true;
			};
		}


		public void Save() {
		}

		public void Main(string argument, UpdateType updateSource) {
			ResolveOperation();
			Governor();

			OperationGraphic.Selection = (UInt16)Operations.ToArrayIndex(Operation);
			LCDWriteText(OperationGraphic.Render());
		}
	}
}
