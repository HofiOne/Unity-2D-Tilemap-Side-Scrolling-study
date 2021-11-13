
using UnityEngine;
using Cinemachine;

public enum UpdateType
{
    CVCUpdate,
    Update,
    FixedUpdate,
    LateUpdate
}

public class TransformHandler : MonoBehaviour
{
    public float speed = 3f;
    public float pixelsPerUnit = 0f;
    public bool usePixelPerfectAlignment = false;
    public Camera camToSyncToY = null;
    public UpdateType updateType = UpdateType.Update;

    private Vector3 pixelPerfectClamp(Vector3 locationVector, float pixelsPerUnit)
    {
        Vector3 vectorInPixels = new Vector3(Mathf.CeilToInt(locationVector.x * pixelsPerUnit), Mathf.CeilToInt(locationVector.y * pixelsPerUnit), Mathf.CeilToInt(locationVector.z * pixelsPerUnit));
        return vectorInPixels / pixelsPerUnit;
    }

    private void OnEnable()
    {
        CinemachineCore.CameraUpdatedEvent.RemoveListener(OnCameraUpdated);
        CinemachineCore.CameraUpdatedEvent.AddListener(OnCameraUpdated);
    }

    private void OnDisable()
    {
        CinemachineCore.CameraUpdatedEvent.RemoveListener(OnCameraUpdated);
    }

    void DoWork(float timeMultiplier)
    {
        if (speed != 0 || camToSyncToY
            ) {
            Vector3 newPos = Vector3.zero;

            if (speed != 0)
                newPos = (Vector3.right * speed * timeMultiplier);

            if (camToSyncToY != null) {
                Vector3 camToSyncPos = camToSyncToY.transform.position;
                newPos.y += (Vector3.up * (camToSyncPos.y - transform.position.y)).y;
            }

            if (usePixelPerfectAlignment && pixelsPerUnit != 0) {
                newPos = pixelPerfectClamp(transform.position + newPos, pixelsPerUnit);
                transform.position = newPos;
            }
            else
                transform.Translate(newPos);
        }
    }

    void OnCameraUpdated(CinemachineBrain brain)
    {
        if (updateType == UpdateType.CVCUpdate)
            DoWork(Time.deltaTime);
    }

    void Update()
    {
        if (updateType == UpdateType.Update)
            DoWork(Time.deltaTime);
    }

    void FixedUpdate()
    {
        if (updateType == UpdateType.FixedUpdate)
            DoWork(Time.fixedDeltaTime);
    }

    void LateUpdate()
    {
        if (updateType == UpdateType.LateUpdate)
            DoWork(Time.deltaTime);
    }
}
