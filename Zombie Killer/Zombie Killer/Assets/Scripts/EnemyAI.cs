using System.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using DG.Tweening;

public class EnemyAI : MonoBehaviour
{
    //public List<Transform> WayPoints;

    //[SerializeField] int wayPointIndex;
    [SerializeField] Transform Target;
    [SerializeField] playercontroller Player;
    [SerializeField] GameObject _healthPickup;
    [SerializeField] GameObject _ammoPickup;
    public float health;
    public float nearDistance;
    public float farDistance;

    Animator animator;
    Rigidbody rigidbody;
    NavMeshAgent agent;

    float attackTimer;

    bool attack = false;
    bool isPain;

    //[SerializeField] Transform CurrenTarget;

    //public bool enmenyWalkWaypoint;

    float distance;
    float distanceFromPlayer;
    bool canWander = true;
    bool chk;

    float soundCounter;

    enum EnemyStates
    {
        Patrolling,
        Chasing,
        Attacking,
        Dead,
        Pain
    };

    [Header("Audio Clips")]
    public AudioClip Attack;
    public AudioClip Die;
    public AudioClip Idle1;


    [SerializeField] EnemyStates enemystates;

    #region SINGLETON
    public static EnemyAI instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    #endregion


    private void Start()
    {


        rigidbody = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        Target = GameplayHandler.Instance._playerController;
        Player = GameplayHandler.Instance._playerController.GetComponent<playercontroller>();

        //for (int i = 0; i < GamePlayHandler.instance.gameLevel[GameManager.instance.levelSelected].EnemyWaypoints.Length; i++)
        //{
        //    WayPoints.Add(GamePlayHandler.instance.gameLevel[GameManager.instance.levelSelected].EnemyWaypoints[i]);
        //}

        //CurrenTarget = WayPoints[wayPointIndex];
        //agent.SetDestination(CurrenTarget.position);
    }


    private void Update()
    {
        soundCounter += Time.deltaTime;

        if (!isPain)
        {

            if(soundCounter >= Random.Range(5 , 10))
            {
                SoundManager.instance.PlayEffect(Idle1);
                soundCounter = 0;
            }
            attackTimer += Time.deltaTime;
            //agent.SetDestination(Target.position);
            distanceFromPlayer = Vector3.Distance(transform.position, Target.position);

            if (distanceFromPlayer > nearDistance && distanceFromPlayer <= farDistance)
            {

                //Debug.Log("Chasing");
                enemystates = EnemyStates.Chasing;
            }
            else if (distanceFromPlayer <= nearDistance)
            {
                //Debug.Log("Attacking");
                enemystates = EnemyStates.Attacking;

            }

            if (enemystates == EnemyStates.Chasing)
            {
                agent.SetDestination(Target.position);
                agent.speed = 2f;
                animator.SetBool("IsRunning", true);
                animator.SetBool("IsAttack", false);
            }
            if (enemystates == EnemyStates.Attacking)
            {
                agent.SetDestination(transform.position);
                animator.SetBool("IsRunning", false);
                if (attackTimer > 3)
                {
                    attackTimer = 0;
                    animator.SetBool("IsAttack", true);
                    AttackPlayer();
                }

            }
            if (enemystates == EnemyStates.Pain)
            {
                Debug.Log("PAIN");
                agent.isStopped = true;
                isPain = true;
                animator.SetBool("isPain", true);
                Invoke(nameof(PainEnded), 1f);
            }

        }
    }


    public void AttackPlayer()
    {
        if (Player.hitpoints > 0)
        {
                SoundManager.instance.PlayEffect(Attack);
                Player.Damage(Random.Range(1, 3));
        }
    }

    public float TakeDamage(float damage)
    {

        // SoundManager.instance.PlayVocal(AudioClipsSource.Instance.zombiePain);

        //int hitEffectAnimationChance = Random.Range(0, 10);
        //if (hitEffectAnimationChance > 5)
        //{
        //    agent.isStopped = true;
        //    isPain = true;
        //    animator.SetBool("isPain", true);
        //    Invoke(nameof(PainEnded),1f);
        //}

        if (health > 0)
        {
            health -= damage;
        }
        else if (health <= 0)
        {
            enemystates = EnemyStates.Dead;
            OnDead();

        }
        return health;
    }

    public void PainEnded()
    {
        isPain = false;
        animator.SetBool("isPain", false);
        enemystates = EnemyStates.Chasing;
        agent.isStopped = false;
    }

    public void OnDead()
    {

        agent.isStopped = true;
        agent.speed = 0f;
        canWander = false;
        agent.SetDestination(this.gameObject.transform.position);
        animator.SetBool("Isdead", true);
        gameObject.tag = "Untagged";
        gameObject.layer = 0;
        animator.SetBool("IsRunning", false);
        animator.SetBool("IsAttack", false);
        Destroy(gameObject, 1f);
        SoundManager.instance.PlayEffect(Attack);

        SpawnPickup();

        if(GameManager.Instance.SelectedMode == 1)
            EnemyKillCounter.Instance.IncrementKill();

    }

    void SpawnPickup()
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);

        int randomizer = Random.Range(0, 10);
        if (randomizer < 5)
        {
            var pickup = Instantiate(_healthPickup, pos, _healthPickup.transform.rotation);
            pickup.transform.parent = null;
        }
        else
        {
            var pickup = Instantiate(_ammoPickup, pos, _ammoPickup.transform.rotation);
            pickup.transform.parent = null;
        }
    }



    public Collider[] allCollider;

    // all the rigidbodies used by ragdoll
    public List<Rigidbody> ragdollRigidBodies;

    // animator used to controll different animation state of the character
    public Animator anim;
    public NavMeshAgent Agent;

    /// <summary>
    /// this stores reference of all the collider and attached rigidbodies used by ragdoll
    /// </summary>
    private void RagDoollActive()
    {
        allCollider = GetComponentsInChildren<Collider>(true); // get all the colliders that are attached
        foreach (var collider in allCollider)
        {
            if (collider.transform != transform) // if this is not parent transform
            {
                var rag_rb = collider.GetComponent<Rigidbody>(); // get attached rigidbody
                if (rag_rb)
                {
                    ragdollRigidBodies.Add(rag_rb); // add to list
                }
            }
        }
    }

    public void EnableRagdoll(bool enableRagdoll)
    {
        anim.enabled = !enableRagdoll;
        Agent.enabled = !enableRagdoll;

        foreach (Collider item in allCollider)
        {
            item.enabled = enableRagdoll; // enable all colliders  if ragdoll is set to enabled
        }

        foreach (var ragdollRigidBody in ragdollRigidBodies)
        {
            ragdollRigidBody.useGravity = enableRagdoll; // make rigidbody use gravity if ragdoll is active
            ragdollRigidBody.isKinematic = !enableRagdoll; // enable or disable kinematic accordig to enableRagdoll variable
        }

        // foreach(Collider item in coll) 
        // {
        //     item.enabled = !enableRagdoll; // flip the normal colliders active state
        // } 
        GetComponent<Rigidbody>().useGravity = !enableRagdoll; // normal rigidbody dont use gravity when ragdoll is active
        GetComponent<Rigidbody>().isKinematic = enableRagdoll;
    }
}
