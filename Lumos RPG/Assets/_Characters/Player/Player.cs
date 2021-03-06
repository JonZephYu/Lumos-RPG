﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

using RPG.CameraUI; // TODO consider re-wiring
using RPG.Weapons;
using RPG.Core;

namespace RPG.Characters {
    public class Player : MonoBehaviour, IDamageable {

        [SerializeField] float maxHealthPoints = 1000f;
        [SerializeField] float attackStat = 10f;

        //[SerializeField] float attackRange = 5f;

        //Projectile info
        [SerializeField] float damagePerShot = 10f;
        [SerializeField] float attackTimer = 1f;
        //[SerializeField] float projectileLifetime = 1f;
        //[SerializeField] float projectileSpeed = 1f;
        [SerializeField] Vector3 aimOffset = new Vector3(0f, 1f, 0f);
        [SerializeField] AnimatorOverrideController animOverrideContoller;
        //[SerializeField] GameObject projectilePrefab;
        [SerializeField] GameObject projectileSocket;
        //[SerializeField] GameObject weaponSocket;

        [SerializeField] Weapon weaponInUse;
        

        //TODO solve serialize and const confliction
        [SerializeField] const int walkableLayer = 8;
        [SerializeField] const int enemyLayer = 9;
        [SerializeField] const int stiffLayer = 10;

        private bool isAttacking = false;
        private float currentHealthPoints;
        private GameObject currentTarget;
        private CameraRaycaster cameraRaycaster;
        private float lastHitTime;
        private float swingTimer;
        private GameObject projectilePrefab;
        private Animator anim;

        private void Start() {
            RegisterForMouseClick();
            SetStartingHealth();
            EquipWeaponInHand();
            SetupRuntimeAnim();


        }

        private void SetupRuntimeAnim() {
            anim = GetComponent<Animator>();
            anim.runtimeAnimatorController = animOverrideContoller;
            animOverrideContoller["DEFAULT_ATTACK"] = weaponInUse.GetAnimClip();

        }

        private void SetStartingHealth() {
            currentHealthPoints = maxHealthPoints;
        }

        private void EquipWeaponInHand() {
            GameObject weaponSocket = RequestDominantHand();
            var weapon = Instantiate(weaponInUse.GetWeaponPrefab(), weaponSocket.transform);
            weapon.transform.localPosition = weaponInUse.weaponTransform.localPosition;
            weapon.transform.localRotation = weaponInUse.weaponTransform.localRotation;

            swingTimer = weaponInUse.GetSwingTimer();
            projectilePrefab = weaponInUse.GetProjectilePrefab();
        }

        private GameObject RequestDominantHand() {
            var dominantHands = GetComponentsInChildren<DominantHand>();
            int numberOfDominantHands = dominantHands.Length;
            Assert.AreNotEqual(numberOfDominantHands, 0, "No dominant hand found on player, add one!");
            Assert.IsFalse(numberOfDominantHands > 1, "Multiple DominantHand scripts found, remove until only 1 remains");
            return dominantHands[0].gameObject;

        }

        private void RegisterForMouseClick() {
            cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
            cameraRaycaster.notifyMouseClickObservers += OnMouseClick;
        }

        public float healthAsPercentage {
            get { return currentHealthPoints / (float)maxHealthPoints; }
        }

        public void TakeDamage(float damage) {
            currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0, maxHealthPoints);

            //if (currentHealthPoints <= 0) { Destroy(gameObject);}
        }


        // TODO need refactoring
        private void OnMouseClick(RaycastHit raycastHit, int layerHit) {

            // TODO swingTimer modified by player Speed stat later on
            // Will also keep firing when mouse is held down, but with the swingTimer delay --- no rapid fire
            if (Time.time - lastHitTime > swingTimer) {
                Attack();
                lastHitTime = Time.time;
            }


            //if (!isAttacking) {
            //    isAttacking = true;
            //    // TODO switch to coroutine, consider attack speed instead of attack delay
            //    InvokeRepeating("Attack", 0f, attackTimer);

            //    //if (Time.time - lastHitTime > swingTimer) {
            //    //    Attack();
            //    //    lastHitTime = Time.time;
            //    //}

            //}
            //else if (isAttacking) {
            //    // TODO only attack while mouse is down, currently cannot cancel your attacking
            //    CancelInvoke("Attack");
            //    isAttacking = false;
            //}


            //switch (layerHit) {
            //    case enemyLayer:
            //        var enemy = raycastHit.collider.gameObject;

            //        //TODO spawn attack 'projectile', even if melee
            //        //Check enemy within range
            //        if ((enemy.transform.position - transform.position).magnitude > attackRange) {
            //            //Debug.Log("enemy out of range");
            //            return;
            //        }

            //        currentTarget = enemy;
            //        if (Time.time - lastHitTime > swingTimer) {
            //            var enemyComponent = enemy.GetComponent<Enemy>();
            //            enemyComponent.TakeDamage(attackStat);
            //            lastHitTime = Time.time;
            //        }


            //        break;
            //}
        }


        private void Attack() {
            GameObject newProjectile = Instantiate(projectilePrefab, projectileSocket.transform.position, Quaternion.identity);
            var projectileComponent = newProjectile.GetComponent<Projectile>();
            projectileComponent.setDamage(damagePerShot + attackStat);

            // TODO possibly modify projectile via player stats/buffs/etc

            projectileComponent.setLifetime(weaponInUse.GetProjectileLifetime());
            projectileComponent.setSpeed(weaponInUse.GetProjectileSpeed());

            projectileComponent.SetShooter(gameObject);
            // TODO allign arrow to destination

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Plane plane = new Plane(Vector3.up, transform.position + aimOffset);
            float distance;
            if (plane.Raycast(ray, out distance)) {
                Vector3 target = ray.GetPoint(distance);
                Vector3 direction = (target - projectileSocket.transform.position).normalized;
                float rotation = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                newProjectile.GetComponent<Rigidbody>().velocity = direction * projectileComponent.getSpeed();

            }


            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            Vector3 playerToMouse = (mousePos - projectileSocket.transform.position).normalized;

            // TODO make const
            anim.SetTrigger("isAttacking");




        }



    }
}
