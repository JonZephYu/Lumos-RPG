using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    //NOTE other classes can set
    //TODO make SF with getter settters
    private float projectileSpeed = 5f;
    private float lifetime = 1f;
    private float damage = 10f;
    private float spawnTime;

    private void Start() {
        spawnTime = Time.time;
    }

    private void Update() {
        if (Time.time - spawnTime >= lifetime) {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision) {
        Component damageableComponent = collision.gameObject.GetComponent(typeof(IDamageable));

        if (damageableComponent) {
            (damageableComponent as IDamageable).TakeDamage(damage);
            //TODO Perhaps some projectiles do not get destroyed on collision, thus bouncing around funnily
            Destroy(gameObject, .05f);
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
