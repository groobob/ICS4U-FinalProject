using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiProjectile : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            //Destroy(collision.gameObject);
        }
    }

    public void Start()
    {
        SoundManager.Instance.PlayAudio(9);

        Destroy(gameObject, 3f);
    }
}
