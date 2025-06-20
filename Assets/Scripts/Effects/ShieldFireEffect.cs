using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Effect/ShieldFire")]
public class ShieldFireEffect : ScriptableObject, IEffect
{
    [SerializeField] protected float shieldduration = 3f;
    public void Apply(GameObject player)
    {
        
    }
}