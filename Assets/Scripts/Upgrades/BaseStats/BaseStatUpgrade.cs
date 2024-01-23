/*
 * Superclass for all base stat upgrades
 * 
 * @author Evan
 * @version January 20
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseStatUpgrade : Upgrade
{
    protected void Init()
    {
        classification = "Stat Upgrade";
    }
}
