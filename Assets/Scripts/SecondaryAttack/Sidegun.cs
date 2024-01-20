using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sidegun : SecondaryAttack
{
    private static int setDamage = 1;
    private static float setReloadTime = 2.5f;

    public Sidegun() : base(setDamage, setReloadTime, null) { }

    public override void Attack()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = transform.position.z;
        Vector3 mousePlayerVector = (mousePos - transform.position).normalized;
        ProjectileManager.Instance.SpawnProjectile(transform.position, mousePlayerVector * 8, 0);
    }
}
