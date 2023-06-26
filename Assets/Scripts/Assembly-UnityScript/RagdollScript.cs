using System;
using UnityEngine;

[Serializable]
public class RagdollScript : MonoBehaviour
{
	public YandereScript Yandere;

	public PromptScript Prompt;

	public Rigidbody[] Rigidbodies;

	public Transform BloodParent;

	public Transform NearestLimb;

	public Transform[] Limb;

	public Vector3[] LimbAnchor;

	public GameObject Character;

	public bool AddingToCount;

	public bool Dragged;

	public float AnimStartTime;

	public int LimbID;

	public int Frame;

	public virtual void Start()
	{
		Physics.IgnoreLayerCollision(9, 11, true);
		if (Yandere == null)
		{
			Yandere = (YandereScript)GameObject.Find("YandereChan").GetComponent(typeof(YandereScript));
		}
		Character.GetComponent<Animation>().Play("f02_down_22");
		Character.GetComponent<Animation>()["f02_down_22"].time = AnimStartTime;
		Character.GetComponent<Animation>()["f02_down_22"].speed = 0f;
	}

	public virtual void Update()
	{
		Character.GetComponent<Animation>().Stop();
		if (!Input.GetButtonDown("LB"))
		{
			if (!(Prompt.Circle[1].fillAmount > 0f))
			{
				if (!Dragged)
				{
					Prompt.AcceptingInput[1] = false;
					Prompt.Circle[1].fillAmount = 1f;
					Prompt.Label[1].text = "     " + "Drop";
					PickNearestLimb();
					Yandere.RagdollDragger.connectedBody = Rigidbodies[LimbID];
					Yandere.RagdollDragger.connectedAnchor = LimbAnchor[LimbID];
					Yandere.Dragging = true;
					Yandere.Ragdoll = gameObject;
					if (Yandere.Armed)
					{
						Yandere.Unequip();
					}
					Dragged = true;
				}
				else
				{
					StopDragging();
				}
			}
		}
		else if (Dragged)
		{
			StopDragging();
		}
		if (!(Vector3.Distance(Yandere.transform.position, transform.position) >= 2f))
		{
			if (!AddingToCount)
			{
				Yandere.NearBodies++;
				AddingToCount = true;
			}
		}
		else if (AddingToCount)
		{
			Yandere.NearBodies--;
			AddingToCount = false;
		}
		if (!Prompt.AcceptingInput[1] && Input.GetButtonUp("B"))
		{
			Prompt.AcceptingInput[1] = true;
		}
	}

	public virtual void StopDragging()
	{
		Prompt.AcceptingInput[1] = false;
		Prompt.Circle[1].fillAmount = 1f;
		Prompt.Label[1].text = "     " + "Drag";
		Yandere.RagdollDragger.connectedBody = null;
		Yandere.Dragging = false;
		Yandere.Ragdoll = null;
		Dragged = false;
	}

	public virtual void PickNearestLimb()
	{
		NearestLimb = Limb[0];
		LimbID = 0;
		for (int i = 1; i < 4; i++)
		{
			if (!(Vector3.Distance(Limb[i].position, Yandere.transform.position) >= Vector3.Distance(NearestLimb.position, Yandere.transform.position)))
			{
				NearestLimb = Limb[i];
				LimbID = i;
			}
		}
	}

	public virtual void Main()
	{
	}
}
