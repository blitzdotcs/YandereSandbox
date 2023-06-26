using System;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class StudentManagerScript : MonoBehaviour
{
	private GameObject NewStudent;

	public StudentScript[] Students;

	public YandereScript Yandere;

	public ClockScript Clock;

	public JsonScript JSON;

	public ListScript Lockers;

	public ListScript LockerFs;

	public ListScript Classrooms;

	public ListScript ClassroomFs;

	public ListScript Hangouts;

	public ListScript HangoutFs;

	public Transform[] SpawnPositions;

	public GameObject StudentChan;

	public float[] SpawnTimes;

	public int StudentsSpawned;

	public int StudentsTotal;

	public int ID;

	public Texture[] Colors;

	public bool AoT;

	public StudentManagerScript()
	{
		StudentsTotal = 5;
	}

	public virtual void Update()
	{
		if (StudentsSpawned < StudentsTotal && !(Clock.PresentTime / 60f < SpawnTimes[StudentsSpawned]))
		{
			NewStudent = (GameObject)UnityEngine.Object.Instantiate(StudentChan, SpawnPositions[StudentsSpawned].position, Quaternion.identity);
			Students[StudentsSpawned] = (StudentScript)NewStudent.GetComponent(typeof(StudentScript));
			Students[StudentsSpawned].StudentID = StudentsSpawned + 1;
			Students[StudentsSpawned].StudentManager = this;
			Students[StudentsSpawned].JSON = JSON;
			if (AoT)
			{
				Students[StudentsSpawned].AttackOnTitan();
			}
			StudentsSpawned++;
		}
	}

	public virtual void UpdateStudents()
	{
		for (ID = 0; ID < Extensions.get_length((System.Array)Students); ID++)
		{
			if (Students[ID] != null)
			{
				if (Yandere.Armed)
				{
					Students[ID].Prompt.HideButton[0] = true;
					Students[ID].Prompt.HideButton[2] = false;
				}
				else
				{
					Students[ID].Prompt.HideButton[0] = false;
					Students[ID].Prompt.HideButton[2] = true;
				}
			}
		}
	}

	public virtual void EnablePrompts()
	{
		for (ID = 0; ID < Extensions.get_length((System.Array)Students); ID++)
		{
			if (Students[ID] != null)
			{
				Students[ID].Prompt.enabled = true;
			}
		}
	}

	public virtual void DisablePrompts()
	{
		for (ID = 0; ID < Extensions.get_length((System.Array)Students); ID++)
		{
			if (Students[ID] != null)
			{
				Students[ID].Prompt.Hide();
				Students[ID].Prompt.enabled = false;
			}
		}
	}

	public virtual void AttackOnTitan()
	{
		AoT = true;
		for (ID = 0; ID < Extensions.get_length((System.Array)Students); ID++)
		{
			if (Students[ID] != null)
			{
				Students[ID].AttackOnTitan();
			}
		}
	}

	public virtual void Main()
	{
	}
}
