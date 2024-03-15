using System.IO.Ports;
using System;
using System.Collections.Generic;

namespace GunIO
{
    public class Port
    {

        public static SerialPort SerialPort;
        public static List<byte> bufferList;
        public static List<byte> dataList;
        public static event EventHandler<GunIOEventArgs> DataReceived;
        public static event EventHandler<UnityMessageEventArgs> UnityMessageReceived;

        public static SerialPort OpenPort(string comName, int baud)
        {
            bufferList = new List<byte>();
            dataList = new List<byte>();
            if (SerialPort == null || !SerialPort.IsOpen)
            {
                
                SerialPort = new SerialPort();
                SerialPort.PortName = comName;
                SerialPort.BaudRate = baud;
                SerialPort.DataBits = 8;
                SerialPort.Parity = Parity.None;
                SerialPort.StopBits = StopBits.One;
                SerialPort.RtsEnable = true;
                SerialPort.ReadTimeout = 1000;
                SerialPort.ReceivedBytesThreshold = 1;
                SerialPort.DataReceived += new SerialDataReceivedEventHandler(ReceiveData);
                SerialPort.ErrorReceived += (sender, e) => { SendUnityDebug(e.ToString()); };
                SerialPort.Open();
                
                
                if (!SerialPort.IsOpen)
                {
                    SendUnityDebug("串口打开失败");
                }
                else
                {
                    SendUnityDebug("串口打开成功");
                }
            }
            return SerialPort;
        }

        public static void ClosePort()
        {
            if (SerialPort != null && SerialPort.IsOpen)
            {
                SerialPort.Close();
                SerialPort.Dispose();
                SendUnityDebug("串口已关闭");
            }
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="data"></param>
        public static void SendData(byte cmd, byte[] data)
        {
            if (SerialPort != null && SerialPort.IsOpen)
            {
                List<byte> list = new List<byte>();
                byte length = (byte)(data.Length + 2);
                list.Add(length);
                list.Add(cmd);
                list.AddRange(data);
                byte checkSum = CalculateSum(list.ToArray());
                list.Add(checkSum);
                list.Insert(0, 0xAA);
                list.Add(0xDD);
                SendUnityDebug("SendData:" + BitConverter.ToString(list.ToArray()));
                SerialPort.Write(list.ToArray(), 0, list.Count);
                
            }
            else
            {
                SendUnityDebug("串口未找到");
            }
        }

        /// <summary>
        /// 接收函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ReceiveData(object sender, SerialDataReceivedEventArgs e)
        {
            
            SendUnityDebug("收到数据");
            SerialPort serialPort = (SerialPort)sender;
            SendUnityDebug("sender：" + serialPort.PortName);
            bufferList.Clear();
            dataList.Clear();
            int _bytesToRead = serialPort.BytesToRead;
            if (_bytesToRead <= 0) return;


            byte[] buffer = new byte[_bytesToRead];

            serialPort.Read(buffer, 0, _bytesToRead);
            SendUnityDebug("ReceiveData:" + BitConverter.ToString(buffer));
            if (buffer[0] != 0xAA) return;

            int effectedLength = buffer[1];
            int dataLength = effectedLength - 2;

            bufferList.AddRange(buffer);
            bufferList.RemoveAt(0);

            //校验数据
            byte checkByte = bufferList[effectedLength + 1];
            if (!CheckSum(bufferList.GetRange(0, effectedLength + 1).ToArray(), checkByte))
            {
                
                return;
            }

            bufferList.RemoveAt(0);
            byte cmd = bufferList[0];
            bufferList.RemoveAt(0);
            if (dataLength > 0)
            {
                dataList.AddRange(bufferList.GetRange(0, dataLength));

                //**************************

                //**************************
            }

            bufferList.RemoveRange(0, dataLength + 1);

            if (bufferList[0] != 0xDD)
            {
                SendUnityDebug("数据接收错误，请重试");
                return;
            }


            DataReceived.Invoke(sender, new GunIOEventArgs
            {
                cmd = cmd,
                data = dataList.ToArray()
            });

            serialPort.DiscardInBuffer();


        }


        /// <summary>
        /// 验证校验码
        /// </summary>
        /// <param name="data"></param>
        /// <param name="sum"></param>
        /// <returns></returns>
        public static bool CheckSum(byte[] data, byte sum)
        {
            if (CalculateSum(data) == sum)
            {
                
                return true;
            }
            else
            {
                SendUnityDebug("校验错误");
                return false;
            }
           
        }

        /// <summary>
        /// 生成校验码
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte CalculateSum(byte[] data)
        {
            byte sum = data[0];
            for (int i = 1; i < data.Length; i++)
            {
                sum ^= data[i];
            }
            sum ^= 0x37;

            return sum;
        }

        private static void SendUnityDebug(string message)
        {
            UnityMessageReceived.Invoke(SerialPort, new UnityMessageEventArgs
            {
                message = message
            });
        }

    }

    
}
