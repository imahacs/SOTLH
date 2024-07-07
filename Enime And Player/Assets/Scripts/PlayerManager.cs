using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    //this script is to connect all other movment scripts 
    InputManager inputManager;
    PlayerLocoMotion playerLocoMotion;
    CameraManager cameraManager;
    Animator animator;
    public bool isInteracting;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        playerLocoMotion = GetComponent<PlayerLocoMotion>();
        cameraManager = FindAnyObjectByType<CameraManager>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        inputManager.HandelAllInputs();
    }

    private void FixedUpdate()
    { 
        // use riditbody better to use fixedupdate
        playerLocoMotion.allMovement();
        
    }

    private void LateUpdate()
    {
        cameraManager.HandleAllCameraMovement();
        isInteracting = animator.GetBool("isInteracting");
        playerLocoMotion.isJumpping = animator.GetBool("isJumpping");
        animator.SetBool("isGrounded", playerLocoMotion.isGrounded);
    }
}
