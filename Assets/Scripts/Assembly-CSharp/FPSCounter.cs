using UnityEngine;

public class FPSCounter : MonoBehaviour
{
	private const float updateTime = 1f;

	private float frames;

	private float time;

	private string fps = string.Empty;

	private void Update()
	{
		time += Time.deltaTime;
		if (time >= 1f)
		{
			fps = "FPS: " + (frames / time).ToString("f2");
			time = 0f;
			frames = 0f;
		}
		frames += 1f;
	}

	private void OnGUI()
	{
		GUI.Label(new Rect(Screen.width - 100, Screen.height - 45, 100f, 20f), fps);
	}
}
