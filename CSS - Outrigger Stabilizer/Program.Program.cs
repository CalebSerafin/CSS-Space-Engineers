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
	partial class Program {
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
	}
}
