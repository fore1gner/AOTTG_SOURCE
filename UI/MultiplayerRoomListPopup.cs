using System;
using System.Collections.Generic;
using Settings;
using UnityEngine;
using UnityEngine.UI;

namespace UI;

internal class MultiplayerRoomListPopup : BasePopup
{
	protected MultiplayerPasswordPopup _multiplayerPasswordPopup;

	protected BasePopup _multiplayerCreatePopup;

	protected MultiplayerFilterPopup _multiplayerFilterPopup;

	protected Text _pageLabel;

	protected Text _playersOnlineLabel;

	protected GameObject _roomList;

	protected GameObject _noRoomsLabel;

	protected List<GameObject> _roomButtons = new List<GameObject>();

	public StringSetting _filterQuery = new StringSetting(string.Empty);

	public BoolSetting _filterShowFull = new BoolSetting(defaultValue: true);

	public BoolSetting _filterShowPassword = new BoolSetting(defaultValue: true);

	protected IntSetting _currentPage = new IntSetting(0, 0);

	private float _maxUpdateDelay = 5f;

	private float _currentUpdateDelay = 5f;

	private int _roomsPerPage = 10;

	private RoomInfo[] _rooms;

	private char[] _roomSeperator = new char[1] { "`"[0] };

	private int _lastPageCount;

	protected override string ThemePanel => "MultiplayerRoomListPopup";

	protected override bool HasPremadeContent => true;

	protected override int HorizontalPadding => 0;

	protected override int VerticalPadding => 0;

	protected override float Width => 1000f;

	protected override float Height => 660f;

	public override void Setup(BasePanel parent = null)
	{
		base.Setup(parent);
		string category = "MainMenu";
		string subCategory = "MultiplayerRoomListPopup";
		ElementStyle elementStyle = new ElementStyle(this.ButtonFontSize, 120f, this.ThemePanel);
		ElementFactory.CreateDefaultButton(base.BottomBar, elementStyle, UIManager.GetLocaleCommon("Create"), 0f, 0f, delegate
		{
			this.OnButtonClick("Create");
		});
		ElementFactory.CreateDefaultButton(base.BottomBar, elementStyle, UIManager.GetLocaleCommon("Back"), 0f, 0f, delegate
		{
			this.OnButtonClick("Back");
		});
		base.TopBar.Find("SearchInputSetting").gameObject.AddComponent<InputSettingElement>().Setup(this._filterQuery, new ElementStyle(24, 0f), UIManager.GetLocaleCommon("Search"), string.Empty, 160f, 40f, multiLine: false, null, delegate
		{
			this.RefreshList();
		});
		base.TopBar.Find("FilterButton").GetComponent<Button>().onClick.AddListener(delegate
		{
			this.OnButtonClick("Filter");
		});
		base.TopBar.Find("RefreshButton").GetComponent<Button>().onClick.AddListener(delegate
		{
			this.OnButtonClick("Refresh");
		});
		base.TopBar.Find("Page/LeftButton").GetComponent<Button>().onClick.AddListener(delegate
		{
			this.OnButtonClick("LeftPage");
		});
		base.TopBar.Find("Page/RightButton").GetComponent<Button>().onClick.AddListener(delegate
		{
			this.OnButtonClick("RightPage");
		});
		this._pageLabel = base.TopBar.Find("Page/PageLabel").GetComponent<Text>();
		this._roomList = base.SinglePanel.Find("RoomList").gameObject;
		this._noRoomsLabel = this._roomList.transform.Find("NoRoomsLabel").gameObject;
		this._noRoomsLabel.GetComponent<Text>().text = UIManager.GetLocale(category, subCategory, "NoRooms");
		this._playersOnlineLabel = base.TopBar.Find("PlayersOnlineLabel").GetComponent<Text>();
		this._playersOnlineLabel.text = "0 " + UIManager.GetLocale(category, subCategory, "PlayersOnline");
		base.TopBar.Find("FilterButton").Find("Text").GetComponent<Text>()
			.text = UIManager.GetLocaleCommon("Filters");
		Button[] componentsInChildren = base.TopBar.GetComponentsInChildren<Button>();
		foreach (Button button in componentsInChildren)
		{
			button.colors = UIManager.GetThemeColorBlock(elementStyle.ThemePanel, "DefaultButton", "");
			if (button.transform.Find("Text") != null)
			{
				button.transform.Find("Text").GetComponent<Text>().color = UIManager.GetThemeColor(elementStyle.ThemePanel, "DefaultButton", "TextColor");
			}
		}
		base.TopBar.Find("Page/PageLabel").GetComponent<Text>().color = UIManager.GetThemeColor(elementStyle.ThemePanel, "DefaultLabel", "TextColor");
		base.TopBar.Find("PlayersOnlineLabel").GetComponent<Text>().color = UIManager.GetThemeColor(elementStyle.ThemePanel, "DefaultLabel", "TextColor");
		this._noRoomsLabel.GetComponent<Text>().color = UIManager.GetThemeColor(elementStyle.ThemePanel, "RoomButton", "TextColor");
		this._roomList.GetComponent<Image>().color = UIManager.GetThemeColor(elementStyle.ThemePanel, "MainBody", "BackgroundColor");
	}

