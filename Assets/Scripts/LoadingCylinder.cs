using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LoadingCylinder : MonoBehaviour
{
    public Transform piston;
    public Transform box;  // Box의 Transform을 추가합니다.
    public string boxLayerName = "Box"; // 할당할 레이어 이름
    Vector3 Origin;
    public Button active;
    public Button reverse;
    public Vector3 direction;
    public int _direction;
    public float speed;
    public float distance;
    public char PLCInput1;
    public char PLCInput2;
    public int isPistonMoving;
    public int endIndex;
    float time = 0;
    public int sensing;
    public float location;

    void Awake()
    {
        // 레이어 이름을 통해 레이어 번호를 가져옴
        int boxLayer = LayerMask.NameToLayer(boxLayerName);

        // 모든 게임 오브젝트를 검색
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        // 특정 레이어를 가진 첫 번째 게임 오브젝트를 찾아 box 변수에 할당
        foreach (GameObject obj in allObjects)
        {
            if (obj.layer == boxLayer)
            {
                box = obj.transform;
                break;
            }
        }

        if (box == null)
        {
            Debug.LogError($"Box object in layer '{boxLayerName}' not found!");
        }
    }

    void Start()
    {
        PLCInput1 = '0';
        PLCInput2 = '0';
        sensing = 0;
        endIndex = 0;
        isPistonMoving = 0;
    }

    void Update()
    {
        
    }

    public void OnActivePistonBtnClkEvent()
    {
        Origin = piston.position;
        time = 0;
        print("Activate Cylinder");

        if (box != null)
        {
            // Box를 piston의 자식으로 설정합니다.
            box.SetParent(piston);
        }
        else
        {
            Debug.LogError("Box is not assigned!");
        }

        StartCoroutine(Pistons(direction, speed, distance));
    }

    public void OnReversePistonBtnClkEvent()
    {
        Origin = piston.position;
        time = 0;
        print("Reverse Activate Cylinder");

        if (box != null)
        {
            // Box를 piston의 자식에서 해제합니다.
            box.SetParent(null);
        }
        else
        {
            Debug.LogError("Box is not assigned!");
        }

        StartCoroutine(Pistons(-direction, speed, distance));
    }

    public void Onsensor()
    {
        sensing = 1;
    }

    IEnumerator FrontPLCPistons()
    {
        while (location <= distance)
        {
            location = location + _direction * speed;
            piston.position = piston.position + direction * speed;
            yield return new WaitForSeconds(0.01f);
        }
        isPistonMoving = 0;
    }

    IEnumerator BackPLCPistons()
    {
        while (location >= 0)
        {
            location = location - _direction * speed;
            piston.position = piston.position - direction * speed;
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
                break;
            if (sensing == 1)
                break;
            piston.position = Vector3.Lerp(Origin, Origin + distance * direction, time * speed / distance);
            yield return new WaitForSeconds(0.01f);
        }
        if (sensing == 1)
        {
            sensing = 0;
            while (true)
            {
                time -= 0.01f;
                if (time <= 0)
                    break;
                piston.position = Vector3.Lerp(Origin, Origin + distance * direction, time * speed / distance);
                yield return new WaitForSeconds(0.01f);
            }
        }
        reverse.interactable = true;
        active.interactable = true;
        endIndex = 1;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TriggerArea"))
        {
            PLCInput1 = '1'; // 트리거가 감지되면 PLCInput1을 '1'로 설정
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("TriggerArea"))
        {
            PLCInput1 = '0'; // 트리거가 벗어나면 PLCInput1을 '0'로 설정
        }
    }
}