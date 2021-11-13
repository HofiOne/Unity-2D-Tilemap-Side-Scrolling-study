//------------------------------------------------------------------------------

using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using Cinemachine;
//------------------------------------------------------------------------------

[RequireComponent(typeof(Rigidbody2D))]

public class FlyingPlayer : MonoBehaviour 
{
    public Vector2 spriteSize;
    public Vector3 staticSpeed;
    public float controlMovementSpeed = 5f;
    public float pixelsPerUnit = 0f;
    public bool usePixelPerfectAlignment = false;

    // Just for simplicity it is a box collider now with a layer mask that never collides with anything in the scene
    // Anything would do with a design time editable box shape that has size property
    public BoxCollider2D framingObject = null;

    public bool useRigidBody = true;

    public UpdateType updateType = UpdateType.FixedUpdate;
    //------------------------------------------------------------------------------

    private Rigidbody2D rb;
    private Vector3 controlMovement;
    private float controlMovementSpeedMultiplier = 1f;
    //------------------------------------------------------------------------------

    private void SetupPhysics()
    {
        if (useRigidBody)
            rb = GetComponent<Rigidbody2D>();

        controlMovementSpeedMultiplier = 1.2f;
    }
    //------------------------------------------------------------------------------

    protected void Start()
    {
        SetupPhysics();
    }
    //------------------------------------------------------------------------------

    private void OnEnable()
    {
        CinemachineCore.CameraUpdatedEvent.RemoveListener(OnCameraUpdated);
        CinemachineCore.CameraUpdatedEvent.AddListener(OnCameraUpdated);
    }
    //------------------------------------------------------------------------------

    private void OnDisable()
    {
        CinemachineCore.CameraUpdatedEvent.RemoveListener(OnCameraUpdated);
    }
    //------------------------------------------------------------------------------

    protected void Update()
    {
        if (false == Input.GetKeyDown(KeyCode.LeftCommand) && false == Input.GetKey(KeyCode.LeftCommand) && false == Input.GetKeyDown(KeyCode.RightCommand) && false == Input.GetKey(KeyCode.RightCommand))
            controlMovement = new Vector3(CrossPlatformInputManager.GetAxis("Horizontal"), CrossPlatformInputManager.GetAxis("Vertical"));

        if (updateType == UpdateType.Update)
            Move(Time.deltaTime);
    }
    //------------------------------------------------------------------------------

    protected void LateUpdate()
    {
        if (updateType == UpdateType.LateUpdate)
            Move(Time.deltaTime);
    }
    //------------------------------------------------------------------------------

    protected void FixedUpdate()
    {
        if (updateType == UpdateType.FixedUpdate)
            Move(Time.fixedDeltaTime);
    }
    //------------------------------------------------------------------------------

    void OnCameraUpdated(CinemachineBrain brain)
    {
        if (updateType == UpdateType.CVCUpdate)
            Move(Time.deltaTime);
    }
    //------------------------------------------------------------------------------

    private Vector3 pixelPerfectClamp(Vector3 locationVector, float pixelsPerUnit)
    {
        Vector3 vectorInPixels = new Vector3(Mathf.CeilToInt(locationVector.x * pixelsPerUnit), Mathf.CeilToInt(locationVector.y * pixelsPerUnit), Mathf.CeilToInt(locationVector.z * pixelsPerUnit));
        return vectorInPixels / pixelsPerUnit;
    }
    //------------------------------------------------------------------------------

    public Vector3 GetPosition()
    {
        if (useRigidBody && rb) {
            return rb.position;
        }

        return transform.position;
    }
    //------------------------------------------------------------------------------

    private void Move(float timeMultiplier)
    {
        Vector3 origPosition = GetPosition();
        Vector3 moveAmount = (staticSpeed + controlMovement/*.normalized*/ * controlMovementSpeed * controlMovementSpeedMultiplier) * timeMultiplier;
        Vector3 newPos = origPosition + moveAmount;

        if (framingObject) {
            newPos.x = Mathf.Clamp(newPos.x, framingObject.transform.position.x - (framingObject.size.x + spriteSize.x) / 2, framingObject.transform.position.x + (framingObject.size.x + spriteSize.x) / 2);
            newPos.y = Mathf.Clamp(newPos.y, framingObject.transform.position.y - (framingObject.size.y + spriteSize.y) / 2, framingObject.transform.position.y + (framingObject.size.y + spriteSize.y) / 2);
        }

        if (newPos.Equals(origPosition))
            return;

        MovePosition(newPos);
    }
    //------------------------------------------------------------------------------
    
    public void MovePosition(Vector3 newPos)
    {
        if (usePixelPerfectAlignment && pixelsPerUnit != 0)
            newPos = pixelPerfectClamp(newPos, pixelsPerUnit);

        if (useRigidBody && rb)
            rb.MovePosition(newPos);
        else {
            if (usePixelPerfectAlignment)
                transform.position = newPos;
            else
                transform.Translate(newPos - GetPosition());
        }
    }
    //------------------------------------------------------------------------------
}
//------------------------------------------------------------------------------
