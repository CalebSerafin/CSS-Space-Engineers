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
using CSS_Common;

namespace IngameScript {
	public static class Diagnostics {
		public static void FormatDumpData(ref StringBuilder DataDump, string Name, string Data) {
			DataDump.Append(string.Format("---BEGIN {1} DUMP BLOCK---{0}", Environment.NewLine, Name));
			DataDump.Append(Data);
			DataDump.Append(string.Format("---END {1} DUMP BLOCK---{0}", Environment.NewLine, Name));
		}
		public static void FormatDumpData<T>(ref StringBuilder DataDump, string Name, IEnumerable<T> Data) where T : class {
			FormatDumpData(ref DataDump, Name, Data?.Aggregate("", (TextBuffer, Item) => TextBuffer + Item?.ToString() + Environment.NewLine));
		}
		public static void FormatDumpData<T, Y>(ref StringBuilder DataDump, string Name, IEnumerable<KeyValuePair<T, Y>> Data) {
			FormatDumpData(ref DataDump, Name, Data?.Aggregate("", (TextBuffer, Pair) => TextBuffer + Pair.ToString() + Environment.NewLine));
		}
		public static void FormatDumpData(ref StringBuilder DataDump, string Name, UpdateEventHandlers Data) {
			FormatDumpData(ref DataDump, $"{Name}.Subscribers", Data?.Subscribers); // Default Event Subscribers are not shown.
		}
		public static void FormatDumpData(ref StringBuilder DataDump, string Name, UpdateFrequency Data) {
			FormatDumpData(ref DataDump, Name, Data.ToString() + Environment.NewLine);
		}
		public static void FormatDumpData(ref StringBuilder DataDump, string Name, UpdateType Data) {
			FormatDumpData(ref DataDump, Name, Data.ToString() + Environment.NewLine);
		}
	}
	partial class Program : MyGridProgram {
		void DumpData(object Sender, UpdateEventArgs e) {
			if (!e.argument.Equals("-DumpData", StringComparison.OrdinalIgnoreCase)) return;
			// Dump Procedure
			StringBuilder DataDump = new StringBuilder();
			Diagnostics.FormatDumpData(ref DataDump, "ConsoleProvider", ConsoleProvider.ToString() + Environment.NewLine);
			Diagnostics.FormatDumpData(ref DataDump, "Console", Console.ToString() + Environment.NewLine);
			Diagnostics.FormatDumpData(ref DataDump, "ArgBuffer", ArgBuffer);
			Diagnostics.FormatDumpData(ref DataDump, "ConsoleBuffers", ConsoleBuffers);
			Diagnostics.FormatDumpData(ref DataDump, "UpdateEvents", UpdateEvents);

			Diagnostics.FormatDumpData(ref DataDump, "Runtime.UpdateFrequency", Runtime.UpdateFrequency);
			Diagnostics.FormatDumpData(ref DataDump, "updateSource", e.updateSource);

			// Save and Notify
			Runtime.UpdateFrequency = UpdateFrequency.None;
			Me.CustomData = DataDump.ToString();
			Echo($"Data Dumped into Custom Data.{Environment.NewLine}Execution Stopped.");
			ConsoleBuffers["DataDump"] = $"Data Dumped into CustomData.{Environment.NewLine}Execution Stopped.";
		}
	}
}
