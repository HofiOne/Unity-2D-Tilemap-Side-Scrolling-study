//------------------------------------------------------------------------------

using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
//------------------------------------------------------------------------------

public class MonoBehaviourPlus : MonoBehaviour
{
    public enum AutoDestroyBehavior
    {
        Unknown,
        Never,
        IfNotUsedAnymore,
        OnBecameInvisible,
        OnBecameInvisibleLeft,
    }

    public Vector2 size;
    public AutoDestroyBehavior autoDestroyBehavior = AutoDestroyBehavior.Unknown;
    public float autoDestroyCleanupCheckInterval = 1.0f;
    //------------------------------------------------------------------------------

    protected float objectWidth = 0f;
    protected float objectHeight = 0f;
    protected SpriteRenderer spriteRenderer = null;
    protected Animator animator = null;
    //------------------------------------------------------------------------------

    private float lastAutoDestroyCleanupCheck = 0f;
    //------------------------------------------------------------------------------

    protected void Awake()
    {
        if (autoDestroyBehavior == AutoDestroyBehavior.Unknown)
            autoDestroyBehavior = AutoDestroyBehavior.Never;
    }
    //------------------------------------------------------------------------------

    protected void Start()
    {
        getSize();
    }
    //------------------------------------------------------------------------------

    // Take care !!! (do not destroy objects during edit time as it will be a permanent deletion!!!)
    //
    protected void CheckShouldAutoDestroy()
    {
        if (UnityEngine.Application.IsPlaying(gameObject)) {
            //if ((autoDestroyBehavior == AutoDestroyBehavior.OnBecameInvisibleLeft &&
            //     Application.Camera &&
            //     CommonHelpers.IsTargetInVisible(Application.Camera, gameObject, CommonHelpers.Direction.Left))
            //    ) {
            //    Destroy(gameObject);
            //}
        }
    }
    //------------------------------------------------------------------------------

    // Update is called once per frame
    protected void Update()
    {
        lastAutoDestroyCleanupCheck += Time.deltaTime;

        if (lastAutoDestroyCleanupCheck > autoDestroyCleanupCheckInterval) {
            CheckShouldAutoDestroy();
            lastAutoDestroyCleanupCheck = 0f;
        }
    }
    //------------------------------------------------------------------------------

    protected virtual void OnBecameInvisible()
    {
        if (UnityEngine.Application.IsPlaying(gameObject)) {
            if (autoDestroyBehavior == AutoDestroyBehavior.OnBecameInvisible) {
                Destroy(gameObject);
            }
        }
    }
    //------------------------------------------------------------------------------

    public Vector2 getSize()
    {
        if (size.x <= 0 || size.y <= 0) {
            TryGetComponent(out spriteRenderer);
            if (spriteRenderer == null)
                spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            if (null != spriteRenderer) {
                TryGetComponent(out animator);
                if (animator == null)
                    animator = GetComponentInChildren<Animator>();
            }

            if (spriteRenderer) {
                objectWidth = spriteRenderer.bounds.extents.x; //extents = size of width / 2
                objectHeight = spriteRenderer.bounds.extents.y; //extents = size of height / 2

                if (size.x <= 0)
                    size.x = objectWidth * 2f;
                if (size.y <= 0)
                    size.y = objectHeight * 2f;
            }
        }
        else {
            if (objectWidth == 0)
                objectWidth = size.x / 2f;
            if (objectHeight == 0)
                objectHeight = size.y / 2f;
        }
        return size;
    }
    //------------------------------------------------------------------------------
}
//------------------------------------------------------------------------------