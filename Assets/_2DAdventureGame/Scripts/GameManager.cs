using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public PlayerController player;
    EnemyController[] enemies;
    public UIHandler uiHandler;
    public AudioClip winClip;
    public float restartDelay = 3f;

    bool ended;
    int lastFixed = -1;

    void Start()
    {
        enemies = FindObjectsByType<EnemyController>(
            FindObjectsSortMode.None);
    }

    void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void Update()
    {
        int fixedCount = CountFixed();
        if (fixedCount != lastFixed)
        {
            lastFixed = fixedCount;
            uiHandler.SetCounter(fixedCount, enemies.Length);
        }

        if (ended)
        {
            return;
        }

        if (player.health <= 0)
        {
            ended = true;
            uiHandler.DisplayLoseScreen();
            Invoke(nameof(ReloadScene), restartDelay);
        }
        else if (enemies.Length > 0 && fixedCount == enemies.Length)
        {
            ended = true;
            Sfx.Play(winClip);
            uiHandler.DisplayWinScreen();
            Invoke(nameof(ReloadScene), restartDelay);
        }
    }

    int CountFixed()
    {
        int count = 0;
        foreach (EnemyController enemy in enemies)
        {
            if (!enemy.isBroken) count++;
        }
        return count;
    }
}
