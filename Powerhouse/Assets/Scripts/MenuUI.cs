using UnityEngine;
using System.Collections;

public class MenuUI : MonoBehaviour 
{
    private Texture _title;
    private Rect _titleRect;
    public  GUISkin _style;

	private void Start () 
    {
        Screen.showCursor = true;
        _title = Resources.Load<Texture>(@"Textures/title");
        _titleRect = new Rect(Screen.width / 2 - Screen.width * 0.75f / 2, 20, Screen.width * 0.75f, Screen.height * 0.3f);
	}

    private void OnGUI()
    {
        Vector2 mousePos = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
        GUISkin current = GUI.skin;
        GUI.skin = _style;
        GUI.DrawTexture(_titleRect, _title);

        Vector2 textSize = GUI.skin.GetStyle("Label").CalcSize(new GUIContent("Start Game"));
        Rect textLocation = new Rect(Screen.width / 2 - textSize.x / 2, Screen.height - 120, textSize.x, textSize.y);

        Color prevColor = GUI.color;
        if (textLocation.Contains(mousePos))
        {
            GUI.color = Color.gray;
            GUI.Label(textLocation, "Start Game");
            if (GUI.Button(textLocation, "", "Label"))
                Application.LoadLevel("Game");
        }
        else
        {
            GUI.Label(textLocation, "Start Game");
        }

        GUI.color = prevColor;
        GUI.skin = current;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
            Application.Quit();
    }
}
