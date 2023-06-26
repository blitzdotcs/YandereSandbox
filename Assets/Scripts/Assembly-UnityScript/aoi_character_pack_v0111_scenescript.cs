using System;
using System.Collections;
using System.IO;
using System.Xml;
using Boo.Lang.Runtime;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class aoi_character_pack_v0111_scenescript : MonoBehaviour
{
	public GameObject animTest;

	public aoi_character_pack_v0111_viewscript viewCam;

	public GUISkin guiSkin;

	public string boneName;

	public Rect camGuiRootRect;

	public Rect camGuiBodyRect;

	public Rect animSpeedGuiBodyRect;

	public Rect textGuiBodyRect;

	public Rect modelBodyRect;

	public string FBXListFile;

	public string AnimationListFile;

	public string TitleTextFile;

	public bool guiOn;

	private string viewerResourcesPath;

	private string settingsPath;

	private string materialsPath;

	private int curBG;

	private int curAnim;

	private int curModel;

	private float curLOD;

	private string curModelName;

	private string curAnimName;

	private string curBgName;

	private float animSpeed;

	private string[] animationList;

	private string[] modelList;

	private string[] backGroundList;

	private string[] lodList;

	private string[] lodTextList;

	private GameObject obj;

	private GameObject loaded;

	private SkinnedMeshRenderer SM;

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

	private Vector3 scale;

	public aoi_character_pack_v0111_scenescript()
	{
		boneName = "Hips";
		camGuiRootRect = new Rect(870f, 25f, 93f, 420f);
		camGuiBodyRect = new Rect(870f, 25f, 93f, 420f);
		animSpeedGuiBodyRect = new Rect(770f, 520f, 170f, 150f);
		textGuiBodyRect = new Rect(20f, 510f, 300f, 70f);
		modelBodyRect = new Rect(20f, 40f, 300f, 500f);
		FBXListFile = "fbx_list";
		AnimationListFile = "animation_list";
		TitleTextFile = "title_text";
		guiOn = true;
		viewerResourcesPath = "Aoi_v0111";
		settingsPath = viewerResourcesPath + "/Viewer Settings";
		materialsPath = viewerResourcesPath + "/Viewer Materials";
		curBG = 1;
		curAnim = 1;
		curModel = 1;
		curModelName = string.Empty;
		curAnimName = string.Empty;
		curBgName = string.Empty;
		animSpeed = 1f;
		lodList = new string[3] { "_h", "_m", "_l" };
		lodTextList = new string[3] { "Hi", "Mid", "Low" };
		CamModeRote = true;
		CamModeFix = true;
		titleText = string.Empty;
	}

	public virtual void Start()
	{
		viewCam = (aoi_character_pack_v0111_viewscript)GameObject.Find("Main Camera").GetComponent("aoi_character_pack_v0111_viewscript");
		faceMat_L = (Material)Resources.Load(materialsPath + "f02_face_l", typeof(Material));
		faceMat_M = (Material)Resources.Load(materialsPath + "f02_face_m", typeof(Material));
		txt = (TextAsset)Resources.Load(settingsPath + "/background_list", typeof(TextAsset));
		backGroundList = txt.text.Split(new string[2] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
		txt = (TextAsset)Resources.Load(settingsPath + "/" + FBXListFile, typeof(TextAsset));
		modelList = txt.text.Split(new string[2] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
		txt = (TextAsset)Resources.Load(settingsPath + "/" + AnimationListFile, typeof(TextAsset));
		animationList = txt.text.Split(new string[2] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
		txt = (TextAsset)Resources.Load(settingsPath + "/" + TitleTextFile, typeof(TextAsset));
		titleText = txt.text;
		txt = (TextAsset)Resources.Load(settingsPath + "/fbx_ctrl", typeof(TextAsset));
		xDoc = new XmlDocument();
		xDoc.LoadXml(txt.text);
		SetInitBackGround();
		SetInitModel();
		SetInitMotion();
		SetAnimationSpeed(animSpeed);
	}

	public virtual void Update()
	{
		if (Input.GetKeyDown("1"))
		{
			SetNextModel(-1);
		}
		if (Input.GetKeyDown("2"))
		{
			SetNextModel(-1);
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
		if (guiOn)
		{
			if ((bool)guiSkin)
			{
				GUI.skin = guiSkin;
			}
			scale.x = (float)Screen.width / 960f;
			scale.y = (float)Screen.height / 600f;
			scale.x = 1f;
			scale.y = 1f;
			scale.z = 1f;
			GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, scale);
			GUI.Label(new Rect(20f, 20f, 500f, 50f), titleText, "Title");
			GUILayout.BeginArea(modelBodyRect);
			GUILayout.BeginVertical();
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
			GUILayout.Space(20f);
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
			GUILayout.Space(20f);
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
			GUILayout.Space(20f);
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
			GUILayout.EndVertical();
			GUILayout.EndArea();
			GUILayout.BeginArea(animSpeedGuiBodyRect);
			GUILayout.FlexibleSpace();
			float num = GUILayout.HorizontalSlider(animSpeed, 0f, 2f);
			if (animSpeed != num)
			{
				animSpeed = num;
				SetAnimationSpeed(animSpeed);
				viewCam.MouseLock(true);
			}
			else
			{
				viewCam.MouseLock(false);
			}
			GUILayout.FlexibleSpace();
			string text = "Animation Speed : " + string.Format("{0:F1}", animSpeed);
			GUILayout.Box(text);
			GUILayout.FlexibleSpace();
			GUILayout.EndArea();
			GUI.Label(camGuiRootRect, string.Empty, "CamBG");
			GUILayout.BeginArea(camGuiBodyRect);
			GUILayout.BeginHorizontal();
			GUILayout.BeginVertical();
			bool flag = default(bool);
			flag = GUILayout.Toggle(CamMode == 0, string.Empty, "Rote");
			if (CamMode != 0 && flag)
			{
				CamMode = 0;
				viewCam.ModeRote();
			}
			GUILayout.FlexibleSpace();
			flag = GUILayout.Toggle(CamMode == 1, string.Empty, "Move");
			if (CamMode != 1 && flag)
			{
				CamMode = 1;
				viewCam.ModeMove();
			}
			GUILayout.FlexibleSpace();
			flag = GUILayout.Toggle(CamMode == 2, string.Empty, "Zoom");
			if (CamMode != 2 && flag)
			{
				CamMode = 2;
				viewCam.ModeZoom();
			}
			GUILayout.FlexibleSpace();
			flag = GUILayout.Toggle(CamModeFix, string.Empty, "Fix");
			if (CamModeFix != flag)
			{
				CamModeFix = flag;
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
			string empty = string.Empty;
			empty += "Costume : " + (curModel + 1) + " / " + Extensions.get_length((System.Array)modelList) + " : " + curModelName + "\n";
			empty += "Animation : " + (curAnim + 1) + " / " + Extensions.get_length((System.Array)animationList) + " : " + curAnimName + "\n";
			empty += "BackGround : " + (curBG + 1) + " / " + Extensions.get_length((System.Array)backGroundList) + " : " + curBgName + "\n";
			empty += "Quality : " + lodTextList[(int)curLOD] + "\n";
			GUI.Box(textGuiBodyRect, empty);
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
		Debug.Log(_name);
		this.obj = ((GameObject)UnityEngine.Object.Instantiate(original)) as GameObject;
		SM = ((SkinnedMeshRenderer)this.obj.GetComponentInChildren(typeof(SkinnedMeshRenderer))) as SkinnedMeshRenderer;
		SM.quality = SkinQuality.Bone4;
		SM.updateWhenOffscreen = true;
		int num = 0;
		int i = 0;
		Material[] sharedMaterials = SM.GetComponent<Renderer>().sharedMaterials;
		for (int length = sharedMaterials.Length; i < length; i++)
		{
			if (sharedMaterials[i].name == "face02_M")
			{
				SM.GetComponent<Renderer>().materials[num] = faceMat_M;
			}
			else if (sharedMaterials[i].name == "face02_L")
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
		viewCam.ModelTarget(GetBone(this.obj, boneName));
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
		if (!string.IsNullOrEmpty(_name))
		{
			MonoBehaviour.print("SetBackGround : " + _name);
			curBgName = Path.GetFileNameWithoutExtension(_name);
			Texture2D mainTexture = (Texture2D)Resources.Load(_name, typeof(Texture2D));
			GameObject gameObject = GameObject.Find("BillBoard") as GameObject;
			gameObject.GetComponent<Renderer>().material.mainTexture = mainTexture;
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
			MonoBehaviour.print("Change Texture To " + innerText + " : " + innerText2 + " : " + innerText3);
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
			MonoBehaviour.print("Change Animation To " + curAnimName);
			_obj.GetComponent<Animation>().Play(curAnimName);
		}
	}

	public virtual void Main()
	{
	}
}
