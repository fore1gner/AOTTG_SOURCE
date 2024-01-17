using System.Collections;
using ApplicationManagers;
using Constants;
using Settings;
using UI;
using UnityEngine;
using Weather;

internal class IN_GAME_MAIN_CAMERA : MonoBehaviour
{
	public enum RotationAxes
	{
		MouseXAndY,
		MouseX,
		MouseY
	}

	public RotationAxes axes;

	public AudioSource bgmusic;

	public static float cameraDistance = 0.6f;

	public static CAMERA_TYPE cameraMode;

	public static int character = 1;

	private float closestDistance;

	private int currentPeekPlayerIndex;

	private float decay;

	public static int difficulty;

	private float distance = 10f;

	private float distanceMulti;

	private float distanceOffsetMulti;

	private float duration;

	private float flashDuration;

	private bool flip;

	public static GAMEMODE gamemode;

	public bool gameOver;

	public static GAMETYPE gametype = GAMETYPE.STOP;

	private bool hasSnapShot;

	private Transform head;

	private float height = 5f;

	private float heightDamping = 2f;

	private float heightMulti;

	public static bool isCheating;

	public static bool isTyping;

	public float justHit;

	public int lastScore;

	public static int level;

	private bool lockAngle;

	private Vector3 lockCameraPosition;

	private GameObject locker;

	private GameObject lockTarget;

	public GameObject main_object;

	public float maximumX = 360f;

	public float maximumY = 60f;

	public float minimumX = -360f;

	public float minimumY = -60f;

	public static bool needSetHUD;

	private float R;

	private float rotationY;

	public int score;

	public static string singleCharacter;

	public Material skyBoxDAWN;

	public Material skyBoxDAY;

	public Material skyBoxNIGHT;

	public GameObject snapShotCamera;

	public RenderTexture snapshotRT;

	public bool spectatorMode;

	public static STEREO_3D_TYPE stereoType;

	private Transform target;

	public Texture texture;

	public float timer;

	public static bool triggerAutoLock;

	public static bool usingTitan;

	private Vector3 verticalHeightOffset = Vector3.zero;

	private float verticalRotationOffset;

	private float xSpeed = -3f;

	private float ySpeed = -0.8f;

	public static IN_GAME_MAIN_CAMERA Instance;

	private Transform _transform;

	private UILabel _bottomRightText;

	private static float _lastRestartTime = 0f;

	private void Awake()
	{
		this.Cache();
		IN_GAME_MAIN_CAMERA.Instance = this;
		IN_GAME_MAIN_CAMERA.isTyping = false;
		GameMenu.TogglePause(pause: false);
		base.name = "MainCamera";
		IN_GAME_MAIN_CAMERA.ApplyGraphicsSettings();
		this.CreateMinimap();
		WeatherManager.TakeFlashlight(base.transform);
	}

	public static void ApplyGraphicsSettings()
	{
		Camera main = Camera.main;
		GraphicsSettings graphicsSettings = SettingsManager.GraphicsSettings;
		if (graphicsSettings != null && main != null)
		{
			main.farClipPlane = graphicsSettings.RenderDistance.Value;
			if (!FengGameManagerMKII.level.StartsWith("Custom"))
			{
				main.GetComponent<TiltShift>().enabled = graphicsSettings.BlurEnabled.Value;
			}
			else
			{
				main.GetComponent<TiltShift>().enabled = false;
			}
		}
	}

	private void Cache()
	{
		this._transform = base.transform;
	}

