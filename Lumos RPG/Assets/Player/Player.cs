using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [SerializeField] float maxHealthPoints = 1000;
    private float currentHealthPoints = 1000;
    

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public float healthAsPercentage {
        get { return currentHealthPoints / (float)maxHealthPoints; }
    }

}
