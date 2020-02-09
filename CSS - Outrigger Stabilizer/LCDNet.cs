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

	public class LCDNet : MyGridProgram {
		private static Func<IMyTerminalBlock, bool> IsType(string TypeIdString) {
			return (Block) => Block.BlockDefinition.TypeIdString.Contains(TypeIdString);
		}
		private static Func<IMyTerminalBlock, bool> IsName(string CustNameString) {
			return (Block) => Block.CustomName.Contains(CustNameString);
		}

		public List<IMyTextSurface> LCDList { get; } = new List<IMyTextSurface>();
		public string TextBuffer { get; }

		public static void Clone(IMyTextSurface SourceLCD, IMyTextSurface TargetLCD, bool IncludeContent = false) {
			TargetLCD.Alignment = SourceLCD.Alignment;
			TargetLCD.BackgroundAlpha = SourceLCD.BackgroundAlpha;
			TargetLCD.BackgroundColor = SourceLCD.BackgroundColor;
			TargetLCD.ChangeInterval = SourceLCD.ChangeInterval;
			TargetLCD.Font = SourceLCD.Font;
			TargetLCD.FontColor = SourceLCD.FontColor;
			TargetLCD.FontSize = SourceLCD.FontSize;
			TargetLCD.ScriptBackgroundColor = SourceLCD.ScriptBackgroundColor;
			TargetLCD.ScriptForegroundColor = SourceLCD.ScriptForegroundColor;

			if (IncludeContent) {
				TargetLCD.ClearImagesFromSelection(); List<string> Images = new List<string>(); SourceLCD.GetSelectedImages(Images); TargetLCD.AddImagesToSelection(Images);
				TargetLCD.ContentType = SourceLCD.ContentType;
				TargetLCD.Script = SourceLCD.Script;
				TargetLCD.WriteText(SourceLCD.GetText());
			}

		}
		public void Redraw(IMyTextSurface LCD) {
			LCD.WriteText(TextBuffer);
		}
		public void Add(IMyTextSurface LCD) {
			LCDList.Add(LCD);
			Redraw(LCD);
		}

		public void Add(IMyTextSurfaceProvider LCDHolder) {
			for (int i = 0; i < LCDHolder.SurfaceCount; i++) {
				LCDList.Add(LCDHolder.GetSurface(i));
			}
		}

		private void WriteText(string Value, bool append = false) {
			if (LCDList.Count < 1) {
				Echo("LCD Not Found!");
				Echo(Value);
				return;
			};
			foreach (IMyTextPanel LCD in LCDList) {
				LCD.WriteText(Value, append);
			}
		}

	}
}
