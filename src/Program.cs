using System.IO.Ports;
using System;
using System.Collections.Generic;
using System.Threading;

namespace GunIO
{
    public class Port
    {
     
        /// <summary>
        /// port接收信息事件
        /// </summary>
        public static event EventHandler<GunIOEventArgs> dataReceived;
        /// <summary>
        /// 自定义日志事件
        /// </summary>
        public static event EventHandler<CustomLogEventArgs> customLogReceived;

        private static SerialPort _SerialPort;
        private static List<byte> _bufferList;
        private static Thread _readThread;
        private static bool _keepReading;
        private static bool _prepareToReceive;

        /// <summary>
        /// 开启Port
        /// </summary>
        /// <param name="comName">端口名</param>
        /// <param name="baud">波特率</param>
        /// <returns></returns>
        public static SerialPort OpenPort(string comName, int baud)
        {
            _bufferList = new List<byte>();
            if (_SerialPort == null || !_SerialPort.IsOpen)
            {

                _SerialPort = new SerialPort();
                _SerialPort.PortName = comName;
                _SerialPort.BaudRate = baud;
                _SerialPort.DataBits = 8;
                _SerialPort.Parity = Parity.None;
                _SerialPort.StopBits = StopBits.One;
                _SerialPort.ReadTimeout = 1000;
               
              
                _SerialPort.ErrorReceived += (sender, e) => { _SendUnityDebug(e.ToString()); };
                _SerialPort.Open();
                _keepReading = true;
                _readThread = new Thread(_ReadPort);
                _readThread.Start();
                
                if (!_SerialPort.IsOpen)
                {
                    _SendUnityDebug("串口打开失败");
                }
                else
                {
                    _SendUnityDebug("串口打开成功");
                }
            }
            return _SerialPort;
        }

        /// <summary>
        /// 关闭Port
        /// </summary>
        public static void ClosePort()
        {
            if (_SerialPort != null && _SerialPort.IsOpen)
            {
                _readThread.Abort();
                _SerialPort.Close();
                _SerialPort.Dispose();
                _SendUnityDebug("串口已关闭");
            }
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="data"></param>
        public static void SendData(byte cmd, byte[] data)
        {
            if (_SerialPort != null && _SerialPort.IsOpen)
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
                _SerialPort.Write(list.ToArray(), 0, list.Count);
                
            }
            else
            {
                _SendUnityDebug("串口未找到");
            }
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
                _SendUnityDebug("校验错误");
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

        private static void _SendUnityDebug(string message)
        {
            customLogReceived.Invoke(_SerialPort, new CustomLogEventArgs
            {
                message = message
            });
        }

        private static void _ReadPort()
        {
            while(_keepReading)
            {
                if(_SerialPort.IsOpen)
                {
                    byte[] readBuffer = new byte[_SerialPort.ReadBufferSize + 1];
                    try
                    {
                        int count = _SerialPort.Read(readBuffer, 0, _SerialPort.ReadBufferSize);
                        if (count!=0)
                        {
                            if (readBuffer[0] == 0xAA && !_prepareToReceive)
                            {
                                _prepareToReceive = true;
                                
                            }
                            else
                            {
                                //SendUnityDebug(BitConverter.ToString(readBuffer));
                                _ReceiveData(readBuffer);
                                _prepareToReceive = false;
                            }
                            

                        }
                    }
                    catch (TimeoutException)
                    {

                    }
                }
                else
                {
                    TimeSpan waitTime = new TimeSpan(0, 0, 0, 20);
                    Thread.Sleep(waitTime);
                }
            }
        }

        private static void _ReceiveData(byte[] data)
        {
            _bufferList.AddRange(data);
            int length = (int)_bufferList[0]+1;
            List<byte> effectedList = _bufferList.GetRange(0, length);
            _bufferList.RemoveRange(0, length);
            if (_bufferList[0] != 0xDD)
            {
                _SendUnityDebug("传输错误，结尾不为0xDD");
                
            }
            else
            {

                byte checkNum = effectedList[effectedList.Count - 1];
                effectedList.RemoveAt(effectedList.Count - 1);
                if (CheckSum(effectedList.ToArray(), checkNum))
                {
                    effectedList.RemoveAt(0);
                    byte cmd = effectedList[0];
                    effectedList.RemoveAt(0);
                    byte[] bytes = effectedList.ToArray();
                    dataReceived.Invoke(_SerialPort, new GunIOEventArgs
                    {
                        cmd = cmd,
                        data = bytes
                    });
                }
                else
                {
                    _SendUnityDebug("校验错误");
                }
            }
            _bufferList.Clear();
        }




    }

    
}
