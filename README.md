# GunIO
此项目采用.Net Framework 2.0开发

以下是Unity的使用案例：
```
using System;
using UnityEngine;
using GunIO;
using Debug = UnityEngine.Debug;

public class IOTest : MonoBehaviour
{
    private void Awake()
    {
        Port.customLogReceived += (sender, args) => Debug.Log(args.message);
        Port.OpenPort("COM5", 115200);
        Port.dataReceived += DataReceived;
    }
    

    private void DataReceived(object sender, GunIOEventArgs args)
    {
        byte cmd = args.cmd;
        byte[] data = args.data;
        
        Debug.Log("cmd:" + cmd);
        Debug.Log("data:" + BitConverter.ToString(data));
    }

    private void SendData(byte cmd, byte[] data)
    {
        Port.SendData(cmd, data);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            SendData(0x00, Array.Empty<byte>());
        }
    }

    private void OnApplicationQuit()
    {
        Port.ClosePort();
    }
}
```