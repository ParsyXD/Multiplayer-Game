using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[RequireComponent(typeof(Camera))]
public class DofFocus : MonoBehaviour
{
    private Camera cameraMain;
    private DepthOfField dof;

    private Ray ray;
    private RaycastHit hit;
    private Vector3 viewportCenter;
    public LayerMask mask;

    public float defaultDistance = 5f;
    public float minDistance = 0.5f;
    private float hitDistance;
    public float focusSpeed = 1f;
    public int updateFrequency = 2;

    private Transform thisTransform;

    private void Awake()
    {
        thisTransform = transform;
    }

    private void Start()
    {
        cameraMain = GetComponent<Camera>();
    }

    private void Update()
    {
        if (Time.frameCount % updateFrequency == 0)
        {
            ray = cameraMain.ViewportPointToRay(viewportCenter);
            if (Physics.Raycast(ray, out hit, defaultDistance - 0.1f, mask))
            {
                hitDistance = hit.distance;
                if (hitDistance < minDistance)
                {
                    hitDistance = minDistance;
                }
                cameraMain.focalLength = Mathf.Lerp(cameraMain.focalLength, hitDistance, focusSpeed);
            }
            else
            {
                if (cameraMain.focalLength < defaultDistance)
                {
                    cameraMain.focalLength = Mathf.Lerp(cameraMain.focalLength, defaultDistance, focusSpeed);
                }
            }
        }
    }
}