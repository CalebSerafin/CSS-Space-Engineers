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
		private static void ResolveOperation() {
			if (Piston == null || Cockpit == null || Foot == null) {
				Operation = Operations.Broken;
				return;
			}
			if (Piston.Velocity == 0 || !Piston.Enabled) {
				if (Piston.Status == OutrigStatus.Extended) {
					Operation = Operations.Extended;
				} else if (Piston.Status == OutrigStatus.Retracted) {
					Operation = Operations.Retracted;
				} else {
					Operation = Operations.Stabilized;
				};
			} else if (Piston.Velocity > 0 && ExtendMatch) {
				if (Piston.Status == PistonStatus.Stopped || Piston.Status == OutrigStatus.Extended) {
					Operation = Operations.Extended;
				} else if (Foot.IsLocked) {
					Operation = Operations.Extending;
				} else {
					Operation = Operations.Stabilizing;
				};
			} else {
				if (Piston.Status == PistonStatus.Stopped || Piston.Status == OutrigStatus.Retracted) {
					Operation = Operations.Retracted;
				} else if (Cockpit.GetShipSpeed() < Math.Abs(Piston.Velocity) * MoveDectect && Foot.IsLocked) {
					Operation = Operations.StabilizeCorrecting;
				} else {
					Operation = Operations.Retracting;
				};
			};
		}
	}
}
