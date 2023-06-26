using System;
using UnityEngine;

[Serializable]
public class aoi_character_pack_v0111_viewscript : MonoBehaviour
{
	public Transform target;

	public string mode;

	public float x;

	public float y;

	public float distance;

	public float xSpeed;

	public float ySpeed;

	public float movSpeed;

	public float yMinLimit;

	public float yMaxLimit;

	public float zoomSpeed;

	public float zoomWheelBias;

	public float zoomMin;

	public float zoomMax;

	public Transform curTarget;

	private float xBk;

	private float yBk;

	private float movX;

	private float movY;

	private float wheel;

	private float distanceBk;

	private Vector3 cameraBk;

	private bool isMouseLocked;

	private bool isFixTarget;

	private Transform localTarget;

	private bool changeFlg;

	public aoi_character_pack_v0111_viewscript()
	{
		mode = "rote";
		x = 180f;
		y = 30f;
		distance = 1f;
		xSpeed = 500f;
		ySpeed = 250f;
		movSpeed = 250f;
		yMinLimit = -90f;
		yMaxLimit = 90f;
		zoomSpeed = 0.5f;
		zoomWheelBias = 5f;
		zoomMin = 0.1f;
		zoomMax = 5f;
		isFixTarget = true;
	}

	public virtual void Start()
	{
		xBk = x;
		yBk = y;
		distanceBk = distance;
	}

	public virtual void LateUpdate()
	{
		movX = Input.GetAxis("Mouse X");
		movY = Input.GetAxis("Mouse Y");
		wheel = Input.GetAxis("Mouse ScrollWheel");
		if (!isMouseLocked && Input.GetMouseButton(0))
		{
			switch (mode)
			{
			case "move":
				TargetMove(movX, movY);
				break;
			case "rote":
				CameraRote(movX, movY);
				break;
			case "zoom":
				CameraZoom(movX, movY);
				break;
			}
		}
		if (Input.GetMouseButton(2))
		{
			TargetMove(movX, movY);
		}
		if (Input.GetMouseButton(1))
		{
			CameraZoom(movX, movY);
		}
		CameraZoom(wheel * zoomWheelBias, 0f);
		if (isFixTarget && (bool)curTarget)
		{
			localTarget = curTarget;
		}
		else
		{
			localTarget = target;
		}
		Quaternion quaternion = Quaternion.Euler(y, x, 0f);
		Vector3 position = quaternion * new Vector3(0f, 0f, 0f - distance) + localTarget.position;
		if (!changeFlg)
		{
			transform.position = position;
		}
		if (isFixTarget && (bool)curTarget)
		{
			localTarget = curTarget;
		}
		else
		{
			localTarget = target;
		}
		if (!changeFlg)
		{
			transform.LookAt(localTarget.position, Vector3.up);
		}
		changeFlg = false;
	}

	public virtual void CameraRote(float _x, float _y)
	{
		x += _x * xSpeed * 0.01f;
		y -= _y * ySpeed * 0.01f;
		y = ClampAngle(y, yMinLimit, yMaxLimit);
	}

	public virtual void CameraZoom(float _x, float _y)
	{
		distance += (0f - _y) * 10f * zoomSpeed * 0.02f;
		distance += (0f - _x) * 10f * zoomSpeed * 0.02f;
		if (!(distance >= zoomMin))
		{
			distance = zoomMin;
		}
		if (!(distance <= zoomMax))
		{
			distance = zoomMax;
		}
	}

	public virtual void TargetMove(float _x, float _y)
	{
		if (!isFixTarget)
		{
			float num = (0f - _x) * movSpeed * 0.055f * Time.deltaTime;
			float num2 = (0f - _y) * movSpeed * 0.055f * Time.deltaTime;
			Vector3 v = new Vector3(num, num2);
			v = GetComponent<Camera>().cameraToWorldMatrix.MultiplyVector(v);
			target.Translate(v);
		}
	}

	public static float ClampAngle(float angle, float min, float max)
	{
		if (!(angle >= -360f))
		{
			angle += 360f;
		}
		if (!(angle <= 360f))
		{
			angle -= 360f;
		}
		return Mathf.Clamp(angle, min, max);
	}

	public virtual void ModeMove()
	{
		mode = "move";
		MonoBehaviour.print("move");
	}

	public virtual void ModeRote()
	{
		mode = "rote";
		MonoBehaviour.print("rote");
	}

	public virtual void ModeZoom()
	{
		mode = "zoom";
		MonoBehaviour.print("zoom");
	}

	public virtual void Reset()
	{
		distance = distanceBk;
		x = xBk;
		y = yBk;
		isFixTarget = true;
	}

	public virtual void FixTarget(bool _flag)
	{
		isFixTarget = _flag;
		if ((bool)curTarget)
		{
			target.position = curTarget.position;
		}
	}

	public virtual void ModelTarget(Transform _transform)
	{
		curTarget = _transform;
		changeFlg = true;
	}

	public virtual void MouseLock(bool _flag)
	{
		if (_flag || !Input.GetMouseButton(0))
		{
			isMouseLocked = _flag;
		}
	}

	public virtual void Main()
	{
	}
}
