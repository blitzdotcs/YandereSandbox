using System;
using UnityEngine;

[Serializable]
public class PromptScript : MonoBehaviour
{
	public YandereScript Yandere;

	public GameObject[] ButtonObject;

	public GameObject CircleObject;

	public GameObject LabelObject;

	public Transform PromptParent;

	public Collider MyCollider;

	public Camera UICamera;

	public bool[] AcceptingInput;

	public bool[] ButtonActive;

	public bool[] HideButton;

	public UISprite[] Button;

	public UISprite[] Circle;

	public UILabel[] Label;

	public float[] OffsetX;

	public float[] OffsetY;

	public string[] Text;

	public bool InView;

	public float MinimumDistance;

	public float Distance;

	public int ButtonHeld;

	public int Priority;

	public int ID;

	public virtual void Start()
	{
		PromptParent = (Transform)GameObject.Find("PromptParent").GetComponent(typeof(Transform));
		Yandere = (YandereScript)GameObject.Find("YandereChan").GetComponent(typeof(YandereScript));
		UICamera = (Camera)GameObject.Find("UI Camera").GetComponent(typeof(Camera));
		if (MyCollider != null)
		{
			Physics.IgnoreCollision(Yandere.GetComponent<Collider>(), MyCollider);
		}
		while (ID < 4)
		{
			if (ButtonActive[ID])
			{
				Button[ID] = (UISprite)((GameObject)UnityEngine.Object.Instantiate(ButtonObject[ID], transform.position, Quaternion.identity)).GetComponent(typeof(UISprite));
				Button[ID].transform.parent = PromptParent;
				Button[ID].transform.localScale = new Vector3(1f, 1f, 1f);
				Button[ID].transform.localEulerAngles = new Vector3(0f, 0f, 0f);
				int num = 0;
				Color color = Button[ID].color;
				float num2 = (color.a = num);
				Color color3 = (Button[ID].color = color);
				Circle[ID] = (UISprite)((GameObject)UnityEngine.Object.Instantiate(CircleObject, transform.position, Quaternion.identity)).GetComponent(typeof(UISprite));
				Circle[ID].transform.parent = PromptParent;
				Circle[ID].transform.localScale = new Vector3(1f, 1f, 1f);
				Circle[ID].transform.localEulerAngles = new Vector3(0f, 0f, 0f);
				int num3 = 0;
				Color color4 = Circle[ID].color;
				float num4 = (color4.a = num3);
				Color color6 = (Circle[ID].color = color4);
				Label[ID] = (UILabel)((GameObject)UnityEngine.Object.Instantiate(LabelObject, transform.position, Quaternion.identity)).GetComponent(typeof(UILabel));
				Label[ID].transform.parent = PromptParent;
				Label[ID].transform.localScale = new Vector3(1f, 1f, 1f);
				Label[ID].transform.localEulerAngles = new Vector3(0f, 0f, 0f);
				int num5 = 0;
				Color color7 = Label[ID].color;
				float num6 = (color7.a = num5);
				Color color9 = (Label[ID].color = color7);
				Label[ID].text = "     " + Text[ID];
			}
			AcceptingInput[ID] = true;
			ID++;
		}
	}

