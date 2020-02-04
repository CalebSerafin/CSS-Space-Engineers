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

		private const string OutRiggerPistonName = "OutriggerPiston";
		private const string OutRiggerLockName = "OutriggerLock";
		private const string OutRiggerFootName = "OutriggerFoot";
		private const float OverideTotalLiftForce = 10e6f;
		private const float OverideOutRiggerLiftForce = 5e3f;
		//End of config

		//Constants
		private const float MinImpulseAxis = 100f; 
		private static class Operations {
			public const int Unknown = -1;

			public const int Extending = 0;
			public const int Retracting = 1;
			public const int Stabilizing = 2;
			public const int StabilizeCorrecting = 2;


			public const int Extended = 10;
			public const int Retracted = 11;
			public const int Stabilized = 12;

			public static string Text(int OperationInt) {
				switch (OperationInt) {
					case 0: return "Extending";
					case 1: return "Retracting";
					case 2: return "Stabilizing";
					case 3: return "StabilizeCorrecting";

					case 10: return "Extended";
					case 11: return "Retracted";
					case 12: return "Stabilized";

					default: return "Unknown";
				};
			}
		}
		private static class OutrigStatus {
			public const Sandbox.ModAPI.Ingame.PistonStatus Extended = ExtendMatch ? PistonStatus.Extended : PistonStatus.Retracted;
			public const Sandbox.ModAPI.Ingame.PistonStatus Extending = ExtendMatch ? PistonStatus.Extending : PistonStatus.Retracting;
			public const Sandbox.ModAPI.Ingame.PistonStatus Retracted = ExtendMatch ? PistonStatus.Retracted : PistonStatus.Extended;
			public const Sandbox.ModAPI.Ingame.PistonStatus Retracting = ExtendMatch ? PistonStatus.Retracting : PistonStatus.Extending;
			public const Sandbox.ModAPI.Ingame.PistonStatus Stopped = PistonStatus.Stopped;
		}
		//Persistent Variables
		IMyCockpit Cockpit;
		private IMyExtendedPistonBase Piston;
		private IMyLandingGear Lock;
		private IMyLandingGear Foot;

		private static int Operation = Operations.Unknown;
		private static float TotalLiftForce = 0f;
		private static float OutRiggerLiftForce = 0f;
		//Delta Variables
		private static float PistonLastLength = -1;
		//Functions
		private static Func<IMyTerminalBlock, bool> IsType(string TypeIdString) {
			return (Block) => Block.BlockDefinition.TypeIdString.Contains(TypeIdString);
		}
		private static Func<IMyTerminalBlock, bool> IsName(string CustNameString) {
			return (Block) => Block.CustomName.Contains(CustNameString);
		}
		private void UpdateDeltaVariables() {
			PistonLastLength = Piston.CurrentPosition;
		}

		public Program() {
			Runtime.UpdateFrequency = UpdateFrequency.Once;
			List<IMyTerminalBlock> PistonsDubious = new List<IMyTerminalBlock>();
			List<IMyTerminalBlock> LocksDubious = new List<IMyTerminalBlock>();
			List<IMyTerminalBlock> FeetDubious = new List<IMyTerminalBlock>();
			List<IMyCockpit> Cockpits = new List<IMyCockpit>();
			GridTerminalSystem.GetBlocksOfType<IMyPistonBase>(PistonsDubious, IsName(OutRiggerPistonName));
			GridTerminalSystem.GetBlocksOfType<IMyLandingGear>(LocksDubious, IsName(OutRiggerLockName));
			GridTerminalSystem.GetBlocksOfType<IMyLandingGear>(FeetDubious, IsName(OutRiggerFootName));
			GridTerminalSystem.GetBlocksOfType<IMyCockpit>(Cockpits);

			foreach (IMyTerminalBlock PistonBase in PistonsDubious) {
				Echo(PistonBase.BlockDefinition.TypeIdString);
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
			UpdateDeltaVariables();
		}

		private void ResolveOperation() {
			if (Piston.Velocity == 0) {
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
				} else if (Cockpit.GetShipSpeed() > Piston.Velocity * 0.50) {
					Operation = Operations.Extending;
				} else {
					Operation = Operations.Stabilizing;
				};
			} else {
				if (Piston.Status == PistonStatus.Stopped || Piston.Status == OutrigStatus.Retracted) {
					Operation = Operations.Retracted;
				} else if (Foot.IsLocked) {
					Operation = Operations.StabilizeCorrecting;
				} else {
					Operation = Operations.Retracting;
				};
			};
		}

		public void Save() {
		}

		public void Main(string argument, UpdateType updateSource) {
			if (Operation == Operations.Unknown) ResolveOperation();
			Echo("Current Operation: " + Operations.Text(Operation));

		}
	}
}
