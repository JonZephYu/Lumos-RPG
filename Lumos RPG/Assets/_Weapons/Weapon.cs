using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Weapons {
    [CreateAssetMenu(menuName = "RPG/Weapon")]
    public class Weapon : ScriptableObject {

        public Transform weaponTransform;

        [SerializeField] GameObject weaponPrefab;
        [SerializeField] AnimationClip attackAnimation;

        public GameObject GetWeaponPrefab() {
            return weaponPrefab;
        }

        // TODO Associate projectiles to weapon instead of having it on player


        public AnimationClip GetAnimClip() {
            ClearAnimationEvents();
            return attackAnimation;
        }

        // Removing animation events so asset packs cannot cause crashes
        private void ClearAnimationEvents() {
            attackAnimation.events = new AnimationEvent[0];
        }
    }
}