	private void camareMovement()
	{
		Camera camera = base.camera;
		this.distanceOffsetMulti = IN_GAME_MAIN_CAMERA.cameraDistance * (200f - camera.fieldOfView) / 150f;
		this._transform.position = ((this.head == null) ? this.main_object.transform.position : this.head.position);
		this._transform.position += Vector3.up * this.heightMulti;
		this._transform.position -= Vector3.up * (0.6f - IN_GAME_MAIN_CAMERA.cameraDistance) * 2f;
		float num = SettingsManager.GeneralSettings.MouseSpeed.Value;
		int num2 = ((!SettingsManager.GeneralSettings.InvertMouse.Value) ? 1 : (-1));
		if (GameMenu.InMenu())
		{
			num = 0f;
		}
		if (IN_GAME_MAIN_CAMERA.cameraMode == CAMERA_TYPE.WOW)
		{
			if (Input.GetKey(KeyCode.Mouse1))
			{
				float angle = Input.GetAxis("Mouse X") * 10f * num;
				float angle2 = (0f - Input.GetAxis("Mouse Y")) * 10f * num * (float)num2;
				this._transform.RotateAround(this._transform.position, Vector3.up, angle);
				this._transform.RotateAround(this._transform.position, this._transform.right, angle2);
			}
			this._transform.position -= this._transform.transform.forward * this.distance * this.distanceMulti * this.distanceOffsetMulti;
		}
		else if (IN_GAME_MAIN_CAMERA.cameraMode == CAMERA_TYPE.ORIGINAL)
		{
			float num3 = 0f;
			if (Input.mousePosition.x < (float)Screen.width * 0.4f)
			{
				num3 = (0f - ((float)Screen.width * 0.4f - Input.mousePosition.x) / (float)Screen.width * 0.4f) * this.getSensitivityMultiWithDeltaTime(num) * 150f;
				this._transform.RotateAround(this._transform.position, Vector3.up, num3);
			}
			else if (Input.mousePosition.x > (float)Screen.width * 0.6f)
			{
				num3 = (Input.mousePosition.x - (float)Screen.width * 0.6f) / (float)Screen.width * 0.4f * this.getSensitivityMultiWithDeltaTime(num) * 150f;
				this._transform.RotateAround(this._transform.position, Vector3.up, num3);
			}
			float x = 140f * ((float)Screen.height * 0.6f - Input.mousePosition.y) / (float)Screen.height * 0.5f;
			this._transform.rotation = Quaternion.Euler(x, this._transform.rotation.eulerAngles.y, this._transform.rotation.eulerAngles.z);
			this._transform.position -= this._transform.forward * this.distance * this.distanceMulti * this.distanceOffsetMulti;
		}
		else if (IN_GAME_MAIN_CAMERA.cameraMode == CAMERA_TYPE.TPS)
		{
			float angle3 = Input.GetAxis("Mouse X") * 10f * num;
			float num4 = (0f - Input.GetAxis("Mouse Y")) * 10f * num * (float)num2;
			this._transform.RotateAround(this._transform.position, Vector3.up, angle3);
			float num5 = this._transform.rotation.eulerAngles.x % 360f;
			float num6 = num5 + num4;
			if ((num4 <= 0f || ((num5 >= 260f || num6 <= 260f) && (num5 >= 80f || num6 <= 80f))) && (num4 >= 0f || ((num5 <= 280f || num6 >= 280f) && (num5 <= 100f || num6 >= 100f))))
			{
				this._transform.RotateAround(this._transform.position, this._transform.right, num4);
			}
			this._transform.position -= this._transform.forward * this.distance * this.distanceMulti * this.distanceOffsetMulti;
		}
		if (IN_GAME_MAIN_CAMERA.cameraDistance < 0.65f)
		{
			this._transform.position += this._transform.right * Mathf.Max((0.6f - IN_GAME_MAIN_CAMERA.cameraDistance) * 2f, 0.65f);
		}
	}

