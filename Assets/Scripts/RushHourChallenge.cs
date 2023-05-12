using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RushHourChallenge : MonoBehaviour
{
    public void OnWin()
    {
        List<DraggableObject> draggableObjects = new List<DraggableObject>(GetComponentsInChildren<DraggableObject>());
        foreach (var draggableObject in draggableObjects)
        {
            draggableObject.enabled = false;
        }
        
    }
}
