using UnityEngine;
using System.Collections;

public class GameUI : MonoBehaviour 
{
    private Texture _crosshair;
    private Rect _crosshairLoc;

	void Start () 
    {
        _crosshair = Resources.Load(@"Textures/crosshair") as Texture;
        _crosshairLoc = new Rect(Screen.width / 2 - 50, Screen.height / 2 - 50, 100, 100);
        Screen.showCursor = false;
	}

    void OnGUI()
    {
        GUI.DrawTexture(_crosshairLoc, _crosshair);
    }
}