	public virtual void Update()
	{
		if (!InView)
		{
			return;
		}
		Distance = Vector3.Distance(Yandere.transform.position, new Vector3(transform.position.x, Yandere.transform.position.y, transform.position.z));
		if (!(Distance >= 5f))
		{
			for (ID = 0; ID < 4; ID++)
			{
				if (ButtonActive[ID])
				{
					Vector2 vector = UICamera.WorldToScreenPoint(transform.position + Vector3.right * OffsetX[ID] + Vector3.up * OffsetY[ID]);
					Button[ID].transform.position = UICamera.ScreenToWorldPoint(new Vector3(vector.x, vector.y, 1f));
					Circle[ID].transform.position = UICamera.ScreenToWorldPoint(new Vector3(vector.x, vector.y, 1f));
					Vector2 vector2 = UICamera.WorldToScreenPoint(transform.position + Vector3.right * OffsetX[ID] + Vector3.up * OffsetY[ID]);
					Label[ID].transform.position = UICamera.ScreenToWorldPoint(new Vector3(vector2.x + OffsetX[ID], vector2.y, 1f));
					if (!HideButton[ID])
					{
						Button[ID].color = new Color(0.5f, 0.5f, 0.5f, 1f);
						int num = 0;
						Color color = Circle[ID].color;
						float num2 = (color.a = num);
						Color color3 = (Circle[ID].color = color);
						float a = 0.5f;
						Color color4 = Label[ID].color;
						float num3 = (color4.a = a);
						Color color6 = (Label[ID].color = color4);
					}
				}
			}
			if (!(Distance >= MinimumDistance))
			{
				if (Yandere.NearestPrompt == null)
				{
					Yandere.NearestPrompt = this;
				}
				else if (Priority > Yandere.NearestPrompt.Priority)
				{
					Yandere.NearestPrompt = this;
				}
				if (Yandere.NearestPrompt == this)
				{
					for (ID = 0; ID < 4; ID++)
					{
						if (ButtonActive[ID])
						{
							Button[ID].color = new Color(1f, 1f, 1f, 1f);
							Circle[ID].color = new Color(0.5f, 0.5f, 0.5f, 1f);
							int num4 = 1;
							Color color7 = Label[ID].color;
							float num5 = (color7.a = num4);
							Color color9 = (Label[ID].color = color7);
						}
					}
					if (Input.GetButton("A"))
					{
						ButtonHeld = 1;
					}
					else if (Input.GetButton("B"))
					{
						ButtonHeld = 2;
					}
					else if (Input.GetButton("X"))
					{
						ButtonHeld = 3;
					}
					else if (Input.GetButton("Y"))
					{
						ButtonHeld = 4;
					}
					else
					{
						ButtonHeld = 0;
					}
					if (ButtonHeld > 0)
					{
						for (ID = 0; ID < 4; ID++)
						{
							if (ButtonActive[ID] && ID != ButtonHeld - 1)
							{
								Circle[ID].fillAmount = 1f;
							}
						}
						if (ButtonActive[ButtonHeld - 1] && !HideButton[ButtonHeld - 1] && AcceptingInput[ButtonHeld - 1] && !Yandere.Attacking)
						{
							Circle[ButtonHeld - 1].color = new Color(1f, 1f, 1f, 1f);
							Circle[ButtonHeld - 1].fillAmount = Circle[ButtonHeld - 1].fillAmount - Time.deltaTime * 2f;
							ID = 0;
						}
					}
					else
					{
						for (ID = 0; ID < 4; ID++)
						{
							if (ButtonActive[ID])
							{
								Circle[ID].fillAmount = 1f;
							}
						}
					}
				}
			}
			else
			{
				if (Yandere.NearestPrompt == gameObject)
				{
					Yandere.NearestPrompt = null;
				}
				for (ID = 0; ID < 4; ID++)
				{
					if (ButtonActive[ID])
					{
						Circle[ID].fillAmount = 1f;
					}
				}
			}
			for (ID = 0; ID < 4; ID++)
			{
				if (ButtonActive[ID] && HideButton[ID])
				{
					int num6 = 0;
					Color color10 = Button[ID].color;
					float num7 = (color10.a = num6);
					Color color12 = (Button[ID].color = color10);
					int num8 = 0;
					Color color13 = Circle[ID].color;
					float num9 = (color13.a = num8);
					Color color15 = (Circle[ID].color = color13);
					int num10 = 0;
					Color color16 = Label[ID].color;
					float num11 = (color16.a = num10);
					Color color18 = (Label[ID].color = color16);
				}
			}
		}
		else
		{
			Hide();
		}
	}

	public virtual void OnBecameVisible()
	{
		InView = true;
	}

	public virtual void OnBecameInvisible()
	{
		InView = false;
		Hide();
	}

	public virtual void Hide()
	{
		if (Yandere.NearestPrompt == this)
		{
			Yandere.NearestPrompt = null;
		}
		for (ID = 0; ID < 4; ID++)
		{
			if (ButtonActive[ID])
			{
				Circle[ID].fillAmount = 1f;
				int num = 0;
				Color color = Button[ID].color;
				float num2 = (color.a = num);
				Color color3 = (Button[ID].color = color);
				int num3 = 0;
				Color color4 = Circle[ID].color;
				float num4 = (color4.a = num3);
				Color color6 = (Circle[ID].color = color4);
				int num5 = 0;
				Color color7 = Label[ID].color;
				float num6 = (color7.a = num5);
				Color color9 = (Label[ID].color = color7);
			}
		}
	}

	public virtual void Main()
	{
	}
}
