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
		private static short N100Tick = 10;
		private const short SkipN100Ticks = 10;
		private static List<IMyRadioAntenna> Antennas = new List<IMyRadioAntenna>();
		private static List<IMyFunctionalBlock> Engines = new List<IMyFunctionalBlock>();
		private static List<IMyGasGenerator> O2H2Gens = new List<IMyGasGenerator>();
		private static readonly Action<IMyFunctionalBlock, string> ApplyAction = (Block, ActionName) => Block.GetActionWithName(ActionName).Apply(Block);
		private static Func<IMyTerminalBlock, bool> IsType(string TypeIdString) {
			return (block) => block.BlockDefinition.TypeIdString.Contains(TypeIdString);
		}

		public Program() {
			Runtime.UpdateFrequency = UpdateFrequency.Once | UpdateFrequency.Update100;

			GridTerminalSystem.GetBlocksOfType<IMyRadioAntenna>(Antennas);
			GridTerminalSystem.GetBlocksOfType<IMyGasGenerator>(O2H2Gens);
			GridTerminalSystem.GetBlocksOfType(Engines, IsType("HydrogenEngine"));
		}
		private static bool SkipTick() {
			N100Tick %= SkipN100Ticks;
			return N100Tick++ != 0; //Must increment after otherwise, evaluation will always be false;
		}
		private void AntennaCheck() {
			if (Antennas.Count == 0) {
				Echo("No antennas found.");
				return;
			};
			Echo($"Found {Antennas.Count} Antennas.");
			bool AnyAntennasEnabled = false;
			foreach (IMyRadioAntenna Antenna in Antennas) {
				if (Antenna.Enabled && Antenna.EnableBroadcasting) {
					AnyAntennasEnabled = true;
					break;
				}
			}
			if (!AnyAntennasEnabled) {
				IMyRadioAntenna Antenna = (Antennas[0]);
				Antenna.EnableBroadcasting = true;
				Antenna.Enabled = true;
			}
		}
		private void EngineFuelCheck() {
			if (Engines.Count == 0) {
				Echo("No Hydrogen Engines found.");
				return;
			};
			if (O2H2Gens.Count == 0) {
				Echo("No O2/H2 Generators found.");
				return;
			};
			Echo($"Found {Engines.Count} Engines.");
			Echo($"Found {O2H2Gens.Count} O2/H2 Generators.");
			bool AnyEnginesEnabled = false;
			foreach (IMyFunctionalBlock Engine in Engines) {
				if (Engine.Enabled) {
					AnyEnginesEnabled = true;
					break;
				}
			}
			if (AnyEnginesEnabled) {
				O2H2Gens.ForEach((Gen) => Gen.Enabled = true);
			} else {
				O2H2Gens.ForEach((Gen) => Gen.Enabled = false);
			}
		}
		public void Main(string argument, UpdateType updateSource) {
			if (SkipTick()) return;
			AntennaCheck();
			EngineFuelCheck();
		}
	}
}
