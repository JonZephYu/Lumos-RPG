using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof (ThirdPersonCharacter))]
public class PlayerMovement : MonoBehaviour
{

    [SerializeField] float walkMoveStopRadius = 0.2f;

    ThirdPersonCharacter m_Character;   // A reference to the ThirdPersonCharacter on the object
    CameraRaycaster cameraRaycaster;
    Vector3 currentClickTarget;
        
    private void Start()
    {
        cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        m_Character = GetComponent<ThirdPersonCharacter>();
        currentClickTarget = transform.position;
    }

    // TODO fix issue with click to move and WASD movement stacking movement speed.


    // Fixed update is called in sync with physics
    private void FixedUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            print(cameraRaycaster.layerHit);
            print("Cursor raycast hit " + cameraRaycaster.hit.collider.gameObject.name.ToString());

            switch (cameraRaycaster.layerHit) {
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
            m_Character.Move(currentClickTarget - transform.position, false, false);
        }
        else {
            m_Character.Move(Vector3.zero, false, false);
        }
        


        //if (cameraRaycaster.layerHit == Layer.Walkable) {
        //    m_Character.Move(currentClickTarget - transform.position, false, false);
        //}

    }
}

