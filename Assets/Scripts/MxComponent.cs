using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class MxComponent : MonoBehaviour
{
    TcpClient client;
    NetworkStream stream;
    bool isTCPConnecting;
    bool isPLCConnecting;

    public GameObject Cylinder1;
    public GameObject Cylinder2;
    public GameObject LoadingCylinderHY;
    public GameObject LoadingCylinderX;
    public GameObject LoadingCylinderY;
    public GameObject LoadingCylinderZ;
    public GameObject Conveyor;

    public GameObject Sensor1;
    public GameObject Sensor2;
    public GameObject Sensor3;
    public GameObject Sensor4;
    public GameObject Sensor5;
    public GameObject Sensor6;
    public GameObject Sensor7;
    public GameObject Sensor8;

    public string preYDataBlock;
    public string yDataBlock;

    void Start()
    {
        isTCPConnecting = false;
        isPLCConnecting = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isTCPConnecting && isPLCConnecting)
        {
            preYDataBlock = yDataBlock;
            Write("R,");
            Read();
            if(preYDataBlock != yDataBlock)
            {
                Cylinder1.GetComponent<Cylinder>().PLCInput1 = yDataBlock[2];
                Cylinder1.GetComponent<Cylinder>().PLCInput2 = yDataBlock[12];
                Cylinder2.GetComponent<Cylinder>().PLCInput1 = yDataBlock[3];
                Cylinder2.GetComponent<Cylinder>().PLCInput2 = yDataBlock[13];
                LoadingCylinderHY.GetComponent<LoadingCylinder>().PLCInput1 = yDataBlock[20];
                LoadingCylinderHY.GetComponent<LoadingCylinder>().PLCInput2 = yDataBlock[30];
                LoadingCylinderX.GetComponent<LoadingCylinder>().PLCInput1 = yDataBlock[21];
                LoadingCylinderX.GetComponent<LoadingCylinder>().PLCInput2 = yDataBlock[31];
                LoadingCylinderY.GetComponent<LoadingCylinder>().PLCInput1 = yDataBlock[22];
                LoadingCylinderY.GetComponent<LoadingCylinder>().PLCInput2 = yDataBlock[32];
                LoadingCylinderZ.GetComponent<LoadingCylinder>().PLCInput1 = yDataBlock[23];
                LoadingCylinderZ.GetComponent<LoadingCylinder>().PLCInput2 = yDataBlock[33];
                Conveyor.GetComponent<Conveyor>().PLCInput1 = yDataBlock[1];
                print(yDataBlock);
            }
            
            Sensor(Sensor1, "X1");
            Sensor(Sensor2, "X2");
            Sensor(Sensor3, "X3");
            Sensor(Sensor4, "X4");
            Sensor(Sensor5, "X5");
            Sensor(Sensor6, "X6");
            Sensor(Sensor7, "X7");
            Sensor(Sensor8, "X8");
        }
    }


    public void OnConveyorBtnClkEvent()
    {
        Write($"W,X0,1,");
    }
    public void OnTotalProcessBtnClkEvent()
    {
        Write($"W,X99,1,");
    }
    public void Sensor(GameObject Sensor,string component)
    {
        if (Sensor.GetComponent<Sensor>().isChange == 1)
        {
            Write($"W,{component},{Sensor.GetComponent<Sensor>().PLCOutput},");
            print($"W,{component},{Sensor.GetComponent<Sensor>().PLCOutput},");
            Sensor.GetComponent<Sensor>().isChange = 0;
        }
    }
    public void Read()
    {
        byte[] buffer = new byte[320];
        stream.Read(buffer, 0, 320);
        yDataBlock = Encoding.ASCII.GetString(buffer);
    }

    public void Write(string word)
    {

        byte[] buffer = Encoding.ASCII.GetBytes(word.PadRight(10));
        stream.Write(buffer, 0, buffer.Length);

    }

    public void ConnectTCPServer()
    {
        client = new TcpClient("127.0.0.1", 7000);

        stream = client.GetStream();
        print("TCP 서버 연결 완료");
        isTCPConnecting = true;
    }
    public void ConnectPLC()
    {
        Write("CP,");
        print("PLC 서버 연결 완료");
        isPLCConnecting = true;
    }
    public void DisconnectPLC()
    {
        Write("DP,");
    }
}
