using Sandbox.ModAPI.Ingame;

namespace IngameScript {
	partial class Program {
		private static class OutrigStatus {
			public const PistonStatus Extended = ExtendMatch ? PistonStatus.Extended : PistonStatus.Retracted;
			public const PistonStatus Extending = ExtendMatch ? PistonStatus.Extending : PistonStatus.Retracting;
			public const PistonStatus Retracted = ExtendMatch ? PistonStatus.Retracted : PistonStatus.Extended;
			public const PistonStatus Retracting = ExtendMatch ? PistonStatus.Retracting : PistonStatus.Extending;
			public const PistonStatus Stopped = PistonStatus.Stopped;
		}
	}
}
