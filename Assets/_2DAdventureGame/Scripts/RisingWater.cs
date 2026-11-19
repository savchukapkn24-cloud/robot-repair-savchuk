using UnityEngine;

public class RisingWater : MonoBehaviour
{
    public float riseSpeed = 0.2f;
    public float maxHeight = -4.5f;
    public int damage = 1;
    public float startX = -999f;

    PlayerController player;
    bool started;

    void Start()
    {
        player = FindFirstObjectByType<PlayerController>();
    }

    void Update()
    {
        if (!started)
        {
            if (player != null && player.transform.position.x >= startX)
            {
                started = true;
            }
            else
            {
                return;
            }
        }

        if (transform.position.y < maxHeight)
        {
            transform.position += Vector3.up * riseSpeed * Time.deltaTime;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        PlayerController controller =
            other.GetComponent<PlayerController>();

        if (controller != null)
        {
            controller.ChangeHealth(-damage);
        }
    }
}
