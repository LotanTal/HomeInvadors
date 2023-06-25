using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pigeon : MonoBehaviour
{
    public GameObject pigeonMount;
    public GameObject dialougePos;
    public GameObject Player;

    public void Update()
    {
        transform.position = pigeonMount.transform.position;
        transform.rotation = pigeonMount.transform.rotation;
        if (dialogueManager.Instance.dialogueBoxAnimator.GetBool("IsOpen") == true)
        {
            transform.position = dialougePos.transform.position;
            transform.LookAt(Player.transform.position);
        }
    }
}
