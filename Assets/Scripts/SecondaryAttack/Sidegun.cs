/*
 * Script for Sidegun. Subclass of Secondary. Activate to fire an attack.
 * 
 * @author Evan
 * @version January 21
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sidegun : SecondaryAttack
{
    private static int setDamage = 1;
    private static float setReloadTime = 5f;

    /**
     * Main Constructor
     */
    public Sidegun() : base(setDamage, setReloadTime, null) { }

    public override void Attack()
    {
        SoundManager.Instance.PlayAudio(3);

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = transform.position.z;
        Vector3 mousePlayerVector = (mousePos - transform.position).normalized;
        ProjectileManager.Instance.SpawnProjectile(transform.position, mousePlayerVector * 30, 2);
    }
}
