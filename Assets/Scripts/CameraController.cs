using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    private void FixedUpdate()
    {
        Vector3 playerPos = Player.Instance.transform.position;
        Vector3 playerPositionOnScreen = Camera.main.WorldToScreenPoint(playerPos);
        Vector3 target = Camera.main.ScreenToWorldPoint((Input.mousePosition + playerPositionOnScreen) / 2);
        Vector3 targetRelativeToPlayer = Vector3.ClampMagnitude(target - playerPos, 5.0f);

        Vector3 nextPosition = Player.Instance.transform.position + targetRelativeToPlayer;

        transform.position = Vector3.Lerp(transform.position, new Vector3(nextPosition.x, transform.position.y, nextPosition.z), 0.2f);


        
    }
}
