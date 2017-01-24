using UnityEngine;
using System.Collections;
using System;

public class DTCResultItem : BaseMenuItem
{
    public override void SetUIEnable(bool enable)
    {
        base.SetUIEnable(enable);

        if (enable)
        {
            mAudioSource.Play();
        }
        else
        {
        }
    }
}
