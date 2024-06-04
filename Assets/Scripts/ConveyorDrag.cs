using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorDrag : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Box"))
        {
            other.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Box"))
        {
            other.transform.SetParent(null);
        }
    }
}
