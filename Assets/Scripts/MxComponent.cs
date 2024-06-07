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
            print("�����");
        }
        else
        {
            print("����ȵ�");
        }
    }

    public void DisConnectPLC()
    {
        int result = mxComponent.Close();
        if (result == 1)
        {
            print("���� ���� �Ϸ�");
            connection = Connection.Disconnected;
        }
        else
        {
            print("���� ���� ����");
        }
    }
    public void GetPLCdata()
    {

    }
    public void SetPLCdata()
    {

    }
}
