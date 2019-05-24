using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="RPG/Weapon")]
public class Weapon : ScriptableObject {

    public Transform weaponTransform;

    [SerializeField] GameObject weaponPrefab;
    [SerializeField] AnimationClip attackAnimation;

    public GameObject GetWeaponPrefab() {
        return weaponPrefab;
    }

    // TODO Associate projectiles to weapon instead of having it on player
}
