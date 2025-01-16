using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField] private Door Door;
    [SerializeField] private Transform player;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == player)
        {
            Door.doorOpen(player.position);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform == player)
        {
            Door.doorClose();
        }
    }
}
