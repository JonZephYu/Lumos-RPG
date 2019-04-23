using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class Enemy : MonoBehaviour, IDamageable {

    [SerializeField] float maxHealthPoints = 1000;
    [SerializeField] float aggroRadius = 1f;
    [SerializeField] float leashRadius = 3f;
    [SerializeField] float attackRadius = 2f;
    [SerializeField] float damagePerShot = 9f;
    [SerializeField] float attackTimer = 1f;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] GameObject projectileSocket;


    private float currentHealthPoints = 1000;
    private bool isAttacking = false;
    private GameObject player = null;
    private AICharacterControl aiCharacterControl = null;


    // Use this for initialization
    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        aiCharacterControl = GetComponent<AICharacterControl>();
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
        else {
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

    private void Attack() {
        GameObject newProjectile = Instantiate(projectilePrefab, projectileSocket.transform.position, Quaternion.identity);
        var projectileComponent = newProjectile.GetComponent<Projectile>();
        projectileComponent.setDamage(damagePerShot);
        Vector3 unitVectorToPlayer = (player.transform.position - projectileSocket.transform.position).normalized;
        newProjectile.GetComponent<Rigidbody>().velocity = unitVectorToPlayer * projectileComponent.projectileSpeed;

    }

}
