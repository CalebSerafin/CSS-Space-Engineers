using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CSS_Common {
	static class TerminalSystem {
		/// <summary>
		/// Checks if block can be type.
		/// </summary>
		/// <param name="TypeIdString"></param>
		/// <returns>Delegate type checker</returns>
		public static bool IsType<Type>(IMyTerminalBlock Block) {
			return Block is Type;
		}
		/// <summary>
		/// Checks if Name is in the block's CustomName. Case Insensitive.
		/// </summary>
		/// <param name="CustNameString">String to find in custom name.</param>
		/// <returns>Delegate partial name checker</returns>
		public static Func<IMyTerminalBlock, bool> IsPartName(string Name) {
			return (Block) => Block.CustomName.ToLower().Contains(Name.ToLower());
		}
		/// <summary>
		/// Checks if the whole CustomName matches description.
		/// </summary>
		/// <param name="CustNameString">String to match custom name.</param>
		/// <param name="ComparisonType">String comparison type.</param>
		/// <param name="TrimChars">Chars to trim off both before comparison.</param>
		/// <returns>Delegate name checker</returns>
		public static Func<IMyTerminalBlock, bool> IsName(string CustNameString, StringComparison ComparisonType = StringComparison.OrdinalIgnoreCase, char[] TrimChars = null) {
				return (Block) => Block.CustomName.Trim(TrimChars ?? Array.Empty<char>()).Equals(CustNameString.Trim(TrimChars), ComparisonType);
		}

		/// <summary>
		/// Shortcut to get blocks
		/// </summary>
		/// <typeparam name="T">Any IMyTerminalBlock</typeparam>
		/// <param name="MyMyGridProgram"></param>
		/// <param name="Blocks">Outputs found blocks to list</param>
		/// <param name="Filter">Return true for matching blocks</param>
		/// <returns>Matching blocks</returns>
		public static List<T> GetBlocksByCustom<T>(this MyGridProgram MyMyGridProgram, ref List<T> Blocks, Func<IMyTerminalBlock, bool> Filter = null) where T : class, IMyTerminalBlock {
			MyMyGridProgram.GridTerminalSystem.GetBlocksOfType(Blocks, Filter);
			return Blocks;
		}
		/// <summary>
		/// Shortcut to get blocks.
		/// </summary>
		/// <typeparam name="T">Any IMyTerminalBlock</typeparam>
		/// <param name="MyMyGridProgram"></param>
		/// <param name="Name">Custom name to matching blocks</param>
		/// <param name="Blocks">Outputs found blocks to list</param>
		/// <param name="Filter">Return true for matching blocks</param>
		/// <returns>Partial Case-Insensitive Matching blocks</returns>
		public static List<T> GetBlocksByName<T>(this MyGridProgram MyMyGridProgram, string Name, ref List<T> Blocks, Func<IMyTerminalBlock, bool> Filter = null) where T : class {
			List<IMyTerminalBlock> BlocksUnknownType = new List<IMyTerminalBlock>();
			MyMyGridProgram.GridTerminalSystem.SearchBlocksOfName(Name, BlocksUnknownType, Filter);
			Blocks = BlocksUnknownType.Where(IsType<T>).Cast<T>().ToList();
			return Blocks;
		}

		/// <summary>
		/// Will return the first block that matches name or throw an exception if not found.
		/// </summary>
		/// <typeparam name="T">Any IMyTerminalBlock</typeparam>
		/// <param name="MyMyGridProgram"></param>
		/// <param name="Name"></param>
		/// <param name="Block"></param>
		/// <param name="Filter"></param>
		/// <returns>Partial Case-Insensitive Matching block</returns>
		/// <exception cref="BlockNullReferenceException"></exception>
		public static T GetBlockWithName<T>(this MyGridProgram MyMyGridProgram, string Name, ref T Block, Func<IMyTerminalBlock, bool> Filter = null) where T : class {
			List<IMyTerminalBlock> BlocksUnknownType = new List<IMyTerminalBlock>();
			MyMyGridProgram.GridTerminalSystem.SearchBlocksOfName(Name, BlocksUnknownType, Filter);
			Block = (T)BlocksUnknownType.FirstOrDefault(IsType<T>);
			if (Block == null) throw new BlockNullReferenceException(Name);
			return Block;
		}
	}
}
