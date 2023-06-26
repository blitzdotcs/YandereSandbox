using System;
using UnityEngine;

[Serializable]
public class DoorScript : MonoBehaviour
{
	public DoorBoxScript DoorBox;

	public Transform Player;

	public string Name;

	public virtual void Start()
	{
		DoorBox = (DoorBoxScript)GameObject.Find("DoorBox").GetComponent(typeof(DoorBoxScript));
		Player = GameObject.Find("SimpleYandereChan").transform;
	}

	public virtual void Update()
	{
		if (!(Vector3.Distance(transform.position, Player.position) >= 1f))
		{
			DoorBox.Label.text = Name;
			DoorBox.Show = true;
		}
	}

	public virtual void Main()
	{
	}
}
