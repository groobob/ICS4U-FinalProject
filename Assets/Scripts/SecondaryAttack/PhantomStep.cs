/*
 * Script for Phantom Step. Subclass of Secondary. Activate to gain IFrames.
 * 
 * @author Evan
 * @version January 21
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhantomStep : SecondaryAttack
{
    private static int setDamage = 1;
    private static float setReloadTime = 7f;
    private float iframeDuration = 2f;

    private float speedBoost = 1.5f;
    private float tempoCost = 20f;

    private SpriteRenderer spriteRenderer;
    public Color originalColor;
    /**
     * Main Constructor
     */
    public PhantomStep() : base(setDamage, setReloadTime, null) { }

    public override void Attack()
    {
        if (_playerStats.SpendTempo(tempoCost))
        {
            // MAKE THE PLAYER TRANSLUCENT AS WELL 
            SoundManager.Instance.PlayAudio(7);
            Debug.Log("Phantom Step");
            _playerStats.GiveIFrames(iframeDuration);
            _playerStats.SpeedBoost(speedBoost, iframeDuration * 1.5f);
            _playerStats.EndlagEntity(iframeDuration / 8);

            StartCoroutine(MakeTranslucentCoroutine());

        }
    }

    private void Start()
    {
        spriteRenderer = PlayerManager.Instance.player.transform.Find("Character").gameObject.GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
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
