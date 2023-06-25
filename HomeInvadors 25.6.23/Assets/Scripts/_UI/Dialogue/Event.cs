using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Event : MonoBehaviour
{
    public DialogueData dialogueData;
    private dialogueManager _dialogueManager;

    public void Start()
    {
        _dialogueManager = dialogueManager.Instance;
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _dialogueManager.StartDialogue(dialogueData);
            Debug.Log("TRIGGER");

            StartCoroutine(ConvoEnd());
        }
    }

    IEnumerator ConvoEnd()
    {
        yield return new WaitUntil(() => _dialogueManager.dialogueBoxAnimator.GetBool("IsOpen") == false);

        Destroy(gameObject);
    }
}
