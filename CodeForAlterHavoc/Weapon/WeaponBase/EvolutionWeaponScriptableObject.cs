using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EvoleWeapon", menuName = "ScriptableObjects/EvoleWeapon")]
public class EvolutionWeaponScriptableObject : ScriptableObject
{
    public WeaponScriptableObject _BaseWeapon;
    public WeaponScriptableObject _CatalystWeapon;
    public WeaponScriptableObject _EvolvedWeaponData;
    public GameObject _EvolvedWeaponController;
}
