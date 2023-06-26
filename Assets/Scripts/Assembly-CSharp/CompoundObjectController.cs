using System.Collections.Generic;
using UnityEngine;

public class CompoundObjectController : FlashingController
{
	private Transform tr;

	private List<GameObject> objects;

	private int currentShaderID;

	private string[] shaderNames = new string[4] { "Diffuse", "Specular", "VertexLit", "Bumped Specular" };

	private int ox = -220;

	private int oy = 20;

	private new void Start()
	{
		base.Start();
		tr = GetComponent<Transform>();
		objects = new List<GameObject>();
		StartCoroutine(DelayFlashing());
	}

	private void OnGUI()
	{
		float left = Screen.width + ox;
		GUI.Label(new Rect(left, oy, 500f, 100f), "Compound object controls:");
		if (GUI.Button(new Rect(left, oy + 30, 200f, 30f), "Add Random Primitive"))
		{
			AddObject();
		}
		if (GUI.Button(new Rect(left, oy + 70, 200f, 30f), "Change Material"))
		{
			ChangeMaterial();
		}
		if (GUI.Button(new Rect(left, oy + 110, 200f, 30f), "Change Shader"))
		{
			ChangeShader();
		}
		if (GUI.Button(new Rect(left, oy + 150, 200f, 30f), "Remove Object"))
		{
			RemoveObject();
		}
	}

	private void AddObject()
	{
		PrimitiveType type = (PrimitiveType)Random.Range(0, 4);
		GameObject gameObject = GameObject.CreatePrimitive(type);
		Transform component = gameObject.GetComponent<Transform>();
		component.parent = tr;
		component.localPosition = Random.insideUnitSphere * 2f;
		objects.Add(gameObject);
		h.ReinitMaterials();
	}

	private void ChangeMaterial()
	{
		if (objects.Count < 1)
		{
			AddObject();
		}
		currentShaderID++;
		if (currentShaderID >= shaderNames.Length)
		{
			currentShaderID = 0;
		}
		foreach (GameObject @object in objects)
		{
			Renderer component = @object.GetComponent<Renderer>();
			Shader shader = Shader.Find(shaderNames[currentShaderID]);
			component.material = new Material(shader);
		}
		h.ReinitMaterials();
	}

	private void ChangeShader()
	{
		if (objects.Count < 1)
		{
			AddObject();
		}
		currentShaderID++;
		if (currentShaderID >= shaderNames.Length)
		{
			currentShaderID = 0;
		}
		foreach (GameObject @object in objects)
		{
			Renderer component = @object.GetComponent<Renderer>();
			Shader shader = Shader.Find(shaderNames[currentShaderID]);
			component.material.shader = shader;
		}
		h.ReinitMaterials();
	}

	private void RemoveObject()
	{
		if (objects.Count >= 1)
		{
			GameObject gameObject = objects[objects.Count - 1];
			objects.Remove(gameObject);
			Object.Destroy(gameObject);
			h.ReinitMaterials();
		}
	}
}
