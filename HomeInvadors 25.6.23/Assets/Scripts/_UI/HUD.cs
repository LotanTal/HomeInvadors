using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public TextMeshProUGUI _characterTitle;
    public Button _endTurnButton;
    public GameObject _turnTitle;
    public TextMeshProUGUI _turnTitleCharacter;
    public TextMeshProUGUI _turnTitleTurn;
    public TextMeshProUGUI _actionPointsText;
    public GameObject raccoonProfilePic;
    public GameObject catProfilePic;


    float _lastTurnStartTime;
    public GameObject inventoryCanvas;

    private RectTransform inventoryCanvasRectTransform;
    private Vector3 inventoryCanvasOriginalPosition;
    private Vector3 inventoryCanvasHiddenPosition;

    // Start is called before the first frame update
    void Start()
    {

        _turnTitleTurn = transform.Find("TurnTitle/Turn").GetComponent<TextMeshProUGUI>();
        _turnTitle.SetActive(false);

        inventoryCanvasRectTransform = inventoryCanvas.GetComponent<RectTransform>();
        inventoryCanvasOriginalPosition = inventoryCanvasRectTransform.transform.position;
        inventoryCanvasHiddenPosition = new Vector3(inventoryCanvasOriginalPosition.x, inventoryCanvasOriginalPosition.y - 500, inventoryCanvasOriginalPosition.z);
    }

    // Update is called once per frame
    void Update()
    {
        HideTurnTitle();
    }

    public void StartTurn(string characterTitle, int turn)
    {
        // hide end turn button when enemy's turn
        if (characterTitle == "Cat")
        {
            _endTurnButton.interactable = false;
            HideInventoryPanel();
            raccoonProfilePic.SetActive(false);
            catProfilePic.SetActive(true);

        }
        else
        {
            _endTurnButton.interactable = true;
            ShowInventoryPanel();
            raccoonProfilePic.SetActive(true);
            catProfilePic.SetActive(false);
        }

        _lastTurnStartTime = Time.time;
        _characterTitle.text = characterTitle;

        _turnTitleCharacter.text = characterTitle + " TURN";
        _turnTitleTurn.text = "TURN " + (turn + 1);
        _turnTitle.SetActive(true);
    }

    public void UpdateActionPoints(int actionPoints)
    {
        _actionPointsText.text = actionPoints.ToString() + " AP";
    }

    void HideTurnTitle()
    {
        if (Time.time - 3 > _lastTurnStartTime)
        {
            _turnTitle.SetActive(false);
        }
    }

    public void HideInventoryPanel()
    {
        // Move the inventory canvas down out of the screen
        StartCoroutine(AnimateMove(inventoryCanvasOriginalPosition, inventoryCanvasHiddenPosition, 0.5f));
    }

    public void ShowInventoryPanel()
    {
        // Move the inventory canvas back up to its original position
        StartCoroutine(AnimateMove(inventoryCanvasHiddenPosition, inventoryCanvasOriginalPosition, 0.5f));
    }

    IEnumerator AnimateMove(Vector3 startingPos, Vector3 targetPosition, float duration)
    {
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            inventoryCanvas.transform.position = Vector3.Lerp(startingPos, targetPosition, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        inventoryCanvas.transform.position = targetPosition;
    }
}
