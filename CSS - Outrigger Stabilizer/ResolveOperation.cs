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

			//Parameter Checking
			if (GroundClearance < 0.0f) {
				Warnings.Add("GroundClearance should be set to correct distance in metres, otherwise possible ship bouncing.");
			}
			if ( Piston == null ) {
				Operation = Operations.Broken;
				throw new BlockNullReferenceException("Piston");
			}
			if ( Cockpit == null ) {
				Operation = Operations.Broken;
				throw new BlockNullReferenceException("Cockpit");
			}
			if ( Foot == null ) {
				Operation = Operations.Broken;
				throw new BlockNullReferenceException("Foot");
			}

			double PistonCurrentPosition = PistonGetCurrentPosition(Piston);

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
				if (	
					( Piston.Velocity < 0 && ExtendMatch && PistonCurrentPosition <= GroundClearance && Foot.IsLocked ) || 
					( Cockpit.GetShipSpeed() < Math.Abs(Piston.Velocity) * MoveDectect && Foot.IsLocked ) //In-case GroundClearance value is incorrect
				) {
					Operation = Operations.StabilizeCorrecting;
				} else if (Piston.Status == PistonStatus.Stopped || Piston.Status == OutrigStatus.Retracted) {
					Operation = Operations.Retracted;
				}  else {
					Operation = Operations.Retracting;
				};
			};
		}
	}
}
