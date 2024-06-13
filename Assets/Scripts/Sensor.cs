using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Sensor : MonoBehaviour
{
    public MxComponent mxComponent;
    public int isSensing;
    public int PLCOutput;
    public int isChange;

    void Start()
    {
        isSensing = 0;
        PLCOutput = 0;
        isChange = 0;
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("CylinderPoint"))
        {
            if (isSensing == 0)
            { 
                isChange = 1;
                PLCOutput = 1;
                isSensing = 1;
            }
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("Box"))
        {
            if (isSensing == 0)
            {
                isChange = 1;
                PLCOutput = 1;
                isSensing = 1;
            }
        }

        if (name.Contains("Sensor8"))
        {
            mxComponent.Write($"R,D22");

            StartCoroutine(CoSaveCoordinates());
        }
    }

    IEnumerator CoSaveCoordinates()
    {
        yield return new WaitUntil(() => mxComponent.isDReceived == true);


        mxComponent.LoadingCylinderHY.GetComponent<LoadingCylinder>().HighDistance = mxComponent.decimalNumbers[7];
        mxComponent.Write($"W,{"X20"},{mxComponent.LoadingCylinderY.GetComponent<LoadingCylinder>().BackEndIndex},");
        print($"W,{"X20"},{mxComponent.LoadingCylinderY.GetComponent<LoadingCylinder>().BackEndIndex},");

        /*  ��ǥ ����
        distance = mxComponent.decimalNumbers[0]; // D22 : ����1 x�� ��ǥ
        distance = mxComponent.decimalNumbers[1]; // D23 : ����1 y�� ��ǥ
        distance = mxComponent.decimalNumbers[2]; // D24 : ����1 z�� ��ǥ
        distance = mxComponent.decimalNumbers[10]; // D32 : ����2 x�� ��ǥ
        distance = mxComponent.decimalNumbers[11]; // D33 : ����2 y�� ��ǥ
        distance = mxComponent.decimalNumbers[12]; // D34 : ����2 z�� ��ǥ
        mxComponent.decimalNumbers[5] : ���� 1, 2 ���� -> 1 = ����1 , 0 = ����2 */

        if (mxComponent.decimalNumbers[5] == 1)
        {
            mxComponent.LoadingCylinderX.GetComponent<LoadingCylinder>().distance = mxComponent.decimalNumbers[0];
            mxComponent.LoadingCylinderY.GetComponent<LoadingCylinder>().distance = mxComponent.decimalNumbers[1];
            mxComponent.LoadingCylinderZ.GetComponent<LoadingCylinder>().distance = mxComponent.decimalNumbers[2];
        }

        if (mxComponent.decimalNumbers[5] == 0)
        {
            mxComponent.LoadingCylinderX.GetComponent<LoadingCylinder>().distance = mxComponent.decimalNumbers[10];
            mxComponent.LoadingCylinderY.GetComponent<LoadingCylinder>().distance = mxComponent.decimalNumbers[11];
            mxComponent.LoadingCylinderZ.GetComponent<LoadingCylinder>().distance = mxComponent.decimalNumbers[12];
        }

        mxComponent.isDReceived = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("CylinderPoint"))
        {
            if (isSensing == 1)
            {
                isChange = 1;
                PLCOutput = 0;
                isSensing = 0;
            }
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("Box"))
        {
            if (isSensing == 1)
            {
                isChange = 1;
                PLCOutput = 0;
                isSensing = 0;
            }
        }
    }
}
