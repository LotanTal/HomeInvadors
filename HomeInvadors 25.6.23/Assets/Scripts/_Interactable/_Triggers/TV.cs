using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TV : Trigger
{
    public GameObject TVDisplay;

    public override void OnTriggerActive()
    {
        TVDisplay.SetActive(true);
        AudioManager.instance.PlaySpatialAudio("TV", 0);
    }

    public override void OnTriggerInactive()
    {
        TVDisplay.SetActive(false);
        AudioManager.instance.musicSource.Stop();
    }
}
