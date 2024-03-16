using System;
using System.Collections.Generic;
using System.Text;

namespace GunIO.src.Events
{
    public partial class ButtonEvents
    {
        public static EventHandler up_ButtonPressed;
        public static EventHandler down_ButtonPressed;
        public static EventHandler left_ButtonPressed;
        public static EventHandler right_ButtonPressed;
        public static EventHandler exit_ButtonPressed;
        public static EventHandler enter_ButtonPressed;
        public static EventHandler in21_ButtonPressed;
        public static EventHandler in22_ButtonPressed;
        public static EventHandler in23_ButtonPressed;
        public static EventHandler in24_ButtonPressed;
        public static EventHandler in25_ButtonPressed;
        public static EventHandler in26_ButtonPressed;
        public static EventHandler in27_ButtonPressed;

        public static EventHandler up_ButtonReleased;
        public static EventHandler down_ButtonReleased;
        public static EventHandler left_ButtonReleased;
        public static EventHandler right_ButtonReleased;
        public static EventHandler exit_ButtonReleased;
        public static EventHandler enter_ButtonReleased;
        public static EventHandler in21_ButtonReleased;
        public static EventHandler in22_ButtonReleased;
        public static EventHandler in23_ButtonReleased;
        public static EventHandler in24_ButtonReleased;
        public static EventHandler in25_ButtonReleased;
        public static EventHandler in26_ButtonReleased;
        public static EventHandler in27_ButtonReleased;

        internal static void Handle(byte cmd, byte[] data)
        {
            switch (cmd)
            {
                case 0:
                    if (data[0] == 0x00)
                    {
                        Port._SendCustomDebug("开机检查中");
                    }
                    else if (data[0] == 0x01)
                    { 
                    }
                    else if (data[0] == 0x03)
                    {
                        Port._SendCustomDebug("机械故障");
                    }
                    break;

                case 1:
                    string str1 = "", str2 = "";
                    for(int i = 0; i < 8; i++)
                    {
                        str1 += ((int)ByteToBit(data[0], i)).ToString();
                        str2 += ((int)ByteToBit(data[1], i)).ToString();
                        
                    }
                    Port._SendCustomDebug("按键：" + str1 + " " + str2);
                    break;

                case 2:
                    string light = "";
                    for (int i = 0; i < 8; i++)
                    {
                        light += ((int)ByteToBit(data[0], i)).ToString();
                        
                    }
                    Port._SendCustomDebug("按键灯： " + light);
                    break;

                case 3:

                    break;

            }
        }

        private static byte ByteToBit(byte b, int bit)
        {
            return (byte)((b << bit) & 0x1);
        }
    }
}
