using HighlightingSystem;
using UnityEngine;

public class Test : MonoBehaviour
{
	private bool e;

	private HighlightingRenderer hr;

	private HighlightingBlitter hb;

	private void Start()
	{
		Camera main = Camera.main;
		hr = main.GetComponent<HighlightingRenderer>();
		hb = main.GetComponent<HighlightingBlitter>();
		InvokeRepeating("Toggle", 0f, 1f);
	}

	private void Toggle()
	{
		e = !e;
		HighlightingRenderer highlightingRenderer = hr;
		bool flag = e;
		hb.enabled = flag;
		highlightingRenderer.enabled = flag;
	}
}
