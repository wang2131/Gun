﻿using System;
using System.Collections.Generic;
using System.Text;

namespace GunIO
{
    public class GunIOEventArgs : EventArgs
    {
        public byte cmd;
        public byte[] data;
    }

    public class CustomLogEventArgs : EventArgs
    {
        public string message;
    }

   
}
