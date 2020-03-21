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
		// | || |||| Config |||| || | \\


		private const float GroundClearance = -1.0f; // metres
		/*	Distance from BOTTOM SURFACE of landing gear to the ground when suspension is sitting normally.
			Too small values will cause your vehicle to rebound like an released elastic band.											
			Too large values will cause your vehicle to tip over on the outrigger.*/

		// (Blocks must contain their respective strings.)
		private const string PistonName = "OutriggerPiston";
		private const string LockName = "OutriggerLock";
		private const string FootName = "OutriggerFoot";

		private const string StatusLCDsName = "OutriggerStatusLCD"; // Debug LCD
		private const string ConsoleLCDsName = "OutriggerConsoleLCD"; // Control LCD

		private const bool ExtendMatch = true;    // Outrigger Extend Direction Matches Piston Extend Direction
		private const float MoveDectect = 0.75f;  // Ship-Speed compared to X*Percent of piston velocity
		// | || |||| End of Config |||| || | \\









		// Magic Numbers
		private static float GravAccel = 9.81f; // Placeholder Value until initialisation
		// Persistent Variables
		private static int Operation = Operations.Unknown;
		StringGraphics.ArrowSelector OperationGraphic = new StringGraphics.ArrowSelector();
		LCDNet StatusLCDs = new LCDNet();
		LCDNet ConsoleLCDs = new LCDNet();

		private static List<string> Warnings = new List<string>();

		private static IMyCockpit Cockpit;
		private static IMyExtendedPistonBase Piston;
		private static IMyLandingGear Lock;
		private static IMyLandingGear Foot;
		// Functions
		private static Func<IMyTerminalBlock, bool> IsType(string TypeIdString) {
			return (Block) => Block.BlockDefinition.TypeIdString.Contains(TypeIdString);
		}
		private static Func<IMyTerminalBlock, bool> IsName(string CustNameString) {
			return (Block) => Block.CustomName.Contains(CustNameString);
		}
		private static void OutRiggerRelease() {
			Lock.Enabled = true;
			Lock.AutoLock = false;
			Lock.Unlock();
		}
		private static void OutRiggerSecure() {
			Lock.Enabled = true;
			Lock.AutoLock = true;
			Lock.Lock();
		}

		public void Save() {
		}

		public void Main(string argument, UpdateType updateSource) {
			try {
				Warnings.Clear();
				ResolveOperation();
				HandleArgs(argument);
				Governor();
				CheckUpdateFrequency();

				OperationGraphic.Selection = (UInt16)Operations.ToArrayIndex(Operation);
				StatusLCDs.WriteText(OperationGraphic.Render(), true);
				StatusLCDs.WriteText(RenderMaxImpulseAxis(), true);
				ConsoleLCDs.WriteText(RenderMaxImpulseAxis(), true);
				ConsoleLCDs.WriteText("Argument: " + (argument ?? "Null") + '\n', true);
				ConsoleLCDs.WriteText("UpdateType: " + updateSource.ToString() + '\n', true);



			} catch (BlockNullReferenceException BlockNull) {
				string ErrorMessageFormatted = "CRITICAL ERROR:\n" + BlockNull.Message;
				ConsoleLCDs.WriteText(ErrorMessageFormatted, false);
				StatusLCDs.WriteText(ErrorMessageFormatted, false);

				StatusLCDs.FlushText();
				ConsoleLCDs.FlushText();
				throw;
			} catch (KlangSafetyException KlangSafety) {
				Piston.Enabled = false;
				OutRiggerRelease();
				Lock.Enabled = false;
				Foot.Enabled = true;
				Foot.AutoLock = true;
				Foot.Lock();
				Runtime.UpdateFrequency = UpdateFrequency.None;

				string ErrorMessageFormatted = "CRITICAL ERROR:\n" + KlangSafety.Message;
				ConsoleLCDs.WriteText(ErrorMessageFormatted, false);
				StatusLCDs.WriteText(ErrorMessageFormatted, false);

				StatusLCDs.FlushText();
				ConsoleLCDs.FlushText();
				throw;
			} catch (Exception) {
				throw;
			}

			if (Warnings.Count > 0) {
				string WarningMessageFormatted = "|| WARNINGS ||:\n";
				foreach (string Warning in Warnings) {
					WarningMessageFormatted += Warning + "\n";
				}
				WarningMessageFormatted += "|| END OF WARNINGS ||\n\n";
				ConsoleLCDs.WriteText(WarningMessageFormatted + ConsoleLCDs.GetText(), false);
				StatusLCDs.WriteText(WarningMessageFormatted + StatusLCDs.GetText(), false);
			}
			Echo(ConsoleLCDs.GetText());
			StatusLCDs.FlushText();
			ConsoleLCDs.FlushText();
		}
	}
}