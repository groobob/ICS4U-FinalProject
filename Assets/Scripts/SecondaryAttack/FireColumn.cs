/*
 * Script for FireColumn/Darkbolt. Subclass of Secondary. Activate to summon an attack.
 * 
 * @author Evan
 * @version January 21
 */

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class FireColumn : SecondaryAttack
{
    private static int setDamage = 1;
    private static float setReloadTime = 8f;

    [SerializeField] private GameObject fireColumn;
    /**
     * Main Constructor
     */
    public FireColumn() : base(setDamage, setReloadTime, null) { }

    private void Start()
    {
        fireColumn = ProjectileManager.Instance.GetProjectileList().ElementAt(4);
    }

    public override void Attack()
    {
        Debug.Log(fireColumn);
        SoundManager.Instance.PlayAudio(13);
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = transform.position.z;
        Instantiate(fireColumn, mousePos, Quaternion.identity);
    }
}
