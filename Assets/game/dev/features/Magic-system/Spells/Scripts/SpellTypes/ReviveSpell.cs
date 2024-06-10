using RoboRyanTron.Unite2017.Variables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Utilities;

[CreateAssetMenu(fileName = "NewReviveSpell", menuName = "Magic System/Spells/ReviveSpell")]

public class ReviveSpell : SpellBase
{
    [SerializeField] private FloatVariable _radius;

    public override void CastSpell(Transform origin, TargetLayerData hitableLayers)
    {
        //origin.GetComponentsInArea<>         
    }
}
