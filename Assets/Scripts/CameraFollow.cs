using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform playerTransform;
    public Vector3 offset = new Vector3(0f, 3f, -5f); 

    void LateUpdate()
    {
        if (playerTransform != null)
        {
            transform.LookAt(playerTransform.position + Vector3.up * 1.0f);
            transform.position = playerTransform.position + offset;
        }
        else
        {
            Debug.LogWarning("Player Transform not assigned to CameraFollow script!");
        }
    }
}