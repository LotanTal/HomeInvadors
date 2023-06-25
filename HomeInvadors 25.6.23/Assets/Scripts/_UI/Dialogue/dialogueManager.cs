using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using TMPro;

public class dialogueManager : MonoBehaviour
{
    public static dialogueManager Instance;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public Animator playerAnimator;
    public Animator npcAnimator;
    public Animator catAnimator;
    public Animator dialogueBoxAnimator;
    public string playerAnimationDefault;
    public string npcAnimationDefault;
    public HUD _hud;



    private Queue<string> sentences;
    private Queue<string> sentencesAfterTalk;

    private DialogueData currentDialogueData;
    public bool isDialogueActive;
    Cinemachine.CinemachineVirtualCamera previousCamera;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        sentences = new Queue<string>();
        sentencesAfterTalk = new Queue<string>();
    }

    public void Initialize(Animator playerAnim, Animator npcAnim, Animator catAnim, Animator catEyeAnim)
    {
        playerAnimator = playerAnim;
        npcAnimator = npcAnim;
        catAnim = catAnimator;
        catEyeAnim = catAnimator;
    }

    public void StartDialogue(DialogueData dialogueData, bool isAfterTalk = false)
    {
        isDialogueActive = true;
        StartCoroutine(WaitAndStartDialogue(dialogueData, isAfterTalk));
    }

    private IEnumerator WaitAndStartDialogue(DialogueData dialogueData, bool isAfterTalk = false)
    {
        // Wait until isCurrentlyPlaying is true
        yield return new WaitUntil(() => GameManager.Instance.currentCharacter.actionPoints == 0);

        currentDialogueData = dialogueData;

        dialogueBoxAnimator.SetBool("IsOpen", true);
        _hud.HideInventoryPanel();
        nameText.text = dialogueData.characterName;
        Queue<string> targetQueue = isAfterTalk ? sentencesAfterTalk : sentences;
        targetQueue.Clear();

        DialogueData.DialogueInfo[] sourceSentences = isAfterTalk ? dialogueData.sentencesAfterTalk : dialogueData.normalSentences;

        foreach (DialogueData.DialogueInfo dialogueInfo in sourceSentences)
        {
            targetQueue.Enqueue(dialogueInfo.sentence);
        }

        DisplayNextSentence(dialogueData, isAfterTalk);
    }

    public void DisplayNextSentence(DialogueData dialogueData, bool isAfterTalk = false)
    {
        Queue<string> targetQueue = isAfterTalk ? sentencesAfterTalk : sentences;
        DialogueData.DialogueInfo[] sourceSentences = isAfterTalk ? dialogueData.sentencesAfterTalk : dialogueData.normalSentences;

        if (targetQueue.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = targetQueue.Dequeue();
        DialogueData.DialogueInfo dialogueInfo = sourceSentences[sourceSentences.Length - targetQueue.Count - 1];

        // Deactivate the previous camera if there is one
        if (previousCamera != null)
        {
            previousCamera.Priority = 0;
        }

        // Activate the camera associated with the current sentence
        if (!string.IsNullOrEmpty(dialogueInfo.virtualCameraName))
        {
            Cinemachine.CinemachineVirtualCamera virtualCamera = GameObject.Find(dialogueInfo.virtualCameraName).GetComponent<Cinemachine.CinemachineVirtualCamera>();
            if (virtualCamera != null)
            {
                virtualCamera.Priority = 100;
                previousCamera = virtualCamera;
            }
        }

        playerAnimator.Play(dialogueInfo.playerAnimation);
        npcAnimator.Play(dialogueInfo.npcAnimation);
        catAnimator.Play(dialogueInfo.enemyAnimation);
        catAnimator.Play(dialogueInfo.enemyEYEAnimation);
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    private IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    public void DisplayNextSentenceButton()
    {
        DisplayNextSentence(currentDialogueData);
    }

    public void EndDialogue()
    {
        dialogueBoxAnimator.SetBool("IsOpen", false);
        _hud.ShowInventoryPanel();
        playerAnimator.Play(playerAnimationDefault);
        npcAnimator.Play(npcAnimationDefault);
        catAnimator.Play(playerAnimationDefault);

        // Deactivate the previous camera if there is one
        if (previousCamera != null)
        {
            previousCamera.Priority = 0;
            previousCamera = null;
        }

        Debug.Log("End of conversation");
        isDialogueActive = false;
    }
}
