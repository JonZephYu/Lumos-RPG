using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class Enemy : MonoBehaviour, IDamageable {

    [SerializeField] float maxHealthPoints = 1000;
    [SerializeField] float aggroRadius = 1f;
    [SerializeField] float leashRadius = 3f;
    [SerializeField] float attackRadius = 2f;
    private float currentHealthPoints = 1000;

    GameObject player = null;
    AICharacterControl aiCharacterControl = null;


    // Use this for initialization
    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        aiCharacterControl = GetComponent<AICharacterControl>();
    }

    // Update is called once per frame
    void Update() {
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        if (distanceToPlayer <= attackRadius){
            //TODO Spawn projectile
            Debug.Log(gameObject.name + " is attacking!");
        }
        else if (distanceToPlayer <= aggroRadius) {
            aiCharacterControl.SetTarget(player.transform);
        }
        else {
            aiCharacterControl.SetTarget(transform);
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

}
