/*
 * Basic starter sword when you begin the game.
 * 
 * @author Evan
 * @version January 09
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarterSword : MeleeWeapons
{
    private static int setDamage = 3;
    private static float setReloadTime = 0.38f;
    private static float setDisplacement = 1f;
    private static float setAngle = 0f;
    private static float setHitWidth = 2.5f;
    private static float setHitLength = 3.5f;
    private static float setKnockbackStrength = 4f;

    private static float setStunDuration = setReloadTime + 0.25f;
    private static int setComboMax = 3;
    private static float setEndlagDuration = setReloadTime - 0.1f;
    /**
     * Constructor for startersword
     */
    public StarterSword() : base(setDamage, setReloadTime, setDisplacement, setAngle, setHitWidth, setHitLength, setKnockbackStrength, setStunDuration, setComboMax, setEndlagDuration, null) { }
}
