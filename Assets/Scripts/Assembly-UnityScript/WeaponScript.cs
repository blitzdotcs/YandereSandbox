using System;
using UnityEngine;

[Serializable]
public class WeaponScript : MonoBehaviour
{
	public OutlineScript Outline;

	public YandereScript Yandere;

	public PromptScript Prompt;

	public Collider MyCollider;

	public virtual void Start()
	{
		Yandere = (YandereScript)GameObject.Find("YandereChan").GetComponent(typeof(YandereScript));
	}

	public virtual void LateUpdate()
	{
		if (!(Prompt.Circle[3].fillAmount > 0f))
		{
			Outline.color = new Color(0f, 0f, 0f, 1f);
			transform.parent = Yandere.ItemParent;
			transform.localPosition = new Vector3(0f, 0f, 0f);
			transform.localEulerAngles = new Vector3(0f, 0f, 0f);
			MyCollider.enabled = false;
			GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
			Yandere.Weapon = this;
			Yandere.Armed = true;
			Yandere.StudentManager.UpdateStudents();
			Prompt.Hide();
			Prompt.enabled = false;
			Yandere.NearestPrompt = null;
			Yandere.NotificationManager.DisplayNotification("Armed");
		}
		if (Yandere.Armed)
		{
			transform.localPosition = new Vector3(0f, 0f, 0f);
			transform.localEulerAngles = new Vector3(0f, 0f, 0f);
		}
	}

	public virtual void Drop()
	{
		Outline.color = new Color(0f, 1f, 1f, 1f);
		transform.parent = null;
		MyCollider.enabled = true;
		GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
		Yandere.Weapon = null;
		Yandere.Armed = false;
		Yandere.StudentManager.UpdateStudents();
		Prompt.enabled = true;
	}

	public virtual void Main()
	{
	}
}
