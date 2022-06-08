using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallHandler : MonoBehaviour
{
    
    private Camera mainCamera;
    private bool isDragging;
    private Rigidbody2D currentBallRigidbody; 
    private SpringJoint2D currentBallSpringJoint;

    [SerializeField] private float respawnDelay;
    [SerializeField] private float detachBallTime = 0.1f;
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Rigidbody2D pivot;
    
    
    void Start()
    {
        mainCamera = Camera.main;
        SpawnNewBall();
    }

    void Update()
    {
        if (currentBallRigidbody == null)
        {
            return;
        }
        
        if (!Touchscreen.current.primaryTouch.press.isPressed)
        {
            if (isDragging)
            {
                LaunchBall();
            }
            isDragging = false;

            return; 
        }
        isDragging = true;
        currentBallRigidbody.isKinematic = true;

        Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();

        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(touchPosition);

        currentBallRigidbody.position = worldPosition;

    }

    private void SpawnNewBall()
    {
        GameObject ballInstance = Instantiate(ballPrefab, pivot.position, Quaternion.identity);
        currentBallRigidbody = ballInstance.GetComponent<Rigidbody2D>();
        currentBallSpringJoint = ballInstance.GetComponent<SpringJoint2D>();

        currentBallSpringJoint.connectedBody = pivot;

    }

    private void LaunchBall()
    {
        currentBallRigidbody.isKinematic = false;
        currentBallRigidbody = null;
        Invoke("DetachBall", detachBallTime);
    } 

    private void DetachBall()
    {
        currentBallSpringJoint.enabled = false;
        currentBallSpringJoint = null;
        
        Invoke("SpawnNewBall",respawnDelay);
        
    }



}
