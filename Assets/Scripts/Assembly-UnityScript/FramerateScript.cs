using System;
using UnityEngine;

[Serializable]
public class FramerateScript : MonoBehaviour
{
	public float updateInterval;

	private float accum;

	private int frames;

	private float timeleft;

	public FramerateScript()
	{
		updateInterval = 0.5f;
	}

	public virtual void Start()
	{
		if (!GetComponent<GUIText>())
		{
			MonoBehaviour.print("FramesPerSecond needs a GUIText component!");
			enabled = false;
		}
		else
		{
			timeleft = updateInterval;
		}
	}

	public virtual void Update()
	{
		timeleft -= Time.deltaTime;
		accum += Time.timeScale / Time.deltaTime;
		frames++;
		if (!(timeleft > 0f))
		{
			GetComponent<GUIText>().text = "FPS: " + (accum / (float)frames).ToString("f0");
			timeleft = updateInterval;
			accum = 0f;
			frames = 0;
		}
	}

	public virtual void Main()
	{
	}
}
