using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : MonoBehaviour
{
    // Cylinder 스크립트를 연결할 변수
    public Cylinder cylinder;

    // 충돌이 감지되었을 때 호출되는 함수
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.layer == LayerMask.NameToLayer("Box"))
        {
            print("gggggg");
            cylinder.Onsensor();
        }
    }
}