using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{

    public int maxHP = 10, curHP = 10;

    public HPBar hpbar;

    public GameObject player;
    private PlayerController pc;


    public AudioClip[] hurtSounds;
    public AudioClip[] dieSounds;
    AudioSource audioSource;

    Rigidbody rb;

    public Animator anim;

    bool isDead;
    public float despawnTime;
    float despawnTimer;
    Vector3 baseScale;

    public float attackCooldown;

    public EnemyAttack attackHitbox;

    bool attacking;


    // THIS SCRIPT DOESN'T ACCOUNT FOR GRAVITY HOWEVER IT SHOULDN'T BE A PROBLEM JUST BE SURE TO LOCK THE Y POSITION IF THERE NEEDS TO BE GRAVITY

    // Create NavMeshAgent on Enemy Add it to this value
    public NavMeshAgent agent;

    // NOTE YOU WILL NEED TO MAKE AN EMPTY GAMEOBJECT CALLED NAVMESH AND PLACE A NAVMESH SURFACE INTO IT, BE SURE TO DROPDOWN THE OBJECT COLLECTION TAG AND SELECT ONLY GROUND LAYER IN THE "INCLUDE LAYERS" SECTION, if you are confused about layers see the layermask object

    // Create multiple empty game objects as checkpoint places (corners of the maze), place in order where you want enemy to go 1st element first checkpoint, last element crystal
    public List<Transform> checkPoints = new List<Transform>();
    private int currentCheckpoint;

    // Create new layer called ground, make the ground the layer, set this value to the ground layer
    public LayerMask ground;

    private Vector3 nextWalkPoint;
    bool nextWalkPointSet;

    // NOTE SPEED CAN BE CHANGED IN THE NAVMESHAGENT SO DON'T WORRY ABOUT A SPEED VARIABLE

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        currentCheckpoint = 0;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hpbar.SetMaxHP(maxHP);
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = AudioManager.instance.GetSFXVolume();
        baseScale = transform.localScale;
        
    }

    // Update is called once per frame
    void Update()
    {

        //Debug.Log("Run?");
        if (!nextWalkPointSet)
        {
            FindNextCheckpoint();
            //Debug.Log("Find Path");
        }

        if (nextWalkPointSet)
        {
            agent.SetDestination(nextWalkPoint);
            //Debug.Log("Go To Path");
        }

        Vector3 distanceToCheckpoint = transform.position - nextWalkPoint;

        // Checks if enemy is next at checkpoint
        if (distanceToCheckpoint.magnitude < 1f)
        {
            nextWalkPointSet = false;

            // If there is another checkpoint move to point
            if (currentCheckpoint != checkPoints.Count - 1)
            {
                currentCheckpoint++;
            }
        }

        if (isDead)
        {
            despawnTimer += Time.deltaTime;
            transform.localScale = baseScale * (1 - despawnTimer / despawnTime);
        }
    }

    public void setupPlayer()
    {
        pc = player.GetComponent<PlayerController>();
    }

    public void TakeDamage(int damage, Vector3 knockback)
    {
        curHP -= damage;

        hpbar.UpdateHP(curHP);

        int whichSound = Random.Range(0, 3);
        float pitch = Random.Range(0.9f, 1.1f);


        if (curHP <= 0)
        {
            audioSource.clip = dieSounds[whichSound];

            agent.Stop();
            Invoke(nameof(DeleteSelf), despawnTime); //TODO: We'd need to delay this for a death animation
            pc.AddGold(200);
            anim.SetTrigger("Die");
            isDead = true;

            Spawner.instance.RemoveEnemy(this.gameObject);

            gameObject.tag = "None";
            

            //Debug.Log(pc.gold);
        }
        else
        {
            audioSource.clip = hurtSounds[whichSound];
            anim.SetTrigger("Hit");
        }
            

        audioSource.pitch = pitch;
        audioSource.Play();

        rb.AddForce(knockback);
    }

    void DeleteSelf()
    {
        Destroy(this.gameObject);
    }

    private void FindNextCheckpoint()
    {
        nextWalkPoint = new Vector3(checkPoints[currentCheckpoint].position.x, transform.position.y, checkPoints[currentCheckpoint].position.z);

        nextWalkPointSet = true;
    }

    public void Setup(GameObject checkpoints)
    {
        for (int i = 0; i < checkpoints.transform.childCount; i++)
        {
            //Debug.Log(checkpoints.transform.GetChild(i).name);
            checkPoints.Add(checkpoints.transform.GetChild(i).transform);
        }

        checkPoints.Add(checkpoints.transform);
    }


    void Attack()
    {
        anim.SetTrigger("Attack");
        attackHitbox.SetActive(true);
        Invoke(nameof(ResetAttack), attackCooldown);
        attacking = true;
    }

    void ResetAttack()
    {
        attacking = false;
        attackHitbox.SetActive(false);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player" && other.tag != "Crystal")
            return;

        if (attacking)
            return;

        Attack();
    }
}
