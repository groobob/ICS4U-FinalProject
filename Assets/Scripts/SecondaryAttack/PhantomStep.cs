using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhantomStep : SecondaryAttack
{
    private static int setDamage = 1;
    private static float setReloadTime = 7f;
    private float iframeDuration = 2f;

    private float speedBoost = 1.5f;
    private float tempoCost = 20f;

    public PhantomStep() : base(setDamage, setReloadTime, null) { }

    public override void Attack()
    {
        if (_playerStats.SpendTempo(tempoCost))
        {
            SoundManager.Instance.PlayAudio(7);
            Debug.Log("Phantom Step");
            _playerStats.GiveIFrames(iframeDuration);
            _playerStats.SpeedBoost(speedBoost, iframeDuration * 1.5f);
            _playerStats.EndlagEntity(iframeDuration / 8);
        }
    }
}
