/*
 * Superclass for all secondary change upgrades. These change your secondary
 * 
 * @author Evan
 * @version January 22
 */

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SecondaryChange : Upgrade
{
    protected void Init()
    {
        classification = "Secondary Change";
    }
}
