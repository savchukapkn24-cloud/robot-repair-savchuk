using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class UIHandler : MonoBehaviour
{
    public static UIHandler instance { get; private set; }

    public float displayTime = 4.0f;
    private VisualElement m_NonPlayerDialogue;
    private float m_TimerDisplay;

    private VisualElement m_Healthbar;

    private VisualElement m_WinScreen;
    private VisualElement m_LoseScreen;

    private VisualElement m_StartScreen;
    private Label m_Counter;
    static bool s_StartDismissed;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        UIDocument uiDocument = GetComponent<UIDocument>();
        m_Healthbar = uiDocument.rootVisualElement
            .Q<VisualElement>("HealthBar");
        SetHealthValue(1.0f);

        m_NonPlayerDialogue = uiDocument.rootVisualElement
            .Q<VisualElement>("NPCDialogue");
        m_NonPlayerDialogue.style.display = DisplayStyle.None;
        m_TimerDisplay = -1.0f;

        m_LoseScreen = uiDocument.rootVisualElement
            .Q<VisualElement>("LoseScreenContainer");
        m_WinScreen = uiDocument.rootVisualElement
            .Q<VisualElement>("WinScreenContainer");

        VisualElement root = uiDocument.rootVisualElement;
        m_LoseScreen.Add(MakeLabel("Поразка. Нова спроба за кілька секунд", 40, true));
        m_WinScreen.Add(MakeLabel("Перемога! Нова гра за кілька секунд", 40, true));

        m_Counter = MakeLabel("", 40, false);
        m_Counter.style.top = 20;
        m_Counter.style.right = 30;
        m_Counter.style.left = StyleKeyword.Auto;
        m_Counter.style.unityTextAlign = TextAnchor.UpperRight;
        root.Add(m_Counter);

        if (!s_StartDismissed)
        {
            m_StartScreen = new VisualElement();
            m_StartScreen.style.position = Position.Absolute;
            m_StartScreen.style.top = 0;
            m_StartScreen.style.bottom = 0;
            m_StartScreen.style.left = 0;
            m_StartScreen.style.right = 0;
            m_StartScreen.style.backgroundColor = new Color(0.04f, 0.1f, 0.18f, 0.94f);
            m_StartScreen.style.justifyContent = Justify.Center;
            m_StartScreen.style.alignItems = Align.Center;
            m_StartScreen.Add(MakeLabel("ROBOT REPAIR", 96, false, true));
            m_StartScreen.Add(MakeLabel("Полагодь усіх зламаних роботів, кидаючи шестерні (C)", 40, false, true));
            m_StartScreen.Add(MakeLabel("Натисни будь-яку клавішу, щоб почати", 32, false, true));
            root.Add(m_StartScreen);
            Time.timeScale = 0f;
        }
    }

    public void SetHealthValue(float percentage)
    {
        m_Healthbar.style.width = Length.Percent(100 * percentage);
    }

    Label MakeLabel(string text, int size, bool bottom, bool centered = false)
    {
        Label label = new Label(text);
        label.pickingMode = PickingMode.Ignore;
        label.style.fontSize = size;
        label.style.color = Color.white;
        label.style.unityFontStyleAndWeight = FontStyle.Bold;
        label.style.unityTextAlign = TextAnchor.MiddleCenter;
        label.style.whiteSpace = WhiteSpace.Normal;
        if (!centered)
        {
            label.style.position = Position.Absolute;
            label.style.left = 0;
            label.style.right = 0;
            if (bottom) label.style.bottom = 40;
        }
        else
        {
            label.style.marginBottom = 24;
            label.style.paddingLeft = 40;
            label.style.paddingRight = 40;
        }
        return label;
    }

    public void SetCounter(int fixedCount, int total)
    {
        if (m_Counter != null)
        {
            m_Counter.text = "Полагоджено: " + fixedCount + " / " + total;
        }
    }

    private void Update()
    {
        if (m_StartScreen != null && Keyboard.current != null
            && Keyboard.current.anyKey.wasPressedThisFrame)
        {
            m_StartScreen.RemoveFromHierarchy();
            m_StartScreen = null;
            s_StartDismissed = true;
            Time.timeScale = 1f;
        }

        if (m_TimerDisplay > 0)
        {
            m_TimerDisplay -= Time.deltaTime;
            if (m_TimerDisplay < 0)
            {
                m_NonPlayerDialogue.style.display = DisplayStyle.None;
            }
        }
    }

    public void DisplayDialogue()
    {
        m_NonPlayerDialogue.style.display = DisplayStyle.Flex;
        m_TimerDisplay = displayTime;
    }

    public void DisplayWinScreen()
    {
        m_WinScreen.style.opacity = 1.0f;
    }

    public void DisplayLoseScreen()
    {
        m_LoseScreen.style.opacity = 1.0f;
    }
}
