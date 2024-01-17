using System.Collections.Generic;
using GameManagers;
using Settings;
using UnityEngine;
using UnityEngine.UI;

namespace UI;

internal class GameMenu : BaseMenu
{
	public static Dictionary<string, Texture2D> EmojiTextures = new Dictionary<string, Texture2D>();

	public static List<string> AvailableEmojis = new List<string> { "Smile", "ThumbsUp", "Cool", "Love", "Shocked", "Crying", "Annoyed", "Angry" };

	public static List<string> AvailableText = new List<string> { "Help", "Thanks", "Sorry", "Titan here", "Good game", "Nice hit", "Oops", "Welcome" };

	public static List<string> AvailableActions = new List<string> { "Salute", "Dance", "Flip", "Wave1", "Wave2", "Eat" };

	private const float EmoteCooldown = 4f;

	public static bool Paused;

	public static bool WheelMenu;

	public static bool HideCrosshair;

	public List<BasePopup> _emoteTextPopups = new List<BasePopup>();

	public List<BasePopup> _emoteEmojiPopups = new List<BasePopup>();

	public BasePopup _settingsPopup;

	public BasePopup _emoteWheelPopup;

	public BasePopup _itemWheelPopup;

	public RawImage _crosshairImageWhite;

	public RawImage _crosshairImageRed;

	public Text _crosshairLabelWhite;

	public Text _crosshairLabelRed;

	private float _currentEmoteCooldown;

	private EmoteWheelState _currentEmoteWheelState;

	public override void Setup()
	{
		base.Setup();
		GameMenu.HideCrosshair = false;
		GameMenu.TogglePause(pause: false);
		GameMenu.WheelMenu = false;
		this.SetupCrosshairs();
	}

	public static bool InMenu()
	{
		if (!GameMenu.Paused)
		{
			return GameMenu.WheelMenu;
		}
		return true;
	}

	public static void TogglePause(bool pause)
	{
		GameMenu.Paused = pause;
		if (UIManager.CurrentMenu != null && UIManager.CurrentMenu.GetComponent<GameMenu>() != null)
		{
			GameMenu component = UIManager.CurrentMenu.GetComponent<GameMenu>();
			if (GameMenu.Paused && !component._settingsPopup.gameObject.activeSelf)
			{
				component._settingsPopup.Show();
				component._emoteWheelPopup.Hide();
				GameMenu.WheelMenu = false;
				if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
				{
					Time.timeScale = 0f;
				}
			}
			else
			{
				GameMenu.Paused = false;
				component._settingsPopup.Hide();
				if (!Camera.main.GetComponent<IN_GAME_MAIN_CAMERA>().enabled)
				{
					Camera.main.GetComponent<SpectatorMovement>().disable = false;
					Camera.main.GetComponent<MouseLook>().disable = false;
				}
			}
		}
		if (!GameMenu.Paused && FengGameManagerMKII.instance.pauseWaitTime <= 0f)
		{
			Time.timeScale = 1f;
		}
	}

	public static void OnEmoteTextRPC(int viewId, string text, PhotonMessageInfo info)
	{
		if (!(UIManager.CurrentMenu == null) && SettingsManager.UISettings.ShowEmotes.Value)
		{
			GameMenu component = UIManager.CurrentMenu.GetComponent<GameMenu>();
			Transform transformFromViewId = GameMenu.GetTransformFromViewId(viewId, info);
			if (transformFromViewId != null && component != null)
			{
				component.ShowEmoteText(text, transformFromViewId);
			}
		}
	}

	public static void OnEmoteEmojiRPC(int viewId, string emoji, PhotonMessageInfo info)
	{
		if (!(UIManager.CurrentMenu == null) && SettingsManager.UISettings.ShowEmotes.Value)
		{
			GameMenu component = UIManager.CurrentMenu.GetComponent<GameMenu>();
			Transform transformFromViewId = GameMenu.GetTransformFromViewId(viewId, info);
			if (transformFromViewId != null && component != null)
			{
				component.ShowEmoteEmoji(emoji, transformFromViewId);
			}
		}
	}

