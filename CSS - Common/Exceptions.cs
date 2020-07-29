using System;
using System.Collections.Generic;
using System.Text;

namespace CSS_Common {
	class BlockNullReferenceException : Exception {
		public BlockNullReferenceException() {

		}

		public BlockNullReferenceException(string MissingBlock)
			: base(
				  "The script couldn't find " + MissingBlock + " anywhere on the ship." + System.Environment.NewLine + 
				  "Try recompiling if the problem was corrected." + System.Environment.NewLine) {
		}

		public BlockNullReferenceException(string message, Exception inner)
			: base(message, inner) {
		}
	}
}
