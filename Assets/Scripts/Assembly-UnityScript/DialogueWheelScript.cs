using System;
using UnityEngine;

[Serializable]
public class DialogueWheelScript : MonoBehaviour
{
	public YandereScript Yandere;

	public UISprite Impatience;

	public UILabel CenterLabel;

	public UISprite[] Segment;

	public UISprite[] Shadow;

	public string[] Text;

	public int Selected;

	public bool Show;

	public virtual void Start()
	{
		transform.localScale = new Vector3(0f, 0f, 0f);
		HideShadows();
	}

	public virtual void Update()
	{
		if (!Show)
		{
			transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(0f, 0f, 0f), Time.deltaTime * 10f);
			return;
		}
		transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1f, 1f, 1f), Time.deltaTime * 10f);
		if (!(Input.GetAxis("Vertical") >= 0.5f) && !(Input.GetAxis("Vertical") <= -0.5f) && !(Input.GetAxis("Horizontal") >= 0.5f) && !(Input.GetAxis("Horizontal") <= -0.5f))
		{
			Selected = 0;
		}
		if (!(Input.GetAxis("Vertical") <= 0.5f) && !(Input.GetAxis("Horizontal") >= 0.5f) && !(Input.GetAxis("Horizontal") <= -0.5f))
		{
			Selected = 1;
		}
		if (!(Input.GetAxis("Vertical") <= 0f) && !(Input.GetAxis("Horizontal") <= 0.5f))
		{
			Selected = 2;
		}
		if (!(Input.GetAxis("Vertical") >= 0f) && !(Input.GetAxis("Horizontal") <= 0.5f))
		{
			Selected = 3;
		}
		if (!(Input.GetAxis("Vertical") >= -0.5f) && !(Input.GetAxis("Horizontal") >= 0.5f) && !(Input.GetAxis("Horizontal") <= -0.5f))
		{
			Selected = 4;
		}
		if (!(Input.GetAxis("Vertical") >= 0f) && !(Input.GetAxis("Horizontal") >= -0.5f))
		{
			Selected = 5;
		}
		if (!(Input.GetAxis("Vertical") <= 0f) && !(Input.GetAxis("Horizontal") >= -0.5f))
		{
			Selected = 6;
		}
		for (int i = 1; i < 7; i++)
		{
			if (Selected == i)
			{
				Segment[i].transform.localScale = Vector3.Lerp(Segment[i].transform.localScale, new Vector3(1.3f, 1.3f, 1f), Time.deltaTime * 10f);
			}
			else
			{
				Segment[i].transform.localScale = Vector3.Lerp(Segment[i].transform.localScale, new Vector3(1f, 1f, 1f), Time.deltaTime * 10f);
			}
		}
		CenterLabel.text = Text[Selected];
		if (Input.GetButtonDown("A"))
		{
			if (Selected == 1 && Shadow[Selected].color.a == 0f)
			{
				Impatience.fillAmount = 0f;
				Yandere.Interaction = 1;
				Yandere.TalkTimer = 3f;
				Show = false;
			}
			if (Selected == 4)
			{
				Impatience.fillAmount = 0f;
				Yandere.Interaction = 4;
				Yandere.TalkTimer = 2f;
				Show = false;
			}
		}
	}

	public virtual void HideShadows()
	{
		Impatience.fillAmount = 0f;
		for (int i = 1; i < 7; i++)
		{
			int num = 0;
			Color color = Shadow[i].color;
			float num2 = (color.a = num);
			Color color3 = (Shadow[i].color = color);
		}
		float a = 0.75f;
		Color color4 = Shadow[2].color;
		float num3 = (color4.a = a);
		Color color6 = (Shadow[2].color = color4);
		float a2 = 0.75f;
		Color color7 = Shadow[3].color;
		float num4 = (color7.a = a2);
		Color color9 = (Shadow[3].color = color7);
		float a3 = 0.75f;
		Color color10 = Shadow[5].color;
		float num5 = (color10.a = a3);
		Color color12 = (Shadow[5].color = color10);
		float a4 = 0.75f;
		Color color13 = Shadow[6].color;
		float num6 = (color13.a = a4);
		Color color15 = (Shadow[6].color = color13);
	}

	public virtual void End()
	{
		Yandere.TargetStudent.Interaction = 0;
		Yandere.TargetStudent.WaitTimer = 1f;
		Yandere.TargetStudent.ShoulderCamera.OverShoulder = false;
		Yandere.TargetStudent.Waiting = true;
		Yandere.TargetStudent = null;
		Yandere.Talking = false;
		Show = false;
	}

	public virtual void Main()
	{
	}
}
