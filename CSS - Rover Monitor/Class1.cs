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
		public class StringGraphics {
			public class ArrowSelector {
				public static string ArrowText;
				public static List<string> Options;
				public static UInt16 Selection;
				
				private static string Padding() {
					return new String(' ', ArrowText.Length);
				}
				public ArrowSelector() {
					ArrowText = "-->";
					Options = new List<string>();
					Selection = 0;
				}
				public static string Render() {
					string 
				}
			}
		}
	}
}
