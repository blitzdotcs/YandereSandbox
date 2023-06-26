using System;
using UnityEngine;

[Serializable]
public class ClockScript : MonoBehaviour
{
	private string MinuteNumber;

	private string HourNumber;

	public YandereScript Yandere;

	public Bloom BloomEffect;

	public MotionBlur Blur;

	public Transform MinuteHand;

	public Transform HourHand;

	public UILabel PeriodLabel;

	public UILabel TimeLabel;

	public UILabel DayLabel;

	public float HalfwayTime;

	public float PresentTime;

	public float TargetTime;

	public float StartTime;

	public float HourTime;

	public float StartHour;

	public float TimeSpeed;

	public float Minute;

	public float Hour;

	public int Period;

	public string TimeText;

	public bool StopTime;

	public bool TimeSkip;

	public AudioSource SchoolBell;

	public ClockScript()
	{
		MinuteNumber = string.Empty;
		HourNumber = string.Empty;
		TimeText = string.Empty;
	}

	public virtual void Start()
	{
		PresentTime = StartHour * 60f;
		if (PlayerPrefs.GetString("Day") == string.Empty)
		{
			PlayerPrefs.SetString("Day", "Monday");
		}
		DayLabel.text = PlayerPrefs.GetString("Day");
		BloomEffect.bloomIntensity = 5f;
		BloomEffect.bloomThreshhold = 0f;
	}

	public virtual void Update()
	{
		if (!(PresentTime >= 1080f))
		{
			BloomEffect.bloomIntensity -= Time.deltaTime * 4.5f;
			BloomEffect.bloomThreshhold += Time.deltaTime / 2f;
			if (!(BloomEffect.bloomThreshhold <= 0.5f))
			{
				BloomEffect.bloomIntensity = 0.5f;
				BloomEffect.bloomThreshhold = 0.5f;
			}
		}
		else
		{
			StopTime = true;
			BloomEffect.bloomIntensity += Time.deltaTime * 4.5f;
			BloomEffect.bloomThreshhold -= Time.deltaTime / 2f;
			if (!(BloomEffect.bloomThreshhold >= 0f))
			{
				Application.LoadLevel(Application.loadedLevel);
			}
		}
		if (!StopTime)
		{
			PresentTime += Time.deltaTime * 0.01666667f * TimeSpeed;
			if (!(PresentTime <= 1440f))
			{
				PresentTime -= 1440f;
			}
			HourTime = PresentTime / 60f;
			Hour = Mathf.Floor(PresentTime / 60f);
			Minute = Mathf.Floor((PresentTime / 60f - Hour) * 60f);
			if (Hour == 0f || Hour == 12f)
			{
				HourNumber = "12";
			}
			else if (!(Hour >= 12f))
			{
				HourNumber = string.Empty + Hour;
			}
			else
			{
				HourNumber = string.Empty + (Hour - 12f);
			}
			if (!(Minute >= 10f))
			{
				MinuteNumber = "0" + Minute;
			}
			else
			{
				MinuteNumber = string.Empty + Minute;
			}
			if (!(Hour >= 12f))
			{
				TimeText = HourNumber + ":" + MinuteNumber + " AM";
			}
			else
			{
				TimeText = HourNumber + ":" + MinuteNumber + " PM";
			}
			TimeLabel.text = TimeText;
			float z = Minute * 6f;
			Vector3 localEulerAngles = MinuteHand.localEulerAngles;
			float num = (localEulerAngles.z = z);
			Vector3 vector2 = (MinuteHand.localEulerAngles = localEulerAngles);
			float z2 = Hour * 30f;
			Vector3 localEulerAngles2 = HourHand.localEulerAngles;
			float num2 = (localEulerAngles2.z = z2);
			Vector3 vector4 = (HourHand.localEulerAngles = localEulerAngles2);
			if (!(HourTime >= 8.5f))
			{
				PeriodLabel.text = "Before School";
				if (Period < 1)
				{
					SchoolBell.Play();
					Period++;
				}
			}
			else if (!(HourTime >= 13f))
			{
				PeriodLabel.text = "Classtime";
				if (Period < 2)
				{
					SchoolBell.Play();
					Period++;
				}
			}
			else if (!(HourTime >= 13.5f))
			{
				PeriodLabel.text = "Lunchtime";
				if (Period < 3)
				{
					SchoolBell.Play();
					Period++;
				}
			}
			else if (!(HourTime >= 15.5f))
			{
				PeriodLabel.text = "Classtime";
				if (Period < 4)
				{
					SchoolBell.Play();
					Period++;
				}
			}
			else
			{
				PeriodLabel.text = "After School";
				if (Period < 5)
				{
					SchoolBell.Play();
					Period++;
				}
			}
		}
		if (!TimeSkip)
		{
			return;
		}
		if (HalfwayTime == 0f)
		{
			HalfwayTime = PresentTime + (TargetTime - PresentTime) * 0.5f;
			Yandere.Hearts.active = true;
			Yandere.Phone.active = true;
			Yandere.TimeSkipping = true;
			Yandere.CanMove = false;
			Blur.enabled = true;
			if (Yandere.Armed)
			{
				Yandere.Unequip();
			}
		}
		if (!(PresentTime >= TargetTime - 10f))
		{
			if (!(Time.timeScale >= 100f))
			{
				Time.timeScale += 1f;
			}
		}
		else if (!(PresentTime >= HalfwayTime))
		{
			Time.timeScale += 1f;
		}
		else
		{
			Time.timeScale = Mathf.Lerp(Time.timeScale, 1f, 0.01666667f);
		}
		Yandere.Character.GetComponent<Animation>()["f02_timeSkip_00"].speed = 1f / Time.timeScale;
		Blur.blurAmount = 0.92f * (Time.timeScale / 100f);
		if (!(PresentTime <= TargetTime))
		{
			EndTimeSkip();
		}
	}

	public virtual void EndTimeSkip()
	{
		Yandere.Hearts.active = false;
		Yandere.Phone.active = false;
		Yandere.TimeSkipping = false;
		Yandere.CanMove = true;
		Blur.enabled = false;
		Time.timeScale = 1f;
		TimeSkip = false;
		HalfwayTime = 0f;
	}

	public virtual void Main()
	{
	}
}
