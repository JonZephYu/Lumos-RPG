using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour, IDamageable {

    [SerializeField] float maxHealthPoints = 1000f;
    [SerializeField] float attackStat = 10f;
    [SerializeField] float swingTimer = 1f;
    [SerializeField] float attackRange = 5f;


    //TODO solve serialize and const confliction
    [SerializeField] const int walkableLayer = 8;
    [SerializeField] const int enemyLayer = 9;
    [SerializeField] const int stiffLayer = 10;


    private float currentHealthPoints;
    private GameObject currentTarget;
    private CameraRaycaster cameraRaycaster;
    private float lastHitTime;

    private void Start() {
        cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        currentHealthPoints = maxHealthPoints;

        cameraRaycaster.notifyMouseClickObservers += OnMouseClick;
    }

    public float healthAsPercentage {
        get { return currentHealthPoints / (float)maxHealthPoints; }
    }

    public void TakeDamage(float damage) {
        currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0, maxHealthPoints);

        //if (currentHealthPoints <= 0) { Destroy(gameObject);}
    }

    private void OnMouseClick(RaycastHit raycastHit, int layerHit) {
        switch (layerHit) {
            case enemyLayer:
                var enemy = raycastHit.collider.gameObject;
                
                //TODO spawn attack 'projectile', even if melee
                //Check enemy within range
                if ((enemy.transform.position - transform.position).magnitude > attackRange) {
                    Debug.Log("enemy out of range");
                    return;
                }

                currentTarget = enemy;
                if (Time.time - lastHitTime > swingTimer) {
                    var enemyComponent = enemy.GetComponent<Enemy>();
                    enemyComponent.TakeDamage(attackStat);
                    lastHitTime = Time.time;
                }   
                

                break;
        }
    }


}
