/*
 * Script for Second Soul. Saves the user with IFrames when at lethal HP.
 * 
 * @author Evan
 * @version January 21
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondSoul : Upgrade
{

    private bool Used;
    private float iframeDuration = 4f;

    private float movementBuff = 1.5f;
    protected PlayerStats _playerStats;
    protected PlayerController _playerController;

    private SpriteRenderer spriteRenderer;
    public Color originalColor;

    public void Start()
    {
        _playerStats = PlayerManager.Instance._playerStats;
        _playerController = PlayerManager.Instance._playerControl;
        Used = false;
        healthBoost = 1;

        spriteRenderer = PlayerManager.Instance.player.transform.Find("Character").gameObject.GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;

        classification = "Unique";
        upgradeName = "Second Soul";
        description = "Upon taking lethal damage, enter a spectral form where you cannot be damaged and gain a speed boost. Additionally gain health. Works once per level.";
    }
    /**
     * Proccs to give iframes and speed boost. This only occurs at lethal health
     * 
     */
    public void UpgradeProcc()
    {
        if (!Used)
        {
            Used = true;
            Debug.Log("Phantom Step");
            _playerStats.GiveIFrames(iframeDuration);
            _playerStats.SpeedBoost(movementBuff, iframeDuration * 1.5f);
            _playerStats.EndlagEntity(iframeDuration / 8);

            StartCoroutine(MakeTranslucentCoroutine());
        }
    }

    private IEnumerator MakeTranslucentCoroutine()
    {
        // Set the alpha channel to make the GameObject translucent
        Color translucentColor = spriteRenderer.color;
        translucentColor.a = 0.5f; // Example: 50% opacity
        spriteRenderer.color = translucentColor;

        // Wait for the specified duration
        yield return new WaitForSeconds(iframeDuration);

        // Revert to the original color
        spriteRenderer.color = originalColor;
    }
}