	public static void ToggleEmoteWheel(bool enable)
	{
		if (!(UIManager.CurrentMenu != null) || !(UIManager.CurrentMenu.GetComponent<GameMenu>() != null))
		{
			return;
		}
		GameMenu menu = UIManager.CurrentMenu.GetComponent<GameMenu>();
		if (enable)
		{
			((WheelPopup)menu._emoteWheelPopup).Show(SettingsManager.InputSettings.Interaction.EmoteMenu.ToString(), GameMenu.GetEmoteWheelOptions(menu._currentEmoteWheelState), delegate
			{
				menu.OnEmoteWheelSelect();
			});
			GameMenu.WheelMenu = true;
		}
		else
		{
			menu._emoteWheelPopup.Hide();
			GameMenu.WheelMenu = false;
		}
	}

	public static void NextEmoteWheel()
	{
		if (!(UIManager.CurrentMenu != null) || !(UIManager.CurrentMenu.GetComponent<GameMenu>() != null))
		{
			return;
		}
		GameMenu menu = UIManager.CurrentMenu.GetComponent<GameMenu>();
		if (menu._emoteWheelPopup.gameObject.activeSelf && GameMenu.WheelMenu)
		{
			menu._currentEmoteWheelState++;
			if (menu._currentEmoteWheelState > EmoteWheelState.Action)
			{
				menu._currentEmoteWheelState = EmoteWheelState.Text;
			}
			((WheelPopup)menu._emoteWheelPopup).Show(SettingsManager.InputSettings.Interaction.EmoteMenu.ToString(), GameMenu.GetEmoteWheelOptions(menu._currentEmoteWheelState), delegate
			{
				menu.OnEmoteWheelSelect();
			});
		}
	}

	public void ShowEmoteText(string text, Transform parent)
	{
		EmoteTextPopup obj = (EmoteTextPopup)this.GetAvailablePopup(this._emoteTextPopups);
		if (text.Length > 20)
		{
			text = text.Substring(0, 20);
		}
		obj.Show(text, parent);
	}

	public void ShowEmoteEmoji(string emoji, Transform parent)
	{
		((EmoteEmojiPopup)this.GetAvailablePopup(this._emoteEmojiPopups)).Show(emoji, parent);
	}

