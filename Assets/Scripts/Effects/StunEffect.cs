using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Effect/Stun")]
public class StunEffect : ScriptableObject, IEffect
{
    [SerializeField] protected float stunduration = 3f;
    public void Apply(GameObject player)
    {
        
    }
}