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

    //PLC�� ������ ����


=======
    // Cylinder ��ũ��Ʈ�� ������ ����
    public Cylinder cylinder;

    // �浹�� �����Ǿ��� �� ȣ��Ǵ� �Լ�
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