using UnityEngine;
using System.Collections;

public class ProgressInfo : MonoBehaviour 
{
    // Border width of progress and hp bars
    private const int LINE_WIDTH = 3;

    // Textures for ui
    private Texture2D _progressFore;
    private Texture2D _progressBack;
    private Texture2D _healthFore;
    private Texture2D _healthBack;
    private Texture2D _line;

    // Locations of progress and health bars
    private Rect _progressRect;
    private Rect _healthRect;

    void Start()
    {
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

        // Location of progress bar on screen
        _progressRect = new Rect(Screen.width * 0.25f, Screen.height / 25, Screen.width / 2, Screen.height / 25);
        // Location of health bar on screen
        _healthRect = new Rect(Screen.width * 0.25f, Screen.height / 25 * 2.2f, Screen.width / 2, Screen.height / 25);
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
    }
}
