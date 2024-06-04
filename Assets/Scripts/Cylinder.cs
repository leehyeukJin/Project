using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Cylinder : MonoBehaviour
{

    public Transform piston;
    Vector3 Origin;
    float time = 0;
    public Button active;
    public Button reverse;
    public Vector3 direction;
    public int speed;
    public int distance;
    public int startIndex;
    public int endIndex;

    void Start()
    {
        startIndex = 0;
        endIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (startIndex == 1)
        {
            startIndex = 0;
            OnUpPistonBtnClkEvent(direction, speed, distance);
            endIndex = 1;
        }
        if (startIndex == 2)
        {
            startIndex = 0;
            OnUpPistonBtnClkEvent(direction, speed, distance);
            endIndex = 1;
        }
    }

    public void OnUpPistonBtnClkEvent(Vector3 direction, int speed, int distance)
    {
        Origin = piston.position;
        time = 0;
        print("Activate Cylinder");
        StartCoroutine(Pistons(direction, speed, distance));
        endIndex = 1;
    }
    public void OnDownPistonBtnClkEvent(Vector3 direction, int speed, int distance)
    {
        Origin = piston.position;
        time = 0;
        print("Reverse Activate Cylinder");
        StartCoroutine(Pistons(direction, speed, distance));
        endIndex = 1;
    }

    IEnumerator Pistons(Vector3 direction,int speed,int distance)
    {
        active.interactable = false;
        reverse.interactable = false;
        while (true)
        {
            time += 0.1f;
            if (time > distance / speed)
                break;
            piston.position = Vector3.Lerp(Origin, Origin + distance * direction, time * speed / distance);
            yield return new WaitForSeconds(0.1f);
        }
        reverse.interactable = true;
        active.interactable = true;
    }
}
