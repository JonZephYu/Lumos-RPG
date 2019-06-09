using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobPack : MonoBehaviour {

    [SerializeField] float activateRadius = 10f;

    private GameObject player = null;

    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Awake() {
        // Loop through children and turn off
        foreach (Transform child in transform) {
            Debug.Log(child + " was turned off");
            child.transform.gameObject.SetActive(false);
        }

    }

    private void Update() {
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        if (distanceToPlayer <= activateRadius) {
            ActivateChildren();
        }

    }

    private void ActivateChildren() {
        foreach (Transform child in transform) {
            Debug.Log(child + " was activated");
            child.transform.gameObject.SetActive(true);
        }


    }

    private void OnDrawGizmos() {
        //Draw movement gizmos
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, activateRadius);

    }

}
