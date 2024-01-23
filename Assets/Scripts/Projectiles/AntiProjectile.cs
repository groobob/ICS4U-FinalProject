/*
 * Script for windwall. Destroys all enemy particles.
 * 
 * @author Evan
 * @version January 22
 */

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

    private void Start()
    {
        SoundManager.Instance.PlayAudio(9);

        Destroy(gameObject, 3f);
    }
}
