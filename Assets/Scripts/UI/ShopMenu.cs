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
    [SerializeField] GameObject tempogainSlider;
    [SerializeField] GameObject healthSlider;
    [SerializeField] int speedUpgradeCost;
    [SerializeField] int tempogainUpgradeCost;
    [SerializeField] int healthUpgradeCost;
    [SerializeField] TextMeshProUGUI speedUpgradeCostText;
    [SerializeField] TextMeshProUGUI tempogainUpgradeCostText;
    [SerializeField] TextMeshProUGUI healthUpgradeCostText;
    [SerializeField] Button speedButton;
    [SerializeField] Button tempogainButton;
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
        tempogainSlider.GetComponent<Slider>().value = DataManager.Instance.GetShop(DataManager.upgrade.tempogain);
        healthSlider.GetComponent<Slider>().value = DataManager.Instance.GetShop(DataManager.upgrade.health);
        speedSlider.GetComponentInChildren<TextMeshProUGUI>().text = DataManager.Instance.GetShop(DataManager.upgrade.speed) + "/" + speedSlider.GetComponent<Slider>().maxValue;
        tempogainSlider.GetComponentInChildren<TextMeshProUGUI>().text = DataManager.Instance.GetShop(DataManager.upgrade.tempogain) + "/" + tempogainSlider.GetComponent<Slider>().maxValue;
        healthSlider.GetComponentInChildren<TextMeshProUGUI>().text = DataManager.Instance.GetShop(DataManager.upgrade.health) + "/" + healthSlider.GetComponent<Slider>().maxValue;
        speedUpgradeCostText.text = "$" + speedUpgradeCost;
        tempogainUpgradeCostText.text = "$" + tempogainUpgradeCost;
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
        if (DataManager.Instance.money < tempogainUpgradeCost || tempogainSlider.GetComponent<Slider>().value == tempogainSlider.GetComponent<Slider>().maxValue)
        {
            ColorBlock cb = tempogainButton.colors;
            cb.normalColor = cannotBuyColour;
            cb.selectedColor = cannotBuyColour;
            cb.disabledColor = cannotBuyColour;
            tempogainButton.colors = cb;
            tempogainButton.interactable = false;

        }
        else
        {
            ColorBlock cb = tempogainButton.colors;
            cb.normalColor = canBuyColour;
            cb.selectedColor = canBuyColour;
            tempogainButton.colors = cb;
            tempogainButton.interactable = true;

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
     * Method to handle the purchasing of a tempogain upgrade
     */
    public void PurchaseTempoGain()
    {
        if (DataManager.Instance.money > tempogainUpgradeCost && tempogainSlider.GetComponent<Slider>().value < tempogainSlider.GetComponent<Slider>().maxValue)
        {
            DataManager.Instance.money -= tempogainUpgradeCost;
            DataManager.Instance.IncrementShop(DataManager.upgrade.tempogain);
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
