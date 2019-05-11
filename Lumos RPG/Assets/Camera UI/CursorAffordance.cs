using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CameraRaycaster))]
public class CursorAffordance : MonoBehaviour {

    [SerializeField] Texture2D walkCursor = null;

    [SerializeField] Texture2D targetCursor = null;
    [SerializeField] Texture2D unknownCursor = null;
    [SerializeField] Vector2 cursorHotspot = new Vector2(0, 0);


    //TODO solve serialize and const confliction
    [SerializeField] const int walkableLayer = 8;
    [SerializeField] const int enemyLayer = 9;
    [SerializeField] const int stiffLayer = 10;



    CameraRaycaster cameraRaycaster;

	// Use this for initialization
	void Start () {
        cameraRaycaster = GetComponent<CameraRaycaster>();
        //Registering as an observer
        cameraRaycaster.notifyLayerChangeObservers += OnLayerChanged;
	}
	
	// Only called when layer changes, when delegate is called
	private void OnLayerChanged (int newLayer) {
        switch (newLayer) {
            case walkableLayer:
                Cursor.SetCursor(walkCursor, cursorHotspot, CursorMode.Auto);
                break;
            case enemyLayer:
                Cursor.SetCursor(targetCursor, cursorHotspot, CursorMode.Auto);
                break;
            default:
                Debug.LogError("Unknown raycaster cursor case");
                Cursor.SetCursor(unknownCursor, cursorHotspot, CursorMode.Auto);
                return;
        }
    }

    // TODO consider de-registering OnLayerChanged on leaving all game scenes

}
