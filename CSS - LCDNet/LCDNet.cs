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
using Sandbox.Game.GameSystems;

namespace IngameScript {
	/// <summary>
	/// Requires shared project: CSS - SandboxEmulation
	/// </summary>
	public class LCDNet : SandboxEmulation.MyTextSurface {
		public List<IMyTextSurface> ConnectedDisplays { get; } = new List<IMyTextSurface>();
		/// <summary>
		/// true: Immediately broadcasts WriteText to all connected displays.
		/// false: Waits on manual SyncText/FlushText invocation.
		/// false: Recommend if many appendages occur during a single cycle.
		/// </summary>
		public bool AutoSyncText { get; set; } = true;
		/// <summary>
		/// Syncs text to all connected displays.
		/// </summary>
		public void SyncText() {
			foreach (IMyTextPanel Display in ConnectedDisplays) {
				Display.WriteText(Text, false);
			}
		}
		/// <summary>
		/// Syncs Text and empties buffer.
		/// </summary>
		public void FlushText() {
			SyncText();
			Text = new StringBuilder();
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
			if (AutoSyncText) SyncText();
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
			if (AutoSyncText) SyncText();
			return true;
		}

		/// <summary>
		/// Connects to a new block that only has one display. +Overloads.
		/// </summary>
		/// <param name="Display">Example: A Text Panel</param>
		public void ConnectDisplay(IMyTextSurface Display) {
			if (ConnectedDisplays.Contains(Display)) return;
			ConnectedDisplays.Add(Display);
			CopyTo(Display, true);
		}
		/// <summary>
		/// Connects to new blocks that only have one display each.
		/// </summary>
		/// <param name="Display">Example: A Text Panel</param>
		public void ConnectDisplay(List<IMyTextSurface> Displays) {
			foreach (IMyTextSurface Display in Displays) {
				ConnectDisplay(Display);
			}
		}
		/// <summary>
		/// Connects to all displays in a multi-display block.
		/// </summary>
		/// <param name="DisplayHolder">Example: A Cockpit.</param>
		public void ConnectDisplay(IMyTextSurfaceProvider DisplayHolder) {
			for (int i = 0; i < DisplayHolder.SurfaceCount; i++) {
				ConnectDisplay(DisplayHolder.GetSurface(i));
			}
		}
		/// <summary>
		/// Connects to new TerminalBlocks that only have one TextSurface each.
		/// </summary>
		/// <param name="Display">Example: A Text Panel</param>
		public void ConnectDisplay(List<IMyTerminalBlock> DubiousDisplays) {

			foreach (IMyTerminalBlock DubiousDisplay in DubiousDisplays) {
				ConnectDisplay((IMyTextSurface)DubiousDisplay);
			}
		}
		/// <summary>
		/// Disconnects provided display. +Overloads.
		/// </summary>
		/// <param name="Display">Example: A Text Panel</param>
		public void DisconnectDisplay(IMyTextSurface Display) {
			if (!ConnectedDisplays.Contains(Display)) return;
			ConnectedDisplays.Remove(Display);
			Copy(new SandboxEmulation.MyTextSurface(), Display, true);
		}
		/// <summary>
		/// Disconnects listed displays.
		/// </summary>
		/// <param name="Display">Example: A Text Panel</param>
		public void DisconnectDisplay(List<IMyTextSurface> Displays) {
			foreach (IMyTextSurface Display in Displays) {
				DisconnectDisplay(Display);
			}
		}
		/// <summary>
		/// Disconnects all displays in provided multi-display block.
		/// </summary>
		/// <param name="DisplayHolder">Example: A Cockpit.</param>
		public void DisconnectDisplay(IMyTextSurfaceProvider DisplayHolder) {
			for (int i = 0; i < DisplayHolder.SurfaceCount; i++) {
				DisconnectDisplay(DisplayHolder.GetSurface(i));
			}
		}
		/// <summary>
		/// Disconnects all listed TerminalBlocks that only have one TextSurface each.
		/// </summary>
		/// <param name="Display">Example: A Text Panel</param>
		public void DisconnectDisplay(List<IMyTerminalBlock> DubiousDisplays) {

			foreach (IMyTerminalBlock DubiousDisplay in DubiousDisplays) {
				if (DubiousDisplay.BlockDefinition.TypeIdString.Contains("TextSurface")) {
					DisconnectDisplay((IMyTextSurface)DubiousDisplay);
				}
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
			foreach (IMyTextSurface Display in ConnectedDisplays) {
				Refresh(Display);
			}

		}
	}
}
