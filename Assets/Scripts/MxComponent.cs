using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ActUtlType64Lib;
using UnityEditor.MemoryProfiler;

public class MxComponent : MonoBehaviour
{
    public enum Connection
    {
        Connected,
        Disconnected,
    }

    ActUtlType64 mxComponent;
    public Connection connection = Connection.Disconnected;
    private void Start()
    {
        mxComponent = new ActUtlType64();
        mxComponent.ActLogicalStationNumber = 1;
    }

    public void ConnectPLC()
    {
        
        int result=mxComponent.Open();
        if (result == 0)
        {
            print("연결됨");
        }
        else
        {
            print("연결안됨");
        }
    }

    public void DisConnectPLC()
    {
        int result = mxComponent.Close();
        if (result == 1)
        {
            print("연결 해지 완료");
            connection = Connection.Disconnected;
        }
        else
        {
            print("연결 해지 실패");
        }
    }
    public void GetPLCdata()
    {

    }
    public void SetPLCdata()
    {

    }
}
