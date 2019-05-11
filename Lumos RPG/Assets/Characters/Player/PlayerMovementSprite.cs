using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.AI;


//[RequireComponent(typeof (ThirdPersonCharacter))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(AICharacterControl))]
public class PlayerMovementSprite : MonoBehaviour {

    //[SerializeField] float walkMoveStopRadius = 0.2f;
    //[SerializeField] float attackMoveStopRadius = 5f;
    //[SerializeField] float meleeMoveStopRadius = 3f;
    [SerializeField] float runSpeed = 5f;

    //private ThirdPersonCharacter thirdPersonCharacter;   // A reference to the ThirdPersonCharacter on the object
    private CameraRaycaster cameraRaycaster;
    private Vector3 currentDestination, clickPoint;

    private GameObject walkTarget = null;
    private AICharacterControl aiCharacterControl = null;

    //TODO solve serialize and const confliction
    [SerializeField] const int walkableLayer = 8;
    [SerializeField] const int enemyLayer = 9;
    [SerializeField] const int stiffLayer = 10;

    private bool isMouseMode = false;
    private Rigidbody rigidBody;


    private void Start() {
        cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        //thirdPersonCharacter = GetComponent<ThirdPersonCharacter>();
        currentDestination = transform.position;
        aiCharacterControl = GetComponent<AICharacterControl>();
        rigidBody = GetComponent<Rigidbody>();

        walkTarget = new GameObject("walkTarget");

        //TODO Refactor better, clean up code, implement AI movement with 2d character
        //TODO Fix sprite resolution issue (scaling causing graininess?)


        //Registering as an observer
        //cameraRaycaster.notifyMouseClickObservers += ProcessMouseClick;
    }

    //Fixed update is called in sync with physics
    private void FixedUpdate() {
        // TODO add to keybinding menu
        if (Input.GetKeyDown(KeyCode.G)) { //G for gamepad
            isMouseMode = !isMouseMode;
            currentDestination = transform.position;
        }

        if (isMouseMode) {
            //ProcessMouseMovement();
            Debug.Log("Not currently supporting mouse movement for 2d character");
        }
        else {
            ProcessDirectMovement();
        }




        //    //if (cameraRaycaster.layerHit == Layer.Walkable) {
        //    //    m_Character.Move(currentClickTarget - transform.position, false, false);
        //    //}

        //}
    }


    //TODO make this get called again
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

        //Apply velocity
        rigidBody.velocity = movement * runSpeed;

        // pass all parameters to the character control script
        //thirdPersonCharacter.Move(movement, false, false);
    }




    //private void WalkToDestination() {
    //    var playerToClickPoint = transform.position - currentDestination;
    //    if (playerToClickPoint.magnitude >= walkMoveStopRadius) {
    //        thirdPersonCharacter.Move(currentDestination - transform.position, false, false);
    //    }
    //    else {
    //        thirdPersonCharacter.Move(Vector3.zero, false, false);
    //    }
    //}

    //private void ProcessMouseClick(RaycastHit raycastHit, int layerHit) {
    //    Debug.Log("Mouse clicked");

    //    switch (layerHit) {
    //        case enemyLayer:
    //            //navigate to enemy
    //            GameObject enemy = raycastHit.collider.gameObject;
    //            aiCharacterControl.SetTarget(enemy.transform);
    //            break;
    //        case walkableLayer:
    //            //Walk to mouse position on ground
    //            walkTarget.transform.position = raycastHit.point;
    //            aiCharacterControl.SetTarget(walkTarget.transform);
    //            break;

    //        default:
    //            Debug.LogError("unknown mouse click player movement");
    //            return;
    //    }
    //}

    //private Vector3 ShortDestination(Vector3 destination, float shortening) {
    //    //reductionVector is the vector normalized to 1 between destination and current pos (direction vector)
    //    //Multiplied by the amount we want shortened
    //    Vector3 reductionVector = (destination - transform.position).normalized * shortening;

    //    //Returning destination shortened by our designated shortening amount, along the correct direction vector
    //    return destination - reductionVector;
    //}


    //private void OnDrawGizmos() {
    //    //Draw movement gizmos
    //    Gizmos.color = Color.black;
    //    Gizmos.DrawLine(transform.position, clickPoint);
    //    Gizmos.DrawSphere(currentDestination, 0.1f);

    //    Gizmos.DrawSphere(clickPoint, 0.2f);

    //    //Draw attack sphere
    //    Gizmos.color = new Color(255f, 0f, 0f, .5f);
    //    //Gizmos.DrawSphere(transform.position, attackMoveStopRadius);
    //    Gizmos.DrawWireSphere(transform.position, attackMoveStopRadius);

    //    Gizmos.color = new Color(100f, 255f, 255f, .75f);
    //    Gizmos.DrawWireSphere(transform.position, meleeMoveStopRadius);

    //}






    //private void ProcessMouseMovement() {
    //    if (Input.GetMouseButton(0)) {
    //        //print(cameraRaycaster.layerHit);
    //        //print("Cursor raycast hit " + cameraRaycaster.hit.collider.gameObject.name.ToString());

    //        clickPoint = cameraRaycaster.hit.point;

    //        switch (cameraRaycaster.currentLayerHit) {
    //            case Layer.Walkable:
    //                currentDestination = ShortDestination(clickPoint, walkMoveStopRadius);
    //                //if Move is here, will only move when button is held down.
    //                break;
    //            case Layer.Enemy:
    //                currentDestination = ShortDestination(clickPoint, attackMoveStopRadius);
    //                break;
    //            default:
    //                print("Unexpected case");
    //                return;
    //        }

    //        //currentClickTarget = cameraRaycaster.hit.point;  // So not set in default case
    //    }

    //    //Using the move outside the input If will allow you to move even after releasing the button
    //    WalkToDestination();
    //}



}

