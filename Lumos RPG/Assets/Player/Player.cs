using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour, IDamageable {

    [SerializeField] float maxHealthPoints = 1000;
    private float currentHealthPoints = 1000;

    public float healthAsPercentage {
        get { return currentHealthPoints / (float)maxHealthPoints; }
    }

    public void TakeDamage(float damage) {
        currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0, maxHealthPoints);

        if (currentHealthPoints <= 0) { Destroy(gameObject);}
    }



}
