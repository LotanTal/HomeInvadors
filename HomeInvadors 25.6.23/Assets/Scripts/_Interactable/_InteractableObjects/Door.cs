using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    public GameObject door;
    public List<GameObject> obstacles;

    protected override void Interact()
    {
        door.transform.position = new Vector3(0, 90, 0);
        foreach (GameObject obstacle in obstacles)
        {
            Destroy(obstacle);
        }
    }
}
