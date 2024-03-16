using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GunIO
{
    public partial class Events
    {
        internal static Dictionary<string, byte> buttonStates = new Dictionary<string, byte>
        {
            { "left", 1},
            { "right", 1},
            { "up", 1},
            { "down", 1},
            { "enter", 1},
            { "exit", 1},
            { "in11", 1},
            { "in12", 1},
            { "in21", 1},
            { "in22", 1},
            { "in23", 1},
            { "in24", 1},
            { "in25", 1},
            { "in26", 1},
            { "in27", 1},
        };

    }
}
