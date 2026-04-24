using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public static PlayerController playcon;

    [Header("Movement")]
    public float moveSpeed;

    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    public bool upgrading;

    public Gun gun;

    //  upgrades
    public float timeBetweenShots;

    public int gold;
    public TMP_Text goldDisplay;
    public GameObject wave;


    public Transform camera;
    Vector3 prevMouse;


    public GameObject towerPrefab;


    bool nearCrystal;
    public CrystalActivation crystalAct;

    public HPBar hpbar;

    public int maxHP;
    int curHP;

    bool playerDead = false;
    public float respawnTime = 5;
    float respawenTimeCur;

    bool crystalDead = false;

    public Animator deathAnim;
    Vector3 spawnPoint;

    public Animator crystalDeathAnim;

    public TextMeshProUGUI respawnText1;
    public TextMeshProUGUI respawnText2;

    private void Awake()
    {
        timeBetweenShots = (float) 0.75;
        
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (playcon == null)
            playcon = this;

        gold = PlayerPrefs.GetInt("gold"); //gold from previous campaigns
        Cursor.lockState = CursorLockMode.Locked;
        if (playcon == null)
            playcon = this;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;
        upgrading = false;


        curHP = maxHP;
        hpbar.SetMaxHP(maxHP);
        hpbar.UpdateHP(curHP);

        spawnPoint = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (crystalDead)
        {

            if (Input.GetButtonDown("Shoot") || Input.GetKeyDown(KeyCode.E))
            {
                AudioManager.instance.PlaySound("menu_back");
                AudioManager.instance.TransitionSong("menu", 2, 5);
                SceneManager.LoadScene("MainMenu");
            }

            return;
        }


        //if the player is dead, accept no inputs
        if (playerDead)
        {
            respawenTimeCur -= Time.deltaTime;
            if (respawenTimeCur < 0)
            {
                playerDead = false;
                curHP = maxHP;
                deathAnim.SetBool("OnOff", false);
                transform.position = spawnPoint;
                respawenTimeCur = 0;
                rb.linearDamping = 0;
            }

            rb.linearDamping = 1000;
            respawnText1.text = "You Died!\nRespawn in: " + (int)(respawenTimeCur+1) + "s";
            respawnText2.text = "You Died!\nRespawn in: " + (int)(respawenTimeCur+1) + "s";


            return;
        }



        if (!upgrading)
        {
            bool wasGrounded = grounded;
            grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

            if (!grounded && grounded)
                AudioManager.instance.PlaySound("player_land");

            MyInput();
            SpeedControl();

            if (grounded)
            {
                rb.linearDamping = groundDrag;
            }
            else
            {
                rb.linearDamping = 0;
            }

            //Gun stuff
            bool shoot = Input.GetButtonDown("Shoot");
            if (shoot)
            {
                gun.Shoot(camera.rotation);
            }
            gun.UpdateSpeed(rb.linearVelocity.magnitude);
        }


        if (Input.GetButtonDown("Exit"))
            Application.Quit();
    }

    private void FixedUpdate()
    {
        MovePlayer();
        updateGoldDisplay();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(KeyCode.Space) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        else if (!grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        AudioManager.instance.PlaySound("player_jump");
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    public void SwitchUpgrading()
    {
        upgrading = !upgrading;
    }

    public void UpgradeShotTime(float upgradeBy)
    {
        timeBetweenShots -= upgradeBy;
    }

    public void AddGold(int value)
    {
        gold = gold + value;
        updateGoldDisplay();
    }

    public void RemoveGold(int value)
    {
        gold = gold - value;
        updateGoldDisplay();
    }

    public int GetGold()
    {
        return gold;
    }

    private void updateGoldDisplay()
    {
        goldDisplay.text = gold.ToString();
    }

    public void SaveGold()
    {
        PlayerPrefs.SetInt("gold", gold);
    }


    public void TakeDamage(int damage, Vector3 knockback)
    {
        curHP -= damage;

        if (curHP <= 0)
        {
            curHP = 0;
            AudioManager.instance.PlaySound("player_die");
            deathAnim.SetBool("OnOff", true);
            respawenTimeCur = respawnTime;
            playerDead = true;
            rb.linearVelocity = Vector3.zero;
        }    
        else
        {
            AudioManager.instance.PlaySound("player_hurt");
            rb.AddForce(knockback);
        }

        
        hpbar.UpdateHP(curHP);
        

        
    }

    public void Heal()
    {
        curHP = maxHP;
        hpbar.UpdateHP(curHP);
    }

    public void GameLose()
    {
        crystalDeathAnim.SetBool("OnOff", true);
        crystalDead = true;
        Debug.Log("Game Lost");
    }

    public bool IsDead()
    {
        return playerDead;
    }
}
