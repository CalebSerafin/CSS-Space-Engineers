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
		//// Config ////
		// Blocks must contain their respective strings.
		private const string PistonName = "OutriggerPiston";
		private const string LockName = "OutriggerLock";
		private const string FootName = "OutriggerFoot";

		private const string StatusLCDsName = "OutriggerStatusLCD"; //Debug LCD
		private const string ConsoleLCDsName = "OutriggerConsoleLCD"; //Control LCD

		private const bool ExtendMatch = true;    //Outrigger Extend Direction Matches Piston Extend Direction
		private const float MoveDectect = 0.75f;     //Ship-Speed compared to X*Percent of piston velocity
		//// End of config ////









		// Magic Numbers
		private static float GravAccel = 9.81f;
		//Persistent Variables
		private static int Operation = Operations.Unknown;
		StringGraphics.ArrowSelector OperationGraphic = new StringGraphics.ArrowSelector();
		StringGraphics.ArrowSelector CommandGraphic = new StringGraphics.ArrowSelector();
		LCDNet StatusLCDs = new LCDNet();
		LCDNet ConsoleLCDs = new LCDNet();

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
		

		public void Save() {
		}

		public void Main(string argument, UpdateType updateSource) {
			ResolveOperation();
			switch (argument) {
				case "1":
					break;
				case "2":
					break;
				case "3":
					break;
				default:
					break;
			}
			;
			OperationGraphic.Selection = (UInt16)Operations.ToArrayIndex(Operation);
			StatusLCDs.WriteText(OperationGraphic.Render(), true);
			StatusLCDs.WriteText(RenderMaxImpulseAxis(), true);
			ConsoleLCDs.WriteText(RenderMaxImpulseAxis(), true);
			ConsoleLCDs.WriteText("Argument: " + (argument ?? "Null") + '\n', true);
			ConsoleLCDs.WriteText("UpdateType: " + updateSource.ToString() + '\n', true);

			Echo(ConsoleLCDs.GetText());
			StatusLCDs.FlushText();
			ConsoleLCDs.FlushText();
			Governor();
		}
	}
}