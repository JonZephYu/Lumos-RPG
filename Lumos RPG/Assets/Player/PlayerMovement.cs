using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof (ThirdPersonCharacter))]
public class PlayerMovement : MonoBehaviour
{

    [SerializeField] float walkMoveStopRadius = 0.2f;

    private ThirdPersonCharacter thirdPersonCharacter;   // A reference to the ThirdPersonCharacter on the object
    private CameraRaycaster cameraRaycaster;
    private Vector3 currentClickTarget;

    private bool isMouseMode = true;
        
    private void Start()
    {
        cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        thirdPersonCharacter = GetComponent<ThirdPersonCharacter>();
        currentClickTarget = transform.position;
    }

    // Fixed update is called in sync with physics
    private void FixedUpdate() {
        // TODO add to keybinding menu
        if (Input.GetKeyDown(KeyCode.G)) { //G for gamepad
            isMouseMode = !isMouseMode;
            currentClickTarget = transform.position;
        }

        if (isMouseMode) {
            ProcessMouseMovement();
        }
        else {
            ProcessDirectMovement();
        }




        //if (cameraRaycaster.layerHit == Layer.Walkable) {
        //    m_Character.Move(currentClickTarget - transform.position, false, false);
        //}

    }

    private void ProcessDirectMovement() {
        // read inputs
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        //bool crouch = Input.GetKey(KeyCode.C);

        // calculate camera relative direction to move:
        Vector3 camForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 movement = v * camForward + h * Camera.main.transform.right;

#if !MOBILE_INPUT
        // walk speed multiplier
        if (Input.GetKey(KeyCode.LeftShift)) movement *= 0.5f;
#endif

        // pass all parameters to the character control script
        thirdPersonCharacter.Move(movement, false, false);
    }

    private void ProcessMouseMovement() {
        if (Input.GetMouseButton(0)) {
            //print(cameraRaycaster.layerHit);
            //print("Cursor raycast hit " + cameraRaycaster.hit.collider.gameObject.name.ToString());

            switch (cameraRaycaster.currentLayerHit) {
                case Layer.Walkable:
                    currentClickTarget = cameraRaycaster.hit.point;
                    //if Move is here, will only move when button is held down.
                    break;
                case Layer.Enemy:
                    print("Not moving towards enemy");
                    break;
                default:
                    print("Unexpected case");
                    return;
            }

            //currentClickTarget = cameraRaycaster.hit.point;  // So not set in default case
        }
        //Using the move outside the input If will allow you to move even after releasing the button
        var playerToClickPoint = transform.position - currentClickTarget;
        if (playerToClickPoint.magnitude >= walkMoveStopRadius) {
            thirdPersonCharacter.Move(currentClickTarget - transform.position, false, false);
        }
        else {
            thirdPersonCharacter.Move(Vector3.zero, false, false);
        }
    }
}

