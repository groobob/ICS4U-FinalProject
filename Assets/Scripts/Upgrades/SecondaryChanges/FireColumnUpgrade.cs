using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FireColumnUpgrade : SecondaryChange
{
    // Start is called before the first frame update
    void Start()
    {
        base.Init();
        upgradeName = "Dark Bolt";
        description = "Harness dark magic! Your secondary is now replaced with a lightning strike that deals two ticks of damage";
    }
}
