using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class PlayerCamera : MonoBehaviour
{
    public Transform player;
    public float rotationX = 45f;

    private float rotationAngle = 0f;
    private float cameraDistance;
    public float rotationSpeed = 10f;
    private float targetRotationAngle;
    public float dragSpeed = 10f;
    private bool snapBack = true;
    public bool isDraggingCam = false;

    private Vector3 initialPosition;
    private Vector3 dragStartPosition;

    public CameraManager cameraManager;

    private List<MeshRenderer> hitMeshRenderer = new List<MeshRenderer>();


    public void Start()
    {
        initialPosition = player.transform.position;
        cameraManager.SwtichCamera(cameraManager.raccoonMainRotateCam);
    }

    void Update()
    {
        HandleRotationInput();
        HandleDragInput();
        UpdateCameraPosition();
        CheckWallVisibility();

    }

    void HandleRotationInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) && isDraggingCam == false)
        {
            targetRotationAngle = Mathf.Round((rotationAngle + 90f) / 90f) * 90f;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && isDraggingCam == false)
        {
            targetRotationAngle = Mathf.Round((rotationAngle - 90f) / 90f) * 90f;
        }


        rotationAngle = Mathf.LerpAngle(rotationAngle, targetRotationAngle, Time.deltaTime * rotationSpeed);
        transform.rotation = Quaternion.Euler(rotationX, rotationAngle, 0f);
    }

    void HandleDragInput()
    {
        if (Input.GetMouseButton(1))
        {
            isDraggingCam = true;
            dragStartPosition = transform.position;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            isDraggingCam = false;
            if (snapBack)
            {
                initialPosition = player.transform.position;
                transform.LookAt(player);
            }
        }
    }

    void UpdateCameraPosition()
    {
        Vector3 targetPosition = player.position - transform.forward * cameraDistance;

        if (isDraggingCam)
        {
            float mouseX = Input.GetAxis("Mouse X") * dragSpeed * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * dragSpeed * Time.deltaTime;

            Vector3 forward = transform.forward;
            forward.y = 0f;
            forward = forward.normalized;

            Vector3 right = new Vector3(forward.z, 0f, -forward.x);

            Vector3 targetDragPosition = dragStartPosition + (right * mouseX) + (forward * mouseY);

            // Clamp target position to a maximum distance from the initial position
            float maxDistance = 2f;
            Vector3 deltaPosition = targetDragPosition - initialPosition;
            if (deltaPosition.magnitude > maxDistance)
            {
                deltaPosition = deltaPosition.normalized * maxDistance;
                targetDragPosition = initialPosition + deltaPosition;
            }

            targetDragPosition.y = transform.position.y;

            // Raycast to check for collisions with walls
            int layerMask = 1 << LayerMask.NameToLayer("walls");
            RaycastHit hitInfo;
            if (Physics.Linecast(transform.position, targetDragPosition, out hitInfo, layerMask))
            {
                // If there is a wall in the way, clamp the target position to the closest point on the wall
                targetDragPosition = hitInfo.point;
            }

            targetPosition = targetDragPosition;
        }

        transform.position = targetPosition;
    }

    void CheckWallVisibility()
    {
        int layerMask = (1 << LayerMask.NameToLayer("walls")) | (1 << LayerMask.NameToLayer("doors")); // Combine layers using bitwise OR
        float castRadius = 1.7f; // Set the desired radius of the sphere cast

        Debug.DrawLine(cameraManager.currentCam.transform.position, player.transform.position, Color.green);

        RaycastHit[] hits = Physics.SphereCastAll(cameraManager.currentCam.transform.position, castRadius, player.transform.position - cameraManager.currentCam.transform.position, layerMask);

        if (!IsPlayerVisible())
        {
            foreach (MeshRenderer renderer in hitMeshRenderer)
            {
                if (!IsHitMeshRenderer(renderer, hits))
                {
                    foreach (Material material in renderer.materials)
                    {
                        material.SetFloat("_Opacity", 1.0f);
                    }
                }
            }

            hitMeshRenderer.Clear();

            if (hits.Length > 0)
            {
                // Get all materials on the hit objects and set their "_Opacity" property to 0.3
                foreach (RaycastHit hit in hits)
                {
                    MeshRenderer[] meshRenderers = hit.collider.GetComponentsInChildren<MeshRenderer>();
                    foreach (MeshRenderer renderer in meshRenderers)
                    {
                        foreach (Material material in renderer.materials)
                        {
                            material.SetFloat("_Opacity", 0.3f);
                            hitMeshRenderer.Add(renderer);
                        }
                    }
                }
            }
        }
        else
        {
            // Set opacity back to 1 for all previously hit mesh renderers
            foreach (MeshRenderer renderer in hitMeshRenderer)
            {
                foreach (Material material in renderer.materials)
                {
                    material.SetFloat("_Opacity", 1.0f);
                }
            }

            hitMeshRenderer.Clear();
        }
    }

    bool IsPlayerVisible()
    {
        int layerMask = (1 << LayerMask.NameToLayer("walls")) | (1 << LayerMask.NameToLayer("doors"));
        RaycastHit hitInfo;
        if (Physics.Linecast(cameraManager.currentCam.transform.position, player.transform.position, out hitInfo, layerMask))
        {
            // If there is a wall or door in the way, the player is not visible
            return false;
        }
        else
        {
            // If there is no obstacle in the way, the player is visible
            return true;
        }
    }

    bool IsHitMeshRenderer(MeshRenderer renderer, RaycastHit[] hits)
    {
        foreach (RaycastHit hit in hits)
        {
            MeshRenderer[] meshRenderers = hit.collider.GetComponentsInChildren<MeshRenderer>();
            if (Array.IndexOf(meshRenderers, renderer) >= 0)
            {
                return true;
            }
        }
        return false;
    }

}

