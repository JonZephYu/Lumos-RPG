using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RPG.Core;

namespace RPG.Weapons {
    public class Projectile : MonoBehaviour {

        // SF so can inspect when paused
        [SerializeField] GameObject shooter;

        //NOTE other classes can set
        //TODO make SF with getter settters
        private float projectileSpeed = 5f;
        private float lifetime = 1f;
        private float damage = 10f;
        private float spawnTime;
        private Layer myLayer;
        private float DESTROY_DELAY = .05f;


        private void Start() {
            spawnTime = Time.time;
        }

        private void Update() {
            if (Time.time - spawnTime >= lifetime) {
                Destroy(gameObject);
            }
        }

        public void SetShooter(GameObject shooter) {
            this.shooter = shooter;
        }

        //private void OnCollisionEnter(Collision collision) {
        //    if (collision.gameObject.layer != shooter.layer) {
        //        HandleDamage(collision);
        //    }



        //}

        private void OnTriggerEnter(Collider coll) {
            if (shooter && coll.gameObject.layer != shooter.layer) {
                HandleDamage(coll);
            }



        }



        private void HandleDamage(Collider coll) {
            Component damageableComponent = coll.gameObject.GetComponent(typeof(IDamageable));

            if (damageableComponent) {
                (damageableComponent as IDamageable).TakeDamage(damage);
                //TODO Perhaps some projectiles do not get destroyed on collision, thus bouncing around funnily
                Destroy(gameObject, DESTROY_DELAY);
            }
            //(damageableComponent as IDamageable).TakeDamage()
        }

        //Projectiles are now collision based
        //private void OnTriggerEnter(Collider collider) {
        //    Component damageableComponent = collider.gameObject.GetComponent(typeof(IDamageable));

        //    if (damageableComponent) {
        //        (damageableComponent as IDamageable).TakeDamage(damage);
        //    }
        //    //(damageableComponent as IDamageable).TakeDamage()


        //}

        public void setDamage(float newDamage) {
            damage = newDamage;
        }

        public void setLifetime(float newDuration) {
            lifetime = newDuration;
        }

        public void setSpeed(float newSpeed) {
            projectileSpeed = newSpeed;
        }

        public float getSpeed() {
            return projectileSpeed;
        }

    }
}
