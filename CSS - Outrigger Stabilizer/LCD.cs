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
		private static List<IMyTextPanel> StatusLCDs = new List<IMyTextPanel>();
		private static List<IMyTextPanel> ConsoleLCDs = new List<IMyTextPanel>();

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
	}
}
