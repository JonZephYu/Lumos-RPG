using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Weapons {
    [CreateAssetMenu(menuName = "RPG/Weapon")]
    public class Weapon : ScriptableObject {

        public Transform weaponTransform;

        [SerializeField] GameObject weaponPrefab;
        [SerializeField] GameObject projectilePrefab;
        [SerializeField] AnimationClip attackAnimation;
        [SerializeField] float swingTimer = 1f;
        [SerializeField] float projectileLifetime = 1f;
        [SerializeField] float projectileSpeed = 1f;

        // TODO link weapon to specific projectile, instead of on player
        //[SerializeField] GameObject projectilePrefab;

        //public void Start() {
        //    var projectileComponent = projectilePrefab.GetComponent<Projectile>();
        //    projectileComponent.setLifetime(projectileLifetime);
        //    projectileComponent.setSpeed(projectileSpeed);
        //}

        public GameObject GetWeaponPrefab() {
            return weaponPrefab;
        }



        public GameObject GetProjectilePrefab() {
            return projectilePrefab;
        }

        public float GetProjectileSpeed() {
            return projectileSpeed;
        }

        public float GetProjectileLifetime() {
            return projectileLifetime;
        }

        // TODO Associate projectiles to weapon instead of having it on player


        public AnimationClip GetAnimClip() {
            ClearAnimationEvents();
            return attackAnimation;
        }

        public float GetSwingTimer() {
            return swingTimer;
        }
        
        // Removing animation events so asset packs cannot cause crashes
        private void ClearAnimationEvents() {
            attackAnimation.events = new AnimationEvent[0];
        }


    }
}
