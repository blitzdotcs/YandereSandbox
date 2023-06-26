using System;
using UnityEngine;

[Serializable]
public class YouTubeScript : MonoBehaviour
{
	public float Timer;

	public virtual void Update()
	{
		Timer += Time.deltaTime;
		if (!(Timer <= 1f))
		{
			GetComponent<AudioSource>().Play();
			UnityEngine.Object.Destroy(this);
		}
	}

	public virtual void Main()
	{
	}
}
