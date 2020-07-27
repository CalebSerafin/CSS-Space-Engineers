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
using static CSS__Common.TerminalSystem;

namespace IngameScript {
	partial class Program {
		public Program() {
			Runtime.UpdateFrequency = UpdateFrequency.Once;

			OperationGraphic.ArrowText = "Op.=> ";
			OperationGraphic.Options = Operations.ToArray();
			OperationGraphic.Selection = (UInt16)Operations.ToArrayIndex(-1);

			List<IMyExtendedPistonBase> PistonsDubious = new List<IMyExtendedPistonBase>();
			List<IMyLandingGear> LocksDubious = new List<IMyLandingGear>();
			List<IMyLandingGear> FeetDubious = new List<IMyLandingGear>();
			List<IMyCockpit> Cockpits = new List<IMyCockpit>();
			GridTerminalSystem.GetBlocksOfType(PistonsDubious, IsPartName(PistonName));
			GridTerminalSystem.GetBlocksOfType(LocksDubious, IsPartName(LockName));
			GridTerminalSystem.GetBlocksOfType(FeetDubious, IsPartName(FootName));
			GridTerminalSystem.GetBlocksOfType(Cockpits);

			Piston = PistonsDubious.FirstOrDefault();
			Lock = LocksDubious.FirstOrDefault();
			Foot = FeetDubious.FirstOrDefault();

			foreach (IMyCockpit X in Cockpits) {
				if (!X.IsMainCockpit) continue;
				Cockpit = X;
				GravAccel = (float)Cockpit.GetTotalGravity().Length();
				break;
			};

			StatusLCDs.ContentType = ContentType.TEXT_AND_IMAGE;
			StatusLCDs.Font = "Monospace";
			StatusLCDs.AutoSyncText = false;
			Echo("Before Copy");
			StatusLCDs.CopyTo(ConsoleLCDs);
			List<IMyTerminalBlock> StatusLCDsDubious = new List<IMyTerminalBlock>();
			GridTerminalSystem.GetBlocksOfType<IMyTextSurface>(StatusLCDsDubious, IsName(StatusLCDsName));
			StatusLCDs.ConnectDisplay(StatusLCDsDubious);
			List<IMyTerminalBlock> ConsoleLCDsDubious = new List<IMyTerminalBlock>();
			GridTerminalSystem.GetBlocksOfType<IMyTextSurface>(ConsoleLCDsDubious, IsName(ConsoleLCDsName));
			ConsoleLCDs.ConnectDisplay(ConsoleLCDsDubious);

		}
	}
}
