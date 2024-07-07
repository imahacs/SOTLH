using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocoMotion : MonoBehaviour
{
    PlayerManager playerManager;
    InputManager inputManager;
    AnimatorMAnager animatorMAnager;

    
   
    Vector3 moveDirection;
    Transform cameraObj;
    Rigidbody playerRigidbody;

    [Header("Falling")]
    public float inAirTimer;
    public float leapinVelocity ;
    public float fallingSpeed;
    public LayerMask groundLayer;
    private float rayCastHightoffSet = 0.50f;
    public float maxDistance = 0.01f;
    [Header("Jumpping")]
    public float gravity = -15 ;
    public float jumpingspeed = 3;

    [Header("Movment FLags")]
    public bool isSprinting;
    public bool isGrounded;
    public bool isJumpping;

    [Header("Movment Speed")]
    public float walkingSpeed = 1.5f ; // can changed based on animation 
    public float sprintSpeed = 7;
    public float runningSpeed = 5;
    public float rotationSpeed = 15;

    //--------------- 
    [Header("Enemy")]
    public Light playerLight;
    private bool isLightOn = true;
    private bool isDead = false;
    public bool lightOn;
    private EnemyController[] enemies;


    private void Awake()
    {
        enemies = FindObjectsOfType<EnemyController>();
        inputManager = GetComponent<InputManager>();
        playerRigidbody = GetComponent<Rigidbody>();
        cameraObj = Camera.main.transform;
        playerManager = GetComponent<PlayerManager>();
        animatorMAnager = GetComponent<AnimatorMAnager>();
    }
    //incapsulation 
    public void allMovement()
    {// we put it in first line so no mater what palyer is doing if he's falling , he's falling
        HandelFallAndLanding();

        if (playerManager.isInteracting)
            return;
        

        // so player don't walk while falling and so on 
        HandleMovment();
        HandleRotation();
        ToggleLight();




    }

    //Enimey 
    public void ToggleLight()
    {
        if (lightOn)
        {
            isLightOn = !isLightOn;
            playerLight.enabled = isLightOn;
            NotifyEnemies();
        }
    }
    public bool IsLightOn()
    {
        return isLightOn;
    }

    private void NotifyEnemies()
    {
        foreach (var enemy in enemies)
        {
            enemy.OnLightToggled();
        }
    }


    private void HandleMovment()
    {

        //the movment will be the direction of the camera and how much movment we are pressing
        moveDirection = cameraObj.forward * inputManager.verticalinput;
        moveDirection = moveDirection + cameraObj.right * inputManager.horizontalinput; // to move left and right
        moveDirection.Normalize();
        moveDirection.y = 0; // to not fly :)

        if (isSprinting)
        {
            moveDirection *= sprintSpeed;
        }
        else
        {
            // if we are sprinting select spriinting speed
            // if running chose running speed 
            if (inputManager.moveAmount >= 0.5f)
            {
                moveDirection = moveDirection * runningSpeed;

            }
            else
            {
                moveDirection = moveDirection * walkingSpeed;
            }


        }


        Vector3 movementVelocity = moveDirection;
        playerRigidbody.velocity = movementVelocity;
    }

    private void HandleRotation()
    {
        if (isJumpping)
            return;
        // where we want player to rotate 
        Vector3 targetDirection = Vector3.zero;

        targetDirection = cameraObj.forward * inputManager.verticalinput;
        targetDirection = targetDirection + cameraObj.right* inputManager.horizontalinput;
        targetDirection.Normalize();
        targetDirection.y = 0;

        if(targetDirection == Vector3.zero)
        
            targetDirection = transform.forward;
        
        // to calculater rotation , look towareds target direction 
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        //slerp rotation between point A & B
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        transform.rotation = playerRotation;
    }

    public void HandleJumpping()
    {
        if (isGrounded)
        {
            animatorMAnager.animator.SetBool("isJumpping", true);
            animatorMAnager.PlayTargetAnimation("Jump", false);

            float jumpingVelocity = Mathf.Sqrt(-2 * gravity * jumpingspeed);
            Vector3 playerVelocity = moveDirection;
            playerVelocity.y = jumpingVelocity;
            playerRigidbody.velocity = playerVelocity;
        }

    }

    private void HandelFallAndLanding()
    {
        // to clime staires 
       Vector3 tarfetPos;
        tarfetPos = transform.position;
        RaycastHit hit;
        Vector3 rayCastOrigin = transform.position; // here we put empty obgj to determen if player if on ground of not to fall 
        rayCastOrigin.y += rayCastHightoffSet;

        if (!isGrounded && !isJumpping)
        {
            if (!playerManager.isInteracting) {
                animatorMAnager.PlayTargetAnimation("Falling", true);
            
               }

            inAirTimer += Time.deltaTime;
            playerRigidbody.AddForce(transform.forward * leapinVelocity);
            playerRigidbody.AddForce(-Vector3.up * fallingSpeed * inAirTimer);
        }

        if(Physics.SphereCast(rayCastOrigin, 0.2f, -Vector3.up, out hit, maxDistance , groundLayer))
        {
            if(!isGrounded && !playerManager.isInteracting)
            {
                animatorMAnager.PlayTargetAnimation("Land", true);

            }
            Vector3 raycastHitPoaint = hit.point;
            tarfetPos.y = raycastHitPoaint.y;

            inAirTimer = 0;
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

       if(isGrounded && !isJumpping)
        {
            if(playerManager.isInteracting || inputManager.moveAmount > 0)
            {
                transform.position = Vector3.Lerp(transform.position, tarfetPos, Time.deltaTime / 0.1f);
            }
            else
            {
                transform.position = tarfetPos;
            }
        }
    }

    
}
