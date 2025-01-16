using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ObjectActivate : Interactable
{
    public string ObjectName;
    public GameObject Object;

    public override void Interact()
    {
        // Activate or deactivate the object
        Debug.Log("Activated: " + ObjectName);
        Object.SetActive(!Object.activeSelf);  
    }
}
