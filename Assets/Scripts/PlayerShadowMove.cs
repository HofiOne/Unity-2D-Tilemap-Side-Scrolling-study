using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerShadowMove : MonoBehaviour
{
    public FlyingPlayer Player;
    public CinemachineVirtualCamera MainVcam;
    
    CinemachineFramingTransposer ft;

    private void Start()
    {
        ft = MainVcam.GetCinemachineComponent<CinemachineFramingTransposer>();
        if (ft == null)
            Debug.LogError("Framing Transposer required on the vcam");
    }

    // LateUpdate so that it happens after PlayeMove.Update
    void LateUpdate()
    {
        // Move at copnstant speed
        var pos = transform.position;
        pos.x += Player.staticSpeed.x * Time.deltaTime;

        // Lock Y to player
        var playerPos = Player.transform.position;
        pos.y = playerPos.y;

        // Don't let player escape the vcam's soft zone.
        //
        // Here we calculate the maximum X distance the shadow can be from the player,
        // using the soft zone width set in the vcam's framing transposer.
        //
        // Note: dead zone X and bias X are assumed to be 0.  If nonzero, then calculation
        // would be a little more complicated, but doable.
        //
        // This dynamic calculation is better than a hardcoded distance, because
        // it's robust for all aspect ratios.  Computing it every frame means that you can
        // tune it live by adjusting the FT's game view guides.

        var screenWidth = MainVcam.m_Lens.OrthographicSize * MainVcam.m_Lens.Aspect;
        var maxDistance = ft.m_SoftZoneWidth * screenWidth;

        var offset = Mathf.Clamp(pos.x - playerPos.x, -maxDistance, maxDistance);// + (ft.m_ScreenX - 0.5f));
        pos.x = playerPos.x + offset;

        transform.position = pos;
    }
}
