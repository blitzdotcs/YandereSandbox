using System;
using System.Collections;
using System.IO;
using System.Xml;
using Boo.Lang.Runtime;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class taichi_twin_scenescript : MonoBehaviour
{
	public GUISkin guiSkin;

	public taichi_twin_viewscript viewCam;

	public string boneName;

	public Rect camGuiBodyRect;

	public Rect modelBodyRect;

	public Rect textGuiBodyRect;

	public Rect sliderGuiBodyRect;

	public Rect sliderTextBodyRect;

	public Rect guiOnButtonRect;

	public Rect guiOnBGRect;

	private bool guiShowFlg;

	private float guiShowTime;

	private float guiShowAlpha;

	private string segmentCode;

	private string CharacterCode;

	private string FBXListFile;

	private string AnimationListFile;

	private string AnimationListFileAll;

	private string FbxCtrlFile;

	private string ParticleListFile;

	private string ParticleAnimationListFile;

	private string FacialTexListFile;

	private string facialMatName;

	private float curParticle;

	private Vector2 oldMousePosition;

	private float popupWaitingTime;

	private float popupWaitingTimeNow;

	private bool guiOn;

	private float initPosX;

	private bool autoResourceMode;

	private string settingFileDir;

	private int curBG;

	private int curAnim;

	private int curModel;

	private int curCharacter;

	private int curFacial;

	private bool animReplay;

	private bool playOnceFlg;

	private string resourcesPathFull;

	private string resourcesPath;

	private float animSpeed;

	private float motionDelay;

	private float curLOD;

	private string curModelName;

	private string curAnimName;

	private string curBgName;

	private string curFacialName;

	private string curParticleName;

	private string curCharacterName;

	private int facialCount;

	private float positionY;

	private string animationPath;

	private string[] animationList;

	private string[] animationListAll;

	private string[] animationNameList;

	private string[] modelList;

	private string[] modelNameList;

	private string[] facialTexList;

	private string[] particleAnimationList;

	private string[] particleList;

	private float animSpeedSet;

	private string[] backGroundList;

	private string[] stageTexList;

	private string[] lodList;

	private string[] lodTextList;

	private string[] modeTextList;

	private GameObject obj;

	private GameObject loaded;

	private SkinnedMeshRenderer SM;

	private SkinnedMeshRenderer faceSM;

	private string faceObjName;

	private TextAsset txt;

	private bool CamModeRote;

	private bool CamModeMove;

	private bool CamModeZoom;

	private bool CamModeFix;

	private int CamMode;

	private XmlDocument xDoc;

	private XmlNodeList xNodeList;

	private float nowTime;

	private bool playFlg;

	private Material faceMat_L;

	private Material faceMat_M;

	private GameObject BGObject;

	private GameObject BGEff;

	private GameObject BGPlane;

	private Hashtable functionList;

	private GameObject planeObj;

	private bool animationPlayFlgOld;

	private int onSliderFlg;

	private Vector3 scale;

	public taichi_twin_scenescript()
	{
		boneName = "Hips";
		camGuiBodyRect = new Rect(870f, 25f, 93f, 420f);
		modelBodyRect = new Rect(20f, 40f, 300f, 500f);
		textGuiBodyRect = new Rect(20f, 510f, 300f, 70f);
		sliderGuiBodyRect = new Rect(770f, 520f, 170f, 150f);
		sliderTextBodyRect = new Rect(770f, 520f, 170f, 150f);
		guiOnButtonRect = new Rect(20f, 40f, 300f, 500f);
		guiOnBGRect = new Rect(20f, 40f, 300f, 500f);
		guiShowAlpha = 1f;
		segmentCode = "_A";
		CharacterCode = "M01/";
		FBXListFile = "fbx_list";
		AnimationListFile = "animation_list";
		AnimationListFileAll = "animation_list_all";
		FbxCtrlFile = "fbx_ctrl";
		ParticleListFile = "ParticleList";
		ParticleAnimationListFile = "ParticleAnimationList";
		FacialTexListFile = "facial_texture_list";
		facialMatName = "chr01_F03_face";
		curParticle = 1f;
		popupWaitingTime = 2f;
		guiOn = true;
		initPosX = 0.3f;
		autoResourceMode = true;
		settingFileDir = "Taichi/TwinViewer Settings/";
		curFacial = 1;
		animReplay = true;
		playOnceFlg = true;
		resourcesPathFull = "Assets/Taichi Character Pack/Resources/Taichi";
		resourcesPath = string.Empty;
		animSpeed = 1f;
		curModelName = string.Empty;
		curAnimName = string.Empty;
		curBgName = string.Empty;
		curFacialName = string.Empty;
		curParticleName = string.Empty;
		curCharacterName = string.Empty;
		lodList = new string[3] { "_h", "_m", "_l" };
		lodTextList = new string[3] { "Hi", "Mid", "Low" };
		modeTextList = new string[2] { "AddPerticle", "Original" };
		CamModeRote = true;
		CamModeFix = true;
		playFlg = true;
		functionList = new Hashtable();
	}

	public virtual void Start()
	{
		viewCam = (taichi_twin_viewscript)GameObject.Find("Main Camera").GetComponent("taichi_twin_viewscript");
		nowTime += Time.deltaTime;
		planeObj = GameObject.Find("Plane") as GameObject;
		txt = (TextAsset)Resources.Load(settingFileDir + "background_list", typeof(TextAsset));
		backGroundList = txt.text.Split(new string[2] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
		txt = (TextAsset)Resources.Load(settingFileDir + "stage_texture_list", typeof(TextAsset));
		stageTexList = txt.text.Split(new string[2] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
		SetSettings(0);
		SetInitBackGround();
		SetInitModel();
		SetInitMotion();
		SetAnimationSpeed(animSpeed);
		float x = initPosX;
		Vector3 position = obj.transform.position;
		float num = (position.x = x);
		Vector3 vector2 = (obj.transform.position = position);
	}

	public virtual void Update()
	{
		if (!obj.GetComponent<Animation>().IsPlaying(curAnimName) && animationPlayFlgOld)
		{
			nowTime = 0f;
			playFlg = true;
		}
		animationPlayFlgOld = obj.GetComponent<Animation>().IsPlaying(curAnimName);
		nowTime += Time.deltaTime;
		if (!(nowTime <= motionDelay))
		{
			SetAnimationSpeed(animSpeedSet);
			playAnimation();
		}
		else
		{
			SetAnimationSpeed(0f);
			playAnimation();
		}
		if (Input.GetKeyDown("1"))
		{
			SetNextModel(-1);
		}
		if (Input.GetKeyDown("2"))
		{
			SetNextModel(1);
		}
		if (Input.GetKeyDown("q"))
		{
			SetNextMotion(-1);
		}
		if (Input.GetKeyDown("w"))
		{
			SetNextMotion(1);
		}
		if (Input.GetKeyDown("a"))
		{
			SetNextBackGround(-1);
		}
		if (Input.GetKeyDown("s"))
		{
			SetNextBackGround(1);
		}
		if (Input.GetKeyDown("z"))
		{
			SetNextLOD(-1);
		}
		if (Input.GetKeyDown("x"))
		{
			SetNextLOD(1);
		}
	}

	public virtual void popUp()
	{
		if (Input.GetMouseButton(0))
		{
			popupWaitingTimeNow = 0f;
		}
		if (oldMousePosition == (Vector2)Input.mousePosition)
		{
			popupWaitingTimeNow += Time.deltaTime;
		}
		else
		{
			popupWaitingTimeNow = 0f;
		}
		oldMousePosition = Input.mousePosition;
		if (!(popupWaitingTime <= popupWaitingTimeNow))
		{
			return;
		}
		float num = Screen.width;
		float num2 = Screen.height;
		Vector2[] array = null;
		Vector2[] array2 = null;
		Rect[] array3 = null;
		string[] array4 = null;
		int num3 = 17;
		array = new Vector2[num3];
		array2 = new Vector2[num3];
		array3 = new Rect[num3];
		array4 = new string[num3];
		float num4 = num2 - 10f;
		float num5 = 50f;
		float num6 = 100f;
		float num7 = 9.5f;
		float num8 = default(float);
		float num9 = default(float);
		float num10 = default(float);
		float num11 = default(float);
		if (guiOn)
		{
			num11 = num4;
			num9 = num4 - num5;
			num8 = 10f;
			num10 = num6 + num8;
			array[0] = new Vector2(num8, num9);
			array2[0] = new Vector2(num10, num11);
			array3[0] = new Rect(123f, 10f, 120f, 23f);
			array4[0] = "Character Change.";
			num11 = num4 - num5 - num7;
			num9 = num4 - num5 * 2f - num7;
			array[1] = new Vector2(num8, num9);
			array2[1] = new Vector2(num10, num11);
			array3[1] = new Rect(123f, 69f, 120f, 23f);
			array4[1] = "Model Change.";
			num11 = num4 - num5 * 2f - num7 * 2f;
			num9 = num4 - num5 * 3f - num7 * 2f;
			array[2] = new Vector2(num8, num9);
			array2[2] = new Vector2(num10, num11);
			array3[2] = new Rect(123f, 129f, 120f, 40f);
			array4[2] = "Motion Change.\nRepeat Setting.";
			num11 = num4 - num5 * 3f - num7 * 3f;
			num9 = num4 - num5 * 4f - num7 * 3f;
			array[3] = new Vector2(num8, num9);
			array2[3] = new Vector2(num10, num11);
			array3[3] = new Rect(123f, 190f, 120f, 23f);
			array4[3] = "Facial Change.";
			num11 = num4 - num5 * 4f - num7 * 4f;
			num9 = num4 - num5 * 5f - num7 * 4f;
			array[4] = new Vector2(num8, num9);
			array2[4] = new Vector2(120f, num11);
			array3[4] = new Rect(123f, 250f, 120f, 23f);
			array4[4] = "Lod Change.";
			float num12 = 245f;
			float num13 = default(float);
			num13 = (float)Screen.width - num12;
			num11 = num4;
			num9 = num4 - num5;
			num10 = num - 10f;
			num8 = num - 10f - num6;
			array[5] = new Vector2(num8, num9);
			array2[5] = new Vector2(num10, num11);
			array3[5] = new Rect(num13, 10f, 120f, 23f);
			array4[5] = "Character Change.";
			num11 = num4 - num5 - num7;
			num9 = num4 - num5 * 2f - num7;
			array[6] = new Vector2(num8, num9);
			array2[6] = new Vector2(num10, num11);
			array3[6] = new Rect(num13, 69f, 120f, 23f);
			array4[6] = "Model Change.";
			num11 = num4 - num5 * 2f - num7 * 2f;
			num9 = num4 - num5 * 3f - num7 * 2f;
			array[7] = new Vector2(num8, num9);
			array2[7] = new Vector2(num10, num11);
			array3[7] = new Rect(num13, 129f, 120f, 40f);
			array4[7] = "Motion Change.\nRepeat Setting.";
			num11 = num4 - num5 * 3f - num7 * 3f;
			num9 = num4 - num5 * 4f - num7 * 3f;
			array[8] = new Vector2(num8, num9);
			array2[8] = new Vector2(num10, num11);
			array3[8] = new Rect(num13, 190f, 120f, 23f);
			array4[8] = "Facial Change.";
			num11 = num4 - num5 * 4f - num7 * 4f;
			num9 = num4 - num5 * 5f - num7 * 4f;
			array[9] = new Vector2(num8, num9);
			array2[9] = new Vector2(num10, num11);
			array3[9] = new Rect(num13, 250f, 120f, 23f);
			array4[9] = "Lod Change.";
		}
		if (guiShowFlg)
		{
			num11 = num2 - 50f;
			num9 = num11 - 50f;
			float x = camGuiBodyRect.x;
			num6 = 50f;
			num7 = 8f;
			num10 = x + num6;
			num8 = x;
			array[10] = new Vector2(num8, num9);
			array2[10] = new Vector2(num10, num11);
			array3[10] = new Rect(num8, 25f, 150f, 20f);
			array4[10] = "Camera Rotate.";
			num10 += num6 + num7;
			num8 += num6 + num7;
			array[11] = new Vector2(num8, num9);
			array2[11] = new Vector2(num10, num11);
			array3[11] = new Rect(num8, 25f, 150f, 20f);
			array4[11] = "Camera Move.";
			num10 += num6 + num7;
			num8 += num6 + num7;
			array[12] = new Vector2(num8, num9);
			array2[12] = new Vector2(num10, num11);
			array3[12] = new Rect(num8, 25f, 150f, 20f);
			array4[12] = "Camera Zoom.";
			num10 += num6 + num7;
			num8 += num6 + num7;
			array[13] = new Vector2(num8, num9);
			array2[13] = new Vector2(num10, num11);
			array3[13] = new Rect(num8 - 100f, 25f, 150f, 20f);
			array4[13] = "Camera Target Lock.";
			num10 += num6 + num7;
			num8 += num6 + num7;
			array[14] = new Vector2(num8, num9);
			array2[14] = new Vector2(num10, num11);
			array3[14] = new Rect(num8 - 100f, 25f, 150f, 20f);
			array4[14] = "Camera Reset.";
			num8 = guiOnButtonRect.x;
			num10 = num8 + 100f;
			array[15] = new Vector2(num8, num9);
			array2[15] = new Vector2(num10, num11);
			array3[15] = new Rect(num8 + 50f - 73f, 25f, 150f, 20f);
			array4[15] = "BackGround:" + (curBG + 1) + "/" + Extensions.get_length((System.Array)backGroundList) + ":" + curBgName + "\n";
			num8 = guiOnButtonRect.x + num6 * 2f + 70f;
			num10 = num8 + num6;
			array[16] = new Vector2(num8, num9);
			array2[16] = new Vector2(num10, num11);
			array3[16] = new Rect(num8 + num6 / 2f - 50f, 25f, 100f, 20f);
			if (guiOn)
			{
				array4[16] = "Viewer UI:On";
			}
			else
			{
				array4[16] = "Viewer UI:Off";
			}
		}
		for (int i = 0; i < num3; i++)
		{
			if (!(Input.mousePosition.x <= array[i].x) && !(Input.mousePosition.x >= array2[i].x) && !(Input.mousePosition.y <= array[i].y) && !(Input.mousePosition.y >= array2[i].y))
			{
				GUI.Box(array3[i], array4[i]);
			}
		}
	}

	public virtual void scrollBarPos()
	{
		Vector2[] array = null;
		Vector2[] array2 = null;
		int num = 10;
		onSliderFlg = 0;
		array = new Vector2[num];
		array2 = new Vector2[num];
		float num2 = default(float);
		float num3 = default(float);
		float num4 = default(float);
		float num5 = default(float);
		num2 = 10f;
		num3 = 270f;
		array[0] = new Vector2(num2, 267f);
		array2[0] = new Vector2(num3, 295f);
		array[1] = new Vector2(num2, 235f);
		array2[1] = new Vector2(num3, 267f);
		array[2] = new Vector2(num2, 205f);
		array2[2] = new Vector2(num3, 235f);
		array[3] = new Vector2(num2, 175f);
		array2[3] = new Vector2(num3, 205f);
		array[4] = new Vector2(num2, 130f);
		array2[4] = new Vector2(num3, 175f);
		array[5] = new Vector2(num2, 85f);
		array2[5] = new Vector2(num3, 130f);
		for (int i = 0; i < num; i++)
		{
			if (!(Input.mousePosition.x <= array[i].x) && !(Input.mousePosition.x >= array2[i].x) && !(Input.mousePosition.y <= array[i].y) && !(Input.mousePosition.y >= array2[i].y))
			{
				onSliderFlg = i + 1;
			}
		}
	}

	public virtual void OnGUI()
	{
		float num = Screen.width;
		float num2 = Screen.height;
		float num3 = 40f;
		float num4 = 20f;
		float num5 = (float)(Screen.width / 2) - num3 / 2f;
		float num6 = num5 + num3;
		float num7 = (float)Screen.height - num4;
		float num8 = Screen.height;
		guiOnBGRect.x = num / 2f - guiOnBGRect.width / 2f;
		guiOnButtonRect.x = num / 2f + 40f;
		camGuiBodyRect.x = num / 2f - guiOnBGRect.width / 2f + 20f;
		if (!(Input.mousePosition.x <= num5) && !(Input.mousePosition.x >= num6) && !(Input.mousePosition.y <= num7) && !(Input.mousePosition.y >= num8))
		{
			guiShowFlg = true;
			guiShowAlpha = 1f;
			guiShowTime = 0f;
		}
		float num9 = (float)Screen.height - guiOnBGRect.height + num4;
		float num10 = (float)(Screen.width / 2) - guiOnBGRect.width / 2f;
		float num11 = (float)(Screen.width / 2) + guiOnBGRect.width / 2f;
		if (Input.mousePosition.x < num10 || Input.mousePosition.x > num11 || !(Input.mousePosition.y >= num9))
		{
			guiShowFlg = false;
		}
		if (!guiShowFlg)
		{
			guiShowTime += Time.deltaTime;
		}
		if (!guiShowFlg && !(guiShowTime <= 5f) && !(guiShowAlpha <= 0.5f))
		{
			guiShowAlpha -= Time.deltaTime * 0.5f;
		}
		float a = guiShowAlpha;
		Color color = GUI.color;
		float num12 = (color.a = a);
		Color color3 = (GUI.color = color);
		taichi_twin2nd_scenescript taichi_twin2nd_scenescript2 = (taichi_twin2nd_scenescript)gameObject.GetComponent("taichi_twin2nd_scenescript");
		if ((bool)guiSkin)
		{
			GUI.skin = guiSkin;
		}
		float num13 = 200f;
		if (guiShowFlg)
		{
			if (!(guiOnBGRect.y >= -30f))
			{
				guiOnBGRect.y += Time.deltaTime * num13;
				guiOnButtonRect.y += Time.deltaTime * num13;
				camGuiBodyRect.y += Time.deltaTime * num13;
			}
			else
			{
				guiOnBGRect.y = -30f;
				guiOnButtonRect.y = 50f;
				camGuiBodyRect.y = 50f;
			}
		}
		else if (!(guiOnBGRect.y <= -158f))
		{
			guiOnBGRect.y -= Time.deltaTime * num13;
			guiOnButtonRect.y -= Time.deltaTime * num13;
			camGuiBodyRect.y -= Time.deltaTime * num13;
		}
		else
		{
			guiOnBGRect.y = -158f;
			guiOnButtonRect.y = -80f;
			camGuiBodyRect.y = -80f;
		}
		GUI.Label(guiOnBGRect, string.Empty, "GUIOnBG");
		GUILayout.BeginArea(guiOnButtonRect);
		GUILayout.BeginHorizontal();
		if (GUILayout.Button(string.Empty, "Left"))
		{
			SetNextBackGround(-1);
		}
		GUILayout.Label(string.Empty, "BG");
		if (GUILayout.Button(string.Empty, "Right"))
		{
			SetNextBackGround(1);
		}
		GUILayout.FlexibleSpace();
		bool flag = GUILayout.Toggle(guiOn, string.Empty, "GUIOn");
		if (guiOn != flag)
		{
			guiOn = flag;
			taichi_twin2nd_scenescript2.guiOn = flag;
		}
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
		float a2 = 1f;
		Color color4 = GUI.color;
		float num14 = (color4.a = a2);
		Color color6 = (GUI.color = color4);
		GUILayout.BeginArea(camGuiBodyRect);
		GUILayout.BeginHorizontal();
		bool flag2 = default(bool);
		flag2 = GUILayout.Toggle(CamMode == 0, string.Empty, "Rote");
		if (CamMode != 0 && flag2)
		{
			CamMode = 0;
			viewCam.ModeRote();
		}
		GUILayout.FlexibleSpace();
		flag2 = GUILayout.Toggle(CamMode == 1, string.Empty, "Move");
		if (CamMode != 1 && flag2)
		{
			CamMode = 1;
			viewCam.ModeMove();
		}
		GUILayout.FlexibleSpace();
		flag2 = GUILayout.Toggle(CamMode == 2, string.Empty, "Zoom");
		if (CamMode != 2 && flag2)
		{
			CamMode = 2;
			viewCam.ModeZoom();
		}
		GUILayout.FlexibleSpace();
		CamModeFix = viewCam.isFixTarget;
		flag2 = GUILayout.Toggle(CamModeFix, string.Empty, "Fix");
		if (CamModeFix != flag2)
		{
			CamModeFix = flag2;
			viewCam.FixTarget(CamModeFix);
		}
		GUILayout.FlexibleSpace();
		if (GUILayout.Button(string.Empty, "Reset"))
		{
			viewCam.ModelTarget(GetBone(obj, boneName));
			viewCam.Reset();
		}
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
		if (!guiOn)
		{
			popUp();
			return;
		}
		scale.x = 1f;
		scale.y = 1f;
		scale.z = 1f;
		GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, scale);
		textGuiBodyRect.y = num2 - (textGuiBodyRect.height + 10f);
		sliderGuiBodyRect.y = num2 - (sliderGuiBodyRect.height + textGuiBodyRect.height + 28f);
		sliderTextBodyRect.y = num2 - (sliderTextBodyRect.height + textGuiBodyRect.height + 15f);
		scrollBarPos();
		if (GUI.Button(new Rect(Screen.width / 2 - 150, Screen.height - 80, 300f, 30f), "MotionSyncro") || Input.GetKeyDown("space"))
		{
			timerReset();
			taichi_twin2nd_scenescript2.timerReset();
		}
		if (GUI.Button(new Rect(Screen.width / 2 - 150, Screen.height - 40, 300f, 30f), "Reset"))
		{
			int num15 = 0;
			Vector3 eulerAngles = obj.transform.eulerAngles;
			float num16 = (eulerAngles.y = num15);
			Vector3 vector2 = (obj.transform.eulerAngles = eulerAngles);
			float x = 0.3f;
			Vector3 position = obj.transform.position;
			float num17 = (position.x = x);
			Vector3 vector4 = (obj.transform.position = position);
			int num18 = 0;
			Vector3 position2 = obj.transform.position;
			float num19 = (position2.y = num18);
			Vector3 vector6 = (obj.transform.position = position2);
			int num20 = 0;
			Vector3 position3 = obj.transform.position;
			float num21 = (position3.z = num20);
			Vector3 vector8 = (obj.transform.position = position3);
			animSpeed = 1f;
			motionDelay = 0f;
			SetAnimationSpeed(animSpeed);
			taichi_twin2nd_scenescript2.sliderReset();
		}
		float pixels = 8f;
		GUILayout.BeginArea(modelBodyRect);
		GUILayout.BeginVertical();
		GUILayout.BeginHorizontal();
		if (GUILayout.Button(string.Empty, "Left"))
		{
			SetNextCharacter(-1);
		}
		GUILayout.Label(string.Empty, "Chara");
		if (GUILayout.Button(string.Empty, "Right"))
		{
			SetNextCharacter(1);
		}
		GUILayout.EndHorizontal();
		GUILayout.Space(pixels);
		if (RuntimeServices.ToBool(functionList["model"]))
		{
			GUILayout.BeginHorizontal();
			if (GUILayout.Button(string.Empty, "Left"))
			{
				SetNextModel(-1);
			}
			GUILayout.Label(string.Empty, "Costume");
			if (GUILayout.Button(string.Empty, "Right"))
			{
				SetNextModel(1);
			}
			GUILayout.EndHorizontal();
			GUILayout.Space(pixels);
		}
		else
		{
			float a3 = 0.5f;
			Color color7 = GUI.color;
			float num22 = (color7.a = a3);
			Color color9 = (GUI.color = color7);
			GUILayout.BeginHorizontal();
			if (GUILayout.Button(string.Empty, "LeftGrayOut"))
			{
			}
			GUILayout.Label(string.Empty, "Costume");
			if (GUILayout.Button(string.Empty, "RightGrayOut"))
			{
			}
			GUILayout.EndHorizontal();
			GUILayout.Space(pixels);
			float a4 = 1f;
			Color color10 = GUI.color;
			float num23 = (color10.a = a4);
			Color color12 = (GUI.color = color10);
		}
		GUILayout.BeginHorizontal();
		if (GUILayout.Button(string.Empty, "Left"))
		{
			SetNextMotion(-1);
		}
		bool flag3 = GUILayout.Toggle(animReplay, string.Empty, "AnimReplay");
		if (animReplay != flag3)
		{
			animReplay = flag3;
		}
		if (animReplay)
		{
			playOnceFlg = true;
		}
		if (GUILayout.Button(string.Empty, "Right"))
		{
			SetNextMotion(1);
		}
		GUILayout.EndHorizontal();
		GUILayout.Space(pixels);
		if (RuntimeServices.ToBool(functionList["facial"]))
		{
			GUILayout.BeginHorizontal();
			if (GUILayout.Button(string.Empty, "Left"))
			{
				SetNextFacial(-1);
			}
			GUILayout.Label(string.Empty, "Facial");
			if (GUILayout.Button(string.Empty, "Right"))
			{
				SetNextFacial(1);
			}
			GUILayout.EndHorizontal();
			GUILayout.Space(pixels);
		}
		else
		{
			float a5 = 0.5f;
			Color color13 = GUI.color;
			float num24 = (color13.a = a5);
			Color color15 = (GUI.color = color13);
			GUILayout.BeginHorizontal();
			if (GUILayout.Button(string.Empty, "LeftGrayOut"))
			{
			}
			GUILayout.Label(string.Empty, "Facial");
			if (GUILayout.Button(string.Empty, "RightGrayOut"))
			{
			}
			GUILayout.EndHorizontal();
			GUILayout.Space(pixels);
			float a6 = 1f;
			Color color16 = GUI.color;
			float num25 = (color16.a = a6);
			Color color18 = (GUI.color = color16);
		}
		if (RuntimeServices.ToBool(functionList["lod"]))
		{
			GUILayout.BeginHorizontal();
			if (GUILayout.Button(string.Empty, "Left"))
			{
				SetNextLOD(-1);
			}
			GUILayout.Label(string.Empty, "LOD");
			if (GUILayout.Button(string.Empty, "Right"))
			{
				SetNextLOD(1);
			}
			GUILayout.EndHorizontal();
			GUILayout.FlexibleSpace();
		}
		else
		{
			float a7 = 0.5f;
			Color color19 = GUI.color;
			float num26 = (color19.a = a7);
			Color color21 = (GUI.color = color19);
			GUILayout.BeginHorizontal();
			if (GUILayout.Button(string.Empty, "LeftGrayOut"))
			{
			}
			GUILayout.Label(string.Empty, "LOD");
			if (GUILayout.Button(string.Empty, "RightGrayOut"))
			{
			}
			GUILayout.EndHorizontal();
			GUILayout.FlexibleSpace();
			float a8 = 1f;
			Color color22 = GUI.color;
			float num27 = (color22.a = a8);
			Color color24 = (GUI.color = color22);
		}
		GUILayout.EndVertical();
		GUILayout.EndArea();
		GUILayout.BeginArea(sliderTextBodyRect);
		GUILayout.FlexibleSpace();
		string text = "Position X : " + string.Format("{0:F1}", obj.transform.position.x);
		GUILayout.Box(text);
		GUILayout.FlexibleSpace();
		string text2 = "Position Y : " + string.Format("{0:F1}", obj.transform.position.y);
		GUILayout.Box(text2);
		GUILayout.FlexibleSpace();
		string text3 = "Position Z : " + string.Format("{0:F1}", obj.transform.position.z);
		GUILayout.Box(text3);
		GUILayout.FlexibleSpace();
		string text4 = "Rotate : " + string.Format("{0:F1}", obj.transform.eulerAngles.y);
		GUILayout.Box(text4);
		GUILayout.FlexibleSpace();
		string text5 = "Animation\nSpeed : " + string.Format("{0:F1}", animSpeed);
		GUILayout.Box(text5);
		GUILayout.FlexibleSpace();
		string text6 = "Motion\nDelay : " + string.Format("{0:F1}", motionDelay);
		GUILayout.Box(text6);
		GUILayout.FlexibleSpace();
		GUILayout.EndArea();
		GUILayout.BeginArea(sliderGuiBodyRect);
		if (onSliderFlg == 1)
		{
			float a9 = 1f;
			Color color25 = GUI.color;
			float num28 = (color25.a = a9);
			Color color27 = (GUI.color = color25);
		}
		else
		{
			float a10 = 0.4f;
			Color color28 = GUI.color;
			float num29 = (color28.a = a10);
			Color color30 = (GUI.color = color28);
		}
		float num30 = GUILayout.HorizontalSlider(obj.transform.position.x, 1f, -1f);
		if (obj.transform.position.x != num30)
		{
			float x2 = num30;
			Vector3 position4 = obj.transform.position;
			float num31 = (position4.x = x2);
			Vector3 vector10 = (obj.transform.position = position4);
			viewCam.MouseLock(true);
		}
		else
		{
			viewCam.MouseLock(false);
		}
		GUILayout.Space(0f);
		if (onSliderFlg == 2)
		{
			float a11 = 1f;
			Color color31 = GUI.color;
			float num32 = (color31.a = a11);
			Color color33 = (GUI.color = color31);
		}
		else
		{
			float a12 = 0.4f;
			Color color34 = GUI.color;
			float num33 = (color34.a = a12);
			Color color36 = (GUI.color = color34);
		}
		float num34 = GUILayout.HorizontalSlider(obj.transform.position.y, 0f, 3f);
		if (obj.transform.position.y != num34)
		{
			float y = num34;
			Vector3 position5 = obj.transform.position;
			float num35 = (position5.y = y);
			Vector3 vector12 = (obj.transform.position = position5);
			viewCam.MouseLock(true);
		}
		else
		{
			viewCam.MouseLock(false);
		}
		GUILayout.Space(0f);
		if (onSliderFlg == 3)
		{
			float a13 = 1f;
			Color color37 = GUI.color;
			float num36 = (color37.a = a13);
			Color color39 = (GUI.color = color37);
		}
		else
		{
			float a14 = 0.4f;
			Color color40 = GUI.color;
			float num37 = (color40.a = a14);
			Color color42 = (GUI.color = color40);
		}
		float num38 = GUILayout.HorizontalSlider(obj.transform.position.z, 1f, -1f);
		if (obj.transform.position.z != num38)
		{
			float z = num38;
			Vector3 position6 = obj.transform.position;
			float num39 = (position6.z = z);
			Vector3 vector14 = (obj.transform.position = position6);
			viewCam.MouseLock(true);
		}
		else
		{
			viewCam.MouseLock(false);
		}
		GUILayout.Space(-3f);
		if (onSliderFlg == 4)
		{
			float a15 = 1f;
			Color color43 = GUI.color;
			float num40 = (color43.a = a15);
			Color color45 = (GUI.color = color43);
		}
		else
		{
			float a16 = 0.4f;
			Color color46 = GUI.color;
			float num41 = (color46.a = a16);
			Color color48 = (GUI.color = color46);
		}
		float num42 = GUILayout.HorizontalSlider(obj.transform.eulerAngles.y, 0f, 359.9f);
		if (obj.transform.eulerAngles.y != num42)
		{
			float y2 = num42;
			Vector3 eulerAngles2 = obj.transform.eulerAngles;
			float num43 = (eulerAngles2.y = y2);
			Vector3 vector16 = (obj.transform.eulerAngles = eulerAngles2);
			viewCam.MouseLock(true);
		}
		else
		{
			viewCam.MouseLock(false);
		}
		GUILayout.Space(6f);
		if (onSliderFlg == 5)
		{
			float a17 = 1f;
			Color color49 = GUI.color;
			float num44 = (color49.a = a17);
			Color color51 = (GUI.color = color49);
		}
		else
		{
			float a18 = 0.4f;
			Color color52 = GUI.color;
			float num45 = (color52.a = a18);
			Color color54 = (GUI.color = color52);
		}
		animSpeedSet = GUILayout.HorizontalSlider(animSpeed, 0f, 2f);
		if (animSpeed != animSpeedSet)
		{
			animSpeed = animSpeedSet;
			SetAnimationSpeed(animSpeed);
			viewCam.MouseLock(true);
		}
		else
		{
			viewCam.MouseLock(false);
		}
		GUILayout.Space(13f);
		if (onSliderFlg == 6)
		{
			float a19 = 1f;
			Color color55 = GUI.color;
			float num46 = (color55.a = a19);
			Color color57 = (GUI.color = color55);
		}
		else
		{
			float a20 = 0.4f;
			Color color58 = GUI.color;
			float num47 = (color58.a = a20);
			Color color60 = (GUI.color = color58);
		}
		float num48 = GUILayout.HorizontalSlider(motionDelay, 0f, 5f);
		if (motionDelay != num48)
		{
			motionDelay = num48;
			viewCam.MouseLock(true);
		}
		else
		{
			viewCam.MouseLock(false);
		}
		GUILayout.EndArea();
		float a21 = 1f;
		Color color61 = GUI.color;
		float num49 = (color61.a = a21);
		Color color63 = (GUI.color = color61);
		string empty = string.Empty;
		empty += "Character : " + curCharacterName + "\n";
		if (RuntimeServices.ToBool(functionList["model"]))
		{
			empty += "Costume : " + (curModel + 1) + " / " + Extensions.get_length((System.Array)modelList) + " : " + curModelName + "\n";
		}
		empty += "Animation : " + (curAnim + 1) + " / " + Extensions.get_length((System.Array)animationList) + " : " + curAnimName + "\n";
		if (RuntimeServices.ToBool(functionList["facial"]))
		{
			empty += "Facial : " + (curFacial + 1) + " / " + facialCount + " : " + curFacialName + "\n";
		}
		if (RuntimeServices.ToBool(functionList["lod"]))
		{
			empty += "Quality : " + lodTextList[(int)curLOD] + "\n";
		}
		GUI.Box(textGuiBodyRect, empty);
		popUp();
	}

	public virtual void SetInit()
	{
		SetInitModel();
		SetInitMotion();
		SetAnimationSpeed(animSpeed);
		if (RuntimeServices.ToBool(functionList["facial"]))
		{
			SetInitFacial();
		}
	}

	public virtual void timerReset()
	{
		nowTime = 0f;
		obj.GetComponent<Animation>().Stop();
		playOnceFlg = true;
	}

	public virtual void SetSettings(int _i)
	{
		string text = null;
		string text2 = null;
		string path = null;
		curAnim = 0;
		curModel = 0;
		curLOD = 0f;
		switch (_i)
		{
		case 0:
			resourcesPathFull = "Assets/Taichi Character Pack/Resources/Taichi";
			resourcesPath = "Taichi/";
			animationPath = "Taichi/Animations Legacy/m01@";
			CharacterCode = "M01/";
			AnimationListFile = "animation_list";
			AnimationListFileAll = "animation_list";
			path = "Taichi/TwinViewer Settings/" + CharacterCode + "fbx_ctrl";
			faceMat_L = (Material)Resources.Load("Taichi/Materials/m01_face_00_l", typeof(Material));
			faceMat_M = (Material)Resources.Load("Taichi/Materials/m01_face_00_m", typeof(Material));
			functionList["model"] = true;
			functionList["facial"] = false;
			functionList["lod"] = true;
			curCharacterName = "Taichi Hayami";
			break;
		case 1:
			resourcesPathFull = "Assets/Taichi Character Pack/Resources/Puppet";
			resourcesPath = "Puppet/";
			animationPath = "Taichi/Animations Legacy/m01@";
			AnimationListFile = "animation_list";
			AnimationListFileAll = "animation_list";
			CharacterCode = "Puppet/";
			path = settingFileDir + CharacterCode + "fbx_ctrl";
			functionList["model"] = false;
			functionList["facial"] = false;
			functionList["lod"] = false;
			curCharacterName = "Puppet";
			break;
		case 2:
			resourcesPathFull = "Assets/HonokaFutabaBasicSet/Resources/Honoka";
			resourcesPath = "Honoka/";
			animationPath = "Honoka/Animations Legacy/f01@";
			AnimationListFile = "animation_list";
			AnimationListFileAll = "animation_list_all";
			CharacterCode = "F01/";
			path = settingFileDir + CharacterCode + "fbx_ctrl";
			functionList["model"] = true;
			functionList["facial"] = false;
			functionList["lod"] = true;
			faceMat_L = (Material)Resources.Load("Honoka/Materials/f01_face_00_l", typeof(Material));
			faceMat_M = (Material)Resources.Load("Honoka/Materials/f01_face_00_m", typeof(Material));
			curCharacterName = "Honoka Futaba";
			break;
		case 3:
			resourcesPathFull = "Assets/Aoi Character Pack/Resources/Aoi";
			resourcesPath = "Aoi/";
			animationPath = "Aoi/Animations Legacy/f02@";
			AnimationListFile = "animation_list";
			AnimationListFileAll = "animation_list_all";
			CharacterCode = "F02/";
			path = settingFileDir + CharacterCode + "fbx_ctrl";
			functionList["model"] = true;
			functionList["facial"] = false;
			functionList["lod"] = true;
			faceMat_L = (Material)Resources.Load("Aoi/Materials/f02_face_00_l", typeof(Material));
			faceMat_M = (Material)Resources.Load("Aoi/Materials/f02_face_00_m", typeof(Material));
			curCharacterName = "Aoi Kiryu";
			break;
		case 4:
			resourcesPathFull = "Assets/Succubus Twins Character Pack Ver1.10/Resources/Arum_v0111/";
			resourcesPath = "Arum_v0111/";
			animationPath = "Arum_v0111/Animations Legacy/animation@";
			AnimationListFile = "animation_list_a";
			AnimationListFileAll = "animation_list_a";
			FacialTexListFile = settingFileDir + CharacterCode + "facial_texture_list_a";
			CharacterCode = "F03/Arum/";
			path = settingFileDir + CharacterCode + "fbx_ctrl";
			functionList["model"] = false;
			functionList["facial"] = true;
			functionList["lod"] = true;
			txt = (TextAsset)Resources.Load(FacialTexListFile, typeof(TextAsset));
			facialTexList = txt.text.Split(new string[2] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
			faceObjName = "succubus_a";
			facialMatName = "succubus_a_face";
			faceMat_L = (Material)Resources.Load("Arum_v0111/Materials/succubus_a_face_l", typeof(Material));
			faceMat_M = (Material)Resources.Load("Arum_v0111/Materials/succubus_a_face_m", typeof(Material));
			FacialTexListFile = settingFileDir + CharacterCode + "facial_texture_list_a";
			curCharacterName = "Succubus Arum";
			break;
		case 5:
			resourcesPathFull = "Assets/Succubus Twins Character Pack Ver1.10/Resources/Asphodel_v0111/";
			resourcesPath = "Asphodel_v0111/";
			animationPath = "Asphodel_v0111/Animations Legacy/animation@";
			CharacterCode = "F03/Asphodel/";
			AnimationListFile = "animation_list_b";
			AnimationListFileAll = "animation_list_b";
			FacialTexListFile = settingFileDir + CharacterCode + "facial_texture_list_b";
			path = settingFileDir + CharacterCode + "fbx_ctrl";
			functionList["model"] = false;
			functionList["facial"] = true;
			functionList["lod"] = true;
			txt = (TextAsset)Resources.Load(FacialTexListFile, typeof(TextAsset));
			facialTexList = txt.text.Split(new string[2] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
			faceObjName = "succubus_b";
			facialMatName = "succubus_b_face";
			faceMat_L = (Material)Resources.Load("Asphodel_v0111/Materials/succubus_b_face_l", typeof(Material));
			faceMat_M = (Material)Resources.Load("Asphodel_v0111/Materials/succubus_b_face_m", typeof(Material));
			curCharacterName = "Succubus Asphodel";
			break;
		case 6:
			resourcesPathFull = "Assets/Satomi Character Pack/Resources/Satomi";
			resourcesPath = "Satomi/";
			animationPath = "Satomi/Animations Legacy/f05@";
			AnimationListFile = "animation_list";
			AnimationListFileAll = "animation_list_all";
			CharacterCode = "F05/";
			path = settingFileDir + CharacterCode + "fbx_ctrl";
			faceMat_L = (Material)Resources.Load("Satomi/Materials/f05_face_00_l", typeof(Material));
			faceMat_M = (Material)Resources.Load("Satomi/Materials/f05_face_00_m", typeof(Material));
			functionList["model"] = true;
			functionList["facial"] = false;
			functionList["lod"] = true;
			curCharacterName = "Satomi Makise";
			break;
		}
		txt = (TextAsset)Resources.Load(settingFileDir + CharacterCode + FBXListFile, typeof(TextAsset));
		modelList = txt.text.Split(new string[2] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
		txt = (TextAsset)Resources.Load(settingFileDir + CharacterCode + AnimationListFile, typeof(TextAsset));
		animationList = txt.text.Split(new string[2] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
		txt = (TextAsset)Resources.Load(settingFileDir + CharacterCode + AnimationListFileAll, typeof(TextAsset));
		animationListAll = txt.text.Split(new string[2] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
		txt = (TextAsset)Resources.Load(path, typeof(TextAsset));
		xDoc = new XmlDocument();
		xDoc.LoadXml(txt.text);
		SetInit();
	}

	public virtual void SetNextCharacter(int _add)
	{
		curCharacter += _add;
		if (curCharacter > 6)
		{
			curCharacter = 0;
		}
		else if (curCharacter < 0)
		{
			curCharacter = 6;
		}
		switch (curCharacter)
		{
		case 0:
			CharacterCode = "M01/";
			break;
		case 1:
			CharacterCode = "Puppet/";
			break;
		case 2:
			CharacterCode = "F01/";
			break;
		case 3:
			CharacterCode = "F02/";
			break;
		case 4:
			CharacterCode = "F03/Arum/";
			break;
		case 5:
			CharacterCode = "F03/Asphodel/";
			break;
		case 6:
			CharacterCode = "F05/";
			break;
		}
		txt = (TextAsset)Resources.Load(settingFileDir + CharacterCode + FBXListFile, typeof(TextAsset));
		modelList = txt.text.Split(new string[2] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
		GameObject gameObject = (GameObject)Resources.Load(modelList[0] + "_h", typeof(GameObject));
		if (gameObject == null)
		{
			SetNextCharacter(_add);
		}
		SetSettings(curCharacter);
	}

	public virtual void setAnimationList_old()
	{
		object[] array = Resources.LoadAll("Animations Legacy", typeof(AnimationClip));
		int i = 0;
		object[] array2 = array;
		for (int length = array2.Length; i < length; i++)
		{
			object obj = array2[i];
			if (!(obj is AnimationClip))
			{
				obj = RuntimeServices.Coerce(obj, typeof(AnimationClip));
			}
			AnimationClip animationClip = (AnimationClip)obj;
		}
	}

	public virtual void SetNextFacial(int _add)
	{
		curFacial += _add;
		if (facialCount <= curFacial)
		{
			curFacial = 0;
		}
		else if (curFacial < 0)
		{
			curFacial = facialCount - 1;
		}
		if (curLOD == 0f)
		{
			SetFacialBlendShape(curFacial);
		}
		else
		{
			SetFacialTex(curFacial);
		}
	}

	public virtual void SetInitFacial()
	{
		curFacial = 0;
		curFacialName = "Default";
		GameObject gameObject = GameObject.Find(curModelName + "_face");
		Mesh sharedMesh = ((SkinnedMeshRenderer)gameObject.GetComponent(typeof(SkinnedMeshRenderer))).sharedMesh;
		facialCount = sharedMesh.blendShapeCount + 1;
		gameObject.name += segmentCode;
	}

	public virtual void SetFacialBlendShape(int _i)
	{
		SkinnedMeshRenderer skinnedMeshRenderer = (SkinnedMeshRenderer)GameObject.Find(curModelName + "_face" + segmentCode).GetComponent(typeof(SkinnedMeshRenderer));
		Mesh sharedMesh = ((SkinnedMeshRenderer)GameObject.Find(curModelName + "_face" + segmentCode).GetComponent(typeof(SkinnedMeshRenderer))).sharedMesh;
		int num = facialCount - 1;
		int num2 = _i - 1;
		for (int i = 0; i < facialCount - 1; i++)
		{
			skinnedMeshRenderer.SetBlendShapeWeight(i, 0f);
		}
		if (num2 >= 0)
		{
			curFacialName = sharedMesh.GetBlendShapeName(num2);
			skinnedMeshRenderer.SetBlendShapeWeight(num2, 100f);
		}
		else
		{
			curFacialName = "default";
		}
	}

	public virtual void SetFacialTex(int _i)
	{
		string path = resourcesPath + "Textures/" + facialTexList[_i] + lodList[(int)curLOD];
		string text = facialMatName + lodList[(int)curLOD] + " (Instance)";
		Texture2D texture = (Texture2D)Resources.Load(path, typeof(Texture2D));
		curFacialName = facialTexList[_i];
		int i = 0;
		Material[] sharedMaterials = faceSM.GetComponent<Renderer>().sharedMaterials;
		for (int length = sharedMaterials.Length; i < length; i++)
		{
			if ((bool)sharedMaterials[i] && sharedMaterials[i].name == text)
			{
				sharedMaterials[i].SetTexture("_MainTex", texture);
			}
		}
	}

	public virtual void facialMaterialSet()
	{
		if (curLOD == 0f)
		{
			return;
		}
		string text = faceObjName + lodList[(int)curLOD] + "_face";
		int num = 0;
		GameObject gameObject = GameObject.Find(text);
		faceSM = (SkinnedMeshRenderer)gameObject.GetComponent(typeof(SkinnedMeshRenderer));
		int i = 0;
		Material[] sharedMaterials = faceSM.GetComponent<Renderer>().sharedMaterials;
		for (int length = sharedMaterials.Length; i < length; i++)
		{
			if (sharedMaterials[i].name == faceMat_M.name)
			{
				faceSM.GetComponent<Renderer>().materials[num] = faceMat_M;
			}
			else if (sharedMaterials[i].name == faceMat_L.name)
			{
				faceSM.GetComponent<Renderer>().materials[num] = faceMat_L;
			}
			num++;
		}
	}

	public virtual void SetInitModel()
	{
		curModel = 0;
		ModelChange(modelList[curModel] + lodList[(int)curLOD]);
	}

	public virtual void SetNextModel(int _add)
	{
		curModel += _add;
		if (modelList.Length <= curModel)
		{
			curModel = 0;
		}
		else if (curModel < 0)
		{
			curModel = modelList.Length - 1;
		}
		ModelChange(modelList[curModel] + lodList[(int)curLOD]);
	}

	public virtual void SetNextLOD(int _add)
	{
		curLOD += _add;
		if (!((float)lodList.Length > curLOD))
		{
			curLOD = 0f;
		}
		else if (!(curLOD >= 0f))
		{
			curLOD = lodList.Length - 1;
		}
		ModelChange(modelList[curModel] + lodList[(int)curLOD]);
		if (!RuntimeServices.ToBool(functionList["facial"]))
		{
			return;
		}
		if (curLOD == 0f)
		{
			GameObject gameObject = GameObject.Find(curModelName + "_face");
			if ((bool)gameObject)
			{
				gameObject.name += segmentCode;
			}
			SetFacialBlendShape(curFacial);
		}
		else
		{
			SetFacialTex(curFacial);
		}
	}

	public virtual void ModelChange(string _name)
	{
		if (string.IsNullOrEmpty(_name))
		{
			return;
		}
		curModelName = Path.GetFileNameWithoutExtension(_name);
		GameObject original = (GameObject)Resources.Load(_name, typeof(GameObject));
		Vector3 position = default(Vector3);
		float y = default(float);
		if ((bool)obj)
		{
			position = obj.transform.position;
			y = obj.transform.eulerAngles.y;
		}
		UnityEngine.Object.Destroy(obj);
		obj = ((GameObject)UnityEngine.Object.Instantiate(original)) as GameObject;
		obj.transform.position = position;
		float y2 = y;
		Vector3 eulerAngles = obj.transform.eulerAngles;
		float num = (eulerAngles.y = y2);
		Vector3 vector2 = (obj.transform.eulerAngles = eulerAngles);
		SM = ((SkinnedMeshRenderer)obj.GetComponentInChildren(typeof(SkinnedMeshRenderer))) as SkinnedMeshRenderer;
		SM.quality = SkinQuality.Bone4;
		SM.updateWhenOffscreen = false;
		if (curCharacter != 1)
		{
			int num2 = 0;
			int i = 0;
			Material[] sharedMaterials = SM.GetComponent<Renderer>().sharedMaterials;
			for (int length = sharedMaterials.Length; i < length; i++)
			{
				if (sharedMaterials[i].name == faceMat_M.name)
				{
					SM.GetComponent<Renderer>().materials[num2] = faceMat_M;
				}
				else if (sharedMaterials[i].name == faceMat_L.name)
				{
					SM.GetComponent<Renderer>().materials[num2] = faceMat_L;
				}
				num2++;
			}
		}
		int j = 0;
		string[] array = animationListAll;
		for (int length2 = array.Length; j < length2; j++)
		{
			GameObject gameObject = (GameObject)Resources.Load(animationPath + array[j]);
			if ((bool)gameObject)
			{
				AnimationClip clip = gameObject.GetComponent<Animation>().clip;
				obj.GetComponent<Animation>().AddClip(clip, array[j]);
			}
		}
		viewCam.ModelTarget(GetBone(obj, boneName));
		if (RuntimeServices.ToBool(functionList["facial"]))
		{
			facialMaterialSet();
		}
		SetAnimation(string.Empty + animationList[curAnim]);
		SetAnimationSpeed(animSpeed);
	}

	public virtual void SetAnimationSpeed(float _speed)
	{
		IEnumerator enumerator = UnityRuntimeServices.GetEnumerator(this.obj.GetComponent<Animation>());
		while (enumerator.MoveNext())
		{
			object obj = enumerator.Current;
			if (!(obj is AnimationState))
			{
				obj = RuntimeServices.Coerce(obj, typeof(AnimationState));
			}
			AnimationState animationState = (AnimationState)obj;
			animationState.speed = _speed;
			UnityRuntimeServices.Update(enumerator, animationState);
		}
	}

	public virtual void SetInitMotion()
	{
		curAnim = 0;
		SetAnimation(animationList[curAnim]);
		SetAnimationSpeed(animSpeed);
	}

	public virtual void SetNextMotion(int _add)
	{
		curAnim += _add;
		playOnceFlg = true;
		if (Extensions.get_length((System.Array)animationList) <= curAnim)
		{
			curAnim = 0;
		}
		else if (curAnim < 0)
		{
			curAnim = Extensions.get_length((System.Array)animationList) - 1;
		}
		SetAnimation(animationList[curAnim]);
		SetAnimationSpeed(animSpeed);
	}

	public virtual void playAnimation()
	{
		obj.GetComponent<Animation>().wrapMode = WrapMode.Once;
		GameObject gameObject = (GameObject)Resources.Load(animationPath + curAnimName);
		if ((bool)gameObject)
		{
			if (playOnceFlg)
			{
				obj.GetComponent<Animation>().Play(curAnimName);
			}
		}
		else
		{
			SetNextModel(-1);
			SetNextModel(1);
			MonoBehaviour.print(curAnimName + " animation clip does not exist");
		}
		if (animReplay && playOnceFlg)
		{
			playOnceFlg = true;
		}
		else
		{
			playOnceFlg = false;
		}
	}

	public virtual void SetAnimation(string _name)
	{
		if (!string.IsNullOrEmpty(_name))
		{
			curAnimName = string.Empty + _name;
			timerReset();
			SetFixedFbx(xDoc, obj, curModelName, curAnimName, (int)curLOD);
		}
	}

	public virtual void SetInitBackGround()
	{
		curBG = 0;
		SetBackGround(backGroundList[curBG]);
	}

	public virtual void SetNextBackGround(int _add)
	{
		curBG += _add;
		if (Extensions.get_length((System.Array)backGroundList) <= curBG)
		{
			curBG = 0;
		}
		else if (curBG < 0)
		{
			curBG = Extensions.get_length((System.Array)backGroundList) - 1;
		}
		SetBackGround(backGroundList[curBG]);
	}

	public virtual void SetBackGround(string _name)
	{
		if (!string.IsNullOrEmpty(_name))
		{
			curBgName = Path.GetFileNameWithoutExtension(_name);
			Texture2D mainTexture = (Texture2D)Resources.Load(_name, typeof(Texture2D));
			GameObject gameObject = GameObject.Find("BillBoard") as GameObject;
			gameObject.GetComponent<Renderer>().material.mainTexture = mainTexture;
			mainTexture = (Texture2D)Resources.Load(stageTexList[curBG], typeof(Texture2D));
			planeObj.GetComponent<Renderer>().material.mainTexture = mainTexture;
			if (curBG == 0)
			{
				planeObj.SetActive(false);
			}
			else
			{
				planeObj.SetActive(true);
			}
		}
	}

	public virtual Transform GetBone(GameObject _obj, string _bone)
	{
		SkinnedMeshRenderer skinnedMeshRenderer = ((SkinnedMeshRenderer)_obj.GetComponentInChildren(typeof(SkinnedMeshRenderer))) as SkinnedMeshRenderer;
		int num;
		Transform[] bones;
		if ((bool)skinnedMeshRenderer)
		{
			num = 0;
			bones = skinnedMeshRenderer.bones;
			int length = bones.Length;
			while (num < length)
			{
				if (!(bones[num].name == _bone))
				{
					num++;
					continue;
				}
				goto IL_004f;
			}
		}
		object result = null;
		goto IL_0064;
		IL_004f:
		result = bones[num];
		goto IL_0064;
		IL_0064:
		return (Transform)result;
	}

	public virtual void SetFixedFbx(XmlDocument _xDoc, GameObject _obj, string _model, string _anim, int _lod)
	{
		if (RuntimeServices.EqualityOperator(_xDoc, null) || _obj == null)
		{
			return;
		}
		XmlNode xmlNode = null;
		XmlNode xmlNode2 = null;
		XmlNode xmlNode3 = null;
		string text = null;
		text = "Root/Texture[@Lod=''or@Lod='" + _lod + "'][Info[@Model=''or@Model='" + _model + "'][@Ani=''or@Ani='" + _anim + "']]";
		xmlNode2 = _xDoc.SelectSingleNode(text);
		if (xmlNode2 != null)
		{
			string innerText = xmlNode2.Attributes["Material"].InnerText;
			string innerText2 = xmlNode2.Attributes["Property"].InnerText;
			string innerText3 = xmlNode2.Attributes["File"].InnerText;
			int i = 0;
			Material[] sharedMaterials = SM.GetComponent<Renderer>().sharedMaterials;
			for (int length = sharedMaterials.Length; i < length; i++)
			{
				if ((bool)sharedMaterials[i] && sharedMaterials[i].name == innerText)
				{
					Texture2D texture = (Texture2D)Resources.Load(innerText3, typeof(Texture2D));
					sharedMaterials[i].SetTexture(innerText2, texture);
				}
			}
		}
		text = "Root/Animation[@Lod=''or@Lod='" + _lod + "'][Info[@Model=''or@Model='" + _model + "'][@Ani=''or@Ani='" + _anim + "']]";
		xmlNode3 = _xDoc.SelectSingleNode(text);
		if (xmlNode3 != null)
		{
			string innerText4 = xmlNode3.Attributes["File"].InnerText;
			curAnimName = innerText4;
			_obj.GetComponent<Animation>().Play(curAnimName);
		}
		Vector3 vector = default(Vector3);
		Vector3 vector2 = default(Vector3);
		text = "Root/Position[@Ani=''or@Ani='" + _anim + "']";
		xmlNode3 = _xDoc.SelectSingleNode(text);
		if (xmlNode3 != null)
		{
			vector.x = float.Parse(xmlNode3.Attributes["PosX"].InnerText);
			vector.y = float.Parse(xmlNode3.Attributes["PosY"].InnerText);
			vector.z = float.Parse(xmlNode3.Attributes["PosZ"].InnerText);
			vector2.x = float.Parse(xmlNode3.Attributes["RotX"].InnerText);
			vector2.y = float.Parse(xmlNode3.Attributes["RotY"].InnerText);
			vector2.z = float.Parse(xmlNode3.Attributes["RotZ"].InnerText);
			float y = vector.y;
			Vector3 position = obj.transform.position;
			float num = (position.y = y);
			Vector3 vector4 = (obj.transform.position = position);
		}
	}

	public virtual void Main()
	{
	}
}
