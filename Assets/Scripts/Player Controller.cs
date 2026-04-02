using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{



    public static PlayerController playcon;

    public bool fixedMouse = false;

    public float jumpForce = 500;
    public float moveSpeed;
    public float extraGravity;

    //  upgrades
    public float timeBetweenShots;
    public bool readyToShoot;

    public int gold;
    public TMP_Text goldDisplay;
    public GameObject wave;


    public Transform camera;
    Vector3 prevMouse;


    
    Rigidbody rb;


    public GameObject towerPrefab;
    public GameObject bulletPrefab;

    public Transform gunBarrel;


    public ColliderList groundedCheck;


    bool nearCrystal;
    public CrystalActivation crystalAct;

    private void Awake()
    {
        readyToShoot = true;
        timeBetweenShots = (float) 0.75;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gold = PlayerPrefs.GetInt("Gold"); //gold from previous campaigns
        Cursor.lockState = CursorLockMode.Locked;
        if (playcon == null)
            playcon = this;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("l"))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;


        }

        if (Input.GetKeyDown("u"))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;


        }

        float horiz = Input.GetAxis("Horizontal");
        float vert = Input.GetAxis("Vertical");

        Vector3 mouseDelta = Input.mousePositionDelta;
        if (!Input.GetButton("Exit") && fixedMouse)
            Mouse.current.WarpCursorPosition(Vector2.zero);

        transform.Rotate(0, mouseDelta.x, 0);

        camera.Rotate(-mouseDelta.y, 0, 0);
        if (camera.localRotation.eulerAngles.x < 320 && camera.localRotation.eulerAngles.x > 180)
            camera.localRotation = Quaternion.Euler(320, 0, 0);
        if (camera.localRotation.eulerAngles.x > 40 && camera.localRotation.eulerAngles.x < 180)
            camera.localRotation = Quaternion.Euler(40, 0, 0);
        

        rb.AddForce(transform.right * Time.deltaTime * moveSpeed * horiz);
        rb.AddForce(transform.forward * Time.deltaTime * moveSpeed * vert);

        bool jump = Input.GetButtonDown("Jump");

        if (jump && !groundedCheck.isEmpty())
            rb.AddForce(jumpForce * Vector3.up);

        if (groundedCheck.isEmpty())
        {
            rb.AddForce(-transform.up * Time.deltaTime * extraGravity);
        }

        bool shoot = Input.GetButtonDown("Shoot");

        if (shoot)
        {
            if (nearCrystal)
                WaveStart();
            else
            {
                if (readyToShoot)
                {
                    readyToShoot = false;

                    Projectile tempBullet = Instantiate<GameObject>(bulletPrefab).GetComponent<Projectile>();

                    tempBullet.transform.position = gunBarrel.position;
                    tempBullet.Shoot(camera.rotation);

                    Invoke("ResetShot", timeBetweenShots);
                }
            }
                
        }

        /* //infinite towers
        bool placeTower = Input.GetButtonDown("PlaceTower");

        if (placeTower)
        {
            Tower tempTower = Instantiate<GameObject>(towerPrefab).GetComponent<Tower>();

            tempTower.transform.position = new Vector3(gunBarrel.position.x, 1.5f, gunBarrel.position.z);
            tempTower.transform.rotation = transform.rotation;
        }*/


        if (Input.GetButtonDown("Exit"))
            Application.Quit();
    }

    private void FixedUpdate()
    {
        updateGoldDisplay();
    }

    public void SetNearCrystal(bool near)
    {
        nearCrystal = near;
    }


    public void WaveStart()
    {
        crystalAct.WaveStart();

        print("HERES WHERE THE WAVE WOULD START");
        WaveManager manager = wave.GetComponent<WaveManager>() ;
        manager.StartWave();
        Debug.Log("Wave has started");
    }

    public void ResetShot()
    {
        readyToShoot = true;
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
}
