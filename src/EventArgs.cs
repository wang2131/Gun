using System;

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

    public class ButtonLightEventArgs : EventArgs
    {
        public byte light0;
        public byte light1;
        public byte light2;
        public byte light3;
        public byte light4;
        public byte light5;
        public byte light6;
        public byte light7;
    }

    public class RGBLightEventArgs : EventArgs
    {
        public byte id;
        public byte R;
        public byte G;
        public byte B;
    }

    public class ShakingEventArgs : EventArgs
    {
        public byte states;
        
    }


    public class PotentiometerEventArgs : EventArgs
    {
        public byte player;
        public byte[] values;
    }

    public class LookUpTableEventArgs : EventArgs
    {
        public byte[] values;
    }
    public enum State
    {
        off, on
    }
}
