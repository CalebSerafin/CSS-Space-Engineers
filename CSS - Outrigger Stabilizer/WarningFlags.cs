namespace IngameScript {
	partial class Program {
		private static class WarningFlags {
			public const int Unknown = -1;

			public const int Extend = 0;
			public const int Retract = 1;
			public const int Stabilize = 2;

			public static string Text(int OperationInt) {
				switch (OperationInt) {
					case 0: return "Extend";
					case 1: return "Retract";
					case 2: return "Stabilize";

					default: return "Unknown";
				};
			}

			public static string[] ToArray() {
				return new string[] {
					"Extend",
					"Retract",
					"Stabilize",

					"Unknown"
				};
			}

			public static int ToArrayIndex(int Operation) {
				return System.Array.IndexOf<string>(ToArray(), Text(Operation));
			}
		}
	}
}
