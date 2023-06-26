using System;
using System.Collections.Generic;
using System.IO;
using Boo.Lang.Runtime;
using JsonFx.Json;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class JsonScript : MonoBehaviour
{
	public string StudentFileName;

	public string[] StudentNames;

	public int[] StudentClasses;

	public string[] StudentClubs;

	public string[] StudentPersonalites;

	public int[] StudentCrushes;

	public string[] StudentHairstyles;

	public string[] StudentColors;

	public string[] StudentStockings;

	public string[] StudentPanties;

	public UnityScript.Lang.Array[] StudentTimes;

	public UnityScript.Lang.Array[] StudentDestinations;

	public int TotalStudents;

	private string[] TempStringArray;

	private float[] TempFloatArray;

	private int[] TempIntArray;

	private string TempString;

	private float TempFloat;

	private int TempInt;

	private int ID;

	public virtual Dictionary<string, object>[] StudentData()
	{
		string text = null;
		string path = Path.Combine("JSON", StudentFileName + ".json");
		path = Path.Combine(Application.streamingAssetsPath, path);
		text = File.ReadAllText(path);
		return JsonReader.Deserialize(text) as Dictionary<string, object>[];
	}

	public virtual void Start()
	{
		StudentTimes = new UnityScript.Lang.Array[TotalStudents + 1];
		StudentDestinations = new UnityScript.Lang.Array[TotalStudents + 1];
		UnityEngine.Object.DontDestroyOnLoad(transform.gameObject);
		int i = 0;
		Dictionary<string, object>[] array = StudentData();
		for (int length = array.Length; i < length; i++)
		{
			ID = TFUtils.LoadInt(array[i], "ID");
			if (RuntimeServices.EqualityOperator(ID, null) || ID == 0)
			{
				break;
			}
			StudentNames[ID] = TFUtils.LoadString(array[i], "Name");
			StudentClasses[ID] = TFUtils.LoadInt(array[i], "Class");
			StudentClubs[ID] = TFUtils.LoadString(array[i], "Club");
			StudentPersonalites[ID] = TFUtils.LoadString(array[i], "Personality");
			StudentCrushes[ID] = TFUtils.LoadInt(array[i], "Crush");
			StudentHairstyles[ID] = TFUtils.LoadString(array[i], "Hairstyle");
			StudentColors[ID] = TFUtils.LoadString(array[i], "Color");
			StudentStockings[ID] = TFUtils.LoadString(array[i], "Stockings");
			StudentPanties[ID] = TFUtils.LoadString(array[i], "Panties");
			TempString = TFUtils.LoadString(array[i], "ScheduleTime");
			ConstructTempFloatArray();
			StudentTimes[ID] = TempFloatArray;
			TempString = TFUtils.LoadString(array[i], "ScheduleDestination");
			ConstructTempStringArray();
			StudentDestinations[ID] = TempStringArray;
		}
	}

	public virtual void ConstructTempFloatArray()
	{
		TempStringArray = TempString.Split("_"[0]);
		TempFloatArray = new float[TempStringArray.Length];
		for (int i = 0; i < TempStringArray.Length; i++)
		{
			TempFloatArray[i] = float.Parse(TempStringArray[i]);
		}
	}

	public virtual void ConstructTempStringArray()
	{
		TempStringArray = TempString.Split("_"[0]);
	}

	public virtual void Main()
	{
	}
}
