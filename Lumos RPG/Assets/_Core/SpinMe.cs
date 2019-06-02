using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// TODO potentially move to assets
namespace RPG.Core {
    public class SpinMe : MonoBehaviour {

        [SerializeField] float xRotationsPerMinute = 1f;
        [SerializeField] float yRotationsPerMinute = 1f;
        [SerializeField] float zRotationsPerMinute = 1f;

        void Update() {

            //xDegreesPerFrame = Time.DeltaTime, 60sec/min, 360, xRotationPerMinute
            // degrees frame^-1 =  seconds frame^-1, seconds min^-1, degree rotation^-1, rotation min^-1 
            // degrees frame^-1 =  (seconds frame^-1) / (seconds min^-1) * (degree rotation^-1) * (rotation min^-1) 
            // degrees frame^-1 = frames^-1 minute * degrees rotation^-1 * rotation min^-1
            // degrees frame^-1 = frames^-1 * degrees

            float xDegreesPerFrame = Time.deltaTime / 60 * 360 * xRotationsPerMinute;
            transform.RotateAround(transform.position, transform.right, xDegreesPerFrame);

            float yDegreesPerFrame = Time.deltaTime / 60 * 360 * yRotationsPerMinute;
            transform.RotateAround(transform.position, transform.up, yDegreesPerFrame);

            float zDegreesPerFrame = Time.deltaTime / 60 * 360 * zRotationsPerMinute;
            transform.RotateAround(transform.position, transform.forward, zDegreesPerFrame);
        }
    }
}
