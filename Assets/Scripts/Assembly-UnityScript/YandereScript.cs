using System;
using HighlightingSystem;
using UnityEngine;

[Serializable]
public class YandereScript : MonoBehaviour
{
	private Quaternion targetRotation;

	private Vector3 targetDirection;

	private GameObject NewTrail;

	private int ID;

	public FootprintSpawnerScript RightFootprintSpawner;

	public FootprintSpawnerScript LeftFootprintSpawner;

	public NotificationManagerScript NotificationManager;

	public ColorCorrectionCurves YandereColorCorrection;

	public ColorCorrectionCurves ColorCorrection;

	public StudentManagerScript StudentManager;

	public HighlightingRenderer HighlightingR;

	public HighlightingBlitter HighlightingB;

	public SWP_HeartRateMonitor HeartRate;

	public IncineratorScript Incinerator;

	public StudentScript TargetStudent;

	public DepthOfField34 DepthOfField;

	public PromptScript NearestPrompt;

	public SubtitleScript Subtitle;

	public OutlineScript Outline;

	public WeaponScript Weapon;

	public SpringJoint RagdollDragger;

	public Transform RightBreast;

	public Transform LeftBreast;

	public Transform ItemParent;

	public Transform Homeroom;

	public Transform Senpai;

	public Transform Head;

	public AudioSource HeartBeat;

	public AudioSource Jukebox;

	public GameObject Character;

	public GameObject DumpChan;

	public GameObject Eyepatch;

	public GameObject Ragdoll;

	public GameObject Hearts;

	public GameObject Phone;

	public GameObject Trail;

	public Renderer MyRenderer;

	public float YandereTimer;

	public float AttackTimer;

	public float LaughTimer;

	public float DumpTimer;

	public float TalkTimer;

	public float Bloodiness;

	public float Sanity;

	public float TwitchFactor;

	public float TwitchTimer;

	public float NextTwitch;

	public float LaughIntensity;

	public float BreastSize;

	public float WalkSpeed;

	public float RunSpeed;

	public float Slouch;

	public float YandereFade;

	public float YandereTint;

	public float SenpaiFade;

	public float SenpaiTint;

	public int AttackPhase;

	public int Interaction;

	public int NearBodies;

	public int Costume;

	public bool BloodyWarning;

	public bool CorpseWarning;

	public bool SanityWarning;

	public bool WeaponWarning;

	public bool TimeSkipping;

	public bool Attacking;

	public bool Dragging;

	public bool Laughing;

	public bool Throwing;

	public bool Dumping;

	public bool Talking;

	public bool YandereVision;

	public bool Heartbroken;

	public bool DpadPressed;

	public bool NearSenpai;

	public bool CanMove;

	public bool Armed;

	public bool Die;

	public Texture[] UniformTextures;

	public Transform[] Spine;

	public Transform[] Arm;

	public Renderer RightYandereEye;

	public Renderer LeftYandereEye;

	public Vector3 RightEyeOrigin;

	public Vector3 LeftEyeOrigin;

	public Transform RightEye;

	public Transform LeftEye;

	public float EyeShrink;

	public Vector3 Twitch;

	private AudioClip LaughClip;

	public string LaughAnim;

	public AudioClip Laugh1;

	public AudioClip Laugh2;

	public AudioClip Laugh3;

	public AudioClip Laugh4;

	public bool AoT;

	public Texture TitanTexture;

	public YandereScript()
	{
		Sanity = 100f;
		BreastSize = 1f;
		CanMove = true;
		LaughAnim = string.Empty;
	}

	public virtual void Start()
	{
		RightEyeOrigin = RightEye.localPosition;
		LeftEyeOrigin = LeftEye.localPosition;
		Character.GetComponent<Animation>()["f02_yanderePose_00"].weight = 0f;
		ColorCorrectionCurves[] components = Camera.main.GetComponents<ColorCorrectionCurves>();
		YandereColorCorrection = components[1];
		ResetYandereEffects();
		ResetSenpaiEffects();
		UpdateSanity();
	}

