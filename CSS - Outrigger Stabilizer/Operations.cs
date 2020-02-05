namespace IngameScript {
	partial class Program {
		private static class Operations {
			public const int Unknown = -1;

			public const int Extending = 0;
			public const int Retracting = 1;
			public const int Stabilizing = 2;
			public const int StabilizeCorrecting = 3;


			public const int Extended = 10;
			public const int Retracted = 11;
			public const int Stabilized = 12;

			public const int Broken = 20;

			public static string Text(int OperationInt) {
				switch (OperationInt) {
					case 0: return "Extending";
					case 1: return "Retracting";
					case 2: return "Stabilizing";
					case 3: return "StabilizeCorrecting";

					case 10: return "Extended";
					case 11: return "Retracted";
					case 12: return "Stabilized";

					case 20: return "Broken";

					default: return "Unknown";
				};
			}

			public static string[] ToArray() {
				return new string[] {
					"Extending",
					"Retracting",
					"Stabilizing",
					"StabilizeCorrecting",

					"Extended",
					"Retracted",
					"Stabilized",

					"Broken",

					"Unknown"
				};
			}

			public static int ToArrayIndex(int Operation) {
				return System.Array.IndexOf<string>(ToArray(), Text(Operation));
			}
		}
	}
}
