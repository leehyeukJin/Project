using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LoadingCylinder : MonoBehaviour
{
    public Transform piston;
    Vector3 Origin;
    public Button active;
    public Button reverse;
    public Vector3 direction;
    public int _direction;
    public float speed;
    public float distance;
    public float HighDistance;
    public char PLCInput1;
    public char PLCInput2;
    public int isPistonMoving;
    public int endIndex;
    public int FrontEndIndex;
    public int BackEndIndex;
    float time = 0;
    public int sensing;
    public float location;
    public int isChange;
    public int isActivate;

    void Start()
    {
        PLCInput1 = '0';
        PLCInput2 = '0';
        sensing = 0;
        endIndex = 0;
        isPistonMoving = 0;
        isChange = 0;
        isActivate = 0;
    }

    void Update()
    {
        if (PLCInput1 == '1')
        {
            if (isPistonMoving == 0)
            {
                isPistonMoving = 1;
                StartCoroutine(FrontPLCPistons());
            }
            if (location == distance && FrontEndIndex == 0)
            {
                FrontEndIndex = 1;
                isChange = 1;
            }
            if (isActivate == 0)
            {
                isActivate = 1;
            }
        }

        if (PLCInput1 == '0')
        {

            if (isActivate == 1)
            {
                isActivate = 0;
                FrontEndIndex = 0;
                isChange = 1;
            }
        }

        if (PLCInput2 == '1')
        {
            if (isPistonMoving == 0)
            {
                isPistonMoving = 1;
                if (gameObject.tag == "Cy3")
                    location = location * 2;
                StartCoroutine(BackPLCPistons());
            }
            if (location == 0 && BackEndIndex == 0)
            {
                BackEndIndex = 1;
                isChange = 1;
            }
        }

        if (PLCInput2 == '0')
        {

            if (isActivate == 1)
            {
                isActivate = 0;
                BackEndIndex = 0;
                isChange = 1;
            }
        }
    }

    public void OnActivePistonBtnClkEvent()
    {
        Origin = piston.localPosition;
        time = 0;
        print("Activate Cylinder");
        StartCoroutine(Pistons(direction, speed, distance));
    }

    public void OnReversePistonBtnClkEvent()
    {
        Origin = piston.localPosition;
        time = 0;
        print("Reverse Activate Cylinder");
        StartCoroutine(Pistons(-direction, speed, distance));
    }

    public void Onsensor()
    {
        sensing = 1;
    }

    IEnumerator FrontPLCPistons()
    {

         if (gameObject.tag == "Cy2")
            MxComponent.instance.Transfer(this.gameObject, "X11");

        else if (gameObject.tag == "Cy3")
            MxComponent.instance.Transfer(this.gameObject, "X12");

        else if (gameObject.tag == "Cy4")
            MxComponent.instance.Transfer(this.gameObject, "X13");

        else if (gameObject.tag == "Cy5")
            MxComponent.instance.Transfer(this.gameObject, "X14");

        while (location < distance)
        {
            location += _direction * speed * Time.deltaTime * 2;
            if (location >= distance)
            {
                location = distance; // 강제로 위치를 설정
                break;
            }
            piston.position += direction * speed * Time.deltaTime;
            yield return null; // Time.deltaTime 대신 null 사용
        }

        isPistonMoving = 0;
    }

    IEnumerator BackPLCPistons()
    {
        float targetLocation;

        if (gameObject.tag == "Cy2")
            MxComponent.instance.Transfer(this.gameObject, "X21");

        else if (gameObject.tag == "Cy3")
       
            MxComponent.instance.Transfer(this.gameObject, "X22");        

        else if (gameObject.tag == "Cy4")
            MxComponent.instance.Transfer(this.gameObject, "X23");

        else if (gameObject.tag == "Cy5")
            MxComponent.instance.Transfer(this.gameObject, "X24");

        while (location > 0)
        {
            location -= _direction * speed * Time.deltaTime * 2;
            if (location <= 0)
            {
                location = 0; // 강제로 위치를 설정
                break;
            }
            piston.position -= direction * speed * Time.deltaTime;
            yield return null; // Time.deltaTime 대신 null 사용
        }

        isPistonMoving = 0;
    }

    IEnumerator FrontPLCPistonsHigh()
    {
        while (location < HighDistance)
        {
            location = location + _direction * speed;
            piston.localPosition = piston.localPosition + direction * speed;
            yield return new WaitForSeconds(0.01f);
        }
        isPistonMoving = 0;
    }

    IEnumerator BackPLCPistonsHigh()
    {
        while (location > 0)
        {
            location = location - _direction * speed;
            piston.localPosition = piston.localPosition - direction * speed;
            yield return new WaitForSeconds(0.01f);
        }
        isPistonMoving = 0;

    }

    IEnumerator Pistons(Vector3 direction, float speed, float distance)
    {
        active.interactable = false;
        reverse.interactable = false;
        while (true)
        {
            time += 0.01f;
            if (time > distance / speed)
            {

                break;
            }
            if (sensing == 1)
            {
                break;
            }

            piston.localPosition = Vector3.Lerp(Origin, Origin + distance * direction, time * speed / distance);
            yield return new WaitForSeconds(0.01f);
        }
        if (sensing == 1)
        {
            sensing = 0;
            while (true)
            {
                time -= 0.01f;
                if (time <= 0)
                {
                    break;
                }
                piston.localPosition = Vector3.Lerp(Origin, Origin + distance * direction, time * speed / distance);
                yield return new WaitForSeconds(0.01f);
            }
        }

        reverse.interactable = true;
        active.interactable = true;
        endIndex = 1;

    }

}