using System;
using HighlightingSystem;
using UnityEngine;

[Serializable]
public class JSHighlighterController : MonoBehaviour
{
	protected Highlighter h;

	public virtual void Awake()
	{
		h = (Highlighter)GetComponent(typeof(Highlighter));
		if (h == null)
		{
			h = (Highlighter)gameObject.AddComponent(typeof(Highlighter));
		}
	}

	public virtual void Start()
	{
		h.FlashingOn(new Color(1f, 1f, 0f, 0f), new Color(1f, 1f, 0f, 1f), 2f);
	}
}