	public void CameraMovementLive(HERO hero)
	{
		float magnitude = hero.rigidbody.velocity.magnitude;
		if (magnitude > 10f)
		{
			Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, Mathf.Min(100f, magnitude + 40f), 0.1f);
		}
		else
		{
			Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 50f, 0.1f);
		}
		float num = hero.CameraMultiplier * (200f - Camera.main.fieldOfView) / 150f;
		base.transform.position = this.head.transform.position + Vector3.up * this.heightMulti - Vector3.up * (0.6f - IN_GAME_MAIN_CAMERA.cameraDistance) * 2f;
		base.transform.position -= base.transform.forward * this.distance * this.distanceMulti * num;
		if (hero.CameraMultiplier < 0.65f)
		{
			base.transform.position += base.transform.right * Mathf.Max((0.6f - hero.CameraMultiplier) * 2f, 0.65f);
		}
		base.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, hero.GetComponent<SmoothSyncMovement>().correctCameraRot, Time.deltaTime * 5f);
	}

	private void CreateMinimap()
	{
		LevelInfo ınfo = LevelInfo.getInfo(FengGameManagerMKII.level);
		if (ınfo != null)
		{
			Minimap minimap = base.gameObject.AddComponent<Minimap>();
			if (Minimap.instance.myCam == null)
			{
				Minimap.instance.myCam = new GameObject().AddComponent<Camera>();
				Minimap.instance.myCam.nearClipPlane = 0.3f;
				Minimap.instance.myCam.farClipPlane = 1000f;
				Minimap.instance.myCam.enabled = false;
			}
			if (!SettingsManager.GeneralSettings.MinimapEnabled.Value || SettingsManager.LegacyGameSettings.GlobalMinimapDisable.Value)
			{
				minimap.SetEnabled(enabled: false);
				Minimap.instance.myCam.gameObject.SetActive(value: false);
			}
			else
			{
				Minimap.instance.myCam.gameObject.SetActive(value: true);
				minimap.CreateMinimap(Minimap.instance.myCam, 512, 0.3f, ınfo.minimapPreset);
			}
		}
	}

	public void createSnapShotRT2()
	{
		if (this.snapshotRT != null)
		{
			this.snapshotRT.Release();
		}
		if (SettingsManager.GeneralSettings.SnapshotsEnabled.Value)
		{
			this.snapShotCamera.SetActive(value: true);
			this.snapshotRT = new RenderTexture((int)((float)Screen.width * 0.4f), (int)((float)Screen.height * 0.4f), 24);
			this.snapShotCamera.GetComponent<Camera>().targetTexture = this.snapshotRT;
		}
		else
		{
			this.snapShotCamera.SetActive(value: false);
		}
	}

	private GameObject findNearestTitan()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("titan");
		GameObject result = null;
		float num = (this.closestDistance = float.PositiveInfinity);
		Vector3 position = this.main_object.transform.position;
		GameObject[] array2 = array;
		foreach (GameObject gameObject in array2)
		{
			float magnitude = (gameObject.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck").position - position).magnitude;
			if (magnitude < num && (gameObject.GetComponent<TITAN>() == null || !gameObject.GetComponent<TITAN>().hasDie))
			{
				result = gameObject;
				num = (this.closestDistance = magnitude);
			}
		}
		return result;
	}

	public void flashBlind()
	{
		GameObject.Find("flash").GetComponent<UISprite>().alpha = 1f;
		this.flashDuration = 2f;
	}

	private float getSensitivityMultiWithDeltaTime(float sensitivity)
	{
		return sensitivity * Time.deltaTime * 62f;
	}

	private void reset()
	{
		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
		{
			GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().restartGameSingle2();
		}
	}

	private Texture2D RTImage2(Camera cam)
	{
		RenderTexture renderTexture = RenderTexture.active;
		RenderTexture.active = cam.targetTexture;
		cam.Render();
		Texture2D texture2D = new Texture2D(cam.targetTexture.width, cam.targetTexture.height, TextureFormat.RGB24, mipmap: false);
		int num = (int)((float)cam.targetTexture.width * 0.04f);
		int num2 = (int)((float)cam.targetTexture.width * 0.02f);
		try
		{
			texture2D.SetPixel(0, 0, Color.white);
			texture2D.ReadPixels(new Rect(num, num, cam.targetTexture.width - num, cam.targetTexture.height - num), num2, num2);
			RenderTexture.active = renderTexture;
			return texture2D;
		}
		catch
		{
			texture2D = new Texture2D(1, 1);
			texture2D.SetPixel(0, 0, Color.white);
			return texture2D;
		}
	}

	public void UpdateSnapshotSkybox()
	{
		this.snapShotCamera.gameObject.GetComponent<Skybox>().material = base.gameObject.GetComponent<Skybox>().material;
	}

	private void UpdateBottomRightText()
	{
		if (this._bottomRightText == null)
		{
			GameObject gameObject = GameObject.Find("LabelInfoBottomRight");
			if (gameObject != null)
			{
				this._bottomRightText = gameObject.GetComponent<UILabel>();
			}
		}
		if (!(this._bottomRightText != null))
		{
			return;
		}
		this._bottomRightText.text = "Pause : " + SettingsManager.InputSettings.General.Pause.ToString() + " ";
		if (SettingsManager.UISettings.ShowInterpolation.Value && this.main_object != null)
		{
			HERO component = this.main_object.GetComponent<HERO>();
			if (component != null && component.baseRigidBody.interpolation == RigidbodyInterpolation.Interpolate)
			{
				this._bottomRightText.text = "Interpolation : ON \n" + this._bottomRightText.text;
			}
			else
			{
				this._bottomRightText.text = "Interpolation: OFF \n" + this._bottomRightText.text;
			}
		}
	}

	public void setHUDposition()
	{
		GameObject.Find("Flare").transform.localPosition = new Vector3((int)((float)(-Screen.width) * 0.5f) + 14, (int)((float)(-Screen.height) * 0.5f), 0f);
		GameObject.Find("LabelInfoBottomRight").transform.localPosition = new Vector3((int)((float)Screen.width * 0.5f), (int)((float)(-Screen.height) * 0.5f), 0f);
		GameObject.Find("LabelInfoTopCenter").transform.localPosition = new Vector3(0f, (int)((float)Screen.height * 0.5f), 0f);
		GameObject.Find("LabelInfoTopRight").transform.localPosition = new Vector3((int)((float)Screen.width * 0.5f), (int)((float)Screen.height * 0.5f), 0f);
		GameObject.Find("LabelNetworkStatus").transform.localPosition = new Vector3((int)((float)(-Screen.width) * 0.5f), (int)((float)Screen.height * 0.5f), 0f);
		GameObject.Find("LabelInfoTopLeft").transform.localPosition = new Vector3((int)((float)(-Screen.width) * 0.5f), (int)((float)Screen.height * 0.5f - 20f), 0f);
		GameObject.Find("Chatroom").transform.localPosition = new Vector3((int)((float)(-Screen.width) * 0.5f), (int)((float)(-Screen.height) * 0.5f), 0f);
		if (GameObject.Find("Chatroom") != null)
		{
			GameObject.Find("Chatroom").GetComponent<InRoomChat>().setPosition();
		}
		if (!IN_GAME_MAIN_CAMERA.usingTitan || IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
		{
			GameObject.Find("skill_cd_bottom").transform.localPosition = new Vector3(0f, (int)((float)(-Screen.height) * 0.5f + 5f), 0f);
			GameObject.Find("GasUI").transform.localPosition = GameObject.Find("skill_cd_bottom").transform.localPosition;
			GameObject.Find("stamina_titan").transform.localPosition = new Vector3(0f, 9999f, 0f);
			GameObject.Find("stamina_titan_bottom").transform.localPosition = new Vector3(0f, 9999f, 0f);
		}
		else
		{
			Vector3 localPosition = new Vector3(0f, 9999f, 0f);
			GameObject.Find("skill_cd_bottom").transform.localPosition = localPosition;
			GameObject.Find("skill_cd_armin").transform.localPosition = localPosition;
			GameObject.Find("skill_cd_eren").transform.localPosition = localPosition;
			GameObject.Find("skill_cd_jean").transform.localPosition = localPosition;
			GameObject.Find("skill_cd_levi").transform.localPosition = localPosition;
			GameObject.Find("skill_cd_marco").transform.localPosition = localPosition;
			GameObject.Find("skill_cd_mikasa").transform.localPosition = localPosition;
			GameObject.Find("skill_cd_petra").transform.localPosition = localPosition;
			GameObject.Find("skill_cd_sasha").transform.localPosition = localPosition;
			GameObject.Find("GasUI").transform.localPosition = localPosition;
			GameObject.Find("stamina_titan").transform.localPosition = new Vector3(-160f, (int)((float)(-Screen.height) * 0.5f + 15f), 0f);
			GameObject.Find("stamina_titan_bottom").transform.localPosition = new Vector3(-160f, (int)((float)(-Screen.height) * 0.5f + 15f), 0f);
		}
		if (this.main_object != null && this.main_object.GetComponent<HERO>() != null)
		{
			if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
			{
				this.main_object.GetComponent<HERO>().setSkillHUDPosition2();
			}
			else if (this.main_object.GetPhotonView() != null && this.main_object.GetPhotonView().isMine)
			{
				this.main_object.GetComponent<HERO>().setSkillHUDPosition2();
			}
		}
		if (IN_GAME_MAIN_CAMERA.stereoType == STEREO_3D_TYPE.SIDE_BY_SIDE)
		{
			base.gameObject.GetComponent<Camera>().aspect = Screen.width / Screen.height;
		}
		this.createSnapShotRT2();
	}

	public GameObject setMainObject(GameObject obj, bool resetRotation = true, bool lockAngle = false)
	{
		this.main_object = obj;
		if (obj == null)
		{
			this.head = null;
			this.distanceMulti = (this.heightMulti = 1f);
		}
		else if (this.main_object.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck/head") != null)
		{
			this.head = this.main_object.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck/head");
			this.distanceMulti = ((this.head != null) ? (Vector3.Distance(this.head.transform.position, this.main_object.transform.position) * 0.2f) : 1f);
			this.heightMulti = ((this.head != null) ? (Vector3.Distance(this.head.transform.position, this.main_object.transform.position) * 0.33f) : 1f);
			if (resetRotation)
			{
				base.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
			}
		}
		else if (this.main_object.transform.Find("Amarture/Controller_Body/hip/spine/chest/neck/head") != null)
		{
			this.head = this.main_object.transform.Find("Amarture/Controller_Body/hip/spine/chest/neck/head");
			this.distanceMulti = (this.heightMulti = 0.64f);
			if (resetRotation)
			{
				base.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
			}
		}
		else
		{
			this.head = null;
			this.distanceMulti = (this.heightMulti = 1f);
			if (resetRotation)
			{
				base.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
			}
		}
		this.lockAngle = lockAngle;
		return obj;
	}

	public GameObject setMainObjectASTITAN(GameObject obj)
	{
		this.main_object = obj;
		if (this.main_object.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck/head") != null)
		{
			this.head = this.main_object.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck/head");
			this.distanceMulti = ((this.head != null) ? (Vector3.Distance(this.head.transform.position, this.main_object.transform.position) * 0.4f) : 1f);
			this.heightMulti = ((this.head != null) ? (Vector3.Distance(this.head.transform.position, this.main_object.transform.position) * 0.45f) : 1f);
			base.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
		}
		return obj;
	}

	public void setSpectorMode(bool val)
	{
		this.spectatorMode = val;
		GameObject.Find("MainCamera").GetComponent<SpectatorMovement>().disable = !val;
		GameObject.Find("MainCamera").GetComponent<MouseLook>().disable = !val;
	}

	private void shakeUpdate()
	{
		if (this.duration > 0f)
		{
			this.duration -= Time.deltaTime;
			if (this.flip)
			{
				base.gameObject.transform.position += Vector3.up * this.R;
			}
			else
			{
				base.gameObject.transform.position -= Vector3.up * this.R;
			}
			this.flip = !this.flip;
			this.R *= this.decay;
		}
	}

	private void Start()
	{
		GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>().addCamera(this);
		this.locker = GameObject.Find("locker");
		IN_GAME_MAIN_CAMERA.cameraDistance = SettingsManager.GeneralSettings.CameraDistance.Value + 0.3f;
		base.camera.farClipPlane = SettingsManager.GraphicsSettings.RenderDistance.Value;
		this.createSnapShotRT2();
	}

	public void startShake(float R, float duration, float decay = 0.95f)
	{
		if (this.duration < duration)
		{
			this.R = R;
			this.duration = duration;
			this.decay = decay;
		}
	}

	public void startSnapShot2(Vector3 p, int dmg, GameObject target, float startTime)
	{
		if (this.snapShotCamera.activeSelf && dmg >= SettingsManager.GeneralSettings.SnapshotsMinimumDamage.Value)
		{
			base.StartCoroutine(this.CreateSnapshot(p, dmg, target, startTime));
		}
	}

	private IEnumerator CreateSnapshot(Vector3 position, int damage, GameObject target, float startTime)
	{
		UITexture display = GameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[0].transform.Find("snapshot1").GetComponent<UITexture>();
		yield return new WaitForSeconds(startTime);
		this.SetSnapshotPosition(target, position);
		Texture2D snapshot = this.RTImage2(this.snapShotCamera.GetComponent<Camera>());
		yield return new WaitForSeconds(0.2f);
		snapshot.Apply();
		display.mainTexture = snapshot;
		display.transform.localScale = new Vector3((float)Screen.width * 0.4f, (float)Screen.height * 0.4f, 1f);
		display.transform.localPosition = new Vector3((float)(-Screen.width) * 0.225f, (float)Screen.height * 0.225f, 0f);
		display.transform.rotation = Quaternion.Euler(0f, 0f, 10f);
		if (SettingsManager.GeneralSettings.SnapshotsShowInGame.Value)
		{
			display.enabled = true;
		}
		else
		{
			display.enabled = false;
		}
		yield return new WaitForSeconds(0.2f);
		SnapshotManager.AddSnapshot(snapshot, damage);
		yield return new WaitForSeconds(2f);
		display.enabled = false;
		Object.Destroy(snapshot);
	}

	private void SetSnapshotPosition(GameObject target, Vector3 snapshotPosition)
	{
		this.snapShotCamera.transform.position = ((this.head == null) ? this.main_object.transform.position : this.head.transform.position);
		this.snapShotCamera.transform.position += Vector3.up * this.heightMulti;
		this.snapShotCamera.transform.position -= Vector3.up * 1.1f;
		Vector3 position;
		Vector3 vector = (position = this.snapShotCamera.transform.position);
		Vector3 vector2 = (vector + snapshotPosition) * 0.5f;
		this.snapShotCamera.transform.position = vector2;
		vector = vector2;
		this.snapShotCamera.transform.LookAt(snapshotPosition);
		this.snapShotCamera.transform.RotateAround(base.transform.position, Vector3.up, Random.Range(-20f, 20f));
		this.snapShotCamera.transform.LookAt(vector);
		this.snapShotCamera.transform.RotateAround(vector, base.transform.right, Random.Range(-20f, 20f));
		float num = Vector3.Distance(snapshotPosition, position);
		if (target != null && target.GetComponent<TITAN>() != null)
		{
			num += target.transform.localScale.x * 15f;
		}
		this.snapShotCamera.transform.position -= this.snapShotCamera.transform.forward * Random.Range(num + 3f, num + 10f);
		this.snapShotCamera.transform.LookAt(vector);
		this.snapShotCamera.transform.RotateAround(vector, base.transform.forward, Random.Range(-30f, 30f));
		Vector3 end = ((this.head == null) ? this.main_object.transform.position : this.head.transform.position);
		Vector3 vector3 = ((this.head == null) ? this.main_object.transform.position : this.head.transform.position) - this.snapShotCamera.transform.position;
		end -= vector3;
		LayerMask layerMask = 1 << LayerMask.NameToLayer("Ground");
		LayerMask layerMask2 = 1 << LayerMask.NameToLayer("EnemyBox");
		LayerMask layerMask3 = (int)layerMask | (int)layerMask2;
		RaycastHit hitInfo;
		if (this.head != null)
		{
			if (Physics.Linecast(this.head.transform.position, end, out hitInfo, layerMask))
			{
				this.snapShotCamera.transform.position = hitInfo.point;
			}
			else if (Physics.Linecast(this.head.transform.position - vector3 * this.distanceMulti * 3f, end, out hitInfo, layerMask3))
			{
				this.snapShotCamera.transform.position = hitInfo.point;
			}
		}
		else if (Physics.Linecast(this.main_object.transform.position + Vector3.up, end, out hitInfo, layerMask3))
		{
			this.snapShotCamera.transform.position = hitInfo.point;
		}
	}

	public void update2()
	{
		this.UpdateBottomRightText();
		if (this.flashDuration > 0f)
		{
			this.flashDuration -= Time.deltaTime;
			if (this.flashDuration <= 0f)
			{
				this.flashDuration = 0f;
			}
			GameObject.Find("flash").GetComponent<UISprite>().alpha = this.flashDuration * 0.5f;
		}
		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.STOP)
		{
			return;
		}
		if (IN_GAME_MAIN_CAMERA.gametype != 0 && this.gameOver)
		{
			if (SettingsManager.InputSettings.Human.AttackSpecial.GetKeyDown())
			{
				if (this.spectatorMode)
				{
					this.setSpectorMode(val: false);
				}
				else
				{
					this.setSpectorMode(val: true);
				}
			}
			if (SettingsManager.InputSettings.General.SpectateNextPlayer.GetKeyDown())
			{
				this.currentPeekPlayerIndex++;
				int num = GameObject.FindGameObjectsWithTag("Player").Length;
				if (this.currentPeekPlayerIndex >= num)
				{
					this.currentPeekPlayerIndex = 0;
				}
				if (num > 0)
				{
					this.setMainObject(GameObject.FindGameObjectsWithTag("Player")[this.currentPeekPlayerIndex]);
					this.setSpectorMode(val: false);
					this.lockAngle = false;
				}
			}
			if (SettingsManager.InputSettings.General.SpectatePreviousPlayer.GetKeyDown())
			{
				this.currentPeekPlayerIndex--;
				int num2 = GameObject.FindGameObjectsWithTag("Player").Length;
				if (this.currentPeekPlayerIndex >= num2)
				{
					this.currentPeekPlayerIndex = 0;
				}
				if (this.currentPeekPlayerIndex < 0)
				{
					this.currentPeekPlayerIndex = num2 - 1;
				}
				if (num2 > 0)
				{
					this.setMainObject(GameObject.FindGameObjectsWithTag("Player")[this.currentPeekPlayerIndex]);
					this.setSpectorMode(val: false);
					this.lockAngle = false;
				}
			}
			if (this.spectatorMode)
			{
				return;
			}
		}
		if (GameMenu.Paused)
		{
			if (this.main_object != null)
			{
				Vector3 position = base.transform.position;
				position = ((this.head == null) ? this.main_object.transform.position : this.head.transform.position);
				position += Vector3.up * this.heightMulti;
				base.transform.position = Vector3.Lerp(base.transform.position, position - base.transform.forward * 5f, 0.2f);
			}
			return;
		}
		if (SettingsManager.InputSettings.General.Pause.GetKeyDown())
		{
			GameMenu.TogglePause(pause: true);
		}
		if (IN_GAME_MAIN_CAMERA.needSetHUD)
		{
			IN_GAME_MAIN_CAMERA.needSetHUD = false;
			this.setHUDposition();
		}
		if (SettingsManager.InputSettings.General.ToggleFullscreen.GetKeyDown())
		{
			FullscreenHandler.ToggleFullscreen();
			IN_GAME_MAIN_CAMERA.needSetHUD = true;
		}
		if (SettingsManager.InputSettings.General.RestartGame.GetKeyDown())
		{
			float num3 = Time.realtimeSinceStartup - IN_GAME_MAIN_CAMERA._lastRestartTime;
			if (IN_GAME_MAIN_CAMERA.gametype != 0 && PhotonNetwork.isMasterClient && num3 > 2f)
			{
				IN_GAME_MAIN_CAMERA._lastRestartTime = Time.realtimeSinceStartup;
				object[] parameters = new object[2] { "<color=#FFCC00>MasterClient has restarted the game!</color>", "" };
				FengGameManagerMKII.instance.photonView.RPC("Chat", PhotonTargets.All, parameters);
				FengGameManagerMKII.instance.restartRC();
			}
		}
		if (SettingsManager.InputSettings.General.RestartGame.GetKeyDown() || SettingsManager.InputSettings.General.ChangeCharacter.GetKeyDown())
		{
			this.reset();
		}
		if (!(this.main_object != null))
		{
			return;
		}
		if (SettingsManager.InputSettings.General.ChangeCamera.GetKeyDown())
		{
			if (IN_GAME_MAIN_CAMERA.cameraMode == CAMERA_TYPE.ORIGINAL)
			{
				IN_GAME_MAIN_CAMERA.cameraMode = CAMERA_TYPE.WOW;
			}
			else if (IN_GAME_MAIN_CAMERA.cameraMode == CAMERA_TYPE.WOW)
			{
				IN_GAME_MAIN_CAMERA.cameraMode = CAMERA_TYPE.TPS;
			}
			else if (IN_GAME_MAIN_CAMERA.cameraMode == CAMERA_TYPE.TPS)
			{
				IN_GAME_MAIN_CAMERA.cameraMode = CAMERA_TYPE.ORIGINAL;
			}
			this.verticalRotationOffset = 0f;
		}
		if (SettingsManager.InputSettings.General.HideUI.GetKeyDown())
		{
			GameMenu.HideCrosshair = !GameMenu.HideCrosshair;
		}
		if (SettingsManager.InputSettings.Human.FocusTitan.GetKeyDown())
		{
			IN_GAME_MAIN_CAMERA.triggerAutoLock = !IN_GAME_MAIN_CAMERA.triggerAutoLock;
			if (IN_GAME_MAIN_CAMERA.triggerAutoLock)
			{
				this.lockTarget = this.findNearestTitan();
				if (this.closestDistance >= 150f)
				{
					this.lockTarget = null;
					IN_GAME_MAIN_CAMERA.triggerAutoLock = false;
				}
			}
		}
		if (this.gameOver && this.main_object != null)
		{
			if (SettingsManager.InputSettings.General.SpectateToggleLive.GetKeyDown())
			{
				SettingsManager.LegacyGeneralSettings.LiveSpectate.Value = !SettingsManager.LegacyGeneralSettings.LiveSpectate.Value;
			}
			HERO component = this.main_object.GetComponent<HERO>();
			if (component != null && SettingsManager.LegacyGeneralSettings.LiveSpectate.Value && component.GetComponent<SmoothSyncMovement>().enabled && component.isPhotonCamera)
			{
				this.CameraMovementLive(component);
			}
			else if (this.lockAngle)
			{
				base.transform.rotation = Quaternion.Lerp(base.transform.rotation, this.main_object.transform.rotation, 0.2f);
				base.transform.position = Vector3.Lerp(base.transform.position, this.main_object.transform.position - this.main_object.transform.forward * 5f, 0.2f);
			}
			else
			{
				this.camareMovement();
			}
		}
		else
		{
			this.camareMovement();
		}
		if (IN_GAME_MAIN_CAMERA.triggerAutoLock && this.lockTarget != null)
		{
			float z = base.transform.eulerAngles.z;
			Transform transform = this.lockTarget.transform.Find("Amarture/Core/Controller_Body/hip/spine/chest/neck");
			Vector3 vector = transform.position - ((this.head == null) ? this.main_object.transform.position : this.head.transform.position);
			vector.Normalize();
			this.lockCameraPosition = ((this.head == null) ? this.main_object.transform.position : this.head.transform.position);
			this.lockCameraPosition -= vector * this.distance * this.distanceMulti * this.distanceOffsetMulti;
			this.lockCameraPosition += Vector3.up * 3f * this.heightMulti * this.distanceOffsetMulti;
			base.transform.position = Vector3.Lerp(base.transform.position, this.lockCameraPosition, Time.deltaTime * 4f);
			if (this.head != null)
			{
				base.transform.LookAt(this.head.transform.position * 0.8f + transform.position * 0.2f);
			}
			else
			{
				base.transform.LookAt(this.main_object.transform.position * 0.8f + transform.position * 0.2f);
			}
			base.transform.localEulerAngles = new Vector3(base.transform.eulerAngles.x, base.transform.eulerAngles.y, z);
			Vector2 vector2 = base.camera.WorldToScreenPoint(transform.position - transform.forward * this.lockTarget.transform.localScale.x);
			this.locker.transform.localPosition = new Vector3(vector2.x - (float)Screen.width * 0.5f, vector2.y - (float)Screen.height * 0.5f, 0f);
			if (this.lockTarget.GetComponent<TITAN>() != null && this.lockTarget.GetComponent<TITAN>().hasDie)
			{
				this.lockTarget = null;
			}
		}
		else
		{
			this.locker.transform.localPosition = new Vector3(0f, (float)(-Screen.height) * 0.5f - 50f, 0f);
		}
		Vector3 end = ((this.head == null) ? this.main_object.transform.position : this.head.position);
		Vector3 normalized = (((this.head == null) ? this.main_object.transform.position : this.head.position) - this._transform.position).normalized;
		end -= this.distance * normalized * this.distanceMulti;
		LayerMask layerMask = 1 << PhysicsLayer.Ground;
		LayerMask layerMask2 = 1 << PhysicsLayer.EnemyBox;
		LayerMask layerMask3 = (int)layerMask | (int)layerMask2;
		RaycastHit hitInfo;
		if (this.head != null)
		{
			if (Physics.Linecast(this.head.position, end, out hitInfo, layerMask))
			{
				this._transform.position = hitInfo.point;
			}
			else if (Physics.Linecast(this.head.position - normalized * this.distanceMulti * 3f, end, out hitInfo, layerMask2))
			{
				this._transform.position = hitInfo.point;
			}
		}
		else if (Physics.Linecast(this.main_object.transform.position + Vector3.up, end, out hitInfo, layerMask3))
		{
			this._transform.position = hitInfo.point;
		}
		this.shakeUpdate();
	}
}
