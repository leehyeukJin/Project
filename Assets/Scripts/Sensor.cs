using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : MonoBehaviour
{
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
}