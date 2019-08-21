﻿using Events;
using Tool;
using UnityEngine;

public class ReinforceShootPowerProp : VirusBaseProp
{

    [SerializeField] private float _duration;
    public override void Excute(Transform target)
    {
        EventManager.TriggerEvent(new VirusPropAddEvent(_duration, VirusPropEnum.ReinforceShootPower));
        PropPools.Instance.DeSpawn(gameObject);
    }

}