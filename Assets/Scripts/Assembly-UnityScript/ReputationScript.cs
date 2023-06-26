using System;
using UnityEngine;

[Serializable]
public class ReputationScript : MonoBehaviour
{
	public Transform CurrentRepMarker;

	public Transform PendingRepMarker;

	public UILabel PendingRepLabel;

	public ClockScript Clock;

	public float Reputation;

	public float PendingRep;

	public int Phase;

	public virtual void Update()
	{
		if (Phase == 1)
		{
			if (!(Clock.PresentTime / 60f <= 8.5f))
			{
				Reputation += PendingRep;
				PendingRep = 0f;
				Phase++;
			}
		}
		else if (Phase == 2)
		{
			if (!(Clock.PresentTime / 60f <= 13.5f))
			{
				Reputation += PendingRep;
				PendingRep = 0f;
				Phase++;
			}
		}
		else if (Phase == 3 && !(Clock.PresentTime / 60f <= 18f))
		{
			Reputation += PendingRep;
			PendingRep = 0f;
			Phase++;
		}
		if (!(Reputation <= 100f))
		{
			Reputation = 100f;
		}
		if (!(Reputation >= -100f))
		{
			Reputation = -100f;
		}
		float x = Mathf.Lerp(CurrentRepMarker.localPosition.x, -850f + Reputation * 1.5f, Time.deltaTime * 10f);
		Vector3 localPosition = CurrentRepMarker.localPosition;
		float num = (localPosition.x = x);
		Vector3 vector2 = (CurrentRepMarker.localPosition = localPosition);
		if (!(Reputation + PendingRep <= 100f))
		{
			PendingRep = 100f - Reputation;
		}
		if (!(Reputation + PendingRep >= -100f))
		{
			PendingRep = -100f - Reputation;
		}
		float x2 = Mathf.Lerp(PendingRepMarker.localPosition.x, CurrentRepMarker.transform.localPosition.x + PendingRep * 1.5f, Time.deltaTime * 10f);
		Vector3 localPosition2 = PendingRepMarker.localPosition;
		float num2 = (localPosition2.x = x2);
		Vector3 vector4 = (PendingRepMarker.localPosition = localPosition2);
		if (!(PendingRep <= 0f))
		{
			PendingRepLabel.text = "+" + PendingRep;
		}
		else if (!(PendingRep >= 0f))
		{
			PendingRepLabel.text = string.Empty + PendingRep;
		}
		else
		{
			PendingRepLabel.text = string.Empty;
		}
	}

	public virtual void Main()
	{
	}
}
