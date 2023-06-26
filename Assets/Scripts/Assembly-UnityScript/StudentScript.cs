using System;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class StudentScript : MonoBehaviour
{
	private Quaternion targetRotation;

	public DetectionMarkerScript DetectionMarker;

	public ShoulderCameraScript ShoulderCamera;

	public StudentManagerScript StudentManager;

	public CameraEffectsScript CameraEffects;

	public DialogueWheelScript DialogueWheel;

	public ExclamationScript Exclamation;

	public DynamicGridObstacle Obstacle;

	public ReputationScript Reputation;

	public SubtitleScript Subtitle;

	public YandereScript Yandere;

	public OutlineScript Outline;

	public PromptScript Prompt;

	public AIPath Pathfinding;

	public ClockScript Clock;

	public JsonScript JSON;

	public Transform CurrentDestination;

	public string[] DestinationNames;

	public Transform[] Destinations;

	public float[] PhaseTimes;

	public Plane[] Planes;

	public GameObject BloodSpray;

	public GameObject Character;

	public GameObject Ragdoll;

	public GameObject Marker;

	public CharacterController MyController;

	public Collider MyCollider;

	public Camera Eyes;

	public float DistanceToDestination;

	public float DistanceToPlayer;

	public float IgnoreTimer;

	public float AlarmTimer;

	public float TalkTimer;

	public float WaitTimer;

	public float GroundHeight;

	public float PendingRep;

	public float Alarm;

	public bool Forgave;

	public bool Alarmed;

	public bool Reacted;

	public bool Witness;

	public bool Routine;

	public bool Dead;

	public bool Talking;

	public bool Waiting;

	public bool Dying;

	public int Interaction;

	public int StudentID;

	public int Class;

	public int Phase;

	public Vector3 RightEyeOrigin;

	public Vector3 LeftEyeOrigin;

	public Transform RightEye;

	public Transform LeftEye;

	public float EyeShrink;

	public string Witnessed;

	private float MaxSpeed;

	public SkinnedMeshRenderer MyRenderer;

	public Mesh NudeMesh;

	public Texture[] NudeTexture;

	public bool AoT;

	public StudentScript()
	{
		Routine = true;
		Witnessed = string.Empty;
		MaxSpeed = 10f;
	}

	public virtual void Start()
	{
		DetectionMarker = (DetectionMarkerScript)((GameObject)UnityEngine.Object.Instantiate(Marker, GameObject.Find("DetectionPanel").transform.position, Quaternion.identity)).GetComponent(typeof(DetectionMarkerScript));
		DetectionMarker.transform.parent = GameObject.Find("DetectionPanel").transform;
		DetectionMarker.Target = transform;
		Class = JSON.StudentClasses[StudentID];
		GetDestinations();
		DialogueWheel = (DialogueWheelScript)GameObject.Find("DialogueWheel").GetComponent(typeof(DialogueWheelScript));
		Reputation = (ReputationScript)GameObject.Find("Reputation").GetComponent(typeof(ReputationScript));
		Yandere = (YandereScript)GameObject.Find("YandereChan").GetComponent(typeof(YandereScript));
		Subtitle = (SubtitleScript)GameObject.Find("Subtitle").GetComponent(typeof(SubtitleScript));
		Clock = (ClockScript)GameObject.Find("Clock").GetComponent(typeof(ClockScript));
		ShoulderCamera = (ShoulderCameraScript)Camera.main.GetComponent(typeof(ShoulderCameraScript));
		CameraEffects = (CameraEffectsScript)Camera.main.GetComponent(typeof(CameraEffectsScript));
		RightEyeOrigin = RightEye.localPosition;
		LeftEyeOrigin = LeftEye.localPosition;
		if (Yandere.Armed)
		{
			Prompt.HideButton[0] = true;
			Prompt.HideButton[2] = false;
		}
		else
		{
			Prompt.HideButton[0] = false;
			Prompt.HideButton[2] = true;
		}
	}

	public virtual void Update()
	{
		if (Routine)
		{
			if (Phase < Extensions.get_length((System.Array)PhaseTimes) - 1 && !(Clock.PresentTime / 60f < PhaseTimes[Phase]))
			{
				Phase++;
				CurrentDestination = Destinations[Phase];
				Pathfinding.target = Destinations[Phase];
				Pathfinding.canSearch = true;
				Pathfinding.canMove = true;
			}
			DistanceToDestination = Vector3.Distance(transform.position, CurrentDestination.position);
			if (!(DistanceToDestination <= 0.5f))
			{
				Character.GetComponent<Animation>().CrossFade("f02_walk_00");
				Character.GetComponent<Animation>()["f02_walk_00"].speed = Pathfinding.currentSpeed;
			}
			else
			{
				Pathfinding.canSearch = false;
				Pathfinding.canMove = false;
				Character.GetComponent<Animation>().CrossFade("f02_idleShort_00");
				transform.position = Vector3.Lerp(transform.position, CurrentDestination.position, Time.deltaTime * 10f);
				transform.rotation = Quaternion.Slerp(transform.rotation, CurrentDestination.rotation, 10f * Time.deltaTime);
			}
			DistanceToPlayer = Vector3.Distance(transform.position, Yandere.transform.position);
			if (!(DistanceToPlayer >= 10f))
			{
				if (Yandere.Armed || !(Yandere.Bloodiness <= 0f))
				{
					Planes = GeometryUtility.CalculateFrustumPlanes(Eyes);
					if (GeometryUtility.TestPlanesAABB(Planes, Yandere.GetComponent<Collider>().bounds))
					{
						RaycastHit hitInfo = default(RaycastHit);
						if (Physics.Linecast(Eyes.transform.position, Yandere.transform.position + Vector3.up * 1f, out hitInfo))
						{
							if (hitInfo.collider.gameObject == Yandere.gameObject)
							{
								if (!Alarmed && !(IgnoreTimer > 0f))
								{
									Alarm += Time.deltaTime * (100f / DistanceToPlayer);
								}
							}
							else
							{
								Alarm -= Time.deltaTime * 100f;
							}
						}
					}
					else if (!Alarmed)
					{
						Alarm -= Time.deltaTime * 100f;
					}
				}
				else if (!Alarmed)
				{
					Alarm -= Time.deltaTime * 100f;
				}
				if (!(Alarm <= 100f) && !Alarmed)
				{
					Outline.color = new Color(1f, 1f, 0f, 1f);
					Pathfinding.canSearch = false;
					Pathfinding.canMove = false;
					Routine = false;
					Alarmed = true;
					Witness = true;
					Reputation.PendingRep -= 10f;
					PendingRep -= 10f;
					Exclamation.Timer = 3f;
					CameraEffects.Alarm();
					if (Yandere.Armed)
					{
						Witnessed = "Weapon";
					}
					if (Yandere.Armed && !(Yandere.Bloodiness <= 0f))
					{
						Witnessed = "Weapon and Blood";
					}
					else if (Yandere.Armed)
					{
						Witnessed = "Weapon";
					}
					else if (!(Yandere.Bloodiness <= 0f))
					{
						Witnessed = "Blood";
					}
				}
			}
			else if (!Alarmed)
			{
				Alarm -= Time.deltaTime * 100f;
			}
		}
		if (!(Alarm <= 100f))
		{
			Alarm = 100f;
		}
		if (!(Alarm >= 0f))
		{
			Alarm = 0f;
		}
		float y = Alarm / 100f;
		Vector3 localScale = DetectionMarker.Tex.transform.localScale;
		float num = (localScale.y = y);
		Vector3 vector2 = (DetectionMarker.Tex.transform.localScale = localScale);
		float a = Alarm / 100f;
		Color color = DetectionMarker.Tex.color;
		float num2 = (color.a = a);
		Color color3 = (DetectionMarker.Tex.color = color);
		if (Alarm > 0f || Yandere.Armed)
		{
			Prompt.Circle[0].fillAmount = 1f;
		}
		if (!(Prompt.Circle[0].fillAmount > 0f))
		{
			Subtitle.UpdateLabel("Greeting", 0, 3f);
			ShoulderCamera.OverShoulder = true;
			Pathfinding.canSearch = false;
			Pathfinding.canMove = false;
			Obstacle.enabled = true;
			Yandere.TargetStudent = this;
			Yandere.CanMove = false;
			Yandere.Talking = true;
			Reacted = false;
			Talking = true;
			Routine = false;
			StudentManager.DisablePrompts();
			DialogueWheel.HideShadows();
			if (!Witness || Forgave)
			{
				float a2 = 0.75f;
				Color color4 = DialogueWheel.Shadow[1].color;
				float num3 = (color4.a = a2);
				Color color6 = (DialogueWheel.Shadow[1].color = color4);
			}
			DialogueWheel.Show = true;
			TalkTimer = 0f;
		}
		if (!(Prompt.Circle[2].fillAmount > 0f))
		{
			Subtitle.UpdateLabel("Dying", 0, 1f);
			Pathfinding.canSearch = false;
			Pathfinding.canMove = false;
			Yandere.TargetStudent = this;
			Yandere.Attacking = true;
			Yandere.CanMove = false;
			MyCollider.enabled = false;
			Alarmed = false;
			Routine = false;
			Dying = true;
			Prompt.Hide();
			Prompt.enabled = false;
		}
		if (Talking)
		{
			if (Interaction == 0)
			{
				Character.GetComponent<Animation>().CrossFade("f02_idleShort_00");
				if (TalkTimer == 0f)
				{
					DialogueWheel.Impatience.fillAmount = DialogueWheel.Impatience.fillAmount + Time.deltaTime * 0.1f;
					if (!(DialogueWheel.Impatience.fillAmount <= 0.5f) && Subtitle.Timer == 0f)
					{
						Subtitle.UpdateLabel("Impatience", 1, 5f);
					}
					if (!(DialogueWheel.Impatience.fillAmount < 1f))
					{
						Subtitle.UpdateLabel("Impatience", 2, 3f);
						DialogueWheel.End();
						WaitTimer = 0f;
					}
				}
			}
			else if (Interaction == 1)
			{
				if (TalkTimer == 3f)
				{
					Character.GetComponent<Animation>().CrossFade("f02_nod_01");
					Reputation.PendingRep += 5f;
					PendingRep += 5f;
					Outline.color = new Color(0f, 1f, 0f, 1f);
					Forgave = true;
					Subtitle.UpdateLabel("Forgiving", 0, 3f);
				}
				else
				{
					if (!(Character.GetComponent<Animation>()["f02_nod_01"].time < Character.GetComponent<Animation>()["f02_nod_01"].length))
					{
						Character.GetComponent<Animation>().CrossFade("f02_idleShort_00");
					}
					if (!(TalkTimer > 0f))
					{
						DialogueWheel.End();
					}
				}
				TalkTimer -= Time.deltaTime;
			}
			else if (Interaction == 4)
			{
				if (TalkTimer == 2f)
				{
					Subtitle.UpdateLabel("Student Farewell", 0, 2f);
				}
				Character.GetComponent<Animation>().CrossFade("f02_idleShort_00");
				TalkTimer -= Time.deltaTime;
				if (!(TalkTimer > 0f))
				{
					DialogueWheel.End();
				}
			}
			if (Waiting)
			{
				WaitTimer -= Time.deltaTime;
				if (!(WaitTimer > 0f))
				{
					Pathfinding.canSearch = true;
					Pathfinding.canMove = true;
					Obstacle.enabled = false;
					Alarmed = false;
					Talking = false;
					Waiting = false;
					Routine = true;
					StudentManager.EnablePrompts();
				}
			}
			else
			{
				targetRotation = Quaternion.LookRotation(Yandere.transform.position - transform.position);
				transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
			}
		}
		else if (Dying)
		{
			Alarm -= Time.deltaTime * 100f;
			EyeShrink = Mathf.Lerp(EyeShrink, 1f, Time.deltaTime * 10f);
			if (!Dead)
			{
				Character.GetComponent<Animation>().CrossFade("f02_defend_00");
				targetRotation = Quaternion.LookRotation(new Vector3(Yandere.transform.position.x, transform.position.y, Yandere.transform.position.z) - transform.position);
				transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
				transform.position = Vector3.Lerp(transform.position, Yandere.transform.position + Yandere.transform.forward * 0.1f, Time.deltaTime * 10f);
			}
			else
			{
				Character.GetComponent<Animation>().CrossFade("f02_down_22");
				if (!(Character.GetComponent<Animation>()["f02_down_22"].time >= 1f))
				{
					transform.Translate(Vector3.back * Time.deltaTime);
				}
				else
				{
					GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(Ragdoll, transform.position, transform.rotation);
					((RagdollScript)gameObject.GetComponent(typeof(RagdollScript))).AnimStartTime = Character.GetComponent<Animation>()["f02_down_22"].time;
					((RagdollScript)gameObject.GetComponent(typeof(RagdollScript))).Yandere = Yandere;
					BloodSpray.transform.parent = ((RagdollScript)gameObject.GetComponent(typeof(RagdollScript))).BloodParent;
					BloodSpray.transform.localPosition = new Vector3(0f, 0f, 0f);
					BloodSpray.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
					Reputation.PendingRep += PendingRep * -1f;
					UnityEngine.Object.Destroy(DetectionMarker);
					UnityEngine.Object.Destroy(this.gameObject);
				}
			}
		}
		else if (Alarmed)
		{
			Character.GetComponent<Animation>().CrossFade("f02_idleShort_00");
			targetRotation = Quaternion.LookRotation(Yandere.transform.position - transform.position);
			transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
			AlarmTimer += Time.deltaTime;
			Alarm -= Time.deltaTime * 100f;
			if (!(AlarmTimer <= 5f))
			{
				Pathfinding.canSearch = true;
				Pathfinding.canMove = true;
				IgnoreTimer = 5f;
				Alarmed = false;
				Reacted = false;
				Routine = true;
				AlarmTimer = 0f;
			}
			else if (!(AlarmTimer <= 1f) && !Reacted)
			{
				if (Witnessed == "Weapon")
				{
					Subtitle.UpdateLabel("Weapon Reaction", 1, 3f);
				}
				else if (Witnessed == "Blood")
				{
					Subtitle.UpdateLabel("Blood Reaction", 1, 3f);
				}
				else if (Witnessed == "Weapon and Blood")
				{
					Subtitle.UpdateLabel("Weapon and Blood Reaction", 1, 3f);
				}
				Reacted = true;
			}
		}
		if (!(IgnoreTimer <= 0f))
		{
			IgnoreTimer -= Time.deltaTime;
		}
		if (MyController.isGrounded && !(MyController.velocity.y > 3f))
		{
			GroundHeight = transform.position.y;
		}
		if (!(MyController.velocity.y <= 3f))
		{
			float y2 = GroundHeight + 0.1f;
			Vector3 position = transform.position;
			float num4 = (position.y = y2);
			Vector3 vector4 = (transform.position = position);
		}
		if (AoT)
		{
			transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(10f, 10f, 10f), Time.deltaTime);
		}
	}

	public virtual void LateUpdate()
	{
		float z = LeftEyeOrigin.z - EyeShrink * 0.01f;
		Vector3 localPosition = LeftEye.localPosition;
		float num = (localPosition.z = z);
		Vector3 vector2 = (LeftEye.localPosition = localPosition);
		float z2 = RightEyeOrigin.z + EyeShrink * 0.01f;
		Vector3 localPosition2 = RightEye.localPosition;
		float num2 = (localPosition2.z = z2);
		Vector3 vector4 = (RightEye.localPosition = localPosition2);
		float x = 1f - EyeShrink * 0.5f;
		Vector3 localScale = LeftEye.localScale;
		float num3 = (localScale.x = x);
		Vector3 vector6 = (LeftEye.localScale = localScale);
		float y = 1f - EyeShrink * 0.5f;
		Vector3 localScale2 = LeftEye.localScale;
		float num4 = (localScale2.y = y);
		Vector3 vector8 = (LeftEye.localScale = localScale2);
		float x2 = 1f - EyeShrink * 0.5f;
		Vector3 localScale3 = RightEye.localScale;
		float num5 = (localScale3.x = x2);
		Vector3 vector10 = (RightEye.localScale = localScale3);
		float y2 = 1f - EyeShrink * 0.5f;
		Vector3 localScale4 = RightEye.localScale;
		float num6 = (localScale4.y = y2);
		Vector3 vector12 = (RightEye.localScale = localScale4);
	}

	public virtual void GetDestinations()
	{
		DestinationNames = (string[])JSON.StudentDestinations[StudentID].ToBuiltin(typeof(string));
		for (int i = 1; i < Extensions.get_length((System.Array)Destinations); i++)
		{
			if (DestinationNames[i] == "Locker")
			{
				Destinations[i] = StudentManager.Lockers.List[StudentID];
			}
			if (DestinationNames[i] == "Class")
			{
				Destinations[i] = StudentManager.Classrooms.List[Class];
			}
			if (DestinationNames[i] == "Rooftop")
			{
				Destinations[i] = StudentManager.Hangouts.List[0];
			}
			if (DestinationNames[i] == "Exit")
			{
				Destinations[i] = StudentManager.Hangouts.List[1];
			}
		}
	}

	public virtual void AttackOnTitan()
	{
		AoT = true;
		MyRenderer.sharedMesh = NudeMesh;
		MyRenderer.materials[0].mainTexture = NudeTexture[1];
		MyRenderer.materials[3].mainTexture = NudeTexture[0];
		Outline.h.ReinitMaterials();
	}

	public virtual void Main()
	{
	}
}
