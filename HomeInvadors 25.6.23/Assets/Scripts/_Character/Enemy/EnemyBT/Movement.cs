using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public static Movement movement;

    void Awake()
    {
        movement = this;
    }
}
