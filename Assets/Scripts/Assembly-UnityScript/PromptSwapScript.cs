using System;
using UnityEngine;

[Serializable]
public class PromptSwapScript : MonoBehaviour
{
	public InputDeviceScript InputDevice;

	public UISprite MySprite;

	public string KeyboardName;

	public string GamepadName;

	public PromptSwapScript()
	{
		KeyboardName = string.Empty;
		GamepadName = string.Empty;
	}

	public virtual void Start()
	{
		InputDevice = (InputDeviceScript)transform.parent.gameObject.GetComponent("InputDeviceScript");
	}

	public virtual void Update()
	{
		if (InputDevice.Type == 1)
		{
			MySprite.spriteName = GamepadName;
		}
		else
		{
			MySprite.spriteName = KeyboardName;
		}
	}

	public virtual void Main()
	{
	}
}
