using UnityEngine;

public class CameraRaycaster : MonoBehaviour {
    public Layer[] layerPriorities = {
        Layer.Enemy,
        Layer.Walkable
    };

    [SerializeField] float distanceToBackground = 100f;
    Camera viewCamera;

    RaycastHit raycasterHit;
    public RaycastHit hit {
        get { return raycasterHit; }
    }

    Layer layerHit;
    public Layer currentLayerHit {
        get { return layerHit; }
    }

    //New delegate type
    public delegate void onLayerChange(Layer newLayer);
    //Instantiate an observer set
    public event onLayerChange layerChangeObservers;

    //private void LayerChangeHandler() {
    //    Debug.Log("Layer change handled!");
    //}

    //private void OtherLayerChangeHandler() {
    //    Debug.Log("I also handled the layer change!");
    //}



    void Start() 
    {
        viewCamera = Camera.main;
        //layerChangeObservers += LayerChangeHandler;
        //layerChangeObservers += OtherLayerChangeHandler;
        //Call the delegates
        //layerChangeObservers();

    }

    void Update() {
        // Look for and return priority layer hit
        foreach (Layer layer in layerPriorities) {
            var hit = RaycastForLayer(layer);
            if (hit.HasValue) {
                raycasterHit = hit.Value;
                //If layer has changed
                if (layerHit != layer) {
                    layerHit = layer;
                    //Call the delegates
                    layerChangeObservers(layer);
                }
                
                return;
            }
        }

        // Otherwise return background hit
        raycasterHit.distance = distanceToBackground;
        layerHit = Layer.RaycastEndStop;
        layerChangeObservers(layerHit);
    }

    RaycastHit? RaycastForLayer(Layer layer)
    {
        int layerMask = 1 << (int)layer; // See Unity docs for mask formation
        Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit; // used as an out parameter
        bool hasHit = Physics.Raycast(ray, out hit, distanceToBackground, layerMask);
        if (hasHit)
        {
            return hit;
        }
        return null;
    }
}
