using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

using RPG.Weapons;
using RPG.Core;

namespace RPG.Characters {
    public class Enemy : MonoBehaviour, IDamageable {

        [SerializeField] float maxHealthPoints = 100;
        [SerializeField] float aggroRadius = 1f;
        [SerializeField] float leashRadius = 3f;
        [SerializeField] float attackRadius = 2f;
        [SerializeField] float damagePerShot = 10f;
        [SerializeField] float attackTimer = 1f;
        [SerializeField] float projectileLifetime = 1f;
        [SerializeField] float projectileSpeed = 1f;
        [SerializeField] Vector3 aimOffset = new Vector3(0f, 1f, 0f);
        [SerializeField] GameObject projectilePrefab;
        [SerializeField] GameObject projectileSocket;


        private float currentHealthPoints;
        private bool isAttacking = false;
        private GameObject player = null;
        private AICharacterControl aiCharacterControl = null;


        // Use this for initialization
        void Start() {
            player = GameObject.FindGameObjectWithTag("Player");
            aiCharacterControl = GetComponent<AICharacterControl>();

            currentHealthPoints = maxHealthPoints;
        }

        // Update is called once per frame
        void Update() {
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            if (distanceToPlayer <= attackRadius && !isAttacking) {
                isAttacking = true;
                // TODO switch to coroutine, consider attack speed instead of attack delay
                InvokeRepeating("Attack", 0f, attackTimer);

            }
            else if (distanceToPlayer <= aggroRadius) {
                aiCharacterControl.SetTarget(player.transform);
            }
            else if (distanceToPlayer >= leashRadius) {
                aiCharacterControl.SetTarget(transform);
            }


            if (distanceToPlayer > attackRadius) {
                CancelInvoke("Attack");
                isAttacking = false;
            }

        }

        public float healthAsPercentage {
            get { return currentHealthPoints / (float)maxHealthPoints; }
        }

        public void TakeDamage(float damage) {
            currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);

            //TODO animator hit stun if (!stunned || knockeddown)

            if (currentHealthPoints <= 0) { Destroy(gameObject); }
        }


        private void OnDrawGizmos() {
            //Draw movement gizmos
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, aggroRadius);
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, leashRadius);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRadius);
        }


        // TODO separate out character firing logic into separate class
        private void Attack() {
            GameObject newProjectile = Instantiate(projectilePrefab, projectileSocket.transform.position, Quaternion.identity);
            var projectileComponent = newProjectile.GetComponent<Projectile>();
            projectileComponent.setDamage(damagePerShot);
            projectileComponent.setLifetime(projectileLifetime);
            projectileComponent.setSpeed(projectileSpeed);
            projectileComponent.SetShooter(gameObject);

            Vector3 unitVectorToPlayer = (player.transform.position + aimOffset - projectileSocket.transform.position).normalized;
            newProjectile.GetComponent<Rigidbody>().velocity = unitVectorToPlayer * projectileComponent.getSpeed();

        }

    }
}
