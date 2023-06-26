using System;
using System.Collections;
using System.IO;
using System.Xml;
using Boo.Lang.Runtime;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class taichi_scenescript : MonoBehaviour
{
	public GameObject animTest;

	public GUISkin guiSkin;

	public taichi_viewscript viewCam;

	public string boneName;

	public Rect camGuiRootRect;

	public Rect camGuiBodyRect;

	public Rect guiOnBodyRect;

	public Rect sliderTextBodyRect;

	public Rect sliderGuiBodyRect;

	public Rect textGuiBodyRect;

	public Rect modelBodyRect;

	public string FBXListFile;

	public string AnimationListFile;

	public string TitleTextFile;

	public bool guiOn;

	public Material facialMaterial_M_org;

	public Material facialMaterial_L_org;

	public string FacialTexListFile;

	public string ParticleListFile;

	public string ParticleAnimationListFile;

	public string facialMatName;

	private bool guiShowFlg;

	private string viewerResourcesPath;

	private string viewerSettingPath;

	private string viewerMaterialPath;

	private string viewerBackGroundPath;

	private string texturePath;

	private Hashtable functionList;

	private int curBG;

	private int curAnim;

	private int curModel;

	private int curFacial;

	private int curMode;

	private float curLOD;

	private float curParticle;

	private float animSpeed;

	private string curModelName;

	private string curAnimName;

	private string curModeName;

	private string curBgName;

	private string curFacialName;

	private string curParticleName;

	private int facialCount;

	private float positionY;

	private string[] animationList;

	private string[] animationCommonList;

	private string[] facialTexList;

	private string[] particleAnimationList;

	private string[] particleList;

	private string[] modelList;

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

	private string titleText;

	private XmlDocument xDoc;

	private XmlNodeList xNodeList;

	private Material faceMat_L;

	private Material faceMat_M;

	private GameObject BGObject;

	private GameObject BGEff;

	private GameObject BGPlane;

	private GameObject planeObj;

	private Vector2 oldMousePosition;

	private float popupWaitingTime;

	private float popupWaitingTimeNow;

	private Vector3 scale;

	private bool test;

	private bool ParticleMode;

	private int onSliderFlg;

	public taichi_scenescript()
	{
		boneName = "Hips";
		camGuiRootRect = new Rect(870f, 25f, 93f, 420f);
		camGuiBodyRect = new Rect(870f, 25f, 93f, 420f);
		guiOnBodyRect = new Rect(50f, 75f, 300f, 70f);
		sliderTextBodyRect = new Rect(0f, 0f, 0f, 0f);
		sliderGuiBodyRect = new Rect(0f, 0f, 0f, 0f);
		textGuiBodyRect = new Rect(20f, 510f, 300f, 70f);
		modelBodyRect = new Rect(20f, 40f, 300f, 500f);
		FBXListFile = "fbx_list_a";
		AnimationListFile = "animation_list_a";
		TitleTextFile = "title_text_a";
		guiOn = true;
		FacialTexListFile = "facial_texture_list_a";
		ParticleListFile = "particle_list_a";
		ParticleAnimationListFile = "particle_animation_list_a";
		facialMatName = "f01_face_00";
		viewerResourcesPath = "Taichi";
		viewerSettingPath = viewerResourcesPath + "/Viewer Settings";
		viewerMaterialPath = viewerResourcesPath + "/Viewer Materials";
		viewerBackGroundPath = viewerResourcesPath + "/Viewer BackGrounds";
		texturePath = viewerResourcesPath + "/Textures";
		functionList = new Hashtable();
		curBG = 1;
		curAnim = 1;
		curModel = 1;
		curFacial = 1;
		curMode = 1;
		curParticle = 1f;
		animSpeed = 1f;
		curModelName = string.Empty;
		curAnimName = string.Empty;
		curModeName = string.Empty;
		curBgName = string.Empty;
		curFacialName = string.Empty;
		curParticleName = string.Empty;
		lodList = new string[3] { "_h", "_m", "_l" };
		lodTextList = new string[3] { "Hi", "Mid", "Low" };
		modeTextList = new string[2] { "AddPerticle", "Original" };
		CamModeRote = true;
		CamModeFix = true;
		titleText = string.Empty;
		popupWaitingTime = 2f;
	}

	public virtual void Start()
	{
		functionList["particle"] = false;
		functionList["facial"] = false;
		functionList["model"] = true;
		functionList["animation"] = true;
		functionList["background"] = true;
		functionList["lod"] = true;
		functionList["position_x"] = false;
		functionList["position_y"] = false;
		functionList["position_z"] = false;
		functionList["rotate"] = false;
		functionList["animation_speed"] = true;
		viewCam = (taichi_viewscript)GameObject.Find("Main Camera").GetComponent("taichi_viewscript");
		planeObj = GameObject.Find("Plane") as GameObject;
		txt = (TextAsset)Resources.Load(viewerSettingPath + "/background_list", typeof(TextAsset));
		backGroundList = txt.text.Split(new string[2] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
		txt = (TextAsset)Resources.Load(viewerSettingPath + "/" + FBXListFile, typeof(TextAsset));
		modelList = txt.text.Split(new string[2] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
		txt = (TextAsset)Resources.Load(viewerSettingPath + "/stage_texture_list", typeof(TextAsset));
		stageTexList = txt.text.Split(new string[2] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
		if (RuntimeServices.ToBool(functionList["particle"]))
		{
			txt = (TextAsset)Resources.Load(viewerSettingPath + "/" + ParticleListFile, typeof(TextAsset));
			particleList = txt.text.Split(new string[2] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
			txt = (TextAsset)Resources.Load(viewerSettingPath + "/" + ParticleAnimationListFile, typeof(TextAsset));
			particleAnimationList = txt.text.Split(new string[2] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
		}
		txt = (TextAsset)Resources.Load(viewerSettingPath + "/" + AnimationListFile, typeof(TextAsset));
		animationCommonList = txt.text.Split(new string[2] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
		if (RuntimeServices.ToBool(functionList["facial"]))
		{
			txt = (TextAsset)Resources.Load(viewerSettingPath + "/" + FacialTexListFile, typeof(TextAsset));
			facialTexList = txt.text.Split(new string[2] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
		}
		txt = (TextAsset)Resources.Load(viewerSettingPath + "/" + TitleTextFile, typeof(TextAsset));
		titleText = txt.text;
		txt = (TextAsset)Resources.Load(viewerSettingPath + "/fbx_ctrl", typeof(TextAsset));
		xDoc = new XmlDocument();
		xDoc.LoadXml(txt.text);
		faceMat_L = facialMaterial_L_org;
		faceMat_M = facialMaterial_M_org;
		if (curMode == 0)
		{
			animationList = particleAnimationList;
		}
		else if (curMode == 1)
		{
			animationList = animationCommonList;
		}
		curModeName = modeTextList[curMode];
		SetInitBackGround();
		SetInitModel();
		SetInitMotion();
		SetAnimationSpeed(animSpeed);
		if (curMode == 0)
		{
			SetInitParticle();
		}
	}

	public virtual void Update()
	{
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

	public virtual void OnGUI()
	{
		if ((bool)guiSkin)
		{
			GUI.skin = guiSkin;
		}
		bool flag;
		if (!guiOn)
		{
			GUILayout.BeginArea(guiOnBodyRect);
			GUILayout.BeginVertical();
			GUILayout.BeginHorizontal();
			flag = GUILayout.Toggle(guiOn, string.Empty, "GUIOn");
			if (guiOn != flag)
			{
				guiOn = flag;
			}
			GUILayout.EndHorizontal();
			GUILayout.EndVertical();
			GUILayout.EndArea();
			popUp();
			return;
		}
		if (curMode != 0 || curParticle != 4f)
		{
		}
		scale.x = (float)Screen.width / 960f;
		scale.y = (float)Screen.height / 600f;
		scale.x = 1f;
		scale.y = 1f;
		scale.z = 1f;
		GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, scale);
		float num = Screen.width;
		float num2 = Screen.height;
		textGuiBodyRect.y = num2 - (textGuiBodyRect.height + 20f);
		camGuiRootRect.x = num - camGuiRootRect.width * 0.9f;
		camGuiBodyRect.x = num - (camGuiBodyRect.width * 0.9f - 15f);
		sliderTextBodyRect.x = num - (sliderTextBodyRect.width + 20f);
		sliderTextBodyRect.y = num2 - (sliderTextBodyRect.height + 20f);
		sliderGuiBodyRect.x = num - (sliderTextBodyRect.width + sliderGuiBodyRect.width + 25f);
		sliderGuiBodyRect.y = num2 - (sliderGuiBodyRect.height + 20f);
		GUI.Label(new Rect(20f, 20f, 500f, 50f), titleText, "Title");
		float pixels = 10f;
		bool flag2 = default(bool);
		GUILayout.BeginArea(modelBodyRect);
		GUILayout.BeginVertical();
		GUILayout.BeginHorizontal();
		flag = GUILayout.Toggle(guiOn, string.Empty, "GUIOn");
		if (guiOn != flag)
		{
			guiOn = flag;
		}
		GUILayout.EndHorizontal();
		GUILayout.Space(pixels);
		if (RuntimeServices.ToBool(functionList["particle"]))
		{
			GUILayout.BeginHorizontal();
			flag2 = GUILayout.Toggle(ParticleMode, string.Empty, "ParticleMode");
			if (ParticleMode != flag2)
			{
				ParticleMode = flag2;
				if (curMode == 0)
				{
					SetNextMode(1);
				}
				else
				{
					SetNextMode(-1);
				}
			}
			GUILayout.EndHorizontal();
			GUILayout.Space(pixels);
		}
		else
		{
			float a = 0.5f;
			Color color = GUI.color;
			float num3 = (color.a = a);
			Color color3 = (GUI.color = color);
			GUILayout.BeginHorizontal();
			GUILayout.Label(string.Empty, "ParticleMode");
			GUILayout.EndHorizontal();
			GUILayout.Space(pixels);
			float a2 = 1f;
			Color color4 = GUI.color;
			float num4 = (color4.a = a2);
			Color color6 = (GUI.color = color4);
		}
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
			float num5 = (color7.a = a3);
			Color color9 = (GUI.color = color7);
			GUILayout.BeginHorizontal();
			if (GUILayout.Button(string.Empty, "LeftGlayOut"))
			{
			}
			GUILayout.Label(string.Empty, "Costume");
			if (GUILayout.Button(string.Empty, "RightGlayOut"))
			{
			}
			GUILayout.EndHorizontal();
			GUILayout.Space(pixels);
			float a4 = 1f;
			Color color10 = GUI.color;
			float num6 = (color10.a = a4);
			Color color12 = (GUI.color = color10);
		}
		if (RuntimeServices.ToBool(functionList["animation"]))
		{
			if (curMode == 1)
			{
				GUILayout.BeginHorizontal();
				if (GUILayout.Button(string.Empty, "Left"))
				{
					SetNextMotion(-1);
				}
				GUILayout.Label(string.Empty, "Anim");
				if (GUILayout.Button(string.Empty, "Right"))
				{
					SetNextMotion(1);
				}
				GUILayout.EndHorizontal();
				GUILayout.Space(pixels);
			}
			else if (curMode == 0)
			{
				GUILayout.BeginHorizontal();
				if (GUILayout.Button(string.Empty, "Left"))
				{
					SetNextParticle(-1);
				}
				if (GUILayout.Button(string.Empty, "ParticleShot"))
				{
					particleExec();
				}
				if (GUILayout.Button(string.Empty, "Right"))
				{
					SetNextParticle(1);
				}
				GUILayout.EndHorizontal();
				GUILayout.Space(pixels);
			}
		}
		else
		{
			float a5 = 0.5f;
			Color color13 = GUI.color;
			float num7 = (color13.a = a5);
			Color color15 = (GUI.color = color13);
			GUILayout.BeginHorizontal();
			if (GUILayout.Button(string.Empty, "LeftGlayOut"))
			{
			}
			GUILayout.Label(string.Empty, "Anim");
			if (GUILayout.Button(string.Empty, "RightGlayOut"))
			{
			}
			GUILayout.EndHorizontal();
			GUILayout.Space(pixels);
			float a6 = 1f;
			Color color16 = GUI.color;
			float num8 = (color16.a = a6);
			Color color18 = (GUI.color = color16);
		}
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
			float a7 = 0.5f;
			Color color19 = GUI.color;
			float num9 = (color19.a = a7);
			Color color21 = (GUI.color = color19);
			GUILayout.BeginHorizontal();
			if (GUILayout.Button(string.Empty, "LeftGlayOut"))
			{
			}
			GUILayout.Label(string.Empty, "Facial");
			if (GUILayout.Button(string.Empty, "RightGlayOut"))
			{
			}
			GUILayout.EndHorizontal();
			GUILayout.Space(pixels);
			float a8 = 1f;
			Color color22 = GUI.color;
			float num10 = (color22.a = a8);
			Color color24 = (GUI.color = color22);
		}
		if (RuntimeServices.ToBool(functionList["background"]))
		{
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
			GUILayout.EndHorizontal();
			GUILayout.Space(pixels);
		}
		else
		{
			float a9 = 0.5f;
			Color color25 = GUI.color;
			float num11 = (color25.a = a9);
			Color color27 = (GUI.color = color25);
			GUILayout.BeginHorizontal();
			if (GUILayout.Button(string.Empty, "LeftGlayOut"))
			{
			}
			GUILayout.Label(string.Empty, "BG");
			if (GUILayout.Button(string.Empty, "RightGlayOut"))
			{
			}
			GUILayout.EndHorizontal();
			GUILayout.Space(pixels);
			float a10 = 1f;
			Color color28 = GUI.color;
			float num12 = (color28.a = a10);
			Color color30 = (GUI.color = color28);
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
		}
		else
		{
			float a11 = 0.5f;
			Color color31 = GUI.color;
			float num13 = (color31.a = a11);
			Color color33 = (GUI.color = color31);
			GUILayout.BeginHorizontal();
			if (GUILayout.Button(string.Empty, "LeftGlayOut"))
			{
			}
			GUILayout.Label(string.Empty, "LOD");
			if (GUILayout.Button(string.Empty, "RightGlayOut"))
			{
			}
			GUILayout.EndHorizontal();
			GUILayout.Space(pixels);
			float a12 = 1f;
			Color color34 = GUI.color;
			float num14 = (color34.a = a12);
			Color color36 = (GUI.color = color34);
		}
		GUILayout.FlexibleSpace();
		GUILayout.EndVertical();
		GUILayout.EndArea();
		GUILayout.BeginArea(sliderTextBodyRect);
		GUILayout.FlexibleSpace();
		if (RuntimeServices.ToBool(functionList["position_x"]))
		{
			float a13 = 1f;
			Color color37 = GUI.color;
			float num15 = (color37.a = a13);
			Color color39 = (GUI.color = color37);
		}
		else
		{
			float a14 = 0.4f;
			Color color40 = GUI.color;
			float num16 = (color40.a = a14);
			Color color42 = (GUI.color = color40);
		}
		string text = "Position X : " + string.Format("{0:F1}", obj.transform.position.x);
		GUILayout.Box(text);
		GUILayout.FlexibleSpace();
		if (RuntimeServices.ToBool(functionList["position_y"]))
		{
			float a15 = 1f;
			Color color43 = GUI.color;
			float num17 = (color43.a = a15);
			Color color45 = (GUI.color = color43);
		}
		else
		{
			float a16 = 0.4f;
			Color color46 = GUI.color;
			float num18 = (color46.a = a16);
			Color color48 = (GUI.color = color46);
		}
		string text2 = "Position Y : " + string.Format("{0:F1}", obj.transform.position.y);
		GUILayout.Box(text2);
		GUILayout.FlexibleSpace();
		if (RuntimeServices.ToBool(functionList["position_z"]))
		{
			float a17 = 1f;
			Color color49 = GUI.color;
			float num19 = (color49.a = a17);
			Color color51 = (GUI.color = color49);
		}
		else
		{
			float a18 = 0.4f;
			Color color52 = GUI.color;
			float num20 = (color52.a = a18);
			Color color54 = (GUI.color = color52);
		}
		string text3 = "Position Z : " + string.Format("{0:F1}", obj.transform.position.z);
		GUILayout.Box(text3);
		GUILayout.FlexibleSpace();
		if (RuntimeServices.ToBool(functionList["rotate_y"]))
		{
			float a19 = 1f;
			Color color55 = GUI.color;
			float num21 = (color55.a = a19);
			Color color57 = (GUI.color = color55);
		}
		else
		{
			float a20 = 0.4f;
			Color color58 = GUI.color;
			float num22 = (color58.a = a20);
			Color color60 = (GUI.color = color58);
		}
		string text4 = "Rotate : " + string.Format("{0:F1}", obj.transform.eulerAngles.y);
		GUILayout.Box(text4);
		GUILayout.FlexibleSpace();
		if (RuntimeServices.ToBool(functionList["animation_speed"]))
		{
			float a21 = 1f;
			Color color61 = GUI.color;
			float num23 = (color61.a = a21);
			Color color63 = (GUI.color = color61);
		}
		else
		{
			float a22 = 0.4f;
			Color color64 = GUI.color;
			float num24 = (color64.a = a22);
			Color color66 = (GUI.color = color64);
		}
		string text5 = "Animation\nSpeed : " + string.Format("{0:F1}", animSpeed);
		GUILayout.Box(text5);
		GUILayout.FlexibleSpace();
		GUILayout.EndArea();
		GUILayout.BeginArea(sliderGuiBodyRect);
		if (RuntimeServices.ToBool(functionList["position_x"]))
		{
			if (onSliderFlg == 1)
			{
				float a23 = 1f;
				Color color67 = GUI.color;
				float num25 = (color67.a = a23);
				Color color69 = (GUI.color = color67);
			}
			else
			{
				float a24 = 0.4f;
				Color color70 = GUI.color;
				float num26 = (color70.a = a24);
				Color color72 = (GUI.color = color70);
			}
			float num27 = GUILayout.HorizontalSlider(obj.transform.position.x, 1f, -1f);
			if (obj.transform.position.x != num27)
			{
				float x = num27;
				Vector3 position = obj.transform.position;
				float num28 = (position.x = x);
				Vector3 vector2 = (obj.transform.position = position);
				viewCam.MouseLock(true);
			}
			else
			{
				viewCam.MouseLock(false);
			}
			GUILayout.Space(0f);
		}
		else
		{
			float a25 = 0.4f;
			Color color73 = GUI.color;
			float num29 = (color73.a = a25);
			Color color75 = (GUI.color = color73);
			GUILayout.HorizontalSlider(obj.transform.position.x, 0f, 0f);
			GUILayout.Space(0f);
		}
		if (RuntimeServices.ToBool(functionList["position_y"]))
		{
			if (onSliderFlg == 2)
			{
				float a26 = 1f;
				Color color76 = GUI.color;
				float num30 = (color76.a = a26);
				Color color78 = (GUI.color = color76);
			}
			else
			{
				float a27 = 0.4f;
				Color color79 = GUI.color;
				float num31 = (color79.a = a27);
				Color color81 = (GUI.color = color79);
			}
			float num32 = GUILayout.HorizontalSlider(obj.transform.position.y, 0f, 3f);
			if (obj.transform.position.y != num32)
			{
				float y = num32;
				Vector3 position2 = obj.transform.position;
				float num33 = (position2.y = y);
				Vector3 vector4 = (obj.transform.position = position2);
				viewCam.MouseLock(true);
			}
			else
			{
				viewCam.MouseLock(false);
			}
			GUILayout.Space(0f);
		}
		else
		{
			float a28 = 0.4f;
			Color color82 = GUI.color;
			float num34 = (color82.a = a28);
			Color color84 = (GUI.color = color82);
			GUILayout.HorizontalSlider(obj.transform.position.y, 0f, 0f);
			GUILayout.Space(0f);
		}
		if (RuntimeServices.ToBool(functionList["position_z"]))
		{
			if (onSliderFlg == 3)
			{
				float a29 = 1f;
				Color color85 = GUI.color;
				float num35 = (color85.a = a29);
				Color color87 = (GUI.color = color85);
			}
			else
			{
				float a30 = 0.4f;
				Color color88 = GUI.color;
				float num36 = (color88.a = a30);
				Color color90 = (GUI.color = color88);
			}
			float num37 = GUILayout.HorizontalSlider(obj.transform.position.z, 1f, -1f);
			if (obj.transform.position.z != num37)
			{
				float z = num37;
				Vector3 position3 = obj.transform.position;
				float num38 = (position3.z = z);
				Vector3 vector6 = (obj.transform.position = position3);
				viewCam.MouseLock(true);
			}
			else
			{
				viewCam.MouseLock(false);
			}
			GUILayout.Space(0f);
		}
		else
		{
			float a31 = 0.4f;
			Color color91 = GUI.color;
			float num39 = (color91.a = a31);
			Color color93 = (GUI.color = color91);
			GUILayout.HorizontalSlider(obj.transform.position.z, 0f, 0f);
			GUILayout.Space(0f);
		}
		if (RuntimeServices.ToBool(functionList["rotate"]))
		{
			if (onSliderFlg == 4)
			{
				float a32 = 1f;
				Color color94 = GUI.color;
				float num40 = (color94.a = a32);
				Color color96 = (GUI.color = color94);
			}
			else
			{
				float a33 = 0.4f;
				Color color97 = GUI.color;
				float num41 = (color97.a = a33);
				Color color99 = (GUI.color = color97);
			}
			float num42 = GUILayout.HorizontalSlider(obj.transform.eulerAngles.y, 0f, 359.9f);
			if (obj.transform.eulerAngles.y != num42)
			{
				float y2 = num42;
				Vector3 eulerAngles = obj.transform.eulerAngles;
				float num43 = (eulerAngles.y = y2);
				Vector3 vector8 = (obj.transform.eulerAngles = eulerAngles);
				viewCam.MouseLock(true);
			}
			else
			{
				viewCam.MouseLock(false);
			}
			GUILayout.Space(5f);
		}
		else
		{
			float a34 = 0.4f;
			Color color100 = GUI.color;
			float num44 = (color100.a = a34);
			Color color102 = (GUI.color = color100);
			GUILayout.HorizontalSlider(obj.transform.eulerAngles.y, 0f, 0f);
			GUILayout.Space(5f);
		}
		if (RuntimeServices.ToBool(functionList["animation_speed"]))
		{
			if (onSliderFlg == 5)
			{
				float a35 = 1f;
				Color color103 = GUI.color;
				float num45 = (color103.a = a35);
				Color color105 = (GUI.color = color103);
			}
			else
			{
				float a36 = 0.4f;
				Color color106 = GUI.color;
				float num46 = (color106.a = a36);
				Color color108 = (GUI.color = color106);
			}
			float a37 = 1f;
			Color color109 = GUI.color;
			float num47 = (color109.a = a37);
			Color color111 = (GUI.color = color109);
			float num48 = GUILayout.HorizontalSlider(animSpeed, 0f, 2f);
			if (animSpeed != num48)
			{
				animSpeed = num48;
				SetAnimationSpeed(animSpeed);
				viewCam.MouseLock(true);
			}
			else
			{
				viewCam.MouseLock(false);
			}
		}
		else
		{
			float a38 = 0.4f;
			Color color112 = GUI.color;
			float num49 = (color112.a = a38);
			Color color114 = (GUI.color = color112);
			GUILayout.HorizontalSlider(animSpeed, 0f, 0f);
		}
		GUILayout.EndArea();
		float a39 = 1f;
		Color color115 = GUI.color;
		float num50 = (color115.a = a39);
		Color color117 = (GUI.color = color115);
		GUI.Label(camGuiRootRect, string.Empty, "CamBG");
		GUILayout.BeginArea(camGuiBodyRect);
		GUILayout.BeginHorizontal();
		GUILayout.BeginVertical();
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
			viewCam.Reset();
		}
		GUILayout.FlexibleSpace();
		GUILayout.EndVertical();
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
		string lhs = string.Empty;
		if (RuntimeServices.ToBool(functionList["particle"]))
		{
			lhs += "Mode : " + curModeName + "\n";
		}
		if (RuntimeServices.ToBool(functionList["model"]))
		{
			lhs += "Costume : " + (curModel + 1) + " / " + Extensions.get_length((System.Array)modelList) + " : " + curModelName + "\n";
		}
		if (RuntimeServices.ToBool(functionList["animation"]))
		{
			lhs = ((curMode != 0) ? (lhs + ("Animation : " + (curAnim + 1) + " / " + Extensions.get_length((System.Array)animationList) + " : " + curAnimName + "\n")) : (lhs + ("Particle  : " + (curAnim + 1) + " / " + Extensions.get_length((System.Array)animationList) + " : " + curParticleName + "\n")));
		}
		if (RuntimeServices.ToBool(functionList["facial"]))
		{
			lhs += "Facial : " + (curFacial + 1) + " / " + facialCount + " : " + curFacialName + "\n";
		}
		if (RuntimeServices.ToBool(functionList["background"]))
		{
			lhs += "BackGround : " + (curBG + 1) + " / " + Extensions.get_length((System.Array)backGroundList) + " : " + curBgName + "\n";
		}
		if (RuntimeServices.ToBool(functionList["lod"]))
		{
			lhs += "Quality : " + lodTextList[(int)curLOD] + "\n";
		}
		lhs += "Animation Speed : " + string.Format("{0:F2}", animSpeed);
		GUI.Box(textGuiBodyRect, lhs);
		popUp();
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
		float num4 = 60f;
		float num5 = 20f;
		float num6 = num2 - num4;
		float num7 = 50f;
		float num8 = 100f;
		float num9 = 12f;
		float num10 = default(float);
		float num11 = default(float);
		float num12 = default(float);
		float num13 = default(float);
		float num14 = default(float);
		float num15 = default(float);
		if (guiOn)
		{
			num13 = num6;
			num11 = num6 - num7;
			num12 = num8 + num5;
			num10 = num5;
			num14 = num2 - num13;
			num15 = num8 + num5 + 10f;
			array[0] = new Vector2(num10, num11);
			array2[0] = new Vector2(num12, num13);
			array3[0] = new Rect(num15 - 20f, num14, 120f, 23f);
			array4[0] = "GUI On/Off.";
			num13 -= num7 + num9;
			num11 -= num7 + num9;
			num14 = num2 - num13;
			array[1] = new Vector2(num10, num11);
			array2[1] = new Vector2(num12, num13);
			array3[1] = new Rect(num15 - 20f, num14, 120f, 23f);
			array4[1] = "Mode Change.";
			num13 -= num7 + num9;
			num11 -= num7 + num9;
			num14 = num2 - num13;
			array[2] = new Vector2(num10, num11);
			array2[2] = new Vector2(num12, num13);
			array3[2] = new Rect(num15, num14, 120f, 23f);
			array4[2] = "Model Change.";
			num13 -= num7 + num9;
			num11 -= num7 + num9;
			num14 = num2 - num13;
			array[3] = new Vector2(num10, num11);
			array2[3] = new Vector2(num12, num13);
			array3[3] = new Rect(num15, num14, 120f, 23f);
			array4[3] = "Motion Change.";
			num13 -= num7 + num9;
			num11 -= num7 + num9;
			num14 = num2 - num13;
			array[4] = new Vector2(num10, num11);
			array2[4] = new Vector2(num12, num13);
			array3[4] = new Rect(num15, num14, 120f, 23f);
			array4[4] = "Facial Change.";
			num13 -= num7 + num9;
			num11 -= num7 + num9;
			num14 = num2 - num13;
			array[5] = new Vector2(num10, num11);
			array2[5] = new Vector2(num12, num13);
			array3[5] = new Rect(num15, num14, 150f, 23f);
			array4[5] = "BackGround Change.";
			num13 -= num7 + num9;
			num11 -= num7 + num9;
			num14 = num2 - num13;
			array[6] = new Vector2(num10, num11);
			array2[6] = new Vector2(num12, num13);
			array3[6] = new Rect(num15, num14, 120f, 23f);
			array4[6] = "Lod Change.";
			num4 = 43f;
			num6 = num2 - num4;
			num7 = 57.6f;
			num8 = 57.6f;
			num9 = 11.5f;
			float num16 = 220f;
			float num17 = default(float);
			num17 = (float)Screen.width - num16;
			num13 = num6;
			num11 = num6 - num7;
			num12 = num - 10f;
			num10 = num - 10f - num8;
			num14 = num2 - num13;
			array[7] = new Vector2(num10, num11);
			array2[7] = new Vector2(num12, num13);
			array3[7] = new Rect(num17, num14, 120f, 23f);
			array4[7] = "Camera Rotate.";
			num13 -= num7 + num9;
			num11 -= num7 + num9;
			num14 = num2 - num13;
			array[8] = new Vector2(num10, num11);
			array2[8] = new Vector2(num12, num13);
			array3[8] = new Rect(num17, num14, 120f, 23f);
			array4[8] = "Camera Move.";
			num13 -= num7 + num9;
			num11 -= num7 + num9;
			num14 = num2 - num13;
			array[9] = new Vector2(num10, num11);
			array2[9] = new Vector2(num12, num13);
			array3[9] = new Rect(num17, num14, 120f, 23f);
			array4[9] = "Camera Zoom.";
			num13 -= num7 + num9;
			num11 -= num7 + num9;
			num14 = num2 - num13;
			array[10] = new Vector2(num10, num11);
			array2[10] = new Vector2(num12, num13);
			num17 -= 30f;
			array3[10] = new Rect(num17, num14, 150f, 23f);
			array4[10] = "Camera Target Lock.";
			num13 -= num7 + num9;
			num11 -= num7 + num9;
			num14 = num2 - num13;
			num17 += 30f;
			array[11] = new Vector2(num10, num11);
			array2[11] = new Vector2(num12, num13);
			array3[11] = new Rect(num17, num14, 120f, 23f);
			array4[11] = "Camera Reset.";
		}
		else
		{
			num13 = num6;
			num11 = num6 - num7;
			num12 = num8 + num5;
			num10 = num5;
			num14 = num2 - num13;
			num15 = num8 + num5 + 10f;
			array[0] = new Vector2(num10, num11);
			array2[0] = new Vector2(num12, num13);
			array3[0] = new Rect(num15, num14, 120f, 23f);
			array4[0] = "GUI On/Off.";
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
		array[0] = new Vector2(20f, 270f);
		array2[0] = new Vector2(280f, 300f);
		array[1] = new Vector2(20f, 240f);
		array2[1] = new Vector2(280f, 270f);
		array[2] = new Vector2(20f, 210f);
		array2[2] = new Vector2(280f, 240f);
		array[3] = new Vector2(20f, 180f);
		array2[3] = new Vector2(280f, 210f);
		array[4] = new Vector2(20f, 140f);
		array2[4] = new Vector2(280f, 180f);
		array[5] = new Vector2(20f, 100f);
		array2[5] = new Vector2(280f, 140f);
		for (int i = 0; i < num; i++)
		{
			if (!(Input.mousePosition.x <= array[i].x) && !(Input.mousePosition.x >= array2[i].x) && !(Input.mousePosition.y <= array[i].y) && !(Input.mousePosition.y >= array2[i].y))
			{
				onSliderFlg = i + 1;
			}
		}
	}

	public virtual void SetNextMode(int _add)
	{
		curMode += _add;
		if (curMode > 1)
		{
			curMode = 0;
		}
		else if (curMode < 0)
		{
			curMode = 1;
		}
		if (curMode == 0)
		{
			animationList = particleAnimationList;
		}
		else if (curMode == 1)
		{
			animationList = animationCommonList;
		}
		curModeName = modeTextList[curMode];
		curAnim = 0;
		curParticle = 0f;
		curLOD = 0f;
		curParticleName = particleList[(int)curParticle];
		SetInitModel();
		SetInitMotion();
		SetAnimationSpeed(animSpeed);
		SetInitFacial();
		if (curMode == 0)
		{
			SetInitParticle();
		}
	}

	public virtual void SetInitParticle()
	{
		if (curMode == 0)
		{
		}
	}

	public virtual void SetNextParticle(int _add)
	{
		curAnim += _add;
		curParticle += _add;
		if (Extensions.get_length((System.Array)animationList) <= curAnim)
		{
			curAnim = 0;
			curParticle = 0f;
		}
		else if (curAnim < 0)
		{
			curAnim = Extensions.get_length((System.Array)animationList) - 1;
			curParticle = curAnim;
		}
		curParticleName = particleList[(int)curParticle];
		SetParticle();
	}

	public virtual void particleExec()
	{
	}

	public virtual void SetParticle()
	{
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
		SetInitParticle();
		if (RuntimeServices.ToBool(functionList["facial"]))
		{
			if (curLOD == 0f)
			{
				SetFacialBlendShape(curFacial);
			}
			else
			{
				SetFacialTex(curFacial);
			}
		}
	}

	public virtual void ModelChange(string _name)
	{
		if (string.IsNullOrEmpty(_name))
		{
			return;
		}
		MonoBehaviour.print("ModelChange : " + _name);
		curModelName = Path.GetFileNameWithoutExtension(_name);
		GameObject original = (GameObject)Resources.Load(_name, typeof(GameObject));
		UnityEngine.Object.Destroy(this.obj);
		this.obj = ((GameObject)UnityEngine.Object.Instantiate(original)) as GameObject;
		SM = ((SkinnedMeshRenderer)this.obj.GetComponentInChildren(typeof(SkinnedMeshRenderer))) as SkinnedMeshRenderer;
		SM.quality = SkinQuality.Bone4;
		SM.updateWhenOffscreen = true;
		viewCam.ModelTarget(GetBone(this.obj, boneName));
		int num = 0;
		int i = 0;
		Material[] sharedMaterials = SM.GetComponent<Renderer>().sharedMaterials;
		for (int length = sharedMaterials.Length; i < length; i++)
		{
			if (sharedMaterials[i].name == facialMatName + "_m")
			{
				SM.GetComponent<Renderer>().materials[num] = faceMat_M;
			}
			else if (sharedMaterials[i].name == facialMatName + "_l")
			{
				SM.GetComponent<Renderer>().materials[num] = faceMat_L;
			}
			num++;
		}
		IEnumerator enumerator = UnityRuntimeServices.GetEnumerator(animTest.GetComponent<Animation>());
		while (enumerator.MoveNext())
		{
			object obj = enumerator.Current;
			if (!(obj is AnimationState))
			{
				obj = RuntimeServices.Coerce(obj, typeof(AnimationState));
			}
			AnimationState animationState = (AnimationState)obj;
			this.obj.GetComponent<Animation>().AddClip(animationState.clip, animationState.name);
			UnityRuntimeServices.Update(enumerator, animationState);
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

	public virtual void SetInitFacial()
	{
		curFacial = 0;
		curFacialName = "Default";
		Mesh sharedMesh = ((SkinnedMeshRenderer)GameObject.Find(curModelName + "_face").GetComponent(typeof(SkinnedMeshRenderer))).sharedMesh;
		facialCount = sharedMesh.blendShapeCount + 1;
	}

	public virtual void SetFacialBlendShape(int _i)
	{
		SkinnedMeshRenderer skinnedMeshRenderer = (SkinnedMeshRenderer)GameObject.Find(curModelName + "_face").GetComponent(typeof(SkinnedMeshRenderer));
		Mesh sharedMesh = ((SkinnedMeshRenderer)GameObject.Find(curModelName + "_face").GetComponent(typeof(SkinnedMeshRenderer))).sharedMesh;
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
			if (sharedMaterials[i].name == facialMatName + "_m")
			{
				faceSM.GetComponent<Renderer>().materials[num] = faceMat_M;
			}
			else if (sharedMaterials[i].name == facialMatName + "_l")
			{
				faceSM.GetComponent<Renderer>().materials[num] = faceMat_L;
			}
			num++;
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

	public virtual void SetFacialTex(int _i)
	{
		facialMaterialSet();
		string path = texturePath + "/" + facialTexList[_i] + lodList[(int)curLOD];
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

	public virtual void SetNextMotion(int _add)
	{
		curAnim += _add;
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

	public virtual void SetAnimation(string _name)
	{
		if (!string.IsNullOrEmpty(_name))
		{
			MonoBehaviour.print("SetAnimation : " + _name);
			curAnimName = string.Empty + _name;
			obj.GetComponent<Animation>().Play(curAnimName);
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
		GameObject gameObject = GameObject.Find("BillBoard") as GameObject;
		Texture2D mainTexture;
		if (!string.IsNullOrEmpty(_name))
		{
			MonoBehaviour.print("SetBackGround : " + _name);
			curBgName = Path.GetFileNameWithoutExtension(_name);
			mainTexture = (Texture2D)Resources.Load(_name, typeof(Texture2D));
			GameObject gameObject2 = GameObject.Find("BillBoard") as GameObject;
			gameObject2.GetComponent<Renderer>().material.mainTexture = mainTexture;
		}
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
		text = "Root/Animation[@Lod=''or@Lod='" + _lod + "'][Info[@Model=''or@Model='" + _model + "'][@Ani=''or@Ani='" + _anim + "']]";
		xmlNode3 = _xDoc.SelectSingleNode(text);
		if (xmlNode3 != null)
		{
			string innerText = xmlNode3.Attributes["File"].InnerText;
			curAnimName = innerText;
			MonoBehaviour.print("Change Animation To " + curAnimName);
			_obj.GetComponent<Animation>().Play(curAnimName);
		}
		text = "Root/Texture[@Lod=''or@Lod='" + _lod + "'][Info[@Model=''or@Model='" + _model + "'][@Ani=''or@Ani='" + _anim + "']]";
		xmlNode2 = _xDoc.SelectSingleNode(text);
		if (xmlNode2 != null)
		{
			string innerText2 = xmlNode2.Attributes["Material"].InnerText;
			string innerText3 = xmlNode2.Attributes["Property"].InnerText;
			string innerText4 = xmlNode2.Attributes["File"].InnerText;
			MonoBehaviour.print("Change Texture To " + innerText2 + " : " + innerText3 + " : " + innerText4);
			int i = 0;
			Material[] sharedMaterials = SM.GetComponent<Renderer>().sharedMaterials;
			for (int length = sharedMaterials.Length; i < length; i++)
			{
				if ((bool)sharedMaterials[i] && sharedMaterials[i].name == innerText2)
				{
					Texture2D texture = (Texture2D)Resources.Load(innerText4, typeof(Texture2D));
					sharedMaterials[i].SetTexture(innerText3, texture);
				}
			}
		}
		Vector3 position = default(Vector3);
		Vector3 eulerAngles = default(Vector3);
		text = "Root/Position[@Ani=''or@Ani='" + _anim + "']";
		xmlNode3 = _xDoc.SelectSingleNode(text);
		if (xmlNode3 != null)
		{
			position.x = float.Parse(xmlNode3.Attributes["PosX"].InnerText);
			position.y = float.Parse(xmlNode3.Attributes["PosY"].InnerText);
			position.z = float.Parse(xmlNode3.Attributes["PosZ"].InnerText);
			eulerAngles.x = float.Parse(xmlNode3.Attributes["RotX"].InnerText);
			eulerAngles.y = float.Parse(xmlNode3.Attributes["RotY"].InnerText);
			eulerAngles.z = float.Parse(xmlNode3.Attributes["RotZ"].InnerText);
			obj.transform.position = position;
			obj.transform.eulerAngles = eulerAngles;
			positionY = position.y;
		}
	}

	public virtual void Main()
	{
	}
}
