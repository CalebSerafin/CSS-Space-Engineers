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
		// ===== Config
		// Switches
		private static bool DoEngineFuelCheck = false;  // Turns H2/O2 Gens On/Off depending on Engines
		private static bool DoAntennaCheck = false;     // Keeps atLeast 1 Antenna on
		private static bool DoPowerFallback = false;    // Trigger timers depending on power

		// PowerFallback
		private const float PowerFallback_Percent = 0.1f;  //Low power under this point
		private static string[] PowerFallback_Low_Names = { "Low Timer" };
		private static string[] PowerFallback_High_Names = { "High Timer" };

		// ===== End of Config
		private static short N100Tick = 10;
		private const short SkipN100Ticks = 1;
		private static readonly List<IMyRadioAntenna> Antennas = new List<IMyRadioAntenna>();
		private static readonly List<IMyFunctionalBlock> Engines = new List<IMyFunctionalBlock>();
		private static readonly List<IMyGasGenerator> O2H2Gens = new List<IMyGasGenerator>();
		private static readonly List<IMyBatteryBlock> Batteries = new List<IMyBatteryBlock>();
		private static readonly List<IMyTimerBlock> PowerFallback_Low_Timers = new List<IMyTimerBlock>();
		private static readonly List<IMyTimerBlock> PowerFallback_High_Timers = new List<IMyTimerBlock>();

		private static readonly Action<IMyFunctionalBlock, string> ApplyAction = (Block, ActionName) => Block.GetActionWithName(ActionName).Apply(Block);
		private static Func<IMyTerminalBlock, bool> IsType(string TypeIdString) {
			return (Block) => Block.BlockDefinition.TypeIdString.Contains(TypeIdString);
		}
		private static Func<IMyTerminalBlock, bool> IsName(string CustNameString) {
			return (Block) => Block.CustomName.Contains(CustNameString);
		}
		private static Func<IMyTerminalBlock, bool> IsName(string[] CustNameStrings) {
			return (Block) => {
				foreach (string CustNameString in CustNameStrings) {
					if (Block.CustomName.Contains(CustNameString)) return true;
				}
				return false;
			};
		}
		private static Func<IMyTerminalBlock, bool> IsName(List<string> CustNameStrings) {
			return IsName(CustNameStrings.ToArray());
		}

		public Program() {
			Runtime.UpdateFrequency = UpdateFrequency.Once | UpdateFrequency.Update100;

			GridTerminalSystem.GetBlocksOfType<IMyRadioAntenna>(Antennas);
			GridTerminalSystem.GetBlocksOfType<IMyGasGenerator>(O2H2Gens);
			GridTerminalSystem.GetBlocksOfType(Engines, IsType("HydrogenEngine"));
			GridTerminalSystem.GetBlocksOfType<IMyBatteryBlock>(Batteries);
			GridTerminalSystem.GetBlocksOfType<IMyTimerBlock>(PowerFallback_Low_Timers, IsName(PowerFallback_Low_Names));
			GridTerminalSystem.GetBlocksOfType<IMyTimerBlock>(PowerFallback_High_Timers, IsName(PowerFallback_High_Names));
		}
		private static bool SkipTick() {
			N100Tick %= SkipN100Ticks;
			return N100Tick++ != 0; //Must increment after otherwise, evaluation will always be false;
		}
		private void AntennaCheck() {
			if (Antennas.Count == 0) {
				Echo("AntennaCheck:\nNo antennas found.");
				return;
			};
			Echo("AntennaCheck");
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
				Echo("EngineFuelCheck:\nNo Hydrogen Engines found.");
				return;
			};
			if (O2H2Gens.Count == 0) {
				Echo("EngineFuelCheck:\nNo O2/H2 Generators found.");
				return;
			};
			Echo("EngineFuelCheck");
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
		private void PowerFallback() {
			if (Batteries.Count == 0) {
				Echo("PowerFallback:\nNo Batteries found.");
				return;
			};
			Echo("PowerFallback:");
			Echo($"Found {PowerFallback_Low_Timers.Count} Low Power Timers.");
			Echo($"Found {PowerFallback_High_Timers.Count} High Power Timers.");
			Echo($"Found {Batteries.Count} Batteries.");

			float CurrentStored = 0f;
			float MaxStored = 0f;
			foreach (IMyBatteryBlock Battery in Batteries) {
				CurrentStored += Battery.CurrentStoredPower;
				MaxStored += Battery.MaxStoredPower;
			}
			float StoredPercent = CurrentStored / MaxStored;
			Echo($"Power {Math.Floor(CurrentStored)}/{Math.Floor(MaxStored)} MW ({Math.Round(StoredPercent * 100, 2)}%).");

			if (StoredPercent <= PowerFallback_Percent) {
				foreach (IMyTimerBlock Timer in PowerFallback_Low_Timers) {
					Timer.Trigger();
				}
			} else {
				foreach (IMyTimerBlock Timer in PowerFallback_High_Timers) {
					Timer.Trigger();
				}
			}
		}
		public void Main(string argument, UpdateType updateSource) {
			if (SkipTick()) return;
			if (DoAntennaCheck)	AntennaCheck();
			if (DoEngineFuelCheck) EngineFuelCheck();
			if (DoPowerFallback) PowerFallback();
		}
	}
}
