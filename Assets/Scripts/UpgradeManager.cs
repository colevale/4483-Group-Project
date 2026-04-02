using TMPro;
using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;

public class UpgradeManager : MonoBehaviour
{
    public GameObject upgradeMenu;

    [Header("Gold Text Warning Variables")]
    public GameObject goldWarningText;
    public float waitTime;
    public bool displayGoldWarning = false;

    [Header("Upgrade Buttons")]
    public GameObject fireRateButton;
    public GameObject damageButton;
    public GameObject projectileSpeedButton;
    public GameObject knockbackButton;

    [Header("Upgrade Image Holders")]
    public GameObject fireRateButtonHolder;
    public GameObject damageButtonHolder;
    public GameObject projectileSpeedButtonHolder;
    public GameObject knockbackButtonHolder;

    [Header("Button Text")]
    public TextMeshProUGUI fireRateText;
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI projectileSpeedText;
    public TextMeshProUGUI knockbackText;

    [Header("Upgrade Prices")]
    public int[] fireRatePrices;
    public int[] damagePrices;
    public int[] projectileSpeedPrices;
    public int[] knockbackPrices;

    private List<RawImage> fireRateColor = new List<RawImage>();
    private List<RawImage> damageColor = new List<RawImage>();
    private List<RawImage> projectileSpeedColor = new List<RawImage>();
    private List<RawImage> knockbackColor = new List<RawImage>();

    private int fireRateUpgradeLevel = 0;
    private int damageUpgradeLevel = 0;
    private int projectileSpeedUpgradeLevel = 0;
    private int knockbackUpgradeLevel = 0;

    private float timer;
    private bool isOpen = false;

    private PlayerController player;
    private int damage = 2;
    private float projectileSpeed = 10;
    private float knockback = 50;

    private void Awake()
    {
        GoldWarningText(false);
        player = GameObject.Find("Player").GetComponent<PlayerController>();

        GetImages(fireRateButtonHolder, fireRateColor);
        GetImages(damageButtonHolder, damageColor);
        GetImages(projectileSpeedButtonHolder, projectileSpeedColor);
        GetImages(knockbackButtonHolder, knockbackColor);

        UpgradeMenu(false);
    }

    private void Start()
    {
        fireRateText.SetText("Fire Rate\n" + "($" + fireRatePrices[fireRateUpgradeLevel] + ")");
        damageText.SetText("Damage\n" + "($" + damagePrices[damageUpgradeLevel] + ")");
        projectileSpeedText.SetText("Projectile Speed\n" + "($" + projectileSpeedPrices[projectileSpeedUpgradeLevel] + ")");
        knockbackText.SetText("Knockback\n" + "($" + knockbackPrices[knockbackUpgradeLevel] + ")");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (isOpen)
            {
                UpgradeMenu(false);
            }
            else
            {
                UpgradeMenu(true);
            }

            isOpen = !isOpen;
        }

        if (displayGoldWarning)
        {
            GoldWarningText(true);
            timer += Time.deltaTime;

            if (timer >= waitTime)
            {
                timer = 0;
                GoldWarningText(false);
                displayGoldWarning = false;
            }
        }
    }

    public void UpgradeMenu(bool onOff)
    {
        if (onOff)
        {
            upgradeMenu.SetActive(true);
        }
        else
        {
            upgradeMenu.SetActive(false);
        }
    }

    public void GoldWarningText(bool onOff)
    {
        if (onOff)
        {
            goldWarningText.SetActive(true);
        }
        else
        {
            goldWarningText.SetActive(false);
        }
    }

    public void ButtonClick(string button)
    {
        switch (button)
        {
            case "firerate":
                if (Upgrade(fireRateUpgradeLevel, fireRatePrices[fireRateUpgradeLevel]))
                {
                    fireRateColor[fireRateUpgradeLevel].color = Color.green;
                    player.UpgradeShotTime((float) 0.1);
                    fireRateUpgradeLevel++;
                    if (fireRateUpgradeLevel != 5)
                    {
                        fireRateText.SetText("Fire Rate\n" + "($" + fireRatePrices[fireRateUpgradeLevel] + ")");
                    }
                    else
                    {
                        fireRateText.SetText("Fire Rate");
                        fireRateButton.GetComponent<Image>().color = Color.gray;
                        fireRateButton.GetComponent<Button>().interactable = false;
                    }
                }
                break;
            case "damage":
                if (Upgrade(damageUpgradeLevel, damagePrices[damageUpgradeLevel]))
                {
                    damageColor[damageUpgradeLevel].color = Color.green;
                    damage += 2;
                    damageUpgradeLevel++;
                    if (damageUpgradeLevel != 5)
                    {
                        damageText.SetText("Damage\n" + "($" + damagePrices[damageUpgradeLevel] + ")");
                    }
                    else
                    {
                        damageText.SetText("Damage");
                        damageButton.GetComponent<Image>().color = Color.gray;
                        damageButton.GetComponent<Button>().interactable = false;
                    }
                }
                break;
            case "projectilespeed":
                if (Upgrade(projectileSpeedUpgradeLevel, projectileSpeedPrices[projectileSpeedUpgradeLevel]))
                {
                    projectileSpeedColor[projectileSpeedUpgradeLevel].color = Color.green;
                    projectileSpeed += 10;
                    projectileSpeedUpgradeLevel++;
                    if (projectileSpeedUpgradeLevel != 5)
                    {
                        projectileSpeedText.SetText("Projectile Speed\n" + "($" + projectileSpeedPrices[projectileSpeedUpgradeLevel] + ")");
                    }
                    else
                    {
                        projectileSpeedText.SetText("Projectile Speed");
                        projectileSpeedButton.GetComponent<Image>().color = Color.gray;
                        projectileSpeedButton.GetComponent<Button>().interactable = false;
                    }
                }
                break;
            case "knockback":
                if (Upgrade(knockbackUpgradeLevel, knockbackPrices[knockbackUpgradeLevel]))
                {
                    knockbackColor[knockbackUpgradeLevel].color = Color.green;
                    knockback += 10;
                    knockbackUpgradeLevel++;
                    if (knockbackUpgradeLevel != 5)
                    {
                        knockbackText.SetText("Knockback\n" + "($" + knockbackPrices[knockbackUpgradeLevel] + ")");
                    }
                    else
                    {
                        knockbackText.SetText("Knockback");
                        knockbackButton.GetComponent<Image>().color = Color.gray;
                        knockbackButton.GetComponent<Button>().interactable = false;
                    }
                }
                break;
            default:
                return;
        }
    }

    public bool Upgrade(int level, int price)
    {
        if (level <= 4)
        {
            if (price <= player.GetGold())
            {
                player.RemoveGold(price);
                return true;
            }
            else
            {
                displayGoldWarning = true;
            }
        }

        return false;
    }

    private void ChangeColor(List<RawImage> whichHolder, int atWhere)
    {
        whichHolder[atWhere].color = Color.green;
    }

    private void GetImages(GameObject holder, List<RawImage> getImages)
    {
        foreach (Transform child in holder.transform)
        {
            RawImage tempImage = child.GetComponent<RawImage>();
            tempImage.color = Color.red;
            getImages.Add(tempImage);
        }
    }
    
    public int getCurrDamage()
    {
        return damage;
    }

    public float getCurrProjSpeed()
    {
        return projectileSpeed;
    }

    public float getCurrKnockback()
    {
        return knockback;
    }
}
