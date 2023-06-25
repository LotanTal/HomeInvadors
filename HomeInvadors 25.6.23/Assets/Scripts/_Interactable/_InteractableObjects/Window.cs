using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : Interactable
{
    public GameObject Win;
    protected override void Interact()
    {
        Debug.Log("Escape through the window");
        Win.SetActive(true);
    }
}
