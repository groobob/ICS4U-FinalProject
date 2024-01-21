using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Windwall : SecondaryAttack
{
    private static int setDamage = 1;
    private static float setReloadTime = 8f;

    public Windwall() : base(setDamage, setReloadTime, null) { }

    public override void Attack()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = transform.position.z;
        Vector3 mousePlayerVector = (mousePos - transform.position).normalized;
        float angle = -1 * Mathf.Atan2(mousePlayerVector.y, mousePlayerVector.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle + 90, Vector3.back);
        Vector3 weaponOffset = mousePlayerVector;
        Vector3 position = _player.transform.position + weaponOffset;
        ProjectileManager.Instance.SpawnProjectile(position, mousePlayerVector * 0.4f, 3, rotation);

    }
}
