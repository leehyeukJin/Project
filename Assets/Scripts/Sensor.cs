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

        /*  ÁÂÇ¥ ¿¹½Ã
        distance = mxComponent.decimalNumbers[0]; // D22 : »óÀÚ1 xÃà ÁÂÇ¥
        distance = mxComponent.decimalNumbers[1]; // D23 : »óÀÚ1 yÃà ÁÂÇ¥
        distance = mxComponent.decimalNumbers[2]; // D24 : »óÀÚ1 zÃà ÁÂÇ¥
        distance = mxComponent.decimalNumbers[10]; // D32 : »óÀÚ2 xÃà ÁÂÇ¥
        distance = mxComponent.decimalNumbers[11]; // D33 : »óÀÚ2 yÃà ÁÂÇ¥
        distance = mxComponent.decimalNumbers[12]; // D34 : »óÀÚ2 zÃà ÁÂÇ¥
        mxComponent.decimalNumbers[5] : »óÀÚ 1, 2 ±¸ºÐ -> 1 = »óÀÚ1 , 0 = »óÀÚ2 */

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
