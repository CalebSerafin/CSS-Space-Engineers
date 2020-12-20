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
using CSS_Common;
using System.Reflection;
using BulletXNA;

namespace IngameScript {
	partial class Program : MyGridProgram {
		//#pragma warning disable IDE0044 // Add readonly modifier // The IDE cannot detect that these are modified at runtime by a reference. MS pls Fix ❤
		IMyTextSurfaceProvider ConsoleProvider;
		IMyTextSurface Console;
		IMyCockpit Cockpit;
		IMyCameraBlock Camera;
		MyDetectedEntityInfo LastTargetData;
		Queue<string> ArgBuffer = new Queue<string>();
		Dictionary<string, string> ConsoleBuffers = new Dictionary<string, string>();
		UpdateEventHandlers UpdateEvents = new UpdateEventHandlers();
		//#pragma warning restore IDE0044 // Add readonly modifier

		void PopHKHistory(object Sender, UpdateEventArgs e) {
			if (ArgBuffer.Count > 0) ConsoleBuffers["HKHistory"] = e.updateSource.ToString() + ArgBuffer.Dequeue();
		}


		void AddArgToBuffer(object Sender, UpdateEventArgs e) {
			ArgBuffer.Enqueue(e.argument);
			ConsoleBuffers["AddArgToBuffer"] = e.argument;
		}
		void Pinger(object Sender, UpdateEventArgs e) {
			Echo("Ping!: " + e.argument + ", " + e.updateSource.ToString());
		}
		void GetTargetData(object Sender, UpdateEventArgs e) {
			if (!e.argument.Equals("HK:1:1")) return;
			if (!Camera.CanScan(4000)) return;
			LastTargetData = Camera.Raycast(4000);
		}
		double ProjectileGroundInterceptTime(double HeightAboveTarget, Vector3D MeVelocity, Vector3D MeGravity) {
			double S, U, A, T; // Equations of motion in SUVAT notation.
			S = HeightAboveTarget;
			A = MeGravity.Length();
			U = MeVelocity.Dot(Vector3D.Normalize(MeGravity)) / A; //Extract down vector

			// S = U * T + 0.5 * A T*T    // T = (-U +Math.Sqrt( U*U -4*(0.5*A)*(-S) ))/(2*0.5*A)
			T = (-U + Math.Sqrt(U * U - 2 * A * S)) / A;
			return T;
		}
		double ProjectileLateralTravelDistance(double TimeIntercept, Vector3D GroundPosToTarget, Vector3D MeVelocity) {
			//ConsoleBuffers["LastTargetData"] = $"LastTargetData: {(LastTargetData.HitPosition ?? MeGroundPos).ToString("F0")}";
			//ConsoleBuffers["MeGroundPos"] = $"MeGroundPos: {MeGroundPos.ToString("F0")}";
			//ConsoleBuffers["ToTarget"] = $"ToTarget: {ToTarget.ToString("F0")}";

			double S, U, T; // Equations of motion in SUVAT notation.
			U = MeVelocity.Dot(Vector3D.Normalize(GroundPosToTarget));
			T = TimeIntercept;
			S = U * T;
			return S;
		}
		void UpdateReletiveTargetData(object Sender, UpdateEventArgs e) { // Only works in atmosphere
			Vector3D PlanetPos;
			Cockpit.TryGetPlanetPosition(out PlanetPos);
			Vector3D MePos = Me.CubeGrid.GetPosition();
			Vector3D TargetPos = LastTargetData.HitPosition ?? MePos;
			Vector3D MeVelocity = Cockpit.GetShipVelocities().LinearVelocity;
			Vector3D MeGravity = Cockpit.GetNaturalGravity();
			double Distance = Vector3D.Distance(TargetPos, MePos);
			double MeHeight = Vector3D.Distance(MePos, PlanetPos);
			double TargetHeight = Vector3D.Distance(TargetPos, PlanetPos);
			double RelativeTargetHeight = TargetHeight - MeHeight; // Should be negative when aircraft is above

			Vector3D RelativePlanetPos = PlanetPos - MePos;
			Vector3D MeRelativeGroundPos = RelativePlanetPos * (-RelativeTargetHeight / MeHeight);
			Vector3D MeGroundPos = MePos + MeRelativeGroundPos;
			Vector3D GroundPosToTarget = TargetPos - MeGroundPos;

			double S, U, V, A, T; // Equations of motion in SUVAT notation.
			T = ProjectileGroundInterceptTime(RelativeTargetHeight, Cockpit.GetShipVelocities().LinearVelocity, Cockpit.GetNaturalGravity());
			S = ProjectileLateralTravelDistance(T, GroundPosToTarget, Cockpit.GetShipVelocities().LinearVelocity);
			double GroundDistanceToDrop = Vector3D.Distance(MeGroundPos, TargetPos) - S;
			double TimeUntilDrop = GroundDistanceToDrop / Cockpit.GetShipVelocities().LinearVelocity.Dot(Vector3D.Normalize(GroundPosToTarget));

			Vector3D RelativePlanetPosFromTarget = PlanetPos - MePos;

			ConsoleBuffers["GroundIntercept"] = $"GroundIntercept: {T:F0}s";
			ConsoleBuffers["LateralGlideDistance"] = $"LateralGlideDistance: {S:F0}m";
			ConsoleBuffers["GroundDistanceUntillDrop"] = $"GroundDistanceUntillDrop: {GroundDistanceToDrop:F0}m";
			ConsoleBuffers["TimeUntilDrop"] = $"TimeUntilDrop: {TimeUntilDrop:F0}s";
		}

		public Program() {
			Runtime.UpdateFrequency = UpdateFrequency.Once;

			TerminalSystem.GetBlockWithName(this, "SBS-Console", ref ConsoleProvider);
			TerminalSystem.GetBlockWithName(this, "SBS2 Camera Nose", ref Camera);
			TerminalSystem.GetBlockWithName(this, "SBS2 Fighter Cockpit ", ref Cockpit);
			Console = ConsoleProvider.GetSurface(0);
			Console.ContentType = ContentType.TEXT_AND_IMAGE;
			Console.Alignment = TextAlignment.CENTER;
			ConsoleBuffers.Clear();

			Camera.EnableRaycast = true;

			UpdateType UpdatesWithArgs =
				UpdateType.Terminal |
				UpdateType.Trigger |
				UpdateType.Script |
				UpdateType.IGC;
			UpdateType UpdatesTick =
				UpdateType.Update1 |
				UpdateType.Update10 |
				UpdateType.Update100;
			UpdateEvents.Subscribers[AddArgToBuffer] = new EventHandlerMeta(UpdatesWithArgs, "AddArgToBuffer");
			UpdateEvents.Subscribers[DumpData] = new EventHandlerMeta(UpdatesWithArgs, "DumpData");
			UpdateEvents.Subscribers[GetTargetData] = new EventHandlerMeta(UpdatesWithArgs, "GetTargetData");
			UpdateEvents.Subscribers[UpdateReletiveTargetData] = new EventHandlerMeta(UpdatesTick, "UpdateReletiveTargetData");
		}

		public void Save() {
		}

		public void Main(string argument, UpdateType updateSource) {
			Runtime.UpdateFrequency = UpdateFrequency.Update1;
			UpdateEvents.CallUpdateEventHandlers(argument, updateSource);
			Console.WriteText(
				ConsoleBuffers.Aggregate("", (TextBuffer, Pair) => TextBuffer + Pair.Value + Environment.NewLine)
			);
		}
	}
}
