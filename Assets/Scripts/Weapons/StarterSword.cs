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
    private static int setDamage = 10;
    private static float setReloadTime = 0.5f;
    private static float setDisplacement = 0f;
    private static float setAngle = 0f;
    private static float setHitWidth = 1;
    private static float setHitLength = 2;
    /**
     * Constructor for startersword
     */
    public StarterSword() : base(setDamage, setReloadTime, setDisplacement, setAngle, setHitWidth, setHitLength, null) { }
}
