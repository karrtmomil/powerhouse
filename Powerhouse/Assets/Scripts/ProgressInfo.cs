using UnityEngine;
using System.Collections;

public class ProgressInfo : MonoBehaviour 
{
    // Skin for score and multiplier
    public GUISkin Skin;

    // Border width of progress and hp bars
    private const int LINE_WIDTH = 3;

    // Textures for ui
    private Texture2D _progressFore;
    private Texture2D _progressBack;
    private Texture2D _healthFore;
    private Texture2D _healthBack;
    private Texture2D _line;
    private Texture2D _dialBack;

    // Locations of progress and health bars
    private Rect _progressRect;
    private Rect _healthRect;

    // Locations of heading and velocity dials
    private Rect _headingRect;
    private Rect _velocityRect;

    private float unitHeight;

    void Start()
    {
        // Load dial image
        _dialBack = Resources.Load<Texture2D>("Textures/dial");

        // Initializes textures used
        _progressFore = new Texture2D(1, 1);
        _progressBack = new Texture2D(1, 1);
        _healthFore = new Texture2D(1, 1);
        _healthBack = new Texture2D(1, 1);
        _line = new Texture2D(1, 1);

        // Sets texture colors
        _progressFore.SetPixel(0, 0, Color.green);
        _progressBack.SetPixel(0, 0, Color.gray);
        _healthFore.SetPixel(0, 0, Color.red);
        _healthBack.SetPixel(0, 0, Color.gray);
        _line.SetPixel(0, 0, Color.black);

        // Applys colors to textures
        _progressFore.Apply();
        _progressBack.Apply();
        _healthFore.Apply();
        _healthBack.Apply();
        _line.Apply();

        unitHeight = Screen.height / 25 * 0.5f;
        // Location of progress bar on screen
        _progressRect = new Rect(Screen.width * 0.25f, unitHeight, Screen.width / 2, Screen.height / 25);
        // Location of health bar on screen
        _healthRect = new Rect(Screen.width * 0.25f, unitHeight + unitHeight * 2.4f, Screen.width / 2, Screen.height / 25);

        // Locations of heading and velocity dials
        _headingRect = new Rect(20, 20, Screen.width / 6, Screen.height / 6);
        _velocityRect = new Rect(Screen.width - (Screen.width / 6 + 20), 20, Screen.width / 6, Screen.height / 6);
    }

    void OnGUI()
    {
        // Draws progress and health bars
        GUITools.progressBar(_progressFore, _progressBack, _line, LINE_WIDTH, _progressRect, GameController.Instance.Progress);
        GUITools.progressBar(_healthFore, _healthBack, _line, LINE_WIDTH, _healthRect, GameController.Instance.ShipHealth);

        // Gets text size (width and height) of progress and health bar labels
        Vector2 progressTextSize = GUI.skin.GetStyle("Label").CalcSize(new GUIContent("Progress"));
        Vector2 healthTextSize = GUI.skin.GetStyle("Label").CalcSize(new GUIContent("Ship Health"));

        // Creates a rect centered in the progress bar rect
        Rect progressTextRect = new Rect(_progressRect.x + (_progressRect.width / 2 - progressTextSize.x / 2), 
            _progressRect.y + (_progressRect.height / 2 - progressTextSize.y / 2), progressTextSize.x, progressTextSize.y);

        // Creates a rect centered in the health bar rect
        Rect healthTextRect = new Rect(_healthRect.x + (_healthRect.width / 2 - healthTextSize.x / 2), 
            _healthRect.y + (_healthRect.height / 2 - healthTextSize.y / 2), healthTextSize.x, healthTextSize.y); 

        // Draws the labels over the progress and health bars
        GUI.Label(progressTextRect, "Progress");
        GUI.Label(healthTextRect, "Ship Health");

        // Draws the dial backgrounds for heading and velocity
        GUI.DrawTexture(_headingRect, _dialBack);
        GUI.DrawTexture(_velocityRect, _dialBack);

        // Draw labels for score and multiplier
        GUISkin current = GUI.skin;
        Color currentColor = GUI.color;
        GUI.skin = Skin;
        GUI.color = Color.green;
        GUI.Label(new Rect(Screen.width * 0.25f, unitHeight + unitHeight * 2.4f * 2, 400, 100), "Score: " + GameController.Instance.Score);
        GUI.Label(new Rect(Screen.width * 0.25f * 2, unitHeight + unitHeight * 2.4f * 2, 400, 100), "Multiplier: " + GameController.Instance.Multiplier + "x");
        GUI.skin = current;
        GUI.color = currentColor;
    }
}
