using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    //NOTE other classes can set
    //TODO make SF with getter settters
    public float projectileSpeed = 5f;
    [SerializeField] float lifetime = 1f;

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

    private void OnTriggerEnter(Collider collider) {
        

        Component damageableComponent = collider.gameObject.GetComponent(typeof(IDamageable));
        Debug.Log("damageableComponent " + damageableComponent);

        if (damageableComponent) {
            (damageableComponent as IDamageable).TakeDamage(damage);
        }
        //(damageableComponent as IDamageable).TakeDamage()


    }

    public void setDamage(float newDamage) {
        damage = newDamage;
    }


}
