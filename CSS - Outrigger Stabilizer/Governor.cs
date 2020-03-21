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
		private static void Governor() {
			if (Operation == Operations.Broken) {
				return;
			}
			if (Operation == Operations.Stabilizing) {			// Ramming outrigger into ground
				Foot.Enabled = true;
				Foot.Lock();
				Foot.AutoLock = true;
			}
			if (Operation == Operations.StabilizeCorrecting) {  // Elastic Wheel Compression
				Foot.Enabled = true;
				Foot.AutoLock = false;
				Foot.Unlock();
			}
			if (Operations.IsMoving(Operation) && Lock.IsLocked) {
				Lock.Enabled = true;                            // Klang-tom Forces (Piston Force on Self Locked Landing Gear)
				Lock.AutoLock = false;
				Lock.Unlock();
			}
			if (Operation != Operations.Retracted) {            // Driving or movement confusing ResolveOperation
				Cockpit.HandBrake = true;
			}
			if (Operation == Operations.Retracted) {            // Attaching to random items in transit
				Foot.Enabled = true;
				Foot.AutoLock = false;
				Foot.Unlock();
			}
			if ((Cockpit.GetShipSpeed() > (Math.Abs(Piston.Velocity) * MoveDectect) && Operation != Operations.Retracted) || Operation == Operations.Extended) {  // Falling over while extended
				Foot.Enabled = true;
				Foot.Lock();
				Foot.AutoLock = true;
			}
		}
	}
}
