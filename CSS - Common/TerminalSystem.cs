using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CSS__Common {
	static class TerminalSystem {
		/// <summary>
		/// Checks if block can be type.
		/// </summary>
		/// <param name="TypeIdString"></param>
		/// <returns></returns>
		public static Func<IMyTerminalBlock, bool> IsType<Type>() {
			return (Block) => Block is Type;
		}
		/// <summary>
		/// Checks if the part of CustomName matches description. Case Insensitive.
		/// </summary>
		/// <param name="CustNameString">String to find in custom name.</param>
		/// <returns></returns>
		public static Func<IMyTerminalBlock, bool> IsPartName(string CustNameString) {
			return (Block) => Block.CustomName.ToLower().Contains(CustNameString.ToLower());
		}
		/// <summary>
		/// Checks if the whole CustomName matches description.
		/// </summary>
		/// <param name="CustNameString">String to match custom name.</param>
		/// <param name="ComparisonType">String comparison type.</param>
		/// <param name="TrimChars">Chars to trim off both before comparison.</param>
		/// <returns></returns>
		public static Func<IMyTerminalBlock, bool> IsName(string CustNameString, StringComparison ComparisonType = StringComparison.OrdinalIgnoreCase, char[] TrimChars = null) {
				return (Block) => Block.CustomName.Trim(TrimChars ?? Array.Empty<char>()).Equals(CustNameString.Trim(TrimChars), ComparisonType);
		}

		/// <summary>
		/// Shortcut to get blocks
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="MyMyGridProgram"></param>
		/// <param name="Blocks">Output blocks to list</param>
		/// <param name="Filter">Return true for matching blocks</param>
		public static List<T> GetBlocks<T>(this MyGridProgram MyMyGridProgram, List<T> Blocks, Func<IMyTerminalBlock, bool> Filter = null) where T : class, IMyTerminalBlock {
			MyMyGridProgram.GridTerminalSystem.GetBlocksOfType(Blocks, Filter);
			return Blocks;
		}
	}
}
