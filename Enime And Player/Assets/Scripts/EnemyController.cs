using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Animator animator;
    public Transform player;
    public float detectionRange = 10f;
    public float runSpeed = 5f;
    public float walkSpeed = 2f;
    public float attackRange = 1.5f;

    private PlayerLocoMotion locoMotion;
    private bool isScreaming = false;
    private bool isRunning = false;
    private bool isAttacking = false;
    private bool isWalking = false;
    private bool isLookingAround = false;
    private Vector3 randomPosition;

    private void Start()
    {
        locoMotion = FindAnyObjectByType<PlayerLocoMotion>();
        SetRandomPosition();
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        bool playerInRange = distanceToPlayer <= detectionRange;
        bool playerHasLight = locoMotion.IsLightOn();

        if (!playerInRange)
        {
            Debug.Log("Enemy is idle.");
            SetAnimatorStates(true, false, false, false, false, false);
            ResetStates();
        }
        else if (playerInRange && playerHasLight)
        {
            HandlePlayerWithLight(distanceToPlayer);
        }
        else if (playerInRange && !playerHasLight)
        {
            HandlePlayerWithoutLight();
        }
    }

    private void HandlePlayerWithLight(float distanceToPlayer)
    {
        if (!isScreaming && !isRunning && !isAttacking)
        {
            Debug.Log("Enemy is screaming.");
            isScreaming = true;
            SetAnimatorStates(false, false, true, false, false, false);
        }

        if (isScreaming)
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("Scream") && stateInfo.normalizedTime >= 1.0f)
            {
                Debug.Log("Transitioning to run.");
                isScreaming = false;
                isRunning = true;
                SetAnimatorStates(false, false, false, true, false, false);
            }
        }

        if (isRunning)
        {
            FollowPlayer(runSpeed);
            Debug.Log("Running towards player. Distance to player: " + distanceToPlayer);

            if (distanceToPlayer <= attackRange)
            {
                Debug.Log("Within attack range. Transitioning to attack.");
                isRunning = false;
                isAttacking = true;
                SetAnimatorStates(false, false, false, false, true, false);
            }
        }

        if (isAttacking)
        {
            // Check if the attack animation is finished
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("Attack") && stateInfo.normalizedTime >= 1.0f)
            {
                Debug.Log("Attack finished. Transitioning to idle.");
                isAttacking = false;
                SetAnimatorStates(true, false, false, false, false, false);

                // Check if player is within attack range and light is on
                if (distanceToPlayer <= attackRange && locoMotion.IsLightOn())
                {
                    // Trigger player death
                  //  locoMotion.TriggerDeathAnimation();
                }
            }
        }
    }

    private void HandlePlayerWithoutLight()
    {
        if (!isLookingAround && !isWalking)
        {
            Debug.Log("Enemy is looking around.");
            isLookingAround = true;
            SetAnimatorStates(false, false, false, false, false, true);
        }

        if (isLookingAround)
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("LookAround") && stateInfo.normalizedTime >= 1.0f)
            {
                Debug.Log("Transitioning to walk.");
                isLookingAround = false;
                isWalking = true;
                SetRandomPosition();
                SetAnimatorStates(false, true, false, false, false, false);

                // Rotate towards the random position
                Vector3 direction = (randomPosition - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f); // Adjust the rotation speed as needed
            }
        }

        if (isWalking)
        {
            WalkToRandomPosition();
            Debug.Log("Walking to random position. Distance to position: " + Vector3.Distance(transform.position, randomPosition));

            if (Vector3.Distance(transform.position, randomPosition) < 1f)
            {
                SetRandomPosition();
            }
        }
    }

    public void OnLightToggled()
    {
        if (locoMotion.IsLightOn())
        {
            Debug.Log("Light turned on. Resetting states.");
            ResetStates();
            HandlePlayerWithLight(Vector3.Distance(transform.position, player.position));
        }
        else
        {
            Debug.Log("Light turned off. Resetting states.");
            ResetStates();
        }
    }

    private void SetAnimatorStates(bool idle, bool walk, bool scream, bool run, bool attack, bool lookAround)
    {
        animator.SetBool("isIdle", idle);
        animator.SetBool("isWalking", walk);
        animator.SetBool("isScreaming", scream);
        animator.SetBool("isRunning", run);
        animator.SetBool("isAttacking", attack);
        animator.SetBool("isLookingAround", lookAround);
    }

    private void ResetStates()
    {
        isScreaming = false;
        isRunning = false;
        isAttacking = false;
        isWalking = false;
        isLookingAround = false;
    }

    private void FollowPlayer(float speed)
    {
        if (player != null)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            transform.Translate(direction * speed * Time.deltaTime, Space.World);
        }
    }

    private void SetRandomPosition()
    {
        randomPosition = new Vector3(
            Random.Range(transform.position.x - 10f, transform.position.x + 10f),
            transform.position.y,
            Random.Range(transform.position.z - 10f, transform.position.z + 10f)
        );
    }

    private void WalkToRandomPosition()
    {
        Vector3 direction = (randomPosition - transform.position).normalized;
        transform.Translate(direction * walkSpeed * Time.deltaTime, Space.World);
    }
}
