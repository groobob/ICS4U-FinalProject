/*
 * Class to manage the healthpack drops
 * 
 * @author Richard
 * @version January 24
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPackDrop : MonoBehaviour
{
    [SerializeField] private int healing;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if (player)
        {
            PlayerManager.Instance._playerStats.HealDamage(healing);
            Destroy(gameObject);
        }
    }
}
