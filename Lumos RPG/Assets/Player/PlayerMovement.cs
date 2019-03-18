using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof (ThirdPersonCharacter))]
public class PlayerMovement : MonoBehaviour
{

    [SerializeField] float walkMoveStopRadius = 0.2f;
    [SerializeField] float attackMoveStopRadius = 5f;
    [SerializeField] float meleeMoveStopRadius = 3f;

    private ThirdPersonCharacter thirdPersonCharacter;   // A reference to the ThirdPersonCharacter on the object
    private CameraRaycaster cameraRaycaster;
    private Vector3 currentDestination, clickPoint;

    private bool isMouseMode = true;
        
    private void Start()
    {
        cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        thirdPersonCharacter = GetComponent<ThirdPersonCharacter>();
        currentDestination = transform.position;
    }

    // Fixed update is called in sync with physics
    private void FixedUpdate() {
        // TODO add to keybinding menu
        if (Input.GetKeyDown(KeyCode.G)) { //G for gamepad
            isMouseMode = !isMouseMode;
            currentDestination = transform.position;
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

            clickPoint = cameraRaycaster.hit.point;

            switch (cameraRaycaster.currentLayerHit) {
                case Layer.Walkable:
                    currentDestination = ShortDestination(clickPoint, walkMoveStopRadius);
                    //if Move is here, will only move when button is held down.
                    break;
                case Layer.Enemy:
                    currentDestination = ShortDestination(clickPoint, attackMoveStopRadius);
                    break;
                default:
                    print("Unexpected case");
                    return;
            }

            //currentClickTarget = cameraRaycaster.hit.point;  // So not set in default case
        }

        //Using the move outside the input If will allow you to move even after releasing the button
        WalkToDestination();
    }


    private void WalkToDestination() {
        var playerToClickPoint = transform.position - currentDestination;
        if (playerToClickPoint.magnitude >= walkMoveStopRadius) {
            thirdPersonCharacter.Move(currentDestination - transform.position, false, false);
        }
        else {
            thirdPersonCharacter.Move(Vector3.zero, false, false);
        }
    }

    private Vector3 ShortDestination(Vector3 destination, float shortening) {
        //reductionVector is the vector normalized to 1 between destination and current pos (direction vector)
        //Multiplied by the amount we want shortened
        Vector3 reductionVector = (destination - transform.position).normalized * shortening;

        //Returning destination shortened by our designated shortening amount, along the correct direction vector
        return destination - reductionVector;
    }


    private void OnDrawGizmos() {
        //Draw movement gizmos
        Gizmos.color = Color.black;
        Gizmos.DrawLine(transform.position, clickPoint);
        Gizmos.DrawSphere(currentDestination, 0.1f);

        Gizmos.DrawSphere(clickPoint, 0.2f);

        //Draw attack sphere
        Gizmos.color = new Color(255f, 0f, 0f, .5f);
        //Gizmos.DrawSphere(transform.position, attackMoveStopRadius);
        Gizmos.DrawWireSphere(transform.position, attackMoveStopRadius);

        Gizmos.color = new Color(100f, 255f, 255f, .75f);
        Gizmos.DrawWireSphere(transform.position, meleeMoveStopRadius);

    }


}

