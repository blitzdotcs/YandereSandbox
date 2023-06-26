using System;
using UnityEngine;

[Serializable]
public class PanelScript : MonoBehaviour
{
	public UILabel BuildingLabel;

	public DoorBoxScript DoorBox;

	public Transform Player;

	public string Floor;

	public float PracticeBuildingZ;

	public float StairsZ;

	public float Floor1Height;

	public float Floor2Height;

	public float Floor3Height;

	public PanelScript()
	{
		Floor = string.Empty;
	}

	public virtual void Update()
	{
		if (Player.position.z > StairsZ || !(Player.position.z >= StairsZ * -1f))
		{
			Floor = "Stairs";
		}
		else if (!(Player.position.y >= Floor1Height))
		{
			Floor = "First Floor";
		}
		else if (!(Player.position.y <= Floor1Height) && !(Player.position.y >= Floor2Height))
		{
			Floor = "Second Floor";
		}
		else if (!(Player.position.y <= Floor2Height) && !(Player.position.y >= Floor3Height))
		{
			Floor = "Third Floor";
		}
		else
		{
			Floor = "Rooftop";
		}
		if (!(Player.position.z >= PracticeBuildingZ))
		{
			BuildingLabel.text = "Practice Building, " + Floor;
		}
		else
		{
			BuildingLabel.text = "Classroom Building, " + Floor;
		}
		DoorBox.Show = false;
	}

	public virtual void Main()
	{
	}
}
