using UnityEngine;

public class MonsterAI : MonoBehaviour
{
    
    public Transform player;
    [Header("Patrol points")]
    public Transform[] patrolPoints; // Patrol points
    private int currentPatrolIndex = 0;
    public float patrolSpeed = 3f;

    [Header("Distances before detection")]
    public float normalChaseDistance = 10f;
    public float maxChaseDistance = 20f;

    [Header("Chasing")]
    public float chaseDuration = 5f; // How long the monster will chase the player
    private float chaseTimer = 0f;
    public float chaseSpeed = 10f;
    private bool isChasing = false;


    private bool playerInLight;
    private bool playerCrouching;
    private bool playerSeen;
    private bool playerRunning;

    private void Update()
    {
        playerInLight = CheckIfPlayerInLight();
        playerCrouching = CheckIfPlayerIsCrouching();
        playerSeen = CheckIfPlayerSeen();
        playerRunning = CheckIfPlayerIsRunning();

        // Determine chase distance based on player state (crouching, in light, etc.)
        DetermineChaseDistance();

        if (playerSeen && Vector3.Distance(transform.position, player.position) <= normalChaseDistance)
        {
            StartChasingPlayer(); // Start chasing when the player is detected
        }

        if (isChasing)
        {
            chaseTimer -= Time.deltaTime; // Countdown chase time

            if (chaseTimer <= 0)
            {
                isChasing = false; // Stop chasing after time runs out
            }
            else
            {
                // Move toward the player while chasing
                ChasePlayer();
            }
        }
        else
        {
            // Patrol when not chasing
            Patrol();
        }
    }

    private void StartChasingPlayer()
    {
        if (!isChasing)
        {
            
            isChasing = true;
            chaseTimer = chaseDuration; // Reset the chase timer
        }
    }

    private void Patrol()
    {
        Vector3 targetPosition = patrolPoints[currentPatrolIndex].position;
        Vector3 direction = (targetPosition - transform.position).normalized;

        transform.position += direction * patrolSpeed * Time.deltaTime;

        // Check if the monster has reached the patrol point
        if (Vector3.Distance(transform.position, targetPosition) < 1f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length; // Loop through patrol points
        }

        // Optionally rotate towards patrol direction
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    private void ChasePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * chaseSpeed * Time.deltaTime;

        // Rotate towards the player
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    private bool CheckIfPlayerInLight()
    {
        
        return player.GetComponent<PlayerMovement>().inLight; // Placeholder
    }

    private bool CheckIfPlayerIsCrouching()
    {
        return player.GetComponent<PlayerMovement>().state == PlayerMovement.MovementState.crouching;
    }

    private bool CheckIfPlayerIsRunning()
    {
        return player.GetComponent<PlayerMovement>().state == PlayerMovement.MovementState.sprinting || player.GetComponent<PlayerMovement>().state == PlayerMovement.MovementState.sliding;
    }

    private void DetermineChaseDistance()
    {
        // Adjust chase distance based on player state (light, crouching, etc.)
        normalChaseDistance = 10f; // Default chase distance
        if (playerCrouching)
        {
            normalChaseDistance = 5f;
        }
        if(playerRunning)
        {
            normalChaseDistance = 15f;
        }

        if (playerInLight)
        {
            normalChaseDistance *= 1.5f; // Increase chase distance if player is in light
        }

        normalChaseDistance = Mathf.Min(normalChaseDistance, maxChaseDistance); // Clamp to max distance
    }

    private bool CheckIfPlayerSeen()
    {
        RaycastHit hit;
        Vector3 directionToPlayer = player.position - transform.position;
        if (Physics.Raycast(transform.position, directionToPlayer.normalized, out hit, normalChaseDistance))
        {
            // Check if the ray hit the player
            if (hit.collider.CompareTag("Player"))
            {
                return true; // Player is seen
            }
        }
        return false; // Player is not visible
    }
}
