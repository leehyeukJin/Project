using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : MonoBehaviour
{
    public Cylinder cylinder;
    private bool isSensorActive = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!isSensorActive && other.gameObject.layer == LayerMask.NameToLayer("Box"))
        {
            print("gggggg");
            isSensorActive = true; // 센서가 작동 중임을 표시
            cylinder.Onsensor();
        }
    }

    public void DeactivateSensor()
    {
        isSensorActive = false; // 센서 비활성화
    }
}