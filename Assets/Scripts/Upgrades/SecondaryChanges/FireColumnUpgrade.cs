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
        upgradeName = "Fire Column";
        description = "Charm the flames! Your secondary is now replaced with a flaming pillar that deals tick damage";
    }
}