	public override void Show()
	{
		base.Show();
		this._currentPage.Value = 0;
		this.RefreshList();
		this._currentUpdateDelay = 0.5f;
	}

	public override void Hide()
	{
		if (base.gameObject.activeSelf)
		{
			PhotonNetwork.Disconnect();
		}
		base.Hide();
	}

	protected void Update()
	{
		this._currentUpdateDelay -= Time.deltaTime;
		if (this._currentUpdateDelay <= 0f)
		{
			this.RefreshList();
			this._currentUpdateDelay = this._maxUpdateDelay;
		}
	}

	protected override void SetupPopups()
	{
		base.SetupPopups();
		this._multiplayerPasswordPopup = ElementFactory.CreateHeadedPanel<MultiplayerPasswordPopup>(base.transform).GetComponent<MultiplayerPasswordPopup>();
		this._multiplayerFilterPopup = ElementFactory.CreateHeadedPanel<MultiplayerFilterPopup>(base.transform).GetComponent<MultiplayerFilterPopup>();
		this._multiplayerCreatePopup = ElementFactory.CreateHeadedPanel<MultiplayerCreatePopup>(base.transform).GetComponent<MultiplayerCreatePopup>();
		base._popups.Add(this._multiplayerPasswordPopup);
		base._popups.Add(this._multiplayerFilterPopup);
		base._popups.Add(this._multiplayerCreatePopup);
	}

	public void RefreshList(bool refetch = true)
	{
		this._currentUpdateDelay = this._maxUpdateDelay;
		if (refetch)
		{
			this._rooms = PhotonNetwork.GetRoomList();
			this._playersOnlineLabel.text = PhotonNetwork.countOfPlayers + " " + UIManager.GetLocale("MainMenu", "MultiplayerRoomListPopup", "PlayersOnline");
		}
		this.ClearRoomButtons();
		List<RoomInfo> filteredRooms = this.GetFilteredRooms();
		if (filteredRooms.Count == 0)
		{
			this._noRoomsLabel.SetActive(value: true);
			this._pageLabel.text = "0/0";
			return;
		}
		this._noRoomsLabel.SetActive(value: false);
		this._lastPageCount = this.GetPageCount(filteredRooms);
		this._currentPage.Value = Math.Min(this._currentPage.Value, this._lastPageCount - 1);
		this._pageLabel.text = this._currentPage.Value + 1 + "/" + this._lastPageCount;
		foreach (RoomInfo room in this.GetCurrentPageRooms(filteredRooms))
		{
			GameObject gameObject = ElementFactory.InstantiateAndBind(this._roomList.transform, "MultiplayerRoomButton");
			this._roomButtons.Add(gameObject);
			gameObject.GetComponent<Button>().onClick.AddListener(delegate
			{
				this.OnRoomClick(room.name);
			});
			gameObject.transform.Find("Text").GetComponent<Text>().text = this.GetRoomFormattedName(room);
			if (this.GetRoomPassword(room.name) == string.Empty)
			{
				gameObject.transform.Find("PasswordIcon").gameObject.SetActive(value: false);
			}
			gameObject.GetComponent<Button>().colors = UIManager.GetThemeColorBlock(this.ThemePanel, "RoomButton", "");
			gameObject.transform.Find("Text").GetComponent<Text>().color = UIManager.GetThemeColor(this.ThemePanel, "RoomButton", "TextColor");
		}
	}