	public virtual void Update()
	{
		if (CanMove)
		{
			Vector3 vector = Camera.main.transform.TransformDirection(Vector3.forward);
			vector.y = 0f;
			vector = vector.normalized;
			Vector3 vector2 = new Vector3(vector.z, 0f, 0f - vector.x);
			float axis = Input.GetAxis("Vertical");
			float axis2 = Input.GetAxis("Horizontal");
			targetDirection = axis2 * vector2 + axis * vector;
			if (targetDirection != Vector3.zero)
			{
				targetRotation = Quaternion.LookRotation(targetDirection);
				transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
			}
			else
			{
				targetRotation = new Quaternion(0f, 0f, 0f, 0f);
			}
			if (axis != 0f || axis2 != 0f)
			{
				if (Input.GetButton("LB") && !(Vector3.Distance(transform.position, Senpai.position) <= 2f))
				{
					Character.GetComponent<Animation>().CrossFade("f02_sprint_00");
					transform.Translate(Vector3.forward * RunSpeed * Time.deltaTime);
				}
				else if (!Dragging)
				{
					Character.GetComponent<Animation>().CrossFade("f02_walk_00");
					transform.Translate(Vector3.forward * WalkSpeed * Time.deltaTime);
				}
				else
				{
					Character.GetComponent<Animation>().CrossFade("f02_dragWalk_00");
					transform.Translate(Vector3.forward * WalkSpeed * Time.deltaTime);
				}
			}
			else if (!Dragging)
			{
				if (!(Time.timeScale <= 2f))
				{
				}
				Character.GetComponent<Animation>().CrossFade("f02_idleShort_00");
			}
			else
			{
				Character.GetComponent<Animation>().CrossFade("f02_dragIdle_00");
			}
			if (!NearSenpai)
			{
				if (Input.GetButton("RB"))
				{
					YandereTimer += Time.deltaTime;
					if (!(YandereTimer <= 0.5f))
					{
						YandereVision = true;
					}
				}
				if (Input.GetButtonUp("RB"))
				{
					if (!(YandereTimer >= 0.5f))
					{
						if (!Dragging && !Laughing)
						{
							LaughAnim = "f02_laugh_01";
							LaughClip = Laugh1;
							Laughing = true;
							LaughIntensity += 1f;
							LaughTimer = 0.5f;
							GetComponent<AudioSource>().volume = 1f;
							GetComponent<AudioSource>().time = 0f;
							GetComponent<AudioSource>().Play();
							CanMove = false;
						}
					}
					else
					{
						YandereVision = false;
					}
					YandereTimer = 0f;
				}
			}
		}
		else
		{
			if (Dumping)
			{
				targetRotation = Quaternion.LookRotation(Incinerator.transform.position + Vector3.fwd * 4f - transform.position);
				transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
				transform.position = Vector3.Lerp(transform.position, Incinerator.transform.position + Vector3.fwd * 3f, Time.deltaTime * 10f);
				DumpTimer += Time.deltaTime;
				if (!(DumpTimer <= 1f))
				{
					if (Character.GetComponent<Animation>()["f02_throw_20_p"].time == 0f)
					{
						SpawnDumpChan();
					}
					Character.GetComponent<Animation>().CrossFade("f02_throw_20_p");
					if (!(Character.GetComponent<Animation>()["f02_throw_20_p"].time < Character.GetComponent<Animation>()["f02_throw_20_p"].length))
					{
						Incinerator.Prompt.Label[3].text = "     " + "Activate";
						Incinerator.Occupied = true;
						Incinerator.Open = false;
						Dragging = false;
						Dumping = false;
						CanMove = true;
						DumpTimer = 0f;
					}
				}
			}
			if (Heartbroken && !(Character.GetComponent<Animation>()["f02_down_22"].time < Character.GetComponent<Animation>()["f02_down_22"].length))
			{
				Character.GetComponent<Animation>().CrossFade("f02_down_23");
			}
			if (Laughing)
			{
				if (GetComponent<AudioSource>().clip != LaughClip)
				{
					GetComponent<AudioSource>().clip = LaughClip;
					GetComponent<AudioSource>().time = 0f;
					GetComponent<AudioSource>().Play();
				}
				Character.GetComponent<Animation>().CrossFade(LaughAnim);
				if (Input.GetButtonDown("RB"))
				{
					LaughIntensity += 1f;
					if (!(LaughIntensity > 5f))
					{
						LaughAnim = "f02_laugh_01";
						LaughClip = Laugh1;
						LaughTimer = 0.5f;
					}
					else if (!(LaughIntensity > 10f))
					{
						LaughAnim = "f02_laugh_02";
						LaughClip = Laugh2;
						LaughTimer = 1f;
					}
					else if (!(LaughIntensity > 15f))
					{
						LaughAnim = "f02_laugh_03";
						LaughClip = Laugh3;
						LaughTimer = 1.5f;
					}
					else if (!(LaughIntensity > 20f))
					{
						LaughAnim = "f02_laugh_04";
						LaughClip = Laugh4;
						LaughTimer = 2f;
					}
					else
					{
						LaughAnim = "f02_laugh_04";
						LaughIntensity = 20f;
						LaughTimer = 2f;
					}
				}
				if (!(LaughIntensity <= 15f))
				{
					Sanity += Time.deltaTime * 10f;
					UpdateSanity();
				}
				LaughTimer -= Time.deltaTime;
				if (!(LaughTimer > 0f))
				{
					LaughIntensity = 0f;
					Laughing = false;
					LaughClip = null;
					CanMove = true;
				}
			}
			if (TimeSkipping)
			{
				Character.GetComponent<Animation>().CrossFade("f02_timeSkip_00");
				Sanity += Time.deltaTime * 0.17f;
				UpdateSanity();
			}
		}
		if (!Laughing)
		{
			GetComponent<AudioSource>().volume -= Time.deltaTime * 2f;
		}
		if (Die)
		{
			Character.GetComponent<Animation>().CrossFade("f02_down_22");
			Heartbroken = true;
			CanMove = false;
			Die = false;
		}
		if (Input.GetButtonDown("LS") || Input.GetKeyDown("t"))
		{
			Debug.Log("Spawned?");
			if (NewTrail != null)
			{
				UnityEngine.Object.Destroy(NewTrail);
			}
			NewTrail = (GameObject)UnityEngine.Object.Instantiate(Trail, transform.position + Vector3.fwd * 0.5f + Vector3.up * 0.1f, Quaternion.identity);
			((AIPath)NewTrail.GetComponent(typeof(AIPath))).target = Homeroom;
		}
		if (!DpadPressed && Weapon != null && CanMove && !Dragging)
		{
			if (Input.GetKeyDown("1") || !(Input.GetAxis("DpadY") >= -0.5f))
			{
				Unequip();
			}
			else if (Input.GetKeyDown("2") || !(Input.GetAxis("DpadY") <= 0.5f))
			{
			//	Weapon.active = true;
				DpadPressed = true;
				Armed = true;
				StudentManager.UpdateStudents();
				if (!WeaponWarning)
				{
					NotificationManager.DisplayNotification("Armed");
					WeaponWarning = true;
				}
			}
		}
		if (!(Input.GetAxis("DpadY") <= -0.5f) && !(Input.GetAxis("DpadY") >= 0.5f) && !(Input.GetAxis("DpadX") <= -0.5f) && !(Input.GetAxis("DpadX") >= 0.5f))
		{
			DpadPressed = false;
		}
		else
		{
			DpadPressed = true;
		}
		if (!(Vector3.Distance(transform.position, Senpai.position) >= 2f))
		{
			if (!NearSenpai)
			{
				DepthOfField.focalSize = 150f;
				NearSenpai = true;
			}
			YandereVision = false;
			YandereTimer = 0f;
			if (Dragging)
			{
				((RagdollScript)Ragdoll.GetComponent(typeof(RagdollScript))).StopDragging();
			}
			if (Armed)
			{
				WeaponWarning = false;
				Weapon.Drop();
			}
		}
		else
		{
			NearSenpai = false;
		}
		if (NearSenpai)
		{
			DepthOfField.enabled = true;
			DepthOfField.focalSize = Mathf.Lerp(DepthOfField.focalSize, 0f, Time.deltaTime * 10f);
			DepthOfField.focalZStartCurve = Mathf.Lerp(DepthOfField.focalZStartCurve, 20f, Time.deltaTime * 10f);
			DepthOfField.focalZEndCurve = Mathf.Lerp(DepthOfField.focalZEndCurve, 20f, Time.deltaTime * 10f);
			DepthOfField.objectFocus = Senpai.transform;
			ColorCorrection.enabled = true;
			SenpaiFade = Mathf.Lerp(SenpaiFade, 0f, Time.deltaTime * 10f);
			SenpaiTint = 1f - SenpaiFade / 100f;
			ColorCorrection.redChannel.MoveKey(1, new Keyframe(0.5f, 0.5f + SenpaiTint * 0.5f));
			ColorCorrection.greenChannel.MoveKey(1, new Keyframe(0.5f, 0.5f - SenpaiTint * 0.5f));
			ColorCorrection.blueChannel.MoveKey(1, new Keyframe(0.5f, 0.5f + SenpaiTint * 0.5f));
			ColorCorrection.redChannel.SmoothTangents(1, 0f);
			ColorCorrection.greenChannel.SmoothTangents(1, 0f);
			ColorCorrection.blueChannel.SmoothTangents(1, 0f);
			ColorCorrection.UpdateTextures();
			Character.GetComponent<Animation>()["f02_shy_00"].weight = SenpaiTint;
			HeartBeat.volume = SenpaiTint;
			Sanity += Time.deltaTime * 10f;
			UpdateSanity();
		}
		else if (!(SenpaiFade >= 99f))
		{
			DepthOfField.focalSize = Mathf.Lerp(DepthOfField.focalSize, 150f, Time.deltaTime * 10f);
			DepthOfField.focalZStartCurve = Mathf.Lerp(DepthOfField.focalZStartCurve, 0f, Time.deltaTime * 10f);
			DepthOfField.focalZEndCurve = Mathf.Lerp(DepthOfField.focalZEndCurve, 0f, Time.deltaTime * 10f);
			SenpaiFade = Mathf.Lerp(SenpaiFade, 100f, Time.deltaTime * 10f);
			SenpaiTint = SenpaiFade / 100f;
			ColorCorrection.redChannel.MoveKey(1, new Keyframe(0.5f, 1f - SenpaiTint * 0.5f));
			ColorCorrection.greenChannel.MoveKey(1, new Keyframe(0.5f, SenpaiTint * 0.5f));
			ColorCorrection.blueChannel.MoveKey(1, new Keyframe(0.5f, 1f - SenpaiTint * 0.5f));
			ColorCorrection.redChannel.SmoothTangents(1, 0f);
			ColorCorrection.greenChannel.SmoothTangents(1, 0f);
			ColorCorrection.blueChannel.SmoothTangents(1, 0f);
			ColorCorrection.UpdateTextures();
			Character.GetComponent<Animation>()["f02_shy_00"].weight = 1f - SenpaiTint;
			HeartBeat.volume = 1f - SenpaiTint;
		}
		else if (!(SenpaiFade >= 100f))
		{
			ResetSenpaiEffects();
		}
		if (YandereVision)
		{
			if (!HighlightingR.enabled)
			{
				YandereColorCorrection.enabled = true;
				HighlightingR.enabled = true;
				HighlightingB.enabled = true;
			}
			Time.timeScale = Mathf.Lerp(Time.timeScale, 0.5f, 1f / 6f);
			YandereFade = Mathf.Lerp(YandereFade, 0f, Time.deltaTime * 10f);
			YandereTint = 1f - YandereFade / 100f;
			YandereColorCorrection.redChannel.MoveKey(1, new Keyframe(0.5f, 0.5f - YandereTint * 0.25f));
			YandereColorCorrection.greenChannel.MoveKey(1, new Keyframe(0.5f, 0.5f - YandereTint * 0.25f));
			YandereColorCorrection.blueChannel.MoveKey(1, new Keyframe(0.5f, 0.5f + YandereTint * 0.25f));
			YandereColorCorrection.redChannel.SmoothTangents(1, 0f);
			YandereColorCorrection.greenChannel.SmoothTangents(1, 0f);
			YandereColorCorrection.blueChannel.SmoothTangents(1, 0f);
			YandereColorCorrection.UpdateTextures();
		}
		else
		{
			if (HighlightingR.enabled)
			{
				HighlightingR.enabled = false;
				HighlightingB.enabled = false;
			}
			if (!(YandereFade >= 99f))
			{
				Time.timeScale = Mathf.Lerp(Time.timeScale, 1f, 1f / 6f);
				YandereFade = Mathf.Lerp(YandereFade, 100f, Time.deltaTime * 10f);
				YandereTint = YandereFade / 100f;
				YandereColorCorrection.redChannel.MoveKey(1, new Keyframe(0.5f, YandereTint * 0.5f));
				YandereColorCorrection.greenChannel.MoveKey(1, new Keyframe(0.5f, YandereTint * 0.5f));
				YandereColorCorrection.blueChannel.MoveKey(1, new Keyframe(0.5f, 1f - YandereTint * 0.5f));
				YandereColorCorrection.redChannel.SmoothTangents(1, 0f);
				YandereColorCorrection.greenChannel.SmoothTangents(1, 0f);
				YandereColorCorrection.blueChannel.SmoothTangents(1, 0f);
				YandereColorCorrection.UpdateTextures();
			}
			else if (!(YandereFade >= 100f))
			{
				ResetYandereEffects();
			}
		}
		float a = 1f - YandereFade / 100f;
		Color color = RightYandereEye.material.color;
		float num = (color.a = a);
		Color color3 = (RightYandereEye.material.color = color);
		float a2 = 1f - YandereFade / 100f;
		Color color4 = LeftYandereEye.material.color;
		float num2 = (color4.a = a2);
		Color color6 = (LeftYandereEye.material.color = color4);
		if (Armed)
		{
			Character.GetComponent<Animation>()["f02_fist_00"].weight = Mathf.Lerp(Character.GetComponent<Animation>()["f02_fist_00"].weight, 1f, Time.deltaTime * 10f);
		}
		else
		{
			Character.GetComponent<Animation>()["f02_fist_00"].weight = Mathf.Lerp(Character.GetComponent<Animation>()["f02_fist_00"].weight, 0f, Time.deltaTime * 10f);
		}
		if (Talking)
		{
			targetRotation = Quaternion.LookRotation(new Vector3(TargetStudent.transform.position.x, transform.position.y, TargetStudent.transform.position.z) - transform.position);
			transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
			if (Interaction == 0)
			{
				Character.GetComponent<Animation>().CrossFade("f02_idleShort_00");
			}
			else if (Interaction == 1)
			{
				if (TalkTimer == 3f)
				{
					Character.GetComponent<Animation>().CrossFade("f02_greet_00");
					if (TargetStudent.Witnessed == "Weapon and Blood")
					{
						Subtitle.UpdateLabel("Weapon and Blood Apology", 0, 3f);
					}
					else if (TargetStudent.Witnessed == "Weapon")
					{
						Subtitle.UpdateLabel("Weapon Apology", 0, 3f);
					}
					else if (TargetStudent.Witnessed == "Blood")
					{
						Subtitle.UpdateLabel("Blood Apology", 0, 3f);
					}
				}
				else
				{
					if (!(Character.GetComponent<Animation>()["f02_greet_00"].time < Character.GetComponent<Animation>()["f02_greet_00"].length))
					{
						Character.GetComponent<Animation>().CrossFade("f02_idleShort_00");
					}
					if (!(TalkTimer > 0f))
					{
						TargetStudent.Interaction = 1;
						TargetStudent.TalkTimer = 3f;
						Interaction = 0;
					}
				}
				TalkTimer -= Time.deltaTime;
			}
			else if (Interaction == 4)
			{
				if (TalkTimer == 2f)
				{
					Character.GetComponent<Animation>().CrossFade("f02_greet_00");
					Subtitle.UpdateLabel("Player Farewell", 0, 2f);
				}
				else
				{
					if (!(Character.GetComponent<Animation>()["f02_greet_00"].time < Character.GetComponent<Animation>()["f02_greet_00"].length))
					{
						Character.GetComponent<Animation>().CrossFade("f02_idleShort_00");
					}
					if (!(TalkTimer > 0f))
					{
						TargetStudent.Interaction = 4;
						TargetStudent.TalkTimer = 2f;
						Interaction = 0;
					}
				}
				TalkTimer -= Time.deltaTime;
			}
		}
		if (Attacking)
		{
			targetRotation = Quaternion.LookRotation(new Vector3(TargetStudent.transform.position.x, transform.position.y, TargetStudent.transform.position.z) - transform.position);
			transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
			if (AttackPhase == 1)
			{
				Character.GetComponent<Animation>().CrossFade("f02_stab_00");
				if (!(Character.GetComponent<Animation>()["f02_stab_00"].time <= Character.GetComponent<Animation>()["f02_stab_00"].length * 0.4f))
				{
					Character.GetComponent<Animation>().CrossFade("f02_idleShort_00");
					TargetStudent.BloodSpray.active = true;
					TargetStudent.Dead = true;
					Bloodiness += 20f;
					AttackPhase = 2;
					Sanity -= 20f;
					UpdateSanity();
					UpdateBlood();
				}
			}
			else
			{
				AttackTimer += Time.deltaTime;
				if (!(AttackTimer <= 0.3f))
				{
					Attacking = false;
					AttackPhase = 1;
					AttackTimer = 0f;
					CanMove = true;
				}
			}
		}
		if (!Attacking && !Dragging)
		{
			Character.GetComponent<Animation>()["f02_yanderePose_00"].weight = Mathf.Lerp(Character.GetComponent<Animation>()["f02_yanderePose_00"].weight, 1f - Sanity / 100f, Time.deltaTime * 10f);
			Slouch = Mathf.Lerp(Slouch, 5f * (1f - Sanity / 100f), Time.deltaTime * 10f);
		}
		else
		{
			Character.GetComponent<Animation>()["f02_yanderePose_00"].weight = Mathf.Lerp(Character.GetComponent<Animation>()["f02_yanderePose_00"].weight, 0f, Time.deltaTime * 10f);
			Slouch = Mathf.Lerp(Slouch, 0f, Time.deltaTime * 10f);
		}
		EyeShrink = Mathf.Lerp(EyeShrink, 1f - Sanity / 100f, Time.deltaTime * 10f);
		if (!(Sanity >= 100f))
		{
			TwitchTimer += Time.deltaTime;
			if (!(TwitchTimer <= NextTwitch))
			{
				Twitch.x = (1f - Sanity / 100f) * UnityEngine.Random.Range(-10f, 10f);
				Twitch.y = (1f - Sanity / 100f) * UnityEngine.Random.Range(-10f, 10f);
				Twitch.z = (1f - Sanity / 100f) * UnityEngine.Random.Range(-10f, 10f);
				NextTwitch = UnityEngine.Random.Range(0f, 1f);
				TwitchTimer = 0f;
			}
			Head.localEulerAngles += Twitch;
			Twitch = Vector3.Lerp(Twitch, new Vector3(0f, 0f, 0f), Time.deltaTime * 10f);
		}
		if (NearBodies > 0)
		{
			if (!CorpseWarning)
			{
				NotificationManager.DisplayNotification("Body");
				CorpseWarning = true;
			}
		}
		else
		{
			CorpseWarning = false;
		}
		if (transform.position.y < -1f || Input.GetKeyDown("`"))
		{
			Application.LoadLevel(Application.loadedLevel);
		}
		if (Input.GetKeyDown("escape"))
		{
			Application.Quit();
		}
		if (Input.GetKeyDown("p"))
		{
			if (!Eyepatch.active)
			{
				Eyepatch.active = true;
			}
			else
			{
				Eyepatch.active = false;
			}
		}
		if (Input.GetKeyDown("-"))
		{
			Time.timeScale -= 10f;
		}
		if (Input.GetKeyDown("="))
		{
			Time.timeScale += 10f;
		}
		if (Input.GetKey("."))
		{
			BreastSize += Time.deltaTime;
			if (!(BreastSize <= 2f))
			{
				BreastSize = 2f;
			}
		}
		if (Input.GetKey(","))
		{
			BreastSize -= Time.deltaTime;
			if (!(BreastSize >= 0f))
			{
				BreastSize = 0f;
			}
		}
		if (Input.GetKey("l") && !AoT)
		{
			StudentManager.AttackOnTitan();
			AttackOnTitan();
		}
		if (!(transform.position.z >= -50f))
		{
			int num3 = -50;
			Vector3 position = transform.position;
			float num4 = (position.z = num3);
			Vector3 vector4 = (transform.position = position);
		}
		if (!(GetComponent<Rigidbody>().velocity.y <= 0f))
		{
			int num5 = 0;
			Vector3 velocity = GetComponent<Rigidbody>().velocity;
			float num6 = (velocity.y = num5);
			Vector3 vector6 = (GetComponent<Rigidbody>().velocity = velocity);
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
		for (ID = 0; ID < Spine.Length; ID++)
		{
			float x3 = Spine[ID].transform.localEulerAngles.x + Slouch;
			Vector3 localEulerAngles = Spine[ID].transform.localEulerAngles;
			float num7 = (localEulerAngles.x = x3);
			Vector3 vector14 = (Spine[ID].transform.localEulerAngles = localEulerAngles);
		}
		float z3 = Arm[0].transform.localEulerAngles.z - Slouch * 3f;
		Vector3 localEulerAngles2 = Arm[0].transform.localEulerAngles;
		float num8 = (localEulerAngles2.z = z3);
		Vector3 vector16 = (Arm[0].transform.localEulerAngles = localEulerAngles2);
		float z4 = Arm[1].transform.localEulerAngles.z + Slouch * 3f;
		Vector3 localEulerAngles3 = Arm[1].transform.localEulerAngles;
		float num9 = (localEulerAngles3.z = z4);
		Vector3 vector18 = (Arm[1].transform.localEulerAngles = localEulerAngles3);
		RightBreast.localScale = new Vector3(BreastSize, BreastSize, BreastSize);
		LeftBreast.localScale = new Vector3(BreastSize, BreastSize, BreastSize);
	}

	public virtual void ResetSenpaiEffects()
	{
		DepthOfField.focalSize = 150f;
		DepthOfField.focalZStartCurve = 0f;
		DepthOfField.focalZEndCurve = 0f;
		DepthOfField.enabled = false;
		ColorCorrection.redChannel.MoveKey(1, new Keyframe(0.5f, 0.5f));
		ColorCorrection.greenChannel.MoveKey(1, new Keyframe(0.5f, 0.5f));
		ColorCorrection.blueChannel.MoveKey(1, new Keyframe(0.5f, 0.5f));
		ColorCorrection.redChannel.SmoothTangents(1, 0f);
		ColorCorrection.greenChannel.SmoothTangents(1, 0f);
		ColorCorrection.blueChannel.SmoothTangents(1, 0f);
		ColorCorrection.UpdateTextures();
		ColorCorrection.enabled = false;
		Character.GetComponent<Animation>()["f02_shy_00"].weight = 0f;
		HeartBeat.volume = 0f;
		SenpaiFade = 100f;
	}

	public virtual void ResetYandereEffects()
	{
		YandereColorCorrection.redChannel.MoveKey(1, new Keyframe(0.5f, 0.5f));
		YandereColorCorrection.greenChannel.MoveKey(1, new Keyframe(0.5f, 0.5f));
		YandereColorCorrection.blueChannel.MoveKey(1, new Keyframe(0.5f, 0.5f));
		YandereColorCorrection.redChannel.SmoothTangents(1, 0f);
		YandereColorCorrection.greenChannel.SmoothTangents(1, 0f);
		YandereColorCorrection.blueChannel.SmoothTangents(1, 0f);
		YandereColorCorrection.UpdateTextures();
		YandereColorCorrection.enabled = false;
		Time.timeScale = 1f;
		YandereFade = 100f;
	}

	public virtual void SpawnDumpChan()
	{
		UnityEngine.Object.Destroy(Ragdoll);
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(DumpChan, transform.position, Quaternion.identity);
		gameObject.transform.LookAt(Incinerator.transform.position);
		float y = gameObject.transform.eulerAngles.y + 180f;
		Vector3 eulerAngles = gameObject.transform.eulerAngles;
		float num = (eulerAngles.y = y);
		Vector3 vector2 = (gameObject.transform.eulerAngles = eulerAngles);
	}

	public virtual void Unequip()
	{
	//	Weapon.active = false;
		DpadPressed = true;
		Armed = false;
		StudentManager.UpdateStudents();
		WeaponWarning = false;
	}

	public virtual void UpdateSanity()
	{
		if (!(Sanity <= 100f))
		{
			Sanity = 100f;
		}
		else if (!(Sanity >= 0f))
		{
			Sanity = 0f;
		}
		if (!(Sanity <= 66.66666f))
		{
			HeartRate.SetHeartRateColour(HeartRate.NormalColour);
			SanityWarning = false;
		}
		else if (!(Sanity <= 33.33333f))
		{
			HeartRate.SetHeartRateColour(HeartRate.MediumColour);
			SanityWarning = false;
		}
		else
		{
			HeartRate.SetHeartRateColour(HeartRate.BadColour);
			if (!SanityWarning)
			{
				NotificationManager.DisplayNotification("Insane");
				SanityWarning = true;
			}
		}
		HeartRate.BeatsPerMinute = (int)(240f - Sanity * 1.8f);
	}

	public virtual void UpdateBlood()
	{
		if (!BloodyWarning && !(Bloodiness <= 0f))
		{
			NotificationManager.DisplayNotification("Bloody");
			BloodyWarning = true;
		}
		if (!(Bloodiness <= 100f))
		{
			Bloodiness = 100f;
		}
		if (Bloodiness == 100f)
		{
			MyRenderer.material.mainTexture = UniformTextures[5];
		}
		else if (!(Bloodiness < 80f))
		{
			MyRenderer.material.mainTexture = UniformTextures[4];
		}
		else if (!(Bloodiness < 60f))
		{
			MyRenderer.material.mainTexture = UniformTextures[3];
		}
		else if (!(Bloodiness < 40f))
		{
			MyRenderer.material.mainTexture = UniformTextures[2];
		}
		else if (!(Bloodiness < 20f))
		{
			MyRenderer.material.mainTexture = UniformTextures[1];
		}
		else
		{
			MyRenderer.material.mainTexture = UniformTextures[0];
			BloodyWarning = false;
		}
		Outline.h.ReinitMaterials();
	}

	public virtual void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.name == "BloodPool(Clone)")
		{
			RightFootprintSpawner.Bloodiness = 10;
			LeftFootprintSpawner.Bloodiness = 10;
		}
	}

	public virtual void ChangeCostume()
	{
		if (Costume == 0)
		{
			Costume = 1;
		}
		else
		{
			Costume = 0;
		}
	}

	public virtual void AttackOnTitan()
	{
		MyRenderer.materials[0].mainTexture = TitanTexture;
		AoT = true;
		Outline.h.ReinitMaterials();
	}

	public virtual void Main()
	{
		Character.GetComponent<Animation>()["f02_yanderePose_00"].layer = 1;
		Character.GetComponent<Animation>()["f02_yanderePose_00"].weight = 0f;
		Character.GetComponent<Animation>().Play("f02_yanderePose_00");
		Character.GetComponent<Animation>()["f02_shy_00"].layer = 2;
		Character.GetComponent<Animation>()["f02_shy_00"].weight = 0f;
		Character.GetComponent<Animation>().Play("f02_shy_00");
		Character.GetComponent<Animation>()["f02_fist_00"].layer = 3;
		Character.GetComponent<Animation>()["f02_fist_00"].weight = 0f;
		Character.GetComponent<Animation>().Play("f02_fist_00");
	}
}
