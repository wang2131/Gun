using System;

namespace GunIO
{
    public partial class Events
    {
        private static object sender = new object();

        public static EventHandler up_ButtonPressed;
        public static EventHandler down_ButtonPressed;
        public static EventHandler left_ButtonPressed;
        public static EventHandler right_ButtonPressed;
        public static EventHandler exit_ButtonPressed;
        public static EventHandler enter_ButtonPressed;
        public static EventHandler in11_ButtonPressed;
        public static EventHandler in12_ButtonPressed;
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
        public static EventHandler in11_ButtonReleased;
        public static EventHandler in12_ButtonReleased;
        public static EventHandler in21_ButtonReleased;
        public static EventHandler in22_ButtonReleased;
        public static EventHandler in23_ButtonReleased;
        public static EventHandler in24_ButtonReleased;
        public static EventHandler in25_ButtonReleased;
        public static EventHandler in26_ButtonReleased;
        public static EventHandler in27_ButtonReleased;

        public static EventHandler<ButtonLightEventArgs> buttonLightChanged;
        public static EventHandler<RGBLightEventArgs> rgbLightChanged;
        public static EventHandler<ShakingEventArgs> shaking;
        public static EventHandler<PotentiometerEventArgs> potentiometer;
        public static EventHandler<LookUpTableEventArgs> lookUpTable;


        internal static void Handle00(byte[] data)
        {
            if (data.Length != 1)
            {
                Port._SendCustomDebug("数据传输错误， cmd00");
                return;
            }

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
        }
        internal static void Handle01(byte[] data)
        {

            if (data.Length != 2)
            {
                Port._SendCustomDebug("数据传输错误， cmd01");
                return;
            }

            string str1 = "", str2 = "";
            for (int i = 7; i >= 0; i--)
            {
                str1 += ((int)ByteToBit(data[0], i)).ToString();
                str2 += ((int)ByteToBit(data[1], i)).ToString();

            }

            //******************按键事件******************************************
            byte in11 = ByteToBit(data[1], 0);
            if (in11 != buttonStates["in11"])
            {
                if (in11 == 0x0) in11_ButtonPressed.Invoke(sender, EventArgs.Empty);
                else in11_ButtonReleased.Invoke(sender, EventArgs.Empty);

                buttonStates["in11"] = in11;
            }

            byte in12 = ByteToBit(data[1], 1);
            if (in12 != buttonStates["in12"])
            {
                if (in12 == 0x0) in12_ButtonPressed.Invoke(sender, EventArgs.Empty);
                else in12_ButtonReleased.Invoke(sender, EventArgs.Empty);

                buttonStates["in12"] = in12;
            }

            byte in21 = ByteToBit(data[0], 2);
            if (in21 != buttonStates["in21"])
            {
                if (in21 == 0x0) in21_ButtonPressed.Invoke(sender, EventArgs.Empty);
                else in21_ButtonReleased.Invoke(sender, EventArgs.Empty);

                buttonStates["in21"] = in21;
            }

            byte in22 = ByteToBit(data[0], 3);
            if (in22 != buttonStates["in22"])
            {
                if (in22 == 0x0) in22_ButtonPressed.Invoke(sender, EventArgs.Empty);
                else in22_ButtonReleased.Invoke(sender, EventArgs.Empty);

                buttonStates["in22"] = in22;
            }

            byte in23 = ByteToBit(data[0], 4);
            if (in23 != buttonStates["in23"])
            {
                if (in23 == 0x0) in23_ButtonPressed.Invoke(sender, EventArgs.Empty);
                else in23_ButtonReleased.Invoke(sender, EventArgs.Empty);

                buttonStates["in23"] = in23;
            }

            byte in24 = ByteToBit(data[0], 5);
            if (in24 != buttonStates["in24"])
            {
                if (in24 == 0x0) in24_ButtonPressed.Invoke(sender, EventArgs.Empty);
                else in24_ButtonReleased.Invoke(sender, EventArgs.Empty);

                buttonStates["in24"] = in24;
            }

            byte in25 = ByteToBit(data[0], 6);
            if (in25 != buttonStates["in25"])
            {
                if (in25 == 0x0) in25_ButtonPressed.Invoke(sender, EventArgs.Empty);
                else in25_ButtonReleased.Invoke(sender, EventArgs.Empty);

                buttonStates["in25"] = in25;
            }

            byte in26 = ByteToBit(data[0], 0);
            if (in26 != buttonStates["in26"])
            {
                if (in26 == 0x0) in26_ButtonPressed.Invoke(sender, EventArgs.Empty);
                else in26_ButtonReleased.Invoke(sender, EventArgs.Empty);

                buttonStates["in26"] = in26;
            }

            byte in27 = ByteToBit(data[0], 1);
            if (in27 != buttonStates["in27"])
            {
                if (in27 == 0x0) in27_ButtonPressed.Invoke(sender, EventArgs.Empty);
                else in27_ButtonReleased.Invoke(sender, EventArgs.Empty);

                buttonStates["in27"] = in27;
            }

            byte left = ByteToBit(data[1], 3);
            if (left != buttonStates["left"])
            {
                if (left == 0x0) left_ButtonPressed.Invoke(sender, EventArgs.Empty);
                else left_ButtonReleased.Invoke(sender, EventArgs.Empty);

                buttonStates["left"] = left;
            }

            byte right = ByteToBit(data[1], 2);
            if (right != buttonStates["right"])
            {
                if (right == 0x0) right_ButtonPressed.Invoke(sender, EventArgs.Empty);
                else right_ButtonReleased.Invoke(sender, EventArgs.Empty);

                buttonStates["right"] = right;
            }

            byte up = ByteToBit(data[1], 5);
            if (up != buttonStates["up"])
            {
                if (up == 0x0) up_ButtonPressed.Invoke(sender, EventArgs.Empty);
                else up_ButtonReleased.Invoke(sender, EventArgs.Empty);

                buttonStates["up"] = up;
            }

            byte down = ByteToBit(data[1], 0);
            if (down != buttonStates["down"])
            {
                if (down == 0x0) down_ButtonPressed.Invoke(sender, EventArgs.Empty);
                else down_ButtonReleased.Invoke(sender, EventArgs.Empty);

                buttonStates["down"] = down;
            }

            byte enter = ByteToBit(data[0], 7);
            if (enter != buttonStates["enter"])
            {
                if (enter == 0x0) enter_ButtonPressed.Invoke(sender, EventArgs.Empty);
                else enter_ButtonReleased.Invoke(sender, EventArgs.Empty);

                buttonStates["enter"] = enter;
            }

            byte exit = ByteToBit(data[1], 4);
            if (exit != buttonStates["exit"])
            {
                if (exit == 0x0) exit_ButtonPressed.Invoke(sender, EventArgs.Empty);
                else exit_ButtonReleased.Invoke(sender, EventArgs.Empty);

                buttonStates["exit"] = exit;
            }
            //********************************************************************
            Port._SendCustomDebug("按键：" + str1 + " " + str2);


        }

