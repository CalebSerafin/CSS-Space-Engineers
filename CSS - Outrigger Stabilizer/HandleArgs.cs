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
		private void HandleArgs(string Argument) {
			List<string> Args = Argument.Split(' ').ToList();

			if (Args.Contains("Reverse", StringComparer.OrdinalIgnoreCase)) {
				OutRiggerRelease();
				Piston.Enabled = true;
				Piston.Reverse();
			} else
			if (Args.Contains("Halt", StringComparer.OrdinalIgnoreCase)) {
				Piston.Enabled = false;
				OutRiggerSecure();
			} else
			if (Args.Contains("Continue", StringComparer.OrdinalIgnoreCase)) {
				OutRiggerRelease();
				Piston.Enabled = true;
			} else
			if (Args.Contains("Halt/Continue", StringComparer.OrdinalIgnoreCase)) {
				if (Piston.Enabled) {
					Piston.Enabled = false;
					OutRiggerSecure();
				} else {
					OutRiggerRelease();
					Piston.Enabled = true;
				}
			} else
			if (Args.Contains("Klang", StringComparer.OrdinalIgnoreCase)) {         // When shit happens.
				Piston.Enabled = false;
				OutRiggerRelease();
				Foot.Enabled = true;
				Foot.AutoLock = false;
				Foot.Unlock();
				Runtime.UpdateFrequency = UpdateFrequency.None;
				throw new KlangSafetyException("User");
			}
		}
	}
}
