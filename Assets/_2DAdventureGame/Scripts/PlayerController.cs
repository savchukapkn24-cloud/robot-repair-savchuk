using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Дія «рух»: клавіші (стрілки й WASD) призначаються в Inspector
    public InputAction MoveAction;

    // Викликається один раз перед першим Update
    void Start()
    {
        MoveAction.Enable();
    }

    // Викликається рушієм щокадру
    void Update()
    {
        // Читаємо напрямок: x і y від -1 до 1
        Vector2 move = MoveAction.ReadValue<Vector2>();
        Debug.Log(move);

        // 3 одиниці за секунду; Time.deltaTime робить швидкість незалежною від fps
        Vector2 position = (Vector2)transform.position + move * 3.0f * Time.deltaTime;
        transform.position = position;
    }
}
