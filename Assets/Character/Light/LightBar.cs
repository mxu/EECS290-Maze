using UnityEngine;
using System.Collections;

public class LightBar : MonoBehaviour {
	
	public float progress = 1.0f;
	public Vector2 pos = new Vector2(.01f, .01f);
	public Vector2 size = new Vector2(256,256);
	public Texture2D emptyTex;
	public Texture2D fullTex;
	
	void OnGUI() {
		float posX = Screen.width * pos.x;
		float posY = Screen.height * pos.y;
		
		//draw the background:
		GUI.BeginGroup(new Rect(posX, posY, size.x, size.y));
		GUI.DrawTexture(new Rect(0,0, size.x, size.y), emptyTex);
		
		//draw the filled-in part:
		//GUI.BeginGroup(new Rect(0, 0, size.x * progress, size.y));
		//GUI.DrawTexture(new Rect(0,0, size.x, size.y), fullTex);
		//draw the filled-in part FLIPPED:
		int xProg = (int)(size.x * progress);
		GUI.BeginGroup(new Rect(size.x - xProg, 0, xProg, size.y));
		GUI.DrawTexture(new Rect(-size.x + xProg,0, size.x, size.y), fullTex);
		
		GUI.EndGroup();
		GUI.EndGroup();
	}
}