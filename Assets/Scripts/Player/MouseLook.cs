using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    private void FixedUpdate()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        Vector3 targetPosition = Camera.main.ScreenToWorldPoint(mousePos);
        targetPosition.y = Player.pInstance.transform.position.y;
        //Debug.Log(targetPosition);
        float angle = AngleBetweenTwoPoints(transform.position, targetPosition);
        transform.rotation = Quaternion.Euler(new Vector3(90.0f, angle, transform.rotation.z));

        float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
        {
            return Mathf.Atan2(a.z - b.z, b.x - a.x) * Mathf.Rad2Deg;
        }
    }
}
