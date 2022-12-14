using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrol : MonoBehaviour
{
    // Ai's sight
    public bool playerInSight = false;
    public float fieldOfViewAngle = 160f;
    public float LineOfSightRadius = 45f;

    // Ai's sight and memory
    private bool memorizesPlayer = false;
    public float memoryDuration = 10f;
    private float increasingMemoryTime;

    // Ai hearing
    Vector3 noisePosition;
    private bool heardPlayer = false;
    public float noiseTravelDistance = 50f;
    public float spinSpeed = 3f;
    private bool canSpin = false;
    private float isSpinningTime;
    public float spinTime = 3f;

    // Patrolling randomly between waypoints
    public Transform[] moveSpots;
    private int randomSpot;

    // Wait time at waypoint before patrolling
    private float waitTime;
    public float waitDuration = 1f;
    NavMeshAgent nav;

    // Ai strafe
    public float distanceToPlayer = 5f;

    private float randomStrafeDuration;
    private float waitStrafeTime;
    public float minStrafeTime;
    public float maxStrafeTime;

    public Transform strafeRight;
    public Transform strafeLeft;
    private int randomStrafeDirection;

    // When to chase
    public float chaseRadius = 20f;
    public float facePlayerFactor = 20f;

    // Position
    private Transform playerPosition;

    // Attacking
    private bool attacked;
    public GameObject bullet;
    private float aimTime;
    private float aimDuration = 1;

    // Animations
    private Animator animator;

    // Health
    public float health;
    public float maxHealth;

    Rigidbody rb;
    void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        health = maxHealth;
        playerPosition = GameObject.Find("Player").transform;
        nav.enabled = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        waitTime = waitDuration;
        randomSpot = Random.Range(0, moveSpots.Length);
        randomStrafeDirection = Random.Range(0, 2);
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(playerPosition.position, transform.position);
        
        if(distance <= LineOfSightRadius)
        {
            CheckLineOfSight();
        }

        if (nav.isActiveAndEnabled)
        {
            if (playerInSight == false && memorizesPlayer == false && heardPlayer == false)
            {
                Patrol();
                NoiseCheck();

                StopCoroutine(Memory());
            }
            else if(heardPlayer == true && playerInSight == false && memorizesPlayer == false)
            {
                canSpin = true;
                GoToNoisePosition();
            }
            else if(playerInSight == true)
            {
                memorizesPlayer = true;

                FacePlayer();
                ChasePlayer();
            }
            else if (memorizesPlayer == true && playerInSight == false)
            {
                ChasePlayer();
                StartCoroutine(Memory());
            }
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") || animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
        {
            nav.speed = 1f;
            animator.updateMode = AnimatorUpdateMode.Normal;
        }
        else
        {
            nav.speed = 3.5f;
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Aiming") || animator.GetCurrentAnimatorStateInfo(0).IsName("Shoot") || animator.GetCurrentAnimatorStateInfo(0).IsName("Death") || animator.GetCurrentAnimatorStateInfo(0).IsName("Headshot Death"))
        {
            animator.updateMode = AnimatorUpdateMode.AnimatePhysics;
        }
    }

    void NoiseCheck()
    {
        float distance = Vector3.Distance(playerPosition.position, transform.position);

        if(distance <= noiseTravelDistance)
        {
            if (Input.GetMouseButton(0))
            {
                noisePosition = playerPosition.position;
                heardPlayer = true;
            }
            else
            {
                heardPlayer = false;
                canSpin = false;
            }
        }
    }

    void GoToNoisePosition()
    {
        animator.SetBool("Chasing", true);
        nav.SetDestination(noisePosition);

        if (Vector3.Distance(transform.position, noisePosition) <= 5f && canSpin == true)
        {
            isSpinningTime += Time.deltaTime;
            transform.Rotate(Vector3.up * spinSpeed, Space.World);

            if(isSpinningTime >= spinTime)
            {
                canSpin = false;
                heardPlayer = false;
                isSpinningTime = 0f;
            }
        }
    }

    IEnumerator Memory()
    {
        increasingMemoryTime = 0;

        while(increasingMemoryTime < memoryDuration)
        {
            increasingMemoryTime += Time.deltaTime;
            memorizesPlayer = true;
            yield return null;
        }

        heardPlayer = false;
        memorizesPlayer = false;
    }

    void CheckLineOfSight()
    {
        Vector3 direction = playerPosition.position - transform.position;

        float angle = Vector3.Angle(direction, transform.forward);

        if(angle < fieldOfViewAngle * 0.5f)
        {
            RaycastHit hit;

            if(Physics.Raycast(transform.position, direction.normalized, out hit, LineOfSightRadius))
            {
                
                if(hit.collider.tag == "Player")
                {
                    StartCoroutine(AttackPlayer());
                    playerInSight = true;
                    memorizesPlayer = true;
                }
                else
                {
                    aimTime = 0;
                    animator.SetBool("Aiming", false);
                    playerInSight = false;
                }
            }
        }
    }

    void Patrol()
    {
        animator.SetBool("Walking", true);

        nav.SetDestination(moveSpots[randomSpot].position);

        if(Vector3.Distance(transform.position, moveSpots[randomSpot].position) < 2.0f)
        {
            animator.SetBool("Walking", false);
            if (waitTime <= 0)
            {
                randomSpot = Random.Range(0, moveSpots.Length);

                waitTime = waitDuration;
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }
    }

    void ChasePlayer()
    {
        float distance = Vector3.Distance(playerPosition.position, transform.position);

        if(distance <= chaseRadius && distance > distanceToPlayer)
        {
            animator.SetBool("Chasing", true);
            nav.SetDestination(playerPosition.position);
        }
        else if (nav.isActiveAndEnabled && distance <= distanceToPlayer)
        {
            animator.SetBool("Chasing", false);
            animator.SetBool("Walking", false);
            randomStrafeDirection = Random.Range(0, 2);
            randomStrafeDuration = Random.Range(minStrafeTime, maxStrafeTime);

            if(waitDuration <= 0)
            {
                if(randomStrafeDirection == 0)
                {
                    nav.SetDestination(strafeLeft.position);
                }
                else if(randomStrafeDirection == 1)
                {
                    nav.SetDestination(strafeRight.position);
                }
                waitStrafeTime = randomStrafeDuration;
            }
        }
        else
        {
            animator.SetBool("Chasing", false);
            waitStrafeTime -= Time.deltaTime;
        }
    }

    void FacePlayer()
    {
        Vector3 direction = (playerPosition.position - transform.position).normalized;
        Quaternion lookrotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookrotation, Time.deltaTime * facePlayerFactor);
    }

    IEnumerator AttackPlayer()
    {
        Vector3 direction = playerPosition.position - transform.position;
        nav.SetDestination(transform.position);
        aimTime = 0;

        while (aimTime < aimDuration)
        {
            animator.SetBool("Aiming", true);
            aimTime += Time.deltaTime;
            yield return null;
        }

        if (!attacked && aimTime >= aimDuration)
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, direction.normalized, out hit, LineOfSightRadius))
            {
                animator.SetBool("Shoot", true);
                if (hit.collider.tag == "Player")
                {
                    //Debug.Log("Hit");
                }
            }

            attacked = true;
            Invoke(nameof(ResetAttack), 0.5f);
        }
    }

    void ResetAttack()
    {
        attacked = false;
        animator.SetBool("Shoot", false);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Damage taken: " + damage);

        StartCoroutine(Death());
    }

    IEnumerator Death()
    {
        animator.Play("Death");
        this.enabled = false;
        nav.enabled = false;
        yield return new WaitForSeconds(3f);
        animator.enabled = false;
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }
}