        internal static void Handle02(byte[] data)
        {
            if (data.Length != 1)
            {
                Port._SendCustomDebug("数据传输错误， cmd02");
                return;
            }

            byte light0 = ByteToBit(data[0], 7);
            byte light1 = ByteToBit(data[1], 6);
            byte light2 = ByteToBit(data[1], 5);
            byte light3 = ByteToBit(data[1], 4);
            byte light4 = ByteToBit(data[1], 3);
            byte light5 = ByteToBit(data[1], 2);
            byte light6 = ByteToBit(data[1], 1);
            byte light7 = ByteToBit(data[1], 0);

            buttonLightChanged.Invoke(sender, new ButtonLightEventArgs
            {
                light0 = light0,
                light1 = light1,
                light2 = light2,
                light3 = light3,
                light4 = light4,
                light5 = light5,
                light6 = light6,
                light7 = light7,
            });

        }

        internal static void Handle03(byte[] data)
        {
            if (data.Length != 4)
            {
                Port._SendCustomDebug("数据传输错误， cmd03");
                return;
            }

            byte id = data[0];
            byte R = data[1];
            byte G = data[2];
            byte B = data[3];

            rgbLightChanged.Invoke(sender, new RGBLightEventArgs
            {
                id = id,
                R = R,
                G = G,
                B = B
            });
        }

        internal static void Handle05(byte[] data)
        {
            if (data.Length != 1)
            {
                Port._SendCustomDebug("数据传输错误， cmd05");
                return;
            }

            byte states = data[0];

            shaking.Invoke(sender, new ShakingEventArgs
            {
                states = states
            });
        }

        internal static void Handle06(byte[] data)
        {
            if (data.Length != 5)
            {
                Port._SendCustomDebug("数据传输错误， cmd06");
                return;
            }

            byte player = data[0];
            byte[] values = new byte[]{
                data[1], data[2], data[3], data[4]
            };

            potentiometer.Invoke(sender, new PotentiometerEventArgs
            {
                player = player,
                values = values
            });
        }

        internal static void Handle07(byte[] data)
        {
            if (data.Length != 2)
            {
                Port._SendCustomDebug("数据传输错误， cmd07");
                return;
            }

            lookUpTable.Invoke(sender, new LookUpTableEventArgs
            {
                values = data
            });
        }

        private static byte ByteToBit(byte b, int bit)
        {
            return (byte)((b >> bit) & 0x1);
        }
    }
}
