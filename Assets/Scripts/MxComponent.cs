using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ActUtlType64Lib;
using UnityEditor.MemoryProfiler;
using TMPro;

public class MxComponent : MonoBehaviour
{
    public enum Connection
    {
        Connected,
        Disconnected,
    }

    ActUtlType64 mxComponent;
    public Connection connection = Connection.Disconnected;
    public TMP_Text log;
    public Transform cylinderA;
    public Transform cylinderA_start;
    public Transform cylinderA_end;
    public Transform cylinderB;
    public Transform cylinderB_start;
    public Transform cylinderB_end;
    public int Y2 = 0;
    public int Y12 = 0;
    public int Y3 = 0;
    public int Y13 = 0;

    private void Start()
    {
        mxComponent = new ActUtlType64();
        mxComponent.ActLogicalStationNumber = 1;

    }

    int GetDevice(string device)
    {
        if (connection == Connection.Connected)
        {
            int lampData = 0;
            int returnValue = mxComponent.GetDevice(device, out lampData);

            if (returnValue != 0)
                print(returnValue.ToString("X"));

            return lampData;
        }
        else
            return 0;
    }


    public void OnConnectPLCBtnClkEvent()
    {
        if (connection == Connection.Disconnected)
        {
            int result = mxComponent.Open();
            if (result == 0)
            {
                print("연결에 성공하였습니다.");
            }

            else
            {
                print("연결에 실패했습니다.returnValue: 0x" + result.ToString("X"));
            }
            if (result.ToString("X") == "F0000003")
            {
                print("연결 된 상태입니다.");
            }
        }
        else
        {
            print("연결 상태입니다.");
        }
    }

    public void OnDisconnectPLCBtnClkEvent()
    {
        if (connection == Connection.Connected)
        {
            int result = mxComponent.Close();
            if (result == 1)
            {
                print("연결 해지되었습니다.");
                connection = Connection.Disconnected;
            }
            else
            {
                print("연결 해지에 실패했습니다. returnValue: 0x" + result.ToString("X"));
            }
        }
        else
        {
            print("연결 해지 상태입니다.");
        }
    }
    public void OnDataPLCGetClkEvent()
    {
        if (connection == Connection.Connected)
        {
            int data = 0;
            int returnValue = mxComponent.GetDevice("M0", out data);
            if (returnValue != 0)
                print("returnValue: 0x" + returnValue.ToString("X"));
            else
                log.text = $"M0: {data}";
        }

    }

    public void OnDataPLCGetClkEvent(TMP_InputField deviceInput, TMP_InputField deviceValue)
    {
        if (connection == Connection.Connected)
        {
            int data = 0;
            int returnValue = mxComponent.GetDevice(deviceInput.text, out data);
            if (returnValue != 0)
                print("returnValue: 0x" + returnValue.ToString("X"));
            else
            {
                log.text = $"{deviceInput.text}: {data.ToString("X")}";
                deviceValue.text = data.ToString("X");
            }
        }

    }
    public void OnDataPLCSetBtnClkEvent()
    {
        if (connection == Connection.Connected)
        {
            int returnValue = mxComponent.SetDevice("M0", 1);
            if (returnValue != 0)
                print("returnValue: 0x" + returnValue.ToString("X"));
            else
                log.text = $"M0: 1";
        }
    }

    public void OnDataPLCSetBtnClkEvent(TMP_InputField deviceInput, TMP_InputField deviceValue)
    {
        print("ABC");

        if (connection == Connection.Connected)
        {
            int value = int.Parse(deviceValue.text);
            int returnValue = mxComponent.SetDevice(deviceInput.text, value);
            if (returnValue != 0)
                print("returnValue: 0x" + returnValue.ToString("X"));
            else
                log.text = $"{deviceInput.text}: {value}";
        }
    }

    public void OnDataPLCGetBlockBtnClkEvent(TMP_InputField deviceInput, TMP_InputField deviceValue)
    {
        print("Hello");

        if (connection == Connection.Connected)
        {
            short data;

            int returnValue = mxComponent.ReadDeviceRandom2(deviceInput.text, 1, out data);
            if (returnValue != 0)
                print("returnValue: 0x" + returnValue.ToString("X"));
            else
                deviceValue.text = $"{deviceInput.text}: {data}";
        }
    }

    IEnumerator MoveCylinder(Transform cylinder, Vector3 positionA, Vector3 positionB, float duration)
    {
        float currentTime = 0;

        while (true)
        {
            currentTime += Time.deltaTime;

            if (currentTime >= duration)
            {
                currentTime = 0;
                break;
            }

            cylinder.position = Vector3.Lerp(positionA, positionB, currentTime / duration);

            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    IEnumerator CoListener()
    {
        while (true)
        {
            Y2 = GetDevice("Cylinder1 전진");
            Y12 = GetDevice("Cylinder1 후진");
            Y3 = GetDevice("Cylinder2 전진");
            Y13 = GetDevice("Cylinder2 후진");

            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    IEnumerator CoRunMPS()
    {
        while (true)
        {
            if (Y2 == 1)
            {
                yield return MoveCylinder(cylinderA, cylinderA_start.position, cylinderA_end.position, 1);
            }

            if (Y12 == 1)
            {
                yield return MoveCylinder(cylinderA, cylinderA_end.position, cylinderA_start.position, 1);
            }

            if (Y3 == 1)
            {
                yield return MoveCylinder(cylinderB, cylinderB_start.position, cylinderB_end.position, 1);
            }

            if (Y13 == 1)
            {
                yield return MoveCylinder(cylinderB, cylinderB_end.position, cylinderB_start.position, 1);
            }

            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
}

