using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GunIO.src.Events
{
    public partial class ButtonEvent
    {
        Dictionary<string, bool> buttonStates = new Dictionary<string, bool>
        {
            { "left", false},
            { "right", false},
            { "up", false},
            { "down", false},
        };
    }
}
