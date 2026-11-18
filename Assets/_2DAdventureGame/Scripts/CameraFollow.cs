using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    Transform target;

    void Start()
    {
        PlayerController player = FindFirstObjectByType<PlayerController>();
        if (player != null)
        {
            target = player.transform;
        }
    }

    void LateUpdate()
    {
        if (target != null)
        {
            transform.position = new Vector3(
                target.position.x, target.position.y, transform.position.z);
        }
    }
}
