using System;
using UnityEngine;

[Serializable]
public class PassTimeScript : MonoBehaviour
{
	public ClockScript Clock;

	public UILabel TimeDisplay;

	public Transform Highlight;

	public float[] MinimumDigits;

	public float[] Digits;

	public int TargetTime;

	public int Selected;

	public string AMPM;

	public PassTimeScript()
	{
		Selected = 1;
		AMPM = "AM";
	}

	public virtual void Update()
	{
		if (Input.GetKeyDown("left"))
		{
			Selected--;
			if (Selected < 1)
			{
				Selected = 3;
			}
			UpdateHighlightPosition();
		}
		if (Input.GetKeyDown("right"))
		{
			Selected++;
			if (Selected > 3)
			{
				Selected = 1;
			}
			UpdateHighlightPosition();
		}
		if (Input.GetKeyDown("up"))
		{
			UpdateTime(1);
		}
		if (Input.GetKeyDown("down"))
		{
			UpdateTime(-1);
		}
	}

	public virtual void UpdateHighlightPosition()
	{
		if (Selected == 1)
		{
			int num = -130;
			Vector3 localPosition = Highlight.localPosition;
			float num2 = (localPosition.x = num);
			Vector3 vector2 = (Highlight.localPosition = localPosition);
		}
		else if (Selected == 2)
		{
			int num3 = -40;
			Vector3 localPosition2 = Highlight.localPosition;
			float num4 = (localPosition2.x = num3);
			Vector3 vector4 = (Highlight.localPosition = localPosition2);
		}
		else if (Selected == 3)
		{
			int num5 = 15;
			Vector3 localPosition3 = Highlight.localPosition;
			float num6 = (localPosition3.x = num5);
			Vector3 vector6 = (Highlight.localPosition = localPosition3);
		}
	}

	public virtual void GetCurrentTime()
	{
		Digits[1] = Clock.Hour;
		if (!(Clock.Minute >= 9f))
		{
			Digits[2] = 0f;
			Digits[3] = Clock.Minute;
		}
		else
		{
			Digits[2] = Clock.Minute * 0.1f;
			Digits[2] = Mathf.Floor(Digits[2]);
			Digits[3] = Clock.Minute - Digits[2] * 10f;
		}
		MinimumDigits[1] = Digits[1];
		MinimumDigits[2] = Digits[2];
		MinimumDigits[3] = Digits[3];
		UpdateTime(0);
	}

	public virtual void UpdateTime(int Increment)
	{
		Digits[Selected] += (float)Increment;
		if (Selected == 1)
		{
			if (!(Digits[1] >= MinimumDigits[1]))
			{
				Digits[1] = MinimumDigits[1];
			}
			else if (!(Digits[1] <= 17f))
			{
				Digits[1] = 17f;
			}
			if (Digits[1] == MinimumDigits[1])
			{
				if (!(Digits[2] >= MinimumDigits[2]))
				{
					Digits[2] = MinimumDigits[2];
				}
				if (Digits[2] == MinimumDigits[2] && !(Digits[3] >= MinimumDigits[3]))
				{
					Digits[3] = MinimumDigits[3];
				}
			}
		}
		else if (Selected == 2)
		{
			if (Digits[1] == MinimumDigits[1])
			{
				if (!(Digits[2] >= MinimumDigits[2]))
				{
					Digits[2] = MinimumDigits[2];
				}
				else if (!(Digits[2] <= 5f))
				{
					Digits[2] = MinimumDigits[2];
				}
				if (Digits[2] == MinimumDigits[2] && !(Digits[3] >= MinimumDigits[3]))
				{
					Digits[3] = MinimumDigits[3];
				}
			}
			else if (!(Digits[2] >= 0f))
			{
				Digits[2] = 5f;
			}
			else if (!(Digits[2] <= 5f))
			{
				Digits[2] = 0f;
			}
		}
		else if (Selected == 3)
		{
			if (Digits[1] == MinimumDigits[1] && Digits[2] == MinimumDigits[2])
			{
				if (!(Digits[3] >= MinimumDigits[3]))
				{
					Digits[3] = MinimumDigits[3];
				}
				else if (!(Digits[3] <= 9f))
				{
					Digits[3] = MinimumDigits[3];
				}
			}
			else if (!(Digits[3] >= 0f))
			{
				Digits[3] = 9f;
			}
			else if (!(Digits[3] <= 9f))
			{
				Digits[3] = 0f;
			}
		}
		if (!(Digits[1] >= 12f))
		{
			AMPM = " AM";
		}
		else
		{
			AMPM = " PM";
		}
		if (!(Digits[1] >= 10f))
		{
			TimeDisplay.text = "0" + Digits[1] + ":" + Digits[2] + Digits[3] + AMPM;
		}
		else if (!(Digits[1] >= 13f))
		{
			TimeDisplay.text = Digits[1] + ":" + Digits[2] + Digits[3] + AMPM;
		}
		else
		{
			TimeDisplay.text = "0" + (Digits[1] - 12f) + ":" + Digits[2] + Digits[3] + AMPM;
		}
		TargetTime = (int)(Digits[1] * 60f + Digits[2] * 10f + Digits[3]);
	}

	public virtual void Main()
	{
	}
}
