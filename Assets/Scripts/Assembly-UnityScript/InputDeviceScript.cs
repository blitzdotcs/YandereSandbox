using System;
using UnityEngine;

[Serializable]
public class InputDeviceScript : MonoBehaviour
{
	public int Type;

	public Vector3 MousePrevious;

	public Vector3 MouseDelta;

	public InputDeviceScript()
	{
		Type = 1;
	}

	public virtual void Update()
	{
		MouseDelta = Input.mousePosition - MousePrevious;
		MousePrevious = Input.mousePosition;
		if (Input.anyKey || Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButton(2) || MouseDelta != new Vector3(0f, 0f, 0f))
		{
			Type = 2;
		}
		if (Input.GetKey(KeyCode.Joystick1Button0) || Input.GetKey(KeyCode.Joystick1Button1) || Input.GetKey(KeyCode.Joystick1Button2) || Input.GetKey(KeyCode.Joystick1Button3) || Input.GetKey(KeyCode.Joystick1Button4) || Input.GetKey(KeyCode.Joystick1Button5) || Input.GetKey(KeyCode.Joystick1Button6) || Input.GetKey(KeyCode.Joystick1Button7) || Input.GetKey(KeyCode.Joystick1Button8) || Input.GetKey(KeyCode.Joystick1Button9) || Input.GetKey(KeyCode.Joystick1Button10) || Input.GetKey(KeyCode.Joystick1Button11) || Input.GetKey(KeyCode.Joystick1Button12) || Input.GetKey(KeyCode.Joystick1Button13) || Input.GetKey(KeyCode.Joystick1Button14) || Input.GetKey(KeyCode.Joystick1Button15) || Input.GetKey(KeyCode.Joystick1Button16) || Input.GetKey(KeyCode.Joystick1Button17) || Input.GetKey(KeyCode.Joystick1Button18) || Input.GetKey(KeyCode.Joystick1Button19) || (Input.GetAxis("Vertical") != 0f && Input.GetAxis("Vertical") != 1f && Input.GetAxis("Vertical") != -1f) || (Input.GetAxis("Horizontal") != 0f && Input.GetAxis("Horizontal") != 1f && Input.GetAxis("Horizontal") != -1f))
		{
			Type = 1;
		}
	}

	public virtual void Main()
	{
	}
}
