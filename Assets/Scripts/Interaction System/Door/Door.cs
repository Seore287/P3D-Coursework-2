using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool IsOpen = false;

    [SerializeField] private float speed = 1f;
    [SerializeField] private float openAngle = 90f;

    private Coroutine doorCorutine;
    private Quaternion closedRotation;
    private Quaternion openRotation;

    private void Awake()
    {
        closedRotation = transform.localRotation;
        openRotation = Quaternion.Euler(0,openAngle,0) * closedRotation;
    }

    public void doorOpen(Vector3 playerPosition)
    {
        if (!IsOpen)
        {
            if (doorCorutine != null)
            {
                StopCoroutine(doorCorutine);
            }
            doorCorutine = StartCoroutine(OpenDoor());
        }
    }

    public void doorClose()
    {
        if (doorCorutine != null)
        {
            StopCoroutine(doorCorutine);
        }
        doorCorutine = StartCoroutine(CloseDoor());
    }

    private IEnumerator OpenDoor()
    {
        IsOpen = true;
        float openTime = 0f;
        Quaternion startRotation = transform.localRotation;

        while (openTime < 1f)
        {
            transform.localRotation = Quaternion.Slerp(startRotation, openRotation, openTime);   
            openTime += Time.deltaTime * speed; 
            yield return null;
        }
        transform.localRotation = openRotation;   
    }

    private IEnumerator CloseDoor()
    {
        IsOpen = false;
        float openTime = 0f;
        Quaternion startRotation = transform.localRotation;

        while (openTime < 1f)
        {
            transform.localRotation = Quaternion.Slerp(startRotation, closedRotation, openTime);
            openTime += Time.deltaTime * speed;
            yield return null;
        }
        transform.localRotation = closedRotation;
    }
}
