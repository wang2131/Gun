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
        public byte player;
        public State state;
    }

    public class RGBLightEventArgs : EventArgs
    {
        public byte player;
        public byte R;
        public byte G;
        public byte B;
    }

    public class ShakingEventArgs : EventArgs
    {
        public byte player;
        public State state;
    }

    public enum State
    {
        off, on
    }
}
