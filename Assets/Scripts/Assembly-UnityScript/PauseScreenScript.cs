using System;
using UnityEngine;

[Serializable]
public class PauseScreenScript : MonoBehaviour
{
	public PassTimeScript PassTime;

	public YandereScript Yandere;

	public RPG_Camera RPGCamera;

	public ClockScript Clock;

	public Blur ScreenBlur;

	public UILabel PassTimeLabel;

	public GameObject PromptParent;

	public GameObject MainMenu;

	public Transform Highlight;

	public int Selected;

	public bool CorrectingTime;

	public bool PressedA;

	public bool Show;

	public PauseScreenScript()
	{
		Selected = 1;
	}

	public virtual void Start()
	{
		transform.localPosition = new Vector3(1250f, 0f, 0f);
		transform.localScale = new Vector3(0.585f, 0.585f, 0.585f);
	}

	public virtual void Update()
	{
		if (!Show)
		{
			transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(1250f, 0f, 0f), 1f / 6f);
			transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(0.585f, 0.585f, 0.585f), 1f / 6f);
			if (CorrectingTime && !(Time.timeScale >= 0.9f))
			{
				Time.timeScale = Mathf.Lerp(Time.timeScale, 1f, 1f / 6f);
				if (!(Time.timeScale <= 0.9f))
				{
					CorrectingTime = false;
					Time.timeScale = 1f;
				}
			}
			if (Input.GetButtonDown("Start") && !Yandere.TimeSkipping)
			{
				PromptParent.active = false;
				ScreenBlur.enabled = true;
				RPGCamera.enabled = false;
				Show = true;
				if (!Yandere.CanMove || Yandere.Dragging)
				{
					float a = 0.5f;
					Color color = PassTimeLabel.color;
					float num = (color.a = a);
					Color color3 = (PassTimeLabel.color = color);
				}
				else
				{
					int num2 = 1;
					Color color4 = PassTimeLabel.color;
					float num3 = (color4.a = num2);
					Color color6 = (PassTimeLabel.color = color4);
				}
			}
			return;
		}
		transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(0f, 0f, 0f), 1f / 6f);
		transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1f, 1f, 1f), 1f / 6f);
		Time.timeScale = Mathf.Lerp(Time.timeScale, 0f, 1f / 6f);
		if (MainMenu.active)
		{
			if (Input.GetKeyDown("up"))
			{
				float y = Highlight.localPosition.y + 75f;
				Vector3 localPosition = Highlight.localPosition;
				float num4 = (localPosition.y = y);
				Vector3 vector2 = (Highlight.localPosition = localPosition);
				Selected--;
				if (Selected < 1)
				{
					int num5 = -225;
					Vector3 localPosition2 = Highlight.localPosition;
					float num6 = (localPosition2.y = num5);
					Vector3 vector4 = (Highlight.localPosition = localPosition2);
					Selected = 7;
				}
			}
			if (Input.GetKeyDown("down"))
			{
				float y2 = Highlight.localPosition.y - 75f;
				Vector3 localPosition3 = Highlight.localPosition;
				float num7 = (localPosition3.y = y2);
				Vector3 vector6 = (Highlight.localPosition = localPosition3);
				Selected++;
				if (Selected > 7)
				{
					int num8 = 225;
					Vector3 localPosition4 = Highlight.localPosition;
					float num9 = (localPosition4.y = num8);
					Vector3 vector8 = (Highlight.localPosition = localPosition4);
					Selected = 1;
				}
			}
			if (Input.GetButtonDown("A"))
			{
				PressedA = true;
				if (Selected == 1)
				{
					ScreenBlur.enabled = false;
					RPGCamera.enabled = true;
					CorrectingTime = true;
					Show = false;
				}
				else if (Selected == 2)
				{
					if (Yandere.CanMove && !Yandere.Dragging)
					{
						MainMenu.active = false;
					//	PassTime.active = true;
						PassTime.GetCurrentTime();
					}
				}
				else if (Selected == 7)
				{
					Application.Quit();
				}
			}
			if (Input.GetButtonDown("Start"))
			{
				PromptParent.active = true;
				ScreenBlur.enabled = false;
				RPGCamera.enabled = true;
				CorrectingTime = true;
				Show = false;
			}
		}
		if (!PressedA)
		{
			if (Input.GetButtonDown("A"))
			{
				PromptParent.active = true;
				ScreenBlur.enabled = false;
				RPGCamera.enabled = true;
			//	PassTime.active = false;
				MainMenu.active = true;
				Show = false;
				Clock.TargetTime = PassTime.TargetTime;
				Clock.TimeSkip = true;
				Time.timeScale = 1f;
			}
			if (Input.GetButtonDown("B"))
			{
				MainMenu.active = true;
			//	PassTime.active = false;
			}
		}
		if (Input.GetButtonUp("A"))
		{
			PressedA = false;
		}
	}

	public virtual void Main()
	{
	}
}
