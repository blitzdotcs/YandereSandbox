using UnityEngine;

public class SceneLoader : MonoBehaviour
{
	public static readonly string[] sceneNames = new string[6] { "Welcome", "Colors", "Transparency", "Occlusion", "Scripting", "Compound" };

	private int ox = 20;

	private int oy = 100;

	private int h = 30;

	private void OnGUI()
	{
		GUI.Label(new Rect(ox, oy + 10, 500f, 100f), "Load demo scene:");
		for (int i = 0; i < sceneNames.Length; i++)
		{
			string text = sceneNames[i];
			if (GUI.Button(new Rect(ox, oy + 30 + i * h, 120f, 20f), text))
			{
				Application.LoadLevel(text);
			}
		}
	}
}
