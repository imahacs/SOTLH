using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    
    PlayerController player;
    AnimatorMAnager anim;
    PlayerLocoMotion playerLocoMotion;
    //MovmentTest playerContolrs; // to use in future scripts
    public Vector2 movementInput;
    public Vector2 cameraInput; // to control input from action map
    public float moveAmount;
    public float horizontalinput;
    public float verticalinput;
    public float cameraInputX;
    public float cameraInputY;

    public bool b_input;
    public bool jump_input;
    public bool light;
    private void Awake()
    {
        anim = GetComponent<AnimatorMAnager>();
        playerLocoMotion = GetComponent<PlayerLocoMotion>();
       
      // Get componand , when it's on the object , and we use find object : like in camera script when other wise
    }


    private void OnEnable()
    {
        if(player == null)
        {
            player = new PlayerController();
            // player controls . player (refrance map).Movment(action self) ,, recording 
            player.PlayerMovement.Move.performed += i => movementInput = i.ReadValue<Vector2>();
            player.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
            player.PlayerAction.Sprinting.performed += i => b_input = true;
            player.PlayerAction.Sprinting.canceled += i => b_input = false;
            player.PlayerAction.Jumping.performed += i => jump_input = true;
            player.PlayerAction.Light.performed += i => light = true;
            player.PlayerAction.Light.canceled += i => light = false;
        }
        player.Enable();
    }

    private void OnDisable()
    {
        player.Disable();
    }

    public void HandelAllInputs()
    {
       
        HandelMovementInput();
        HandelSprinting();
        HandelJump();
        HandleLight();
        //handel action ...
    }
    private void HandelMovementInput()
    {
        verticalinput = movementInput.y;
        horizontalinput = movementInput.x;

        cameraInputX = cameraInput.x;
        cameraInputY = cameraInputY;

        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalinput) + Mathf.Abs(verticalinput));
        anim.UpdateAniamtorValues(0, moveAmount, playerLocoMotion.isSprinting);
    }

    private void HandelSprinting()
    {
        if (b_input && moveAmount > 0.5f)
        {
            playerLocoMotion.isSprinting = true;
        }
        else
        {
            playerLocoMotion.isSprinting = false;
        }
    }

    private void HandelJump()
    {
        if (jump_input)
        {
            jump_input = false;
            playerLocoMotion.HandleJumpping();
        }

    }

    private void HandleLight()
    {
        if (light)
        {
            playerLocoMotion.lightOn = true;
        }
        else
        {
            playerLocoMotion.lightOn = false;
        }
    }
}
