using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : MonoBehaviour
{
<<<<<<< HEAD
    public Cylinder cylinder;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Box"))
        {
            cylinder.Onsensor();
        }
    }

    //PLC로 데이터 전송


=======
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
>>>>>>> df070744f43f0e3721ccd1e4ac4bcb19d7db64b3
}