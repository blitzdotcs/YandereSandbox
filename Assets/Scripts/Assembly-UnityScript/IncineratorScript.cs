using System;
using UnityEngine;

[Serializable]
public class IncineratorScript : MonoBehaviour
{
	public YandereScript Yandere;

	public PromptScript Prompt;

	public Transform RightDoor;

	public Transform LeftDoor;

	public GameObject Smoke;

	public bool Occupied;

	public bool Open;

	public virtual void Update()
	{
		if (!Open)
		{
			float y = Mathf.Lerp(RightDoor.transform.localEulerAngles.y, 0f, Time.deltaTime * 10f);
			Vector3 localEulerAngles = RightDoor.transform.localEulerAngles;
			float num = (localEulerAngles.y = y);
			Vector3 vector2 = (RightDoor.transform.localEulerAngles = localEulerAngles);
			float y2 = Mathf.Lerp(LeftDoor.transform.localEulerAngles.y, 0f, Time.deltaTime * 10f);
			Vector3 localEulerAngles2 = LeftDoor.transform.localEulerAngles;
			float num2 = (localEulerAngles2.y = y2);
			Vector3 vector4 = (LeftDoor.transform.localEulerAngles = localEulerAngles2);
		}
		else
		{
			float y3 = Mathf.Lerp(RightDoor.transform.localEulerAngles.y, 135f, Time.deltaTime * 10f);
			Vector3 localEulerAngles3 = RightDoor.transform.localEulerAngles;
			float num3 = (localEulerAngles3.y = y3);
			Vector3 vector6 = (RightDoor.transform.localEulerAngles = localEulerAngles3);
			float y4 = Mathf.Lerp(LeftDoor.transform.localEulerAngles.y, 135f, Time.deltaTime * 10f);
			Vector3 localEulerAngles4 = LeftDoor.transform.localEulerAngles;
			float num4 = (localEulerAngles4.y = y4);
			Vector3 vector8 = (LeftDoor.transform.localEulerAngles = localEulerAngles4);
		}
		if (Yandere.Ragdoll == null)
		{
			if (!Occupied)
			{
				if (Prompt.enabled)
				{
					Prompt.Hide();
					Prompt.enabled = false;
				}
				return;
			}
			Prompt.enabled = true;
			if (!(Prompt.Circle[3].fillAmount > 0f))
			{
				Prompt.Label[3].text = "     " + "Dump";
				Prompt.Hide();
				Prompt.enabled = false;
				Smoke.active = true;
				Occupied = false;
			}
		}
		else if (!Occupied)
		{
			Prompt.enabled = true;
			if (!(Prompt.Circle[3].fillAmount > 0f) && !Occupied)
			{
				Yandere.CanMove = false;
				Yandere.Dumping = true;
				Prompt.Hide();
				Prompt.enabled = false;
				Open = true;
			}
		}
	}

	public virtual void Main()
	{
	}
}