	private void OnEmoteWheelSelect()
	{
		if (this._currentEmoteWheelState != EmoteWheelState.Action)
		{
			if (this._currentEmoteCooldown > 0f)
			{
				return;
			}
			this._currentEmoteCooldown = 4f;
		}
		HERO myHero = RCextensions.GetMyHero();
		if (myHero == null)
		{
			return;
		}
		if (this._currentEmoteWheelState == EmoteWheelState.Text)
		{
			string text = GameMenu.AvailableText[((WheelPopup)this._emoteWheelPopup).SelectedItem];
			if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
			{
				this.ShowEmoteText(text, myHero.transform);
			}
			else if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
			{
				CustomRPCManager.PhotonView.RPC("EmoteTextRPC", PhotonTargets.All, myHero.photonView.viewID, text);
			}
		}
		else if (this._currentEmoteWheelState == EmoteWheelState.Emoji)
		{
			string text2 = GameMenu.AvailableEmojis[((WheelPopup)this._emoteWheelPopup).SelectedItem];
			if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
			{
				this.ShowEmoteEmoji(text2, myHero.transform);
			}
			else if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.MULTIPLAYER)
			{
				CustomRPCManager.PhotonView.RPC("EmoteEmojiRPC", PhotonTargets.All, myHero.photonView.viewID, text2);
			}
		}
		else if (this._currentEmoteWheelState == EmoteWheelState.Action)
		{
			switch (GameMenu.AvailableActions[((WheelPopup)this._emoteWheelPopup).SelectedItem])
			{
			case "Salute":
				myHero.EmoteAction("salute");
				break;
			case "Dance":
				myHero.EmoteAction("special_armin");
				break;
			case "Flip":
				myHero.EmoteAction("dodge");
				break;
			case "Wave1":
				myHero.EmoteAction("special_marco_0");
				break;
			case "Wave2":
				myHero.EmoteAction("special_marco_1");
				break;
			case "Eat":
				myHero.EmoteAction("special_sasha");
				break;
			}
		}
		myHero._flareDelayAfterEmote = 2f;
		this._emoteWheelPopup.Hide();
		GameMenu.WheelMenu = false;
	}

	private static Transform GetTransformFromViewId(int viewId, PhotonMessageInfo info)
	{
		PhotonView photonView = PhotonView.Find(viewId);
		if (photonView != null && photonView.owner == info.sender)
		{
			return photonView.transform;
		}
		return null;
	}

	private static List<string> GetEmoteWheelOptions(EmoteWheelState state)
	{
		return state switch
		{
			EmoteWheelState.Text => GameMenu.AvailableText, 
			EmoteWheelState.Emoji => GameMenu.AvailableEmojis, 
			_ => GameMenu.AvailableActions, 
		};
	}

	private BasePopup GetAvailablePopup(List<BasePopup> popups)
	{
		foreach (BasePopup popup in popups)
		{
			if (!popup.gameObject.activeSelf)
			{
				return popup;
			}
		}
		return popups[0];
	}

	protected void SetupCrosshairs()
	{
		this._crosshairImageWhite = ElementFactory.InstantiateAndBind(base.transform, "CrosshairImage").GetComponent<RawImage>();
		this._crosshairImageRed = ElementFactory.InstantiateAndBind(base.transform, "CrosshairImage").GetComponent<RawImage>();
		this._crosshairImageRed.color = Color.red;
		this._crosshairLabelWhite = this._crosshairImageWhite.transform.Find("DefaultLabel").GetComponent<Text>();
		this._crosshairLabelRed = this._crosshairImageRed.transform.Find("DefaultLabel").GetComponent<Text>();
		ElementFactory.SetAnchor(this._crosshairImageWhite.gameObject, TextAnchor.MiddleCenter, TextAnchor.MiddleCenter, Vector2.zero);
		ElementFactory.SetAnchor(this._crosshairImageRed.gameObject, TextAnchor.MiddleCenter, TextAnchor.MiddleCenter, Vector2.zero);
		this._crosshairImageWhite.gameObject.AddComponent<CrosshairScaler>();
		this._crosshairImageRed.gameObject.AddComponent<CrosshairScaler>();
		CursorManager.UpdateCrosshair(this._crosshairImageWhite, this._crosshairImageRed, this._crosshairLabelWhite, this._crosshairLabelRed, force: true);
	}

	protected override void SetupPopups()
	{
		base.SetupPopups();
		this._settingsPopup = ElementFactory.CreateHeadedPanel<SettingsPopup>(base.transform).GetComponent<BasePopup>();
		this._emoteWheelPopup = ElementFactory.InstantiateAndSetupPanel<WheelPopup>(base.transform, "WheelMenu").GetComponent<BasePopup>();
		for (int i = 0; i < 5; i++)
		{
			BasePopup component = ElementFactory.InstantiateAndSetupPanel<EmoteTextPopup>(base.transform, "EmoteTextPopup").GetComponent<BasePopup>();
			this._emoteTextPopups.Add(component);
			BasePopup component2 = ElementFactory.InstantiateAndSetupPanel<EmoteEmojiPopup>(base.transform, "EmoteEmojiPopup").GetComponent<BasePopup>();
			this._emoteEmojiPopups.Add(component2);
		}
		base._popups.Add(this._settingsPopup);
		base._popups.Add(this._emoteWheelPopup);
	}

	private void Update()
	{
		CursorManager.UpdateCrosshair(this._crosshairImageWhite, this._crosshairImageRed, this._crosshairLabelWhite, this._crosshairLabelRed);
		this._currentEmoteCooldown -= Time.deltaTime;
	}
}
