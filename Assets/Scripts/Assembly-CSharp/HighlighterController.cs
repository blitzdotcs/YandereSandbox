using HighlightingSystem;
using UnityEngine;

public class HighlighterController : MonoBehaviour
{
	public bool seeThrough = true;

	protected bool _seeThrough = true;

	protected Highlighter h;

	protected void Awake()
	{
		h = GetComponent<Highlighter>();
		if (h == null)
		{
			h = base.gameObject.AddComponent<Highlighter>();
		}
	}

	private void OnEnable()
	{
		if (seeThrough)
		{
			h.SeeThroughOn();
		}
		else
		{
			h.SeeThroughOff();
		}
	}

	protected void Start()
	{
	}

	protected void Update()
	{
		if (_seeThrough != seeThrough)
		{
			_seeThrough = seeThrough;
			if (_seeThrough)
			{
				h.SeeThroughOn();
			}
			else
			{
				h.SeeThroughOff();
			}
		}
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			h.ConstantSwitch();
		}
		else if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			h.ConstantSwitchImmediate();
		}
		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			h.Off();
		}
	}

	public void MouseOver()
	{
		h.On(Color.red);
	}

	public void Fire1()
	{
		h.FlashingSwitch();
	}

	public void Fire2()
	{
		h.SeeThroughSwitch();
	}
}
