using System;
using UnityEngine;

[Serializable]
public class IntroYandereScript : MonoBehaviour
{
	public Transform Hips;

	public Transform Spine;

	public Transform Spine1;

	public Transform Spine2;

	public Transform Spine3;

	public Transform Neck;

	public Transform Head;

	public Transform RightUpLeg;

	public Transform RightLeg;

	public Transform RightFoot;

	public Transform LeftUpLeg;

	public Transform LeftLeg;

	public Transform LeftFoot;

	public float X;

	public virtual void LateUpdate()
	{
		float x = Hips.localEulerAngles.x + X;
		Vector3 localEulerAngles = Hips.localEulerAngles;
		float num = (localEulerAngles.x = x);
		Vector3 vector2 = (Hips.localEulerAngles = localEulerAngles);
		float x2 = Spine.localEulerAngles.x + X;
		Vector3 localEulerAngles2 = Spine.localEulerAngles;
		float num2 = (localEulerAngles2.x = x2);
		Vector3 vector4 = (Spine.localEulerAngles = localEulerAngles2);
		float x3 = Spine1.localEulerAngles.x + X;
		Vector3 localEulerAngles3 = Spine1.localEulerAngles;
		float num3 = (localEulerAngles3.x = x3);
		Vector3 vector6 = (Spine1.localEulerAngles = localEulerAngles3);
		float x4 = Spine2.localEulerAngles.x + X;
		Vector3 localEulerAngles4 = Spine2.localEulerAngles;
		float num4 = (localEulerAngles4.x = x4);
		Vector3 vector8 = (Spine2.localEulerAngles = localEulerAngles4);
		float x5 = Spine3.localEulerAngles.x + X;
		Vector3 localEulerAngles5 = Spine3.localEulerAngles;
		float num5 = (localEulerAngles5.x = x5);
		Vector3 vector10 = (Spine3.localEulerAngles = localEulerAngles5);
		float x6 = Neck.localEulerAngles.x + X;
		Vector3 localEulerAngles6 = Neck.localEulerAngles;
		float num6 = (localEulerAngles6.x = x6);
		Vector3 vector12 = (Neck.localEulerAngles = localEulerAngles6);
		float x7 = Head.localEulerAngles.x + X;
		Vector3 localEulerAngles7 = Head.localEulerAngles;
		float num7 = (localEulerAngles7.x = x7);
		Vector3 vector14 = (Head.localEulerAngles = localEulerAngles7);
		float x8 = RightUpLeg.localEulerAngles.x - X;
		Vector3 localEulerAngles8 = RightUpLeg.localEulerAngles;
		float num8 = (localEulerAngles8.x = x8);
		Vector3 vector16 = (RightUpLeg.localEulerAngles = localEulerAngles8);
		float x9 = RightLeg.localEulerAngles.x - X;
		Vector3 localEulerAngles9 = RightLeg.localEulerAngles;
		float num9 = (localEulerAngles9.x = x9);
		Vector3 vector18 = (RightLeg.localEulerAngles = localEulerAngles9);
		float x10 = RightFoot.localEulerAngles.x - X;
		Vector3 localEulerAngles10 = RightFoot.localEulerAngles;
		float num10 = (localEulerAngles10.x = x10);
		Vector3 vector20 = (RightFoot.localEulerAngles = localEulerAngles10);
		float x11 = LeftUpLeg.localEulerAngles.x - X;
		Vector3 localEulerAngles11 = LeftUpLeg.localEulerAngles;
		float num11 = (localEulerAngles11.x = x11);
		Vector3 vector22 = (LeftUpLeg.localEulerAngles = localEulerAngles11);
		float x12 = LeftLeg.localEulerAngles.x - X;
		Vector3 localEulerAngles12 = LeftLeg.localEulerAngles;
		float num12 = (localEulerAngles12.x = x12);
		Vector3 vector24 = (LeftLeg.localEulerAngles = localEulerAngles12);
		float x13 = LeftFoot.localEulerAngles.x - X;
		Vector3 localEulerAngles13 = LeftFoot.localEulerAngles;
		float num13 = (localEulerAngles13.x = x13);
		Vector3 vector26 = (LeftFoot.localEulerAngles = localEulerAngles13);
	}

	public virtual void Main()
	{
	}
}
