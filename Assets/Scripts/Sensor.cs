using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : MonoBehaviour
{
    public Cylinder cylinder;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Box"))
        {
            cylinder.Onsensor();
        }
    }

    //PLC로 데이터 전송


}