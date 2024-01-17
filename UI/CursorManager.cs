using System;
using System.Collections.Generic;
using ApplicationManagers;
using Settings;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace UI;

internal class CursorManager : MonoBehaviour
{
	public static CursorState State;

	private static CursorManager _instance;

	private static Texture2D _cursorPointer;

	private static Dictionary<CrosshairStyle, Texture2D> _crosshairs = new Dictionary<CrosshairStyle, Texture2D>();

	private bool _ready;

	private bool _crosshairWhite = true;

	private bool _lastCrosshairWhite;

	private string _crosshairText = string.Empty;

	private bool _forceNextCrosshairUpdate;

	private CrosshairStyle _lastCrosshairStyle;

	public static void Init()
	{
		CursorManager._instance = SingletonFactory.CreateSingleton(CursorManager._instance);
	}

	public static void FinishLoadAssets()
	{
		CursorManager._cursorPointer = (Texture2D)AssetBundleManager.MainAssetBundle.Load("CursorPointer");
		foreach (CrosshairStyle value2 in Enum.GetValues(typeof(CrosshairStyle)))
		{
			Texture2D value = (Texture2D)AssetBundleManager.MainAssetBundle.Load("Cursor" + value2);
			CursorManager._crosshairs.Add(value2, value);
		}
		CursorManager._instance._ready = true;
		CursorManager.SetPointer(force: true);
	}

	private void Update()
	{
		if (Application.loadedLevel == 0 || Application.loadedLevelName == "characterCreation" || Application.loadedLevelName == "Snapshot")
		{
			CursorManager.SetPointer();
		}
		else if (Application.loadedLevel == 2 && (int)FengGameManagerMKII.settingsOld[64] >= 100)
		{
			if (Camera.main.GetComponent<MouseLook>().enabled)
			{
				CursorManager.SetHidden();
			}
			else
			{
				CursorManager.SetPointer();
			}
		}
		else if (GameMenu.InMenu() || IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.STOP)
		{
			CursorManager.SetPointer();
		}
		else if (!FengGameManagerMKII.logicLoaded || !FengGameManagerMKII.customLevelLoaded)
		{
			CursorManager.SetPointer();
		}
		else if (FengGameManagerMKII.instance.needChooseSide && NGUITools.GetActive(FengGameManagerMKII.instance.ui.GetComponent<UIReferArray>().panels[3]))
		{
			CursorManager.SetPointer();
		}
		else if (IN_GAME_MAIN_CAMERA.Instance.main_object != null)
		{
			HERO component = IN_GAME_MAIN_CAMERA.Instance.main_object.GetComponent<HERO>();
			if (SettingsManager.LegacyGeneralSettings.SpecMode.Value || component == null || !component.IsMine())
			{
				CursorManager.SetHidden();
			}
			else
			{
				CursorManager.SetCrosshair();
			}
		}
		else
		{
			CursorManager.SetHidden();
		}
	}

	public static void RefreshCursorLock()
	{
		if (Screen.lockCursor)
		{
			Screen.lockCursor = !Screen.lockCursor;
			Screen.lockCursor = !Screen.lockCursor;
		}
	}

	public static void SetPointer(bool force = false)
	{
		if (force || CursorManager.State != 0)
		{
			Screen.showCursor = true;
			Screen.lockCursor = false;
			CursorManager.State = CursorState.Pointer;
		}
	}

	public static void SetHidden(bool force = false)
	{
		if (force || CursorManager.State != CursorState.Hidden)
		{
			Screen.showCursor = false;
			CursorManager.State = CursorState.Hidden;
		}
		if (IN_GAME_MAIN_CAMERA.cameraMode == CAMERA_TYPE.TPS)
		{
			if (!Screen.lockCursor)
			{
				Screen.lockCursor = true;
			}
		}
		else if (Screen.lockCursor)
		{
			Screen.lockCursor = false;
		}
	}

	public static void SetCrosshair(bool force = false)
	{
		if (force || CursorManager.State != CursorState.Crosshair)
		{
			Screen.showCursor = false;
			CursorManager.State = CursorState.Crosshair;
		}
		if (IN_GAME_MAIN_CAMERA.cameraMode == CAMERA_TYPE.TPS)
		{
			if (!Screen.lockCursor)
			{
				Screen.lockCursor = true;
			}
		}
		else if (Screen.lockCursor)
		{
			Screen.lockCursor = false;
		}
	}

	public static void SetCrosshairColor(bool white)
	{
		if (CursorManager._instance._crosshairWhite != white)
		{
			CursorManager._instance._crosshairWhite = white;
		}
	}

	public static void SetCrosshairText(string text)
	{
		CursorManager._instance._crosshairText = text;
	}

	public static void UpdateCrosshair(RawImage crosshairImageWhite, RawImage crosshairImageRed, Text crosshairLabelWhite, Text crosshairLabelRed, bool force = false)
	{
		if (!CursorManager._instance._ready)
		{
			return;
		}
		if (CursorManager.State != CursorState.Crosshair || GameMenu.HideCrosshair)
		{
			if (crosshairImageRed.gameObject.activeSelf)
			{
				crosshairImageRed.gameObject.SetActive(value: false);
			}
			if (crosshairImageWhite.gameObject.activeSelf)
			{
				crosshairImageWhite.gameObject.SetActive(value: false);
			}
			CursorManager._instance._forceNextCrosshairUpdate = true;
			return;
		}
		CrosshairStyle value = (CrosshairStyle)SettingsManager.UISettings.CrosshairStyle.Value;
		if (CursorManager._instance._lastCrosshairStyle != value || force || CursorManager._instance._forceNextCrosshairUpdate)
		{
			crosshairImageWhite.texture = CursorManager._crosshairs[value];
			crosshairImageRed.texture = CursorManager._crosshairs[value];
			CursorManager._instance._lastCrosshairStyle = value;
		}
		if (CursorManager._instance._crosshairWhite != CursorManager._instance._lastCrosshairWhite || force || CursorManager._instance._forceNextCrosshairUpdate)
		{
			crosshairImageWhite.gameObject.SetActive(CursorManager._instance._crosshairWhite);
			crosshairImageRed.gameObject.SetActive(!CursorManager._instance._crosshairWhite);
			CursorManager._instance._lastCrosshairWhite = CursorManager._instance._crosshairWhite;
		}
		Text text = crosshairLabelWhite;
		RawImage rawImage = crosshairImageWhite;
		if (!CursorManager._instance._crosshairWhite)
		{
			text = crosshairLabelRed;
			rawImage = crosshairImageRed;
		}
		text.text = CursorManager._instance._crosshairText;
		Vector3 mousePosition = Input.mousePosition;
		Transform transform = rawImage.transform;
		if (transform.position != mousePosition)
		{
			if (IN_GAME_MAIN_CAMERA.cameraMode == CAMERA_TYPE.TPS)
			{
				if (Math.Abs(transform.position.x - mousePosition.x) > 1f || Math.Abs(transform.position.y - mousePosition.y) > 1f)
				{
					transform.position = mousePosition;
				}
			}
			else
			{
				transform.position = mousePosition;
			}
		}
		CursorManager._instance._forceNextCrosshairUpdate = false;
	}
}
