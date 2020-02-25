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
	partial class Program {
		/// <summary>
		/// Returns the current extension length of a piston. [SE 1.193.103]
		/// </summary>
		/// <param name="Piston">Base not head!</param>
		/// <returns>Length in metres.</returns>
		public static double PistonGetCurrentPosition(IMyPistonBase Piston) {
			string Info = Piston.DetailedInfo;


			/// Not perfect solution, but there is no alternative!
			string[] InfoLines = Info.Split(':');	// Theres 1 colon in info, and it's right before the current position.
			string CurrentPositionString = InfoLines[1].Split('m')[0];	// Removes everything after first "m"
			return double.Parse(CurrentPositionString);	//There is also whitespace, however it is unimportant.
		}
	}
}
