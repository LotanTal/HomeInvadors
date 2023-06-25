using System.Collections;
using UnityEngine;

public class Pickable : Interactable
{
    public InventoryItem item;
    private bool isUsingItem;

    protected override void Interact()
    {
        Debug.Log("Item picked up");
        transform.position = new Vector3(0, 0, 0);
        GameManager.Instance.inventoryManager.AddItem(this);
    }

    public virtual void UseItem()
    {
        if (isUsingItem)
            return; // Don't start the coroutine if it's already in progress

        isUsingItem = true;
        StartCoroutine(FollowMouse(() =>
        {
            isUsingItem = false;
            Debug.Log("Item usage is complete."); // Action to perform after the coroutine finishes
        }));
    }

    private IEnumerator FollowMouse(System.Action onCoroutineComplete)
    {
        Plane groundPlane = new Plane(Vector3.up, transform.position);
        Ray ray;
        float hitDistance;

        while (true)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (groundPlane.Raycast(ray, out hitDistance))
            {
                Vector3 newPosition = ray.GetPoint(hitDistance);
                newPosition.y = isUsingItem ? 0.5f : 0.5f; // Set Y position based on the item usage state
                transform.position = newPosition;
            }

            if (Input.GetMouseButtonDown(1))
            {
                // Right mouse button clicked - stop following
                transform.position = new Vector3(0, 0, 0);
                break;
            }

            yield return null;
        }

        onCoroutineComplete?.Invoke(); // Invoke the callback action
    }

}