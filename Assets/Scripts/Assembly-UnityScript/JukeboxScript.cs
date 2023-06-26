using System;
using UnityEngine;

[Serializable]
public class JukeboxScript : MonoBehaviour
{
	public YandereScript Yandere;

	public AudioSource Piano;

	public AudioSource HipHop;

	public AudioSource AttackOnTitan;

	public AudioSource Sane;

	public AudioSource Halfsane;

	public AudioSource Insane;

	public float FadeSpeed;

	public float Volume;

	public int Track;

	public float Timer;

	public bool AoT;

	public virtual void Update()
	{
		Timer += Time.deltaTime;
		if (!AoT)
		{
			if (Input.GetKeyDown("m"))
			{
				if (Volume == 1f)
				{
					FadeSpeed = 10f;
					Volume = 0f;
				}
				else
				{
					FadeSpeed = 1f;
					Volume = 1f;
				}
			}
			if (Input.GetKeyDown("n"))
			{
				Sane.time += 60f;
				Halfsane.time += 60f;
				Insane.time += 60f;
			}
			if (!(Timer <= 5f))
			{
				if (!(Yandere.Sanity < 66.66666f))
				{
					Sane.volume = Mathf.MoveTowards(Sane.volume, Volume, Time.deltaTime * FadeSpeed);
					Halfsane.volume = Mathf.MoveTowards(Halfsane.volume, 0f, Time.deltaTime * FadeSpeed);
					Insane.volume = Mathf.MoveTowards(Insane.volume, 0f, Time.deltaTime * FadeSpeed);
				}
				else if (!(Yandere.Sanity < 33.33333f))
				{
					Sane.volume = Mathf.MoveTowards(Sane.volume, 0f, Time.deltaTime * FadeSpeed);
					Halfsane.volume = Mathf.MoveTowards(Halfsane.volume, Volume, Time.deltaTime * FadeSpeed);
					Insane.volume = Mathf.MoveTowards(Insane.volume, 0f, Time.deltaTime * FadeSpeed);
				}
				else
				{
					Sane.volume = Mathf.MoveTowards(Sane.volume, 0f, Time.deltaTime * FadeSpeed);
					Halfsane.volume = Mathf.MoveTowards(Halfsane.volume, 0f, Time.deltaTime * FadeSpeed);
					Insane.volume = Mathf.MoveTowards(Insane.volume, Volume, Time.deltaTime * FadeSpeed);
				}
			}
		}
		else if (!AttackOnTitan.enabled)
		{
			AttackOnTitan.enabled = true;
			Sane.volume = 0f;
			Halfsane.volume = 0f;
			Insane.volume = 0f;
		}
		if (Input.GetKeyDown("l"))
		{
			AoT = true;
		}
	}

	public virtual void Main()
	{
	}
}
