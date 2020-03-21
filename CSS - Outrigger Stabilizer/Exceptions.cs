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
    class BlockNullReferenceException : Exception {
        public BlockNullReferenceException() {

        }

        public BlockNullReferenceException(string MissingBlock)
            : base("The script couldn't find " + MissingBlock + " anywhere on the ship.\nTry recompiling if the problem was corrected.\n") {
        }

        public BlockNullReferenceException(string message, Exception inner)
        : base(message, inner) {
        }
    }
    class KlangSafetyException : Exception {
        public KlangSafetyException() {

        }

        public KlangSafetyException(string Source)
            : base(Source + " has triggered Klang safety mode.\nAll landing gears are released and program execution is suspended.\n") {
        }

        public KlangSafetyException(string message, Exception inner)
        : base(message, inner) {
        }
    }
}
