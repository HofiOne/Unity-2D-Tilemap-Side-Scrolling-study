
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
    public Camera camToSyncToY = null;
    public float pixelsPerUnit = 0f;
    public bool usePixelPerfectAlignment = false;
    public UpdateType updateType = UpdateType.Update;
    public bool useRigidbodyMoving = false;

    private Rigidbody2D[] rigidbody2Ds = null;

    private Vector3 pixelPerfectClamp(Vector3 locationVector, float pixelsPerUnit)
    {
        Vector3 vectorInPixels = new Vector3(Mathf.CeilToInt(locationVector.x * pixelsPerUnit), Mathf.CeilToInt(locationVector.y * pixelsPerUnit), Mathf.CeilToInt(locationVector.z * pixelsPerUnit));
        return vectorInPixels / pixelsPerUnit;
    }

    private void OnEnable()
    {
        CinemachineCore.CameraUpdatedEvent.RemoveListener(OnCameraUpdated);
        CinemachineCore.CameraUpdatedEvent.AddListener(OnCameraUpdated);

        rigidbody2Ds = gameObject.GetComponentsInChildren<Rigidbody2D>();
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
            else {
                if (rigidbody2Ds.Length == 0 || false == useRigidbodyMoving)
                    transform.Translate(newPos);
                else
                    foreach (Rigidbody2D rb in rigidbody2Ds)
                        rb.MovePosition(rb.position + new Vector2(newPos.x, newPos.y));
            }
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
