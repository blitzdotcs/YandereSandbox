using System;
using UnityEngine;

[Serializable]
public class CameraEffectsScript : MonoBehaviour
{
	public YandereScript Yandere;

	public Vignetting Vignette;

	public UITexture Streaks;

	public Bloom AlarmBloom;

	public float EffectStrength;

	public virtual void Start()
	{
		int num = 0;
		Color color = Streaks.color;
		float num2 = (color.a = num);
		Color color3 = (Streaks.color = color);
	}

	public virtual void Update()
	{
		if (!(Streaks.color.a <= 0f))
		{
			AlarmBloom.bloomIntensity -= Time.deltaTime;
			float a = Streaks.color.a - Time.deltaTime;
			Color color = Streaks.color;
			float num = (color.a = a);
			Color color3 = (Streaks.color = color);
			if (!(Streaks.color.a > 0f))
			{
				AlarmBloom.enabled = false;
			}
		}
		EffectStrength = 1f - Yandere.Sanity * 0.01f;
		Vignette.intensity = Mathf.Lerp(Vignette.intensity, EffectStrength * 5f, Time.deltaTime);
		Vignette.blur = Mathf.Lerp(Vignette.blur, EffectStrength, Time.deltaTime);
		Vignette.chromaticAberration = Mathf.Lerp(Vignette.chromaticAberration, EffectStrength * 5f, Time.deltaTime);
	}

	public virtual void Alarm()
	{
		AlarmBloom.bloomIntensity = 1f;
		int num = 1;
		Color color = Streaks.color;
		float num2 = (color.a = num);
		Color color3 = (Streaks.color = color);
		AlarmBloom.enabled = true;
	}

	public virtual void Main()
	{
	}
}
