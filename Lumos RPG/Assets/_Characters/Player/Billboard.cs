using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO namespace

public class Billboard : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update() {
        //transform.LookAt(Camera.main.transform.position);
        transform.forward = Camera.main.transform.forward;

    }
}
