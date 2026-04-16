using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

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

    private void Awake()
    {
        timeBetweenShots = (float) 0.75;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gold = PlayerPrefs.GetInt("gold"); //gold from previous campaigns
        Cursor.lockState = CursorLockMode.Locked;
        if (playcon == null)
            playcon = this;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;
        upgrading = false;
    }

    // Update is called once per frame
    void Update()
    {
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
}
