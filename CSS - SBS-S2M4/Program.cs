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
	partial class Program : MyGridProgram {
		IMyTextSurfaceProvider ConsoleProvider;
		IMyTextSurface Console;
		Queue<string> ArgBuffer = new Queue<string>();
		Dictionary<string, string> ConsoleBuffers = new Dictionary<string, string>();
		UpdateEventHandlers UpdateEvents = new UpdateEventHandlers();

		void PopHKHistory(object Sender, UpdateEventArgs e) {
			if (ArgBuffer.Count > 0) ConsoleBuffers["HKHistory"] = e.updateSource.ToString() + ArgBuffer.Dequeue();
		}
		void DumpHKHistory(object Sender, UpdateEventArgs e) {
			Echo("DumpHKHistory");
			ConsoleBuffers["Terminal"] = ArgBuffer.Aggregate("Terminal", (History, Item) => History + Environment.NewLine + Item);
			ArgBuffer.Clear();
		}
		void AddHKHistory(object Sender, UpdateEventArgs e) {
			ArgBuffer.Enqueue(e.argument);
		}
		void Pinger(object Sender, UpdateEventArgs e) {
			Echo("Ping!: " + e.argument + ", " + e.updateSource.ToString());
		}


		public Program() {
			Runtime.UpdateFrequency = UpdateFrequency.Update10 | UpdateFrequency.Update100;

			TerminalSystem.GetBlockWithName(this, "SBS-Console", ref ConsoleProvider);
			Console = ConsoleProvider.GetSurface(0);
			Console.ContentType = ContentType.TEXT_AND_IMAGE;
			Console.Alignment = TextAlignment.CENTER;

			UpdateEvents.UpdateNone += Pinger;
			UpdateEvents.UpdateTerminal += DumpHKHistory;
			UpdateEvents.UpdateTrigger += AddHKHistory;
			UpdateEvents.Update100 += PopHKHistory;
		}

		public void Save() {
		}

		public void Main(string argument, UpdateType updateSource) {
			ConsoleBuffers["updateSource"] = "updateSource: " + updateSource.ToString();

			UpdateEvents.CallUpdateEventHandlers(argument, updateSource);

			Console.WriteText(
				ConsoleBuffers.Aggregate("", (TextBuffer, Pair) => TextBuffer + Pair.Value + Environment.NewLine)
			);
		}
	}
}
