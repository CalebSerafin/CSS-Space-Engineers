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
		private static string RenderMaxImpulseAxis() {
			double RequiredNewtons = Math.Round(Cockpit.CalculateShipMass().TotalMass * GravAccel, 0);
			int UnitPower = (int)Math.Floor(Math.Log10(RequiredNewtons) / 3);
			double Shortened = RequiredNewtons / Math.Pow(10, UnitPower * 3);       //Inverse square faster than division
			string Rendered = Shortened.ToString("N1");
			string Unit;
			switch (UnitPower) {
				case 0:
					Unit = "N";     //			10^0	One
					break;
				case 1:
					Unit = "KN";    // kilo		10^3	Thousand
					break;
				case 2:
					Unit = "MN";    // mega		10^6	Million
					break;
				case 3:
					Unit = "GN";    // giga		10^9	Billion
					break;
				case 4:
					Unit = "TN";    // tera		10^12	Trillion
					break;
				case 5:
					Unit = "PN";    // peta		10^15	Quadrillion
					break;
				case 6:
					Unit = "EN";    // exa		10^18	Quintillion
					break;
				case 7:
					Unit = "ZN";    // zetta	10^21	Sextillion
					break;
				case 8:
					Unit = "YN";    // yotta	10^24	Septillion, Yo mama so fat...
					break;
				default:
					Unit = "N"; Rendered = RequiredNewtons.ToString("E3"); // Oh f*ck me thats big, or small...
					break;
			}

			return "Set MaxImpulseAxis to " + Rendered + Unit + '\n';
		}
	}
}
