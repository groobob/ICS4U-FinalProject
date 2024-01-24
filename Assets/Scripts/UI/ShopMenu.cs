/*
 * A script to handle the shop menu
 * 
 * @author Richard
 * @version January 24
 */

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopMenu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject speedSlider;
    [SerializeField] GameObject damageSlider;
    [SerializeField] GameObject healthSlider;
    [SerializeField] int speedUpgradeCost;
    [SerializeField] int damageUpgradeCost;
    [SerializeField] int healthUpgradeCost;
    [SerializeField] TextMeshProUGUI speedUpgradeCostText;
    [SerializeField] TextMeshProUGUI damageUpgradeCostText;
    [SerializeField] TextMeshProUGUI healthUpgradeCostText;
    [SerializeField] Button speedButton;
    [SerializeField] Button damageButton;
    [SerializeField] Button healthButton;
    [SerializeField] TextMeshProUGUI currentBalanceText;

    [Header("Values")]
    [SerializeField] Color canBuyColour;
    [SerializeField] Color cannotBuyColour;

    private void Awake()
    {
        UpdateData();
    }

    private void UpdateData()
    {
        speedSlider.GetComponent<Slider>().value = DataManager.Instance.GetShop(DataManager.upgrade.speed);
        damageSlider.GetComponent<Slider>().value = DataManager.Instance.GetShop(DataManager.upgrade.damage);
        healthSlider.GetComponent<Slider>().value = DataManager.Instance.GetShop(DataManager.upgrade.health);
        speedSlider.GetComponentInChildren<TextMeshProUGUI>().text = DataManager.Instance.GetShop(DataManager.upgrade.speed) + "/" + speedSlider.GetComponent<Slider>().maxValue;
        damageSlider.GetComponentInChildren<TextMeshProUGUI>().text = DataManager.Instance.GetShop(DataManager.upgrade.damage) + "/" + damageSlider.GetComponent<Slider>().maxValue;
        healthSlider.GetComponentInChildren<TextMeshProUGUI>().text = DataManager.Instance.GetShop(DataManager.upgrade.health) + "/" + healthSlider.GetComponent<Slider>().maxValue;
        speedUpgradeCostText.text = "$" + speedUpgradeCost;
        damageUpgradeCostText.text = "$" + damageUpgradeCost;
        healthUpgradeCostText.text = "$" + healthUpgradeCost;
        if (DataManager.Instance.money < speedUpgradeCost || speedSlider.GetComponent<Slider>().value == speedSlider.GetComponent<Slider>().maxValue)
        {
            ColorBlock cb = speedButton.colors;
            cb.normalColor = cannotBuyColour;
            cb.selectedColor = cannotBuyColour;
            cb.disabledColor = cannotBuyColour;
            speedButton.colors = cb;
            speedButton.interactable = false;
        }
        else
        {
            ColorBlock cb = speedButton.colors;
            cb.normalColor = canBuyColour;
            cb.selectedColor = canBuyColour;
            speedButton.colors = cb;
            speedButton.interactable = true;
        }
        if (DataManager.Instance.money < damageUpgradeCost || damageSlider.GetComponent<Slider>().value == damageSlider.GetComponent<Slider>().maxValue)
        {
            ColorBlock cb = damageButton.colors;
            cb.normalColor = cannotBuyColour;
            cb.selectedColor = cannotBuyColour;
            cb.disabledColor = cannotBuyColour;
            damageButton.colors = cb;
            damageButton.interactable = false;

        }
        else
        {
            ColorBlock cb = damageButton.colors;
            cb.normalColor = canBuyColour;
            cb.selectedColor = canBuyColour;
            damageButton.colors = cb;
            damageButton.interactable = true;

        }
        if (DataManager.Instance.money < healthUpgradeCost || healthSlider.GetComponent<Slider>().value == healthSlider.GetComponent<Slider>().maxValue)
        {
            ColorBlock cb = healthButton.colors;
            cb.normalColor = cannotBuyColour;
            cb.selectedColor = cannotBuyColour;
            cb.disabledColor = cannotBuyColour;
            healthButton.colors = cb;
            healthButton.interactable = false;

        }
        else
        {
            ColorBlock cb = healthButton.colors;
            cb.normalColor = canBuyColour;
            cb.selectedColor = canBuyColour;
            healthButton.colors = cb;
            healthButton.interactable = true;
        }
        currentBalanceText.text = "Current Balance: $" + DataManager.Instance.money;
    }

    /*
     * Method to handle the purchasing of a speed upgrade
     */
    public void PurchaseSpeed()
    {
        if (DataManager.Instance.money > speedUpgradeCost && speedSlider.GetComponent<Slider>().value < speedSlider.GetComponent<Slider>().maxValue)
        {
            DataManager.Instance.money -= speedUpgradeCost;
            DataManager.Instance.IncrementShop(DataManager.upgrade.speed);
        }
        UpdateData();
    }

    /*
     * Method to handle the purchasing of a damage upgrade
     */
    public void PurchaseDamage()
    {
        if (DataManager.Instance.money > damageUpgradeCost && damageSlider.GetComponent<Slider>().value < damageSlider.GetComponent<Slider>().maxValue)
        {
            DataManager.Instance.money -= damageUpgradeCost;
            DataManager.Instance.IncrementShop(DataManager.upgrade.damage);
        }
        UpdateData();
    }

    /*
     * Method to handle the purchasing of a health upgrade
     */
    public void PurchaseHealth()
    {
        if (DataManager.Instance.money > healthUpgradeCost && healthSlider.GetComponent<Slider>().value < healthSlider.GetComponent<Slider>().maxValue)
        {
            DataManager.Instance.money -= healthUpgradeCost;
            DataManager.Instance.IncrementShop(DataManager.upgrade.health);
        }
        UpdateData();
    }

    /*
     * Resets the shop data
     */
    public void ResetData()
    {
        DataManager.Instance.ResetShopToDefault();
        Invoke("UpdateData", 0.1f);
    }
    /**
     * Closes stats menu.
     */
    public void Close()
    {
        Destroy(gameObject);
    }
}
