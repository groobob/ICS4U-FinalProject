using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageNumScript : MonoBehaviour
{
    [SerializeField] private TextMeshPro _textMeshPro;
    [SerializeField] private Rigidbody2D _rb;

    private float duration = 0.9f;
    void Start()
    {
        _rb.AddForce( new Vector2(0, 2f), ForceMode2D.Impulse);
        Destroy(gameObject, duration);
        StartCoroutine(FadeText());
    }

    private IEnumerator FadeText()
    {
        Color startColor = _textMeshPro.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f);

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            _textMeshPro.color = Color.Lerp(startColor, endColor, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _textMeshPro.color = endColor;
    }


    public void SetNumber(int amount, bool type) // true = damage, false = heal
    {
        _textMeshPro.text = amount.ToString();
        if (type)
        {
            _textMeshPro.color = Color.red;
        }
        else
        {
            _textMeshPro.color = Color.green;
        }
    }
}
