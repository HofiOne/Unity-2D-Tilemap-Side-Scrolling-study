
using UnityEngine;
using Cinemachine;


public class VCamSync : MonoBehaviour
{
    public FlyingPlayer player = null;
    public GameObject playerFramingBox = null;
    public UpdateType updateType = UpdateType.Update;

    private void OnEnable()
    {
        CinemachineCore.CameraUpdatedEvent.RemoveListener(OnCameraUpdated);
        CinemachineCore.CameraUpdatedEvent.AddListener(OnCameraUpdated);
    }

    private void OnDisable()
    {
        CinemachineCore.CameraUpdatedEvent.RemoveListener(OnCameraUpdated);
    }

    void DoWork()
    {
        if (player && playerFramingBox) {
            Vector3 moveAmount = new Vector3(playerFramingBox.transform.position.x, player.GetPosition().y) - transform.position;
            transform.Translate(moveAmount);
        }
    }

    private void Update()
    {
        if (updateType == UpdateType.Update)
            DoWork();
    }

    private void FixedUpdate()
    {
        if (updateType == UpdateType.FixedUpdate)
            DoWork();
    }

    private void LateUpdate()
    {
        if (updateType == UpdateType.LateUpdate)
            DoWork();
    }

    void OnCameraUpdated(CinemachineBrain brain)
    {
        if (updateType == UpdateType.CVCUpdate)
            DoWork();
    }
}
