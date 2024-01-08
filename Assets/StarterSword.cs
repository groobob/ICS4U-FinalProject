using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarterSword : MeleeWeapons
{
    private static int dmg = 10;
    private static float rldTime = 0.5f;
    private static float displacement = 0f;
    private static float angle = 0f;
    private static float hitWidth = 1;
    private static float hitLength = 2;
    /**
     * Constructor for startersword
     */
    public StarterSword() : base(dmg, rldTime, displacement, angle, hitWidth, hitLength, null) { }
}
