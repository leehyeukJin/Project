using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.Windows;

public class MxComponent : MonoBehaviour
{
    TcpClient client;
    NetworkStream stream;
    bool isTCPConnecting;
    bool isPLCConnecting;

    public GameObject Cylinder1;
    public GameObject Cylinder2;
    public GameObject Cylinder3;
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
    public List<int> decimalNumbers = new List<int>();
    public bool isDReceived = false;

    void Start()
    {
        isTCPConnecting = false;
        isPLCConnecting = false;
        
    }

    public void OnDestroy()
    {
        DisconnectPLC();
        DisconnectTCPServer();
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
                Cylinder3.GetComponent<LoadingCylinder>().PLCInput1 = yDataBlock[4];
                Cylinder3.GetComponent<LoadingCylinder>().PLCInput2 = yDataBlock[14];
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

            Transfer(LoadingCylinderHY, "X10", "X20");
            Transfer(LoadingCylinderX, "X11", "X21");
            Transfer(LoadingCylinderY, "X12", "X22");
            Transfer(LoadingCylinderZ, "X13", "X23");
            Transfer(Cylinder3, "X14", "X24");
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
    public void Transfer(GameObject Transfer, string component1, string component2)
    {
        if (Transfer.GetComponent<LoadingCylinder>().isChange == 1)
        {
            Write($"W,{component1},{Transfer.GetComponent<LoadingCylinder>().FrontEndIndex},");
            print($"W,{component1},{Transfer.GetComponent<LoadingCylinder>().FrontEndIndex},");
            Write($"W,{component2},{Transfer.GetComponent<LoadingCylinder>().BackEndIndex},");
            print($"W,{component2},{Transfer.GetComponent<LoadingCylinder>().BackEndIndex},");
            Transfer.GetComponent<LoadingCylinder>().isChange = 0;
        }
    }
    public void Read()
    {
        byte[] buffer = new byte[320];
        stream.Read(buffer, 0, 320);
        yDataBlock = Encoding.ASCII.GetString(buffer);

        if(yDataBlock.Contains("D"))
        {
            yDataBlock = Encoding.ASCII.GetString(buffer);

            // 1. 16�� ���ھ� �ڸ���.
            List<string> splitStrings = SplitString(yDataBlock.Substring(2), 16);

            // 2. �����´�. (reverse)
            List<string> reversedStrings = ReverseStrings(splitStrings);

            // 3. 2���� -> 10����
            decimalNumbers = ConvertBinaryToDecimal(reversedStrings);

            // ��� ���
            foreach (var number in decimalNumbers)
            {
                print(number);
            }

            isDReceived = true;
        }
    }

    static List<string> SplitString(string str, int chunkSize)
    {
        var result = new List<string>();
        for (int i = 0; i < str.Length; i += chunkSize)
        {
            result.Add(str.Substring(i, Math.Min(chunkSize, str.Length - i)));
        }
        return result;
    }

    static List<string> ReverseStrings(List<string> strings)
    {
        return strings.Select(s => new string(s.Reverse().ToArray())).ToList();
    }

    static List<int> ConvertBinaryToDecimal(List<string> binaryStrings)
    {
        return binaryStrings.Select(bin => Convert.ToInt32(bin, 2)).ToList();
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
        print("TCP ���� ���� �Ϸ�");
        isTCPConnecting = true;
    }

    public void DisconnectTCPServer()
    {
        Write("CS,");
        isTCPConnecting = false;
    }
    public void ConnectPLC()
    {
        Write("CP,");
        print("PLC ���� ���� �Ϸ�");
        isPLCConnecting = true;
    }
    public void DisconnectPLC()
    {
        Write("DP,");
    }
}
