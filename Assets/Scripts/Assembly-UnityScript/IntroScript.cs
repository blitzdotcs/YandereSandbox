using System;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class IntroScript : MonoBehaviour
{
	public AudioSource Narration;

	public AudioSource Music;

	public UILabel Label;

	public string[] Lines;

	public float[] Cue;

	public bool Narrating;

	public bool Musicing;

	public float Timer;

	public int ID;

	public virtual void Update()
	{
		Timer += Time.deltaTime;
		if (!(Timer <= 1f))
		{
			if (!Narrating)
			{
				Narration.Play();
				Narrating = true;
			}
			if (!Musicing && !(Timer <= 5f))
			{
				Music.Play();
				Musicing = true;
			}
			if (ID < Extensions.get_length((System.Array)Cue) && !(Timer <= Cue[ID]))
			{
				ID++;
			}
		}
		if (Input.GetKeyDown("space"))
		{
			Music.time += 5f;
		}
	}

	public virtual void Main()
	{
	}
}
