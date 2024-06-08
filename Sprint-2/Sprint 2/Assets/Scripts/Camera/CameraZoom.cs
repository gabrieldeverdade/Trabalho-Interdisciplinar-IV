using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    private float zoom;
	[SerializeField] private float zoomMultiplier = 1f;
    private float minZoom = 2f;
    private float maxZoom = 10f;
    private float velocity = 0f;
    private float smoothTime = 0.25f;

    [SerializeField] private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        zoom = cam.orthographicSize;
      
    }

    // Update is called once per frame
    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        zoom -= scroll * zoomMultiplier;
        zoom = Mathf.Clamp(zoom, minZoom, maxZoom);
        cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, zoom, ref velocity, smoothTime);
    }
}