	protected List<RoomInfo> GetCurrentPageRooms(List<RoomInfo> rooms)
	{
		if (rooms.Count <= this._roomsPerPage)
		{
			return rooms;
		}
		List<RoomInfo> list = new List<RoomInfo>();
		int num = this._currentPage.Value * this._roomsPerPage;
		int num2 = Math.Min(num + this._roomsPerPage, rooms.Count);
		for (int i = num; i < num2; i++)
		{
			list.Add(rooms[i]);
		}
		return list;
	}

	protected List<RoomInfo> GetFilteredRooms()
	{
		List<RoomInfo> list = new List<RoomInfo>();
		RoomInfo[] rooms = this._rooms;
		foreach (RoomInfo roomInfo in rooms)
		{
			if (this.IsValidRoom(roomInfo) && (!(this._filterQuery.Value != string.Empty) || roomInfo.name.ToLower().Contains(this._filterQuery.Value.ToLower())) && (this._filterShowFull.Value || roomInfo.playerCount < roomInfo.maxPlayers) && (this._filterShowPassword.Value || !(this.GetRoomPassword(roomInfo.name) != string.Empty)))
			{
				list.Add(roomInfo);
			}
		}
		return list;
	}

	protected int GetPageCount(List<RoomInfo> rooms)
	{
		if (rooms.Count == 0)
		{
			return 0;
		}
		return (rooms.Count - 1) / this._roomsPerPage + 1;
	}

	protected void ClearRoomButtons()
	{
		foreach (GameObject roomButton in this._roomButtons)
		{
			UnityEngine.Object.Destroy(roomButton);
		}
		this._roomButtons.Clear();
	}

	protected bool IsValidRoom(RoomInfo info)
	{
		return info.name.Split(this._roomSeperator).Length > 5;
	}

	protected string GetRoomPassword(string name)
	{
		string[] array = name.Split(this._roomSeperator);
		if (array.Length > 5)
		{
			return array[5];
		}
		return string.Empty;
	}

	protected string GetRoomFormattedName(RoomInfo room)
	{
		char[] separator = new char[1] { "`"[0] };
		string[] array = room.name.Split(separator);
		return (array[0] + " / " + array[1] + " / " + array[2].UpperFirstLetter() + " / " + array[4].UpperFirstLetter() + "   " + room.playerCount + "/" + room.maxPlayers).hexColor();
	}

	private void OnRoomClick(string name)
	{
		string roomPassword = this.GetRoomPassword(name);
		if (roomPassword != string.Empty)
		{
			this.HideAllPopups();
			this._multiplayerPasswordPopup.Show(roomPassword, name);
		}
		else
		{
			PhotonNetwork.JoinRoom(name);
		}
	}

	private void OnButtonClick(string name)
	{
		this.HideAllPopups();
		switch (name)
		{
		case "Back":
			((MainMenu)UIManager.CurrentMenu).ShowMultiplayerMapPopup();
			break;
		case "Create":
			this._multiplayerCreatePopup.Show();
			break;
		case "Filter":
			this._multiplayerFilterPopup.Show();
			break;
		case "Refresh":
			this.RefreshList();
			break;
		case "LeftPage":
			if (this._currentPage.Value <= 0)
			{
				this._currentPage.Value = this._lastPageCount - 1;
			}
			else
			{
				this._currentPage.Value--;
			}
			this.RefreshList(refetch: false);
			break;
		case "RightPage":
			if (this._currentPage.Value >= this._lastPageCount - 1)
			{
				this._currentPage.Value = 0;
			}
			else
			{
				this._currentPage.Value++;
			}
			this.RefreshList(refetch: false);
			break;
		}
	}
}
