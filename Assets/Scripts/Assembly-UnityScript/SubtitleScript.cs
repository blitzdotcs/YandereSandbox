using System;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class SubtitleScript : MonoBehaviour
{
	public UILabel Label;

	public string[] KnifeReactions;

	public string[] BloodReactions;

	public string[] WeaponAndBloodReactions;

	public string[] WeaponApologies;

	public string[] BloodApologies;

	public string[] WeaponAndBloodApologies;

	public string[] Greetings;

	public string[] PlayerFarewells;

	public string[] StudentFarewells;

	public string[] Forgivings;

	public string[] Impatiences;

	public string[] ImpatientFarewells;

	public string[] Deaths;

	public float Timer;

	public virtual void Start()
	{
		Label.text = string.Empty;
	}

	public virtual void UpdateLabel(string ReactionType, int ID, float Duration)
	{
		switch (ReactionType)
		{
		case "Weapon Reaction":
			if (ID == 1)
			{
				Label.text = KnifeReactions[UnityEngine.Random.Range(0, Extensions.get_length((System.Array)KnifeReactions))];
			}
			break;
		case "Blood Reaction":
			Label.text = BloodReactions[UnityEngine.Random.Range(0, Extensions.get_length((System.Array)BloodReactions))];
			break;
		case "Weapon and Blood Reaction":
			Label.text = WeaponAndBloodReactions[UnityEngine.Random.Range(0, Extensions.get_length((System.Array)WeaponAndBloodReactions))];
			break;
		case "Greeting":
			Label.text = Greetings[UnityEngine.Random.Range(0, Extensions.get_length((System.Array)Greetings))];
			break;
		case "Player Farewell":
			Label.text = PlayerFarewells[UnityEngine.Random.Range(0, Extensions.get_length((System.Array)PlayerFarewells))];
			break;
		case "Student Farewell":
			Label.text = StudentFarewells[UnityEngine.Random.Range(0, Extensions.get_length((System.Array)StudentFarewells))];
			break;
		case "Weapon Apology":
			Label.text = WeaponApologies[UnityEngine.Random.Range(0, Extensions.get_length((System.Array)WeaponApologies))];
			break;
		case "Blood Apology":
			Label.text = BloodApologies[UnityEngine.Random.Range(0, Extensions.get_length((System.Array)BloodApologies))];
			break;
		case "Weapon and Blood Apology":
			Label.text = WeaponAndBloodApologies[UnityEngine.Random.Range(0, Extensions.get_length((System.Array)WeaponAndBloodApologies))];
			break;
		case "Forgiving":
			Label.text = Forgivings[UnityEngine.Random.Range(0, Extensions.get_length((System.Array)Forgivings))];
			break;
		case "Impatience":
			if (ID == 1)
			{
				Label.text = Impatiences[UnityEngine.Random.Range(0, Extensions.get_length((System.Array)Impatiences))];
			}
			else
			{
				Label.text = ImpatientFarewells[UnityEngine.Random.Range(0, Extensions.get_length((System.Array)ImpatientFarewells))];
			}
			break;
		case "Dying":
			Label.text = Deaths[UnityEngine.Random.Range(0, Extensions.get_length((System.Array)Deaths))];
			break;
		}
		Timer = Duration;
	}

	public virtual void Update()
	{
		if (!(Timer <= 0f))
		{
			Timer -= Time.deltaTime;
			if (!(Timer > 0f))
			{
				Label.text = string.Empty;
				Timer = 0f;
			}
		}
	}

	public virtual void Main()
	{
	}
}
