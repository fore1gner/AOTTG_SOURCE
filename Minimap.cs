using System.Collections;
using Settings;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class Minimap : MonoBehaviour
{
	public enum IconStyle
	{
		CIRCLE,
		SUPPLY
	}

	public class MinimapIcon
	{
		private Transform obj;

		private RectTransform pointerRect;

		public readonly bool rotation;

		public readonly IconStyle style;

		private RectTransform uiRect;

		public MinimapIcon(GameObject trackedObject, GameObject uiElement, IconStyle style)
		{
			this.rotation = false;
			this.style = style;
			this.obj = trackedObject.transform;
			this.uiRect = uiElement.GetComponent<RectTransform>();
			CatchDestroy component = this.obj.GetComponent<CatchDestroy>();
			if (component == null)
			{
				this.obj.gameObject.AddComponent<CatchDestroy>().target = uiElement;
			}
			else if (component.target != null && component.target != uiElement)
			{
				Object.Destroy(component.target);
			}
			else
			{
				component.target = uiElement;
			}
		}

		public MinimapIcon(GameObject trackedObject, GameObject uiElement, GameObject uiPointer, IconStyle style)
		{
			this.rotation = true;
			this.style = style;
			this.obj = trackedObject.transform;
			this.uiRect = uiElement.GetComponent<RectTransform>();
			this.pointerRect = uiPointer.GetComponent<RectTransform>();
			CatchDestroy component = this.obj.GetComponent<CatchDestroy>();
			if (component == null)
			{
				this.obj.gameObject.AddComponent<CatchDestroy>().target = uiElement;
			}
			else if (component.target != null && component.target != uiElement)
			{
				Object.Destroy(component.target);
			}
			else
			{
				component.target = uiElement;
			}
		}

		public static MinimapIcon Create(RectTransform parent, GameObject trackedObject, IconStyle style)
		{
			UnityEngine.Sprite spriteForStyle = Minimap.GetSpriteForStyle(style);
			GameObject gameObject = new GameObject("MinimapIcon");
			RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
			Vector2 anchorMin = (rectTransform.anchorMax = new Vector3(0.5f, 0.5f));
			rectTransform.anchorMin = anchorMin;
			rectTransform.sizeDelta = new Vector2(spriteForStyle.texture.width, spriteForStyle.texture.height);
			Image ımage = gameObject.AddComponent<Image>();
			ımage.sprite = spriteForStyle;
			ımage.type = Image.Type.Simple;
			gameObject.transform.SetParent(parent, worldPositionStays: false);
			return new MinimapIcon(trackedObject, gameObject, style);
		}

		public static MinimapIcon CreateWithRotation(RectTransform parent, GameObject trackedObject, IconStyle style, float pointerDist)
		{
			UnityEngine.Sprite spriteForStyle = Minimap.GetSpriteForStyle(style);
			GameObject gameObject = new GameObject("MinimapIcon");
			RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
			Vector2 anchorMin = (rectTransform.anchorMax = new Vector3(0.5f, 0.5f));
			rectTransform.anchorMin = anchorMin;
			rectTransform.sizeDelta = new Vector2(spriteForStyle.texture.width, spriteForStyle.texture.height);
			Image ımage = gameObject.AddComponent<Image>();
			ımage.sprite = spriteForStyle;
			ımage.type = Image.Type.Simple;
			gameObject.transform.SetParent(parent, worldPositionStays: false);
			GameObject gameObject2 = new GameObject("IconPointer");
			RectTransform rectTransform2 = gameObject2.AddComponent<RectTransform>();
			anchorMin = (rectTransform2.anchorMax = rectTransform.anchorMin);
			rectTransform2.anchorMin = anchorMin;
			rectTransform2.sizeDelta = new Vector2(Minimap.pointerSprite.texture.width, Minimap.pointerSprite.texture.height);
			Image ımage2 = gameObject2.AddComponent<Image>();
			ımage2.sprite = Minimap.pointerSprite;
			ımage2.type = Image.Type.Simple;
			gameObject2.transform.SetParent(rectTransform, worldPositionStays: false);
			rectTransform2.anchoredPosition = new Vector2(0f, pointerDist);
			return new MinimapIcon(trackedObject, gameObject, gameObject2, style);
		}

		public void Destroy()
		{
			if (this.uiRect != null)
			{
				Object.Destroy(this.uiRect.gameObject);
			}
		}

		public void SetColor(Color color)
		{
			if (this.uiRect != null)
			{
				this.uiRect.GetComponent<Image>().color = color;
			}
		}

		public void SetDepth(bool aboveAll)
		{
			if (this.uiRect != null)
			{
				if (aboveAll)
				{
					this.uiRect.SetAsLastSibling();
				}
				else
				{
					this.uiRect.SetAsFirstSibling();
				}
			}
		}

		public void SetPointerSize(float size, float originDistance)
		{
			if (this.pointerRect != null)
			{
				this.pointerRect.sizeDelta = new Vector2(size, size);
				this.pointerRect.anchoredPosition = new Vector2(0f, originDistance);
			}
		}

		public void SetSize(Vector2 size)
		{
			if (this.uiRect != null)
			{
				this.uiRect.sizeDelta = size;
			}
		}

		public bool UpdateUI(Bounds worldBounds, float minimapSize)
		{
			if (this.obj == null)
			{
				return false;
			}
			float x = worldBounds.size.x;
			Vector3 vector = this.obj.position - worldBounds.center;
			vector.y = vector.z;
			vector.z = 0f;
			float num = Mathf.Abs(vector.x) / x;
			vector.x = ((vector.x < 0f) ? (0f - num) : num);
			float num2 = Mathf.Abs(vector.y) / x;
			vector.y = ((vector.y < 0f) ? (0f - num2) : num2);
			Vector2 anchoredPosition = vector * minimapSize;
			this.uiRect.anchoredPosition = anchoredPosition;
			if (this.rotation)
			{
				float z = Mathf.Atan2(this.obj.forward.z, this.obj.forward.x) * 57.29578f - 90f;
				this.uiRect.eulerAngles = new Vector3(0f, 0f, z);
			}
			return true;
		}
	}

	public class Preset
	{
		public readonly Vector3 center;

		public readonly float orthographicSize;

		public Preset(Vector3 center, float orthographicSize)
		{
			this.center = center;
			this.orthographicSize = orthographicSize;
		}
	}

	private bool assetsInitialized;

	private static UnityEngine.Sprite borderSprite;

	private RectTransform borderT;

	private Canvas canvas;

	private Vector2 cornerPosition;

	private float cornerSizeRatio;

	private Preset initialPreset;

	public static Minimap instance;

	private bool isEnabled;

	private bool isEnabledTemp;

	private Vector3 lastMinimapCenter;

	private float lastMinimapOrthoSize;

	private Camera lastUsedCamera;

	private bool maximized;

	private RectTransform minimap;

	private float MINIMAP_CORNER_SIZE;

	private float MINIMAP_CORNER_SIZE_SCALED;

	private Vector2 MINIMAP_ICON_SIZE;

	private float MINIMAP_POINTER_DIST;

	private float MINIMAP_POINTER_SIZE;

	private int MINIMAP_SIZE;

	private Vector2 MINIMAP_SUPPLY_SIZE;

	private MinimapIcon[] minimapIcons;

	private bool minimapIsCreated;

	private RectTransform minimapMaskT;

	private Bounds minimapOrthographicBounds;

	public RenderTexture minimapRT;

	public Camera myCam;

	private static UnityEngine.Sprite pointerSprite;

	private CanvasScaler scaler;

	private static UnityEngine.Sprite supplySprite;

	private static UnityEngine.Sprite whiteIconSprite;

	private void AddBorderToTexture(ref Texture2D texture, Color borderColor, int borderPixelSize)
	{
		int num = texture.width * borderPixelSize;
		Color[] array = new Color[num];
		for (int i = 0; i < num; i++)
		{
			array[i] = borderColor;
		}
		texture.SetPixels(0, texture.height - borderPixelSize, texture.width - 1, borderPixelSize, array);
		texture.SetPixels(0, 0, texture.width, borderPixelSize, array);
		texture.SetPixels(0, 0, borderPixelSize, texture.height, array);
		texture.SetPixels(texture.width - borderPixelSize, 0, borderPixelSize, texture.height, array);
		texture.Apply();
	}

	private void AutomaticSetCameraProperties(Camera cam)
	{
		Renderer[] array = Object.FindObjectsOfType<Renderer>();
		if (array.Length != 0)
		{
			this.minimapOrthographicBounds = new Bounds(array[0].transform.position, Vector3.zero);
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].gameObject.layer == 9)
				{
					this.minimapOrthographicBounds.Encapsulate(array[i].bounds);
				}
			}
		}
		Vector3 size = this.minimapOrthographicBounds.size;
		float num = ((size.x > size.z) ? size.x : size.z);
		size.z = (size.x = num);
		this.minimapOrthographicBounds.size = size;
		cam.orthographic = true;
		cam.orthographicSize = num * 0.5f;
		Vector3 center = this.minimapOrthographicBounds.center;
		center.y = cam.farClipPlane * 0.5f;
		Transform obj = cam.transform;
		obj.position = center;
		obj.eulerAngles = new Vector3(90f, 0f, 0f);
		cam.aspect = 1f;
		this.lastMinimapCenter = center;
		this.lastMinimapOrthoSize = cam.orthographicSize;
	}

	private void AutomaticSetOrthoBounds()
	{
		Renderer[] array = Object.FindObjectsOfType<Renderer>();
		if (array.Length != 0)
		{
			this.minimapOrthographicBounds = new Bounds(array[0].transform.position, Vector3.zero);
			for (int i = 0; i < array.Length; i++)
			{
				this.minimapOrthographicBounds.Encapsulate(array[i].bounds);
			}
		}
		Vector3 size = this.minimapOrthographicBounds.size;
		float num = ((size.x > size.z) ? size.x : size.z);
		size.z = (size.x = num);
		this.minimapOrthographicBounds.size = size;
		this.lastMinimapCenter = this.minimapOrthographicBounds.center;
		this.lastMinimapOrthoSize = num * 0.5f;
	}

	private void Awake()
	{
		Minimap.instance = this;
	}

	private Texture2D CaptureMinimap(Camera cam)
	{
		RenderTexture renderTexture = RenderTexture.active;
		RenderTexture.active = cam.targetTexture;
		cam.Render();
		Texture2D texture2D = new Texture2D(cam.targetTexture.width, cam.targetTexture.height, TextureFormat.RGB24, mipmap: false);
		texture2D.filterMode = FilterMode.Bilinear;
		texture2D.ReadPixels(new Rect(0f, 0f, cam.targetTexture.width, cam.targetTexture.height), 0, 0);
		texture2D.Apply();
		RenderTexture.active = renderTexture;
		return texture2D;
	}

	private void CaptureMinimapRT(Camera cam)
	{
		RenderTexture renderTexture = RenderTexture.active;
		RenderTexture.active = this.minimapRT;
		cam.targetTexture = this.minimapRT;
		cam.Render();
		RenderTexture.active = renderTexture;
	}

	private void CheckUserInput()
	{
		if (SettingsManager.GeneralSettings.MinimapEnabled.Value && !SettingsManager.LegacyGameSettings.GlobalMinimapDisable.Value)
		{
			if (!this.minimapIsCreated)
			{
				return;
			}
			if (SettingsManager.InputSettings.General.MinimapMaximize.GetKey())
			{
				if (!this.maximized)
				{
					this.Maximize();
				}
			}
			else if (this.maximized)
			{
				this.Minimize();
			}
			if (SettingsManager.InputSettings.General.MinimapToggle.GetKeyDown())
			{
				this.SetEnabled(!this.isEnabled);
			}
			if (!this.maximized)
			{
				return;
			}
			bool flag = false;
			if (SettingsManager.InputSettings.General.MinimapReset.GetKey())
			{
				if (this.initialPreset != null)
				{
					this.ManualSetCameraProperties(this.lastUsedCamera, this.initialPreset.center, this.initialPreset.orthographicSize);
				}
				else
				{
					this.AutomaticSetCameraProperties(this.lastUsedCamera);
				}
				flag = true;
			}
			else
			{
				float num = Input.GetAxis("Mouse ScrollWheel");
				if (num != 0f)
				{
					if (Input.GetKey(KeyCode.LeftShift))
					{
						num *= 3f;
					}
					this.lastMinimapOrthoSize = Mathf.Max(this.lastMinimapOrthoSize + num, 1f);
					flag = true;
				}
				if (Input.GetKey(KeyCode.UpArrow))
				{
					float num2 = Time.deltaTime * ((Input.GetKey(KeyCode.LeftShift) ? 2f : 0.75f) * this.lastMinimapOrthoSize);
					this.lastMinimapCenter.z += num2;
					flag = true;
				}
				else if (Input.GetKey(KeyCode.DownArrow))
				{
					float num2 = Time.deltaTime * ((Input.GetKey(KeyCode.LeftShift) ? 2f : 0.75f) * this.lastMinimapOrthoSize);
					this.lastMinimapCenter.z -= num2;
					flag = true;
				}
				if (Input.GetKey(KeyCode.RightArrow))
				{
					float num2 = Time.deltaTime * ((Input.GetKey(KeyCode.LeftShift) ? 2f : 0.75f) * this.lastMinimapOrthoSize);
					this.lastMinimapCenter.x += num2;
					flag = true;
				}
				else if (Input.GetKey(KeyCode.LeftArrow))
				{
					float num2 = Time.deltaTime * ((Input.GetKey(KeyCode.LeftShift) ? 2f : 0.75f) * this.lastMinimapOrthoSize);
					this.lastMinimapCenter.x -= num2;
					flag = true;
				}
			}
			if (flag)
			{
				this.RecaptureMinimap(this.lastUsedCamera, this.lastMinimapCenter, this.lastMinimapOrthoSize);
			}
		}
		else if (this.isEnabled)
		{
			this.SetEnabled(!this.isEnabled);
		}
	}

	public void CreateMinimap(Camera cam, int minimapResolution = 512, float cornerSize = 0.3f, Preset mapPreset = null)
	{
		if (Minimap.Supported())
		{
			this.isEnabled = true;
			this.lastUsedCamera = cam;
			if (!this.assetsInitialized)
			{
				this.Initialize();
			}
			GameObject gameObject = GameObject.Find("mainLight");
			Light light = null;
			Quaternion rotation = Quaternion.identity;
			LightShadows shadows = LightShadows.None;
			Color color = Color.clear;
			float intensity = 0f;
			float nearClipPlane = cam.nearClipPlane;
			float farClipPlane = cam.farClipPlane;
			int cullingMask = cam.cullingMask;
			if (gameObject != null)
			{
				light = gameObject.GetComponent<Light>();
				rotation = light.transform.rotation;
				shadows = light.shadows;
				intensity = light.intensity;
				color = light.color;
				light.shadows = LightShadows.None;
				light.color = Color.white;
				light.intensity = 0.5f;
				light.transform.eulerAngles = new Vector3(90f, 0f, 0f);
			}
			cam.nearClipPlane = 0.3f;
			cam.farClipPlane = 1000f;
			cam.cullingMask = 512;
			cam.clearFlags = CameraClearFlags.Color;
			this.MINIMAP_SIZE = minimapResolution;
			this.MINIMAP_CORNER_SIZE = (float)this.MINIMAP_SIZE * cornerSize;
			this.cornerSizeRatio = cornerSize;
			this.CreateMinimapRT(cam, minimapResolution);
			if (mapPreset != null)
			{
				this.initialPreset = mapPreset;
				this.ManualSetCameraProperties(cam, mapPreset.center, mapPreset.orthographicSize);
			}
			else
			{
				this.AutomaticSetCameraProperties(cam);
			}
			this.CaptureMinimapRT(cam);
			if (gameObject != null)
			{
				light.shadows = shadows;
				light.transform.rotation = rotation;
				light.color = color;
				light.intensity = intensity;
			}
			cam.nearClipPlane = nearClipPlane;
			cam.farClipPlane = farClipPlane;
			cam.cullingMask = cullingMask;
			cam.orthographic = false;
			cam.clearFlags = CameraClearFlags.Skybox;
			this.CreateUnityUIRT(minimapResolution);
			this.minimapIsCreated = true;
			base.StartCoroutine(this.HackRoutine());
		}
	}

	private void CreateMinimapRT(Camera cam, int pixelSize)
	{
		if (this.minimapRT == null)
		{
			bool num = SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.RGB565);
			this.minimapRT = new RenderTexture(pixelSize, pixelSize, 16, RenderTextureFormat.RGB565);
			if (!num)
			{
				Debug.Log(SystemInfo.graphicsDeviceName + " (" + SystemInfo.graphicsDeviceVendor + ") does not support RGB565 format, the minimap will have transparency issues on certain maps");
			}
		}
		cam.targetTexture = this.minimapRT;
	}

	private void CreateUnityUI(Texture2D map, int minimapResolution)
	{
		GameObject gameObject = new GameObject("Canvas");
		gameObject.AddComponent<RectTransform>();
		this.canvas = gameObject.AddComponent<Canvas>();
		this.canvas.renderMode = RenderMode.ScreenSpaceOverlay;
		this.scaler = gameObject.AddComponent<CanvasScaler>();
		this.scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
		this.scaler.referenceResolution = new Vector2(900f, 600f);
		this.scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
		GameObject gameObject2 = new GameObject("CircleMask");
		gameObject2.transform.SetParent(gameObject.transform, worldPositionStays: false);
		this.minimapMaskT = gameObject2.AddComponent<RectTransform>();
		gameObject2.AddComponent<CanvasRenderer>();
		RectTransform rectTransform = this.minimapMaskT;
		Vector2 anchorMin = (this.minimapMaskT.anchorMax = Vector2.one);
		rectTransform.anchorMin = anchorMin;
		float num = this.MINIMAP_CORNER_SIZE * 0.5f;
		this.cornerPosition = new Vector2(0f - (num + 5f), 0f - (num + 70f));
		this.minimapMaskT.anchoredPosition = this.cornerPosition;
		this.minimapMaskT.sizeDelta = new Vector2(this.MINIMAP_CORNER_SIZE, this.MINIMAP_CORNER_SIZE);
		GameObject gameObject3 = new GameObject("Minimap");
		gameObject3.transform.SetParent(this.minimapMaskT, worldPositionStays: false);
		this.minimap = gameObject3.AddComponent<RectTransform>();
		gameObject3.AddComponent<CanvasRenderer>();
		RectTransform rectTransform2 = this.minimap;
		anchorMin = (this.minimap.anchorMax = new Vector2(0.5f, 0.5f));
		rectTransform2.anchorMin = anchorMin;
		this.minimap.anchoredPosition = Vector2.zero;
		this.minimap.sizeDelta = this.minimapMaskT.sizeDelta;
		Image ımage = gameObject3.AddComponent<Image>();
		Rect rect = new Rect(0f, 0f, map.width, map.height);
		ımage.sprite = UnityEngine.Sprite.Create(map, rect, new Vector3(0.5f, 0.5f));
		ımage.type = Image.Type.Simple;
	}

	private void CreateUnityUIRT(int minimapResolution)
	{
		GameObject gameObject = new GameObject("Canvas");
		gameObject.AddComponent<RectTransform>();
		this.canvas = gameObject.AddComponent<Canvas>();
		this.canvas.renderMode = RenderMode.ScreenSpaceOverlay;
		this.scaler = gameObject.AddComponent<CanvasScaler>();
		this.scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
		this.scaler.referenceResolution = new Vector2(800f, 600f);
		this.scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
		this.scaler.matchWidthOrHeight = 1f;
		GameObject gameObject2 = new GameObject("Mask");
		gameObject2.transform.SetParent(gameObject.transform, worldPositionStays: false);
		this.minimapMaskT = gameObject2.AddComponent<RectTransform>();
		gameObject2.AddComponent<CanvasRenderer>();
		RectTransform rectTransform = this.minimapMaskT;
		Vector2 anchorMin = (this.minimapMaskT.anchorMax = Vector2.one);
		rectTransform.anchorMin = anchorMin;
		float num = this.MINIMAP_CORNER_SIZE * 0.5f;
		this.cornerPosition = new Vector2(0f - (num + 5f), 0f - (num + 70f));
		this.minimapMaskT.anchoredPosition = this.cornerPosition;
		this.minimapMaskT.sizeDelta = new Vector2(this.MINIMAP_CORNER_SIZE, this.MINIMAP_CORNER_SIZE);
		GameObject gameObject3 = new GameObject("MapBorder");
		gameObject3.transform.SetParent(this.minimapMaskT, worldPositionStays: false);
		this.borderT = gameObject3.AddComponent<RectTransform>();
		RectTransform rectTransform2 = this.borderT;
		anchorMin = (this.borderT.anchorMax = new Vector2(0.5f, 0.5f));
		rectTransform2.anchorMin = anchorMin;
		this.borderT.sizeDelta = this.minimapMaskT.sizeDelta;
		gameObject3.AddComponent<CanvasRenderer>();
		Image ımage = gameObject3.AddComponent<Image>();
		ımage.sprite = Minimap.borderSprite;
		ımage.type = Image.Type.Sliced;
		GameObject gameObject4 = new GameObject("Minimap");
		gameObject4.transform.SetParent(this.minimapMaskT, worldPositionStays: false);
		this.minimap = gameObject4.AddComponent<RectTransform>();
		this.minimap.SetAsFirstSibling();
		gameObject4.AddComponent<CanvasRenderer>();
		RectTransform rectTransform3 = this.minimap;
		anchorMin = (this.minimap.anchorMax = new Vector2(0.5f, 0.5f));
		rectTransform3.anchorMin = anchorMin;
		this.minimap.anchoredPosition = Vector2.zero;
		this.minimap.sizeDelta = this.minimapMaskT.sizeDelta;
		RawImage rawImage = gameObject4.AddComponent<RawImage>();
		rawImage.texture = this.minimapRT;
		rawImage.maskable = true;
		gameObject4.AddComponent<Mask>().showMaskGraphic = true;
	}

	private Vector2 GetSizeForStyle(IconStyle style)
	{
		return style switch
		{
			IconStyle.CIRCLE => this.MINIMAP_ICON_SIZE, 
			IconStyle.SUPPLY => this.MINIMAP_SUPPLY_SIZE, 
			_ => Vector2.zero, 
		};
	}

	private static UnityEngine.Sprite GetSpriteForStyle(IconStyle style)
	{
		return style switch
		{
			IconStyle.CIRCLE => Minimap.whiteIconSprite, 
			IconStyle.SUPPLY => Minimap.supplySprite, 
			_ => null, 
		};
	}

	private IEnumerator HackRoutine()
	{
		yield return new WaitForEndOfFrame();
		this.RecaptureMinimap(this.lastUsedCamera, this.lastMinimapCenter, this.lastMinimapOrthoSize);
	}

	private void Initialize()
	{
		Vector3 vector = new Vector3(0.5f, 0.5f);
		Texture2D texture2D = (Texture2D)FengGameManagerMKII.RCassets.Load("icon");
		Rect rect = new Rect(0f, 0f, texture2D.width, texture2D.height);
		Minimap.whiteIconSprite = UnityEngine.Sprite.Create(texture2D, rect, vector);
		texture2D = (Texture2D)FengGameManagerMKII.RCassets.Load("iconpointer");
		rect = new Rect(0f, 0f, texture2D.width, texture2D.height);
		Minimap.pointerSprite = UnityEngine.Sprite.Create(texture2D, rect, vector);
		texture2D = (Texture2D)FengGameManagerMKII.RCassets.Load("supplyicon");
		rect = new Rect(0f, 0f, texture2D.width, texture2D.height);
		Minimap.supplySprite = UnityEngine.Sprite.Create(texture2D, rect, vector);
		texture2D = (Texture2D)FengGameManagerMKII.RCassets.Load("mapborder");
		Minimap.borderSprite = UnityEngine.Sprite.Create(rect: new Rect(0f, 0f, texture2D.width, texture2D.height), border: new Vector4(5f, 5f, 5f, 5f), texture: texture2D, pivot: vector, pixelsPerUnit: 100f, extrude: 1u, meshType: SpriteMeshType.FullRect);
		this.MINIMAP_ICON_SIZE = new Vector2(Minimap.whiteIconSprite.texture.width, Minimap.whiteIconSprite.texture.height);
		this.MINIMAP_POINTER_SIZE = (float)(Minimap.pointerSprite.texture.width + Minimap.pointerSprite.texture.height) / 2f;
		this.MINIMAP_POINTER_DIST = (this.MINIMAP_ICON_SIZE.x + this.MINIMAP_ICON_SIZE.y) * 0.25f;
		this.MINIMAP_SUPPLY_SIZE = new Vector2(Minimap.supplySprite.texture.width, Minimap.supplySprite.texture.height);
		this.assetsInitialized = true;
	}

	private void ManualSetCameraProperties(Camera cam, Vector3 centerPoint, float orthoSize)
	{
		Transform obj = cam.transform;
		centerPoint.y = cam.farClipPlane * 0.5f;
		obj.position = centerPoint;
		obj.eulerAngles = new Vector3(90f, 0f, 0f);
		cam.orthographic = true;
		cam.orthographicSize = orthoSize;
		float num = orthoSize * 2f;
		this.minimapOrthographicBounds = new Bounds(centerPoint, new Vector3(num, 0f, num));
		this.lastMinimapCenter = centerPoint;
		this.lastMinimapOrthoSize = orthoSize;
	}

	private void ManualSetOrthoBounds(Vector3 centerPoint, float orthoSize)
	{
		float num = orthoSize * 2f;
		this.minimapOrthographicBounds = new Bounds(centerPoint, new Vector3(num, 0f, num));
		this.lastMinimapCenter = centerPoint;
		this.lastMinimapOrthoSize = orthoSize;
	}

	public void Maximize()
	{
		this.isEnabledTemp = true;
		if (!this.isEnabled)
		{
			this.SetEnabledTemp(enabled: true);
		}
		RectTransform rectTransform = this.minimapMaskT;
		Vector2 anchorMin = (this.minimapMaskT.anchorMax = new Vector2(0.5f, 0.5f));
		rectTransform.anchorMin = anchorMin;
		this.minimapMaskT.anchoredPosition = Vector2.zero;
		this.minimapMaskT.sizeDelta = new Vector2(this.MINIMAP_SIZE, this.MINIMAP_SIZE);
		this.minimap.sizeDelta = this.minimapMaskT.sizeDelta;
		this.borderT.sizeDelta = this.minimapMaskT.sizeDelta;
		if (this.minimapIcons != null)
		{
			for (int i = 0; i < this.minimapIcons.Length; i++)
			{
				MinimapIcon minimapIcon = this.minimapIcons[i];
				if (minimapIcon != null)
				{
					minimapIcon.SetSize(this.GetSizeForStyle(minimapIcon.style));
					if (minimapIcon.rotation)
					{
						minimapIcon.SetPointerSize(this.MINIMAP_POINTER_SIZE, this.MINIMAP_POINTER_DIST);
					}
				}
			}
		}
		this.maximized = true;
	}

	public void Minimize()
	{
		this.isEnabledTemp = false;
		if (!this.isEnabled)
		{
			this.SetEnabledTemp(enabled: false);
		}
		RectTransform rectTransform = this.minimapMaskT;
		Vector2 anchorMin = (this.minimapMaskT.anchorMax = Vector2.one);
		rectTransform.anchorMin = anchorMin;
		this.minimapMaskT.anchoredPosition = this.cornerPosition;
		this.minimapMaskT.sizeDelta = new Vector2(this.MINIMAP_CORNER_SIZE, this.MINIMAP_CORNER_SIZE);
		this.minimap.sizeDelta = this.minimapMaskT.sizeDelta;
		this.borderT.sizeDelta = this.minimapMaskT.sizeDelta;
		if (this.minimapIcons != null)
		{
			float num = 1f - ((float)this.MINIMAP_SIZE - this.MINIMAP_CORNER_SIZE) / (float)this.MINIMAP_SIZE;
			float a = this.MINIMAP_POINTER_SIZE * num;
			a = Mathf.Max(a, this.MINIMAP_POINTER_SIZE * 0.5f);
			float num2 = (this.MINIMAP_POINTER_SIZE - a) / this.MINIMAP_POINTER_SIZE;
			num2 = this.MINIMAP_POINTER_DIST * num2;
			for (int i = 0; i < this.minimapIcons.Length; i++)
			{
				MinimapIcon minimapIcon = this.minimapIcons[i];
				if (minimapIcon != null)
				{
					Vector2 sizeForStyle = this.GetSizeForStyle(minimapIcon.style);
					sizeForStyle.x = Mathf.Max(sizeForStyle.x * num, sizeForStyle.x * 0.5f);
					sizeForStyle.y = Mathf.Max(sizeForStyle.y * num, sizeForStyle.y * 0.5f);
					minimapIcon.SetSize(sizeForStyle);
					if (minimapIcon.rotation)
					{
						minimapIcon.SetPointerSize(a, num2);
					}
				}
			}
		}
		this.maximized = false;
	}

	public static void OnScreenResolutionChanged()
	{
		if (Minimap.instance != null && Minimap.Supported())
		{
			Minimap obj = Minimap.instance;
			obj.StartCoroutine(obj.ScreenResolutionChangedRoutine());
		}
	}

	private void RecaptureMinimap()
	{
		if (this.lastUsedCamera != null)
		{
			this.RecaptureMinimap(this.lastUsedCamera, this.lastMinimapCenter, this.lastMinimapOrthoSize);
		}
	}

	private void RecaptureMinimap(Camera cam, Vector3 centerPosition, float orthoSize)
	{
		if (this.minimap != null)
		{
			GameObject gameObject = GameObject.Find("mainLight");
			Light light = null;
			Quaternion rotation = Quaternion.identity;
			LightShadows shadows = LightShadows.None;
			Color color = Color.clear;
			float intensity = 0f;
			float nearClipPlane = cam.nearClipPlane;
			float farClipPlane = cam.farClipPlane;
			int cullingMask = cam.cullingMask;
			if (gameObject != null)
			{
				light = gameObject.GetComponent<Light>();
				rotation = light.transform.rotation;
				shadows = light.shadows;
				color = light.color;
				intensity = light.intensity;
				light.shadows = LightShadows.None;
				light.color = Color.white;
				light.intensity = 0.5f;
				light.transform.eulerAngles = new Vector3(90f, 0f, 0f);
			}
			cam.nearClipPlane = 0.3f;
			cam.farClipPlane = 1000f;
			cam.clearFlags = CameraClearFlags.Color;
			cam.cullingMask = 512;
			this.CreateMinimapRT(cam, this.MINIMAP_SIZE);
			this.ManualSetCameraProperties(cam, centerPosition, orthoSize);
			this.CaptureMinimapRT(cam);
			if (gameObject != null)
			{
				light.shadows = shadows;
				light.transform.rotation = rotation;
				light.color = color;
				light.intensity = intensity;
			}
			cam.nearClipPlane = nearClipPlane;
			cam.farClipPlane = farClipPlane;
			cam.cullingMask = cullingMask;
			cam.orthographic = false;
			cam.clearFlags = CameraClearFlags.Skybox;
		}
	}

	private IEnumerator ScreenResolutionChangedRoutine()
	{
		yield return 0;
		this.RecaptureMinimap();
	}

	public void SetEnabled(bool enabled)
	{
		this.isEnabled = enabled;
		if (this.canvas != null)
		{
			this.canvas.gameObject.SetActive(enabled);
		}
	}

	public void SetEnabledTemp(bool enabled)
	{
		if (this.canvas != null)
		{
			this.canvas.gameObject.SetActive(enabled);
		}
	}

	public void TrackGameObjectOnMinimap(GameObject objToTrack, Color iconColor, bool trackOrientation, bool depthAboveAll = false, IconStyle iconStyle = IconStyle.CIRCLE)
	{
		if (!(this.minimap != null))
		{
			return;
		}
		MinimapIcon minimapIcon = ((!trackOrientation) ? MinimapIcon.Create(this.minimap, objToTrack, iconStyle) : MinimapIcon.CreateWithRotation(this.minimap, objToTrack, iconStyle, this.MINIMAP_POINTER_DIST));
		minimapIcon.SetColor(iconColor);
		minimapIcon.SetDepth(depthAboveAll);
		Vector2 sizeForStyle = this.GetSizeForStyle(iconStyle);
		if (this.maximized)
		{
			minimapIcon.SetSize(sizeForStyle);
			if (minimapIcon.rotation)
			{
				minimapIcon.SetPointerSize(this.MINIMAP_POINTER_SIZE, this.MINIMAP_POINTER_DIST);
			}
		}
		else
		{
			float num = 1f - ((float)this.MINIMAP_SIZE - this.MINIMAP_CORNER_SIZE) / (float)this.MINIMAP_SIZE;
			sizeForStyle.x = Mathf.Max(sizeForStyle.x * num, sizeForStyle.x * 0.5f);
			sizeForStyle.y = Mathf.Max(sizeForStyle.y * num, sizeForStyle.y * 0.5f);
			minimapIcon.SetSize(sizeForStyle);
			if (minimapIcon.rotation)
			{
				float a = this.MINIMAP_POINTER_SIZE * num;
				a = Mathf.Max(a, this.MINIMAP_POINTER_SIZE * 0.5f);
				float num2 = (this.MINIMAP_POINTER_SIZE - a) / this.MINIMAP_POINTER_SIZE;
				num2 = this.MINIMAP_POINTER_DIST * num2;
				minimapIcon.SetPointerSize(a, num2);
			}
		}
		if (this.minimapIcons == null)
		{
			this.minimapIcons = new MinimapIcon[1] { minimapIcon };
			return;
		}
		MinimapIcon[] array = new MinimapIcon[this.minimapIcons.Length + 1];
		for (int i = 0; i < this.minimapIcons.Length; i++)
		{
			array[i] = this.minimapIcons[i];
		}
		array[array.Length - 1] = minimapIcon;
		this.minimapIcons = array;
	}

	public static void TryRecaptureInstance()
	{
		if (Minimap.instance != null)
		{
			Minimap.instance.RecaptureMinimap();
		}
	}

	public IEnumerator TryRecaptureInstanceE(float time)
	{
		yield return new WaitForSeconds(time);
		Minimap.TryRecaptureInstance();
	}

	private void Update()
	{
		this.CheckUserInput();
		if ((!this.isEnabled && !this.isEnabledTemp) || !this.minimapIsCreated || this.minimapIcons == null)
		{
			return;
		}
		for (int i = 0; i < this.minimapIcons.Length; i++)
		{
			MinimapIcon minimapIcon = this.minimapIcons[i];
			if (minimapIcon == null)
			{
				RCextensions.RemoveAt(ref this.minimapIcons, i);
			}
			else if (!minimapIcon.UpdateUI(this.minimapOrthographicBounds, this.maximized ? ((float)this.MINIMAP_SIZE) : this.MINIMAP_CORNER_SIZE))
			{
				minimapIcon.Destroy();
				RCextensions.RemoveAt(ref this.minimapIcons, i);
			}
		}
	}

	public static void WaitAndTryRecaptureInstance(float time)
	{
		Minimap.instance.StartCoroutine(Minimap.instance.TryRecaptureInstanceE(time));
	}

	private static bool Supported()
	{
		return Application.platform == RuntimePlatform.WindowsPlayer;
	}
}
