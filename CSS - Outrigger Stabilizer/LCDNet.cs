using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System;
using VRage.Collections;
using VRage.Game.Components;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame.Utilities;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Game;
using VRage;

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
	//Throw the above in a dumpster when the code below is finished.


	public class LCDNet : SandboxEmulation.MyTextSurface {
		public List<IMyTextSurface> DisplayList { get; } = new List<IMyTextSurface>();


		private void BroadcastText() {
			foreach (IMyTextPanel Surface in DisplayList) {
				Surface.WriteText(Text, false);
			}
		}
		public new bool WriteText(string value, bool append = false) {
			try {
				if (!append) {
					Text.Clear();
				}
				Text.Append(value);
			} catch (Exception) {
				return false;
			}
			BroadcastText();
			return true;
		}

		public new bool WriteText(System.Text.StringBuilder value, bool append = false) {
			try {
				if (append) {
					Text.Append(value);
				} else {
					Text = value;
				}
			} catch (Exception) {
				return false;
			}
			BroadcastText();
			return true;
		}

		/// <summary>
		/// Connects to a new block that is only has one display.
		/// </summary>
		/// <param name="NewLCD">Example: A Text Panel</param>
		public void ConnectDisplay(IMyTextSurface NewLCD) {
			DisplayList.Add(NewLCD);
			CopyTo(NewLCD, true);
		}

		/// <summary>
		/// Connects to all displays in a block that has multiple displays.
		/// </summary>
		/// <param name="LCDHolder">Example: A Cockpit.</param>
		public void ConnectDisplay(IMyTextSurfaceProvider LCDHolder) {
			for (int i = 0; i < LCDHolder.SurfaceCount; i++) {
				ConnectDisplay(LCDHolder.GetSurface(i));
			}
		}
		/// <summary>
		/// Re-syncs settings in provided display.
		/// </summary>
		/// <param name="Display">Example: A Text Panel</param>
		public void Refresh(IMyTextSurface Display) {
			if (Display != null) {
				CopyTo(Display);
			}
		}
		/// <summary>
		/// Re-syncs settings in all connected displays.
		/// </summary>
		public void Refresh() {
			foreach (IMyTextSurface Display in DisplayList) {
				Refresh(Display);
			}

		}
	}
}
