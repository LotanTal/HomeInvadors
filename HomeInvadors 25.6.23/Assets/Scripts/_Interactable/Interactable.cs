using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Collider))]
public abstract class Interactable : MonoBehaviour
{
    [SerializeField] Color outlineColor = Color.red;
    public Pickable requiredItem;
    public Collider _collider;
    private Outline _outline;
    private GameObject _player;

    private bool _isHovering;
    private float _nearRadius = 3f;
    public bool requiresItem;


    protected abstract void Interact();


    protected void Start()
    {
        _player = GameObject.FindWithTag("Player");
        _collider = GetComponent<Collider>();
        _outline = gameObject.AddComponent<Outline>();
        _outline.OutlineColor = outlineColor;
        _outline.OutlineWidth = 5;
        _outline.enabled = false;
    }

    void Update()
    {
        CheckHover();
        Click();
    }

    void CheckHover()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (_collider.Raycast(ray, out hit, 1000f))
        {
            if (!_isHovering)
            {
                _isHovering = true;
                _outline.enabled = true;
            }
        }
        else
        {
            if (_isHovering)
            {
                _isHovering = false;
                _outline.enabled = false;
            }
        }
    }

    void Click()
    {
        if (Input.GetMouseButtonDown(0) && _isHovering)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, _nearRadius);
            bool isCloseEnough = colliders.Any((c) => c.tag == "Player");

            if (!isCloseEnough)
            {
                Debug.Log("Player is not close enough to interact");
            }
            else if (!ShouldAllowInteraction())
            {
                Debug.Log("This object cannot be interacted at the moment");
            }
            else
            {
                Interact();
            }
        }
    }

    private bool ShouldAllowInteraction()
    {
        if (!requiresItem)
        {
            return true; // Interaction is allowed if it doesn't require an item
        }
        else
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, _nearRadius);
            foreach (Collider c in colliders)
            {
                Pickable item = c.GetComponent<Pickable>();
                if (item != null && item == requiredItem && !c.isTrigger)
                {
                    return true;
                }
            }
            return false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _nearRadius);
    }
}