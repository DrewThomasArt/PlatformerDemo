using System.Collections;
using System.Collections.Generic;
using PlatformerCombat;
using UnityEngine;

    public class SquashableEnemyController : MonoBehaviour
    {
        Stats stats;
        Animator anim;
        EnemyCombat enemyCombat;

        Transform enemySprite;
        [HideInInspector] public Rigidbody2D rb { get; set; }

        [Header("Variables")]
        public float moveSpeed;
        bool isMoving;
        public bool isGrounded;

        [HideInInspector] public bool isAttacking { get; set; }
        public float returnRadius = 2; //radius that enemy can leave b4 returning to start
        bool isReturning;
        Vector3 starterPos;
        public bool isPatrolling;
        public float patrolRadius;

        //Patrol variables
        float patrolSin;
        float patrolTimer = 0.02f;
        bool isFollow;
        [HideInInspector] public Transform followTarget;
        float returnTimer = 3;
        bool canMove;

        [Header("Squashing Variables")]
        public Sprite SpriteCrunched;
        public bool Crunched = false;
        public AudioClip CrunchSound;
        private AudioSource audioSource;
        private float timeCrunched;
        private CircleCollider2D circleCollider;

        private void Start()
        {
            canMove = true;

            starterPos = transform.position;
            stats = GetComponent<Stats>();
            rb = GetComponent<Rigidbody2D>();
            enemySprite = transform;
            anim = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
            circleCollider = GetComponent<CircleCollider2D>();

            //Add Death method to event when enemy die 
            //stats.OnDeath += Death;
        }

        public void FixedUpdate()
        {
            if (canMove)
            Move();
        }

        private void Update()
        {
            if (canMove)
            {
            Rotation();
            }

        if (Crunched)
        {
            canMove = false;
            anim.enabled = false;
            GetComponent<SpriteRenderer>().sprite = SpriteCrunched;
            // Make the object remain steady 
            rb.velocity = Vector2.zero;
            if (timeCrunched <= 0)
                timeCrunched = Time.time;
                else
            {
                float secondsSinceCrunched = Time.time - timeCrunched;
                if (secondsSinceCrunched > 2f)
                {
                    Destroy(gameObject);
                }
            }
        }
        }

        public void Move()
        {
            CheckGround();

                isMoving = true;

            if (isPatrolling && !isFollow && !isReturning)
            {
                PatrolSin();
                Patrol();
            }
        }

        //The patrol is made using a sinusoid (meaning, for our purposes, within a range going a little over 0 or a little under 0, which will be the enemy walking a little over and under their initial start pos)
        void PatrolSin()
        {
            patrolSin += patrolTimer * moveSpeed;

            if (patrolSin >= 1)
            {
                patrolTimer = -0.02f;
            }
            else if (patrolSin <= -1)
            {
                patrolTimer = 0.02f;
            }
        }

        //Follow method
        public void Follow(Transform target)
        {
            followTarget = target; //set target to follow

            if (!isFollow)
            {
                isFollow = true;
                StartCoroutine(IFollow());
            }

        }

        void Patrol()
        {
            float sin = patrolSin; //local sin varible
            float x = patrolRadius * sin + starterPos.x; //set x of enemy position

            float y = transform.position.y;
            float z = transform.position.z;

            transform.position = new Vector3(x, y, z); //Change position
        }

        public void Rotation()
        {
            if (isPatrolling)
            {
                if (patrolTimer > 0) //Side of patrol
                {
                    enemySprite.transform.localScale = new Vector3(1, 1, 1);
                }
                else //reverse
                {
                    enemySprite.transform.localScale = new Vector3(-1, 1, 1);
                }
            }

            if (isFollow) //get target and check the difference between them
            {
                if (transform.position.x > followTarget.position.x)
                {
                    enemySprite.transform.localScale = new Vector3(-1, 1, 1);
                }
                else
                {
                    enemySprite.transform.localScale = new Vector3(1, 1, 1);
                }
            }

            if (isReturning) //side of starter position
            {
                if (transform.position.x > starterPos.x)
                {
                    enemySprite.transform.localScale = new Vector3(-1, 1, 1);
                }
                else
                {
                    enemySprite.transform.localScale = new Vector3(1, 1, 1);
                }
            }
        }


        //Death method
        public void Death()
        {
            canMove = false; //block movement
            enemyCombat.enabled = false; //disable combat
            rb.simulated = false;//disable physics

        }

        public void CheckGround()
        {
            float rayStartXoffset = 0.14f; //Offset ray to check ground
            float rayStartX;

            if (patrolTimer > 0) //get rotation
            {
                rayStartX = transform.position.x + rayStartXoffset;
            }
            else
            {
                rayStartX = transform.position.x - rayStartXoffset;
            }

            Vector3 rayStartPos = new Vector3(rayStartX, transform.position.y, transform.position.z); //Raycast start position

            RaycastHit2D raycastHit2D = Physics2D.Raycast(rayStartPos, Vector2.down, 2f); //Raycast

            if (raycastHit2D.collider != null) //If ray hits object
            {
                if (Vector2.Distance(rayStartPos, raycastHit2D.point) <= raycastHit2D.distance) //if distance between object and enemy < local distance variable 
                {
                    Debug.DrawLine(rayStartPos, raycastHit2D.point);
                    isGrounded = true; 
                }

                if (raycastHit2D.collider.gameObject.tag != "Ground")
                {
                    isGrounded = false;
                    patrolTimer = -patrolTimer;
                }

            }
            else
            {
                isGrounded = false;
                patrolTimer = -patrolTimer; //turn enemy in the opposite direction
            }
        }

        IEnumerator IFollow()
        {
            float timer = returnTimer; //timer to return after player leaves

            while (isFollow)
            {
                if (Vector2.Distance(transform.position, followTarget.position) > returnRadius && timer <= 0) //if target leave
                {
                    isFollow = false;
                    followTarget = null;

                    isReturning = true;

                    patrolSin = 0;

                    StartCoroutine(IReturnToStartPos()); //start returning
                }
                else if (!isGrounded) //if the target jumps over the gap  
                {
                    isFollow = false;
                    followTarget = null;

                    isReturning = true;

                    patrolSin = 0;

                    StartCoroutine(IReturnToStartPos());
                }
                else
                {
                    //if (Vector2.Distance(transform.position, followTarget.position) > attackRadius && !enemyCombat.isAttacking)
                        transform.position = Vector2.MoveTowards(transform.position, new Vector2(followTarget.position.x, transform.position.y), moveSpeed / 2 * Time.deltaTime); //follow target
                    // else //if target's in attack radius
                    //     Attack();

                    timer -= Time.deltaTime;
                }
                yield return null;
            }
            yield return null;
        }

        IEnumerator IReturnToStartPos()
        {
            while (transform.position.x != starterPos.x) 
            {
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(starterPos.x, transform.position.y), moveSpeed / 2 * Time.deltaTime); //move to start pos

                yield return null;
            }

            isReturning = false;
            yield return null;
        }

        //Just editor method for drawing variables
        public void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, returnRadius);
        }

    public void Squash()
    {
        if (!this.Crunched)
        {
            this.Crunched = true;
            audioSource.clip = CrunchSound;
            audioSource.Play();
            rb.velocity = Vector2.zero;

            rb.simulated = false;
            circleCollider.enabled = false;
        }
    } 
    }
