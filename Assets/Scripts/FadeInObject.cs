/*
 * A class to make an object fade in
 * 
 * @author Evan
 * @version January 24
 */
using UnityEngine;

public class FadeInObject : MonoBehaviour
{
    public float fadeDuration = 1.5f; // Duration of the fade-in effect in seconds

    private SpriteRenderer spriteRenderer;
    private float currentAlpha = 0f;
    private float targetAlpha = 1f;
    private float fadeTimer = 0f;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, currentAlpha);
    }

    private void Update()
    {
        if (fadeTimer < fadeDuration)
        {
            fadeTimer += Time.deltaTime;
            currentAlpha = Mathf.Lerp(0f, targetAlpha, fadeTimer / fadeDuration);
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, currentAlpha);
        }
    }
}