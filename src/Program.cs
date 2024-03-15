﻿using System.IO.Ports;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Runtime.CompilerServices;

namespace GunIO
{
    public class Port
    {

        public static SerialPort SerialPort;
        public static List<byte> bufferList;
        public static event EventHandler<GunIOEventArgs> DataReceived;
        public static event EventHandler<UnityMessageEventArgs> UnityMessageReceived;


        private static Thread _readThread;
        private static bool _keepReading;
        private static bool _prepareToReceive;

        public static SerialPort OpenPort(string comName, int baud)
        {
            bufferList = new List<byte>();
            if (SerialPort == null || !SerialPort.IsOpen)
            {
                
                SerialPort = new SerialPort();
                SerialPort.PortName = comName;
                SerialPort.BaudRate = baud;
                SerialPort.DataBits = 8;
                SerialPort.Parity = Parity.None;
                SerialPort.StopBits = StopBits.One;
                SerialPort.ReadTimeout = 1000;
               
              
                SerialPort.ErrorReceived += (sender, e) => { SendUnityDebug(e.ToString()); };
                SerialPort.Open();
                _keepReading = true;
                _readThread = new Thread(ReadPort);
                _readThread.Start();
                
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
                _readThread.Abort();
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
                SerialPort.Write(list.ToArray(), 0, list.Count);
                
            }
            else
            {
                SendUnityDebug("串口未找到");
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

        private static void ReadPort()
        {
            while(_keepReading)
            {
                if(SerialPort.IsOpen)
                {
                    byte[] readBuffer = new byte[SerialPort.ReadBufferSize + 1];
                    try
                    {
                        int count = SerialPort.Read(readBuffer, 0, SerialPort.ReadBufferSize);
                        if (count!=0)
                        {
                            if (readBuffer[0] == 0xAA && !_prepareToReceive)
                            {
                                _prepareToReceive = true;
                                
                            }
                            else
                            {
                                //SendUnityDebug(BitConverter.ToString(readBuffer));
                                ReceiveData(readBuffer);
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

        private static void ReceiveData(byte[] data)
        {
            bufferList.AddRange(data);
            int length = (int)bufferList[0]+1;
            List<byte> effectedList = bufferList.GetRange(0, length);
            bufferList.RemoveRange(0, length);
            if (bufferList[0] != 0xDD)
            {
                SendUnityDebug("传输错误，结尾不为0xDD");
                
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
                    DataReceived.Invoke(SerialPort, new GunIOEventArgs
                    {
                        cmd = cmd,
                        data = bytes
                    });
                }
                else
                {
                    SendUnityDebug("校验错误");
                }
            }
            bufferList.Clear();
        }




    }

    
}