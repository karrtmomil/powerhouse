using UnityEngine;
using System.Collections;

public class MenuUI : MonoBehaviour 
{
    private Texture _title;
    private Rect _titleRect;
    public  GUISkin _style;
    private Rect _startRect;

	private void Start () 
    {
        _title = Resources.Load<Texture>(@"Textures/title");
        _titleRect = new Rect(Screen.width / 2 - Screen.width * 0.75f / 2, 20, Screen.width * 0.75f, Screen.height * 0.3f);
        _startRect = new Rect(Screen.width / 2 - 200, Screen.height - 100, 400, 70);
	}

    private void OnGUI()
    {
        Vector2 mousePos = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
        GUISkin current = GUI.skin;
        GUI.skin = _style;
        GUI.DrawTexture(_titleRect, _title);

        Color prevColor = GUI.color;
        if (_startRect.Contains(mousePos))
        {
            GUI.color = Color.gray;
            GUI.Label(_startRect, "Start Game");
            if (GUI.Button(_startRect, "", "Label"))
                Application.LoadLevel("Game");
        }
        else
        {
            GUI.Label(_startRect, "Start Game");
        }

        GUI.color = prevColor;
        GUI.skin = current;
    }
}
