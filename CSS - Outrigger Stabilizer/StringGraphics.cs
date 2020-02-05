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
				public string ArrowText;     // Default "-->"
				public string[] Options;
				public UInt16 Selection;		// Zero-Based Index
				public Boolean MovingArrow;  // False = Moving List
				
				public ArrowSelector(string ArrowTextX = "--> ", string[] OptionsX = null, UInt16 SelectionX = 0, Boolean MovingArrowX = true) {
					ArrowText = ArrowTextX;
					Options = OptionsX ?? Array.Empty<string>();
					Selection = SelectionX;
					MovingArrow = MovingArrowX;
				}

				public string Render() {
					string RenderOut = "";
					string Padding = new String(' ', ArrowText.Length);
					UInt16 OptionsCount = (UInt16)Options.Count();
					Selection = (UInt16) (Selection % OptionsCount);

					if (MovingArrow) {
						for (int i = 0; i < OptionsCount; i++) {
							RenderOut += ((Selection == i) ? ArrowText : Padding) + Options[i] + '\n';
						}
					} else {
						RenderOut += ArrowText + Options[Selection] + '\n';
						for (int i = Selection + 1; i < OptionsCount; i++) {
							RenderOut += Padding + Options[i] + '\n';
						}
						for (int i = 0; i < Selection; i++) {
							RenderOut += Padding + Options[i] + '\n';
						}
					}
					return RenderOut;
				}
			}
		}
	}
}
