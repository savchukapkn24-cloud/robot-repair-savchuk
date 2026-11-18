using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Дія «рух»: клавіші (стрілки й WASD) призначаються в Inspector
    public InputAction MoveAction;

    // Фізичне тіло робота і поточний напрямок руху
    Rigidbody2D rigidbody2d;
    Vector2 move;

    // Викликається один раз перед першим Update
    void Start()
    {
        MoveAction.Enable();
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    // Щокадру тільки читаємо ввід
    void Update()
    {
        move = MoveAction.ReadValue<Vector2>();
        Debug.Log(move);
    }

    // У такт фізики рухаємо тіло через рушій
    void FixedUpdate()
    {
        Vector2 position = (Vector2)rigidbody2d.position
            + move * 3.0f * Time.deltaTime;
        rigidbody2d.MovePosition(position);
    }
}
