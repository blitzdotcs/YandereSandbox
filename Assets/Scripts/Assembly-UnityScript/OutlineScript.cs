using System;
using HighlightingSystem;
using UnityEngine;

[Serializable]
public class OutlineScript : MonoBehaviour
{
	public Highlighter h;

	public Color color;

	public OutlineScript()
	{
		color = new Color(1f, 1f, 1f, 1f);
	}

	public virtual void Awake()
	{
		h = (Highlighter)GetComponent(typeof(Highlighter));
		if (h == null)
		{
			h = (Highlighter)gameObject.AddComponent(typeof(Highlighter));
		}
	}

	public virtual void Update()
	{
		h.ConstantOnImmediate(color);
	}
}
