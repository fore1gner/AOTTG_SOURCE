using System.Collections;
using UnityEngine;

public class PanelMultiJoin : MonoBehaviour
{
	private int currentPage = 1;

	private float elapsedTime = 10f;

	private string filter = string.Empty;

	private ArrayList filterRoom;

	public GameObject[] items;

	private int totalPage = 1;

	public void connectToIndex(int index, string roomName)
	{
		int num = 0;
		for (num = 0; num < 10; num++)
		{
			this.items[num].SetActive(value: false);
		}
		num = 10 * (this.currentPage - 1) + index;
		char[] separator = new char[1] { "`"[0] };
		string[] array = roomName.Split(separator);
		if (array[5] != string.Empty)
		{
			PanelMultiJoinPWD.Password = array[5];
			PanelMultiJoinPWD.roomName = roomName;
			NGUITools.SetActive(GameObject.Find("UIRefer").GetComponent<UIMainReferences>().PanelMultiPWD, state: true);
			NGUITools.SetActive(GameObject.Find("UIRefer").GetComponent<UIMainReferences>().panelMultiROOM, state: false);
		}
		else
		{
			PhotonNetwork.JoinRoom(roomName);
		}
	}

	private string getServerDataString(RoomInfo room)
	{
		char[] separator = new char[1] { "`"[0] };
		string[] array = room.name.Split(separator);
		return ((!(array[5] == string.Empty)) ? "[PWD]" : string.Empty) + array[0] + "/" + array[1] + "/" + array[2] + "/" + array[4] + " " + room.playerCount + "/" + room.maxPlayers;
	}

	private void OnDisable()
	{
	}

	private void OnEnable()
	{
		this.currentPage = 1;
		this.totalPage = 0;
		this.refresh();
	}

	private void OnFilterSubmit(string content)
	{
		this.filter = content;
		this.updateFilterRooms();
		this.showlist();
	}

	public void pageDown()
	{
		this.currentPage++;
		if (this.currentPage > this.totalPage)
		{
			this.currentPage = 1;
		}
		this.showServerList();
	}

	public void pageUp()
	{
		this.currentPage--;
		if (this.currentPage < 1)
		{
			this.currentPage = this.totalPage;
		}
		this.showServerList();
	}

	public void refresh()
	{
		this.showlist();
	}

	private void showlist()
	{
		if (this.filter == string.Empty)
		{
			if (PhotonNetwork.GetRoomList().Length != 0)
			{
				this.totalPage = (PhotonNetwork.GetRoomList().Length - 1) / 10 + 1;
			}
			else
			{
				this.totalPage = 1;
			}
		}
		else
		{
			this.updateFilterRooms();
			if (this.filterRoom.Count > 0)
			{
				this.totalPage = (this.filterRoom.Count - 1) / 10 + 1;
			}
			else
			{
				this.totalPage = 1;
			}
		}
		if (this.currentPage < 1)
		{
			this.currentPage = this.totalPage;
		}
		if (this.currentPage > this.totalPage)
		{
			this.currentPage = 1;
		}
		this.showServerList();
	}

	private void showServerList()
	{
		if (PhotonNetwork.GetRoomList().Length != 0)
		{
			int num = 0;
			if (this.filter == string.Empty)
			{
				for (num = 0; num < 10; num++)
				{
					int num2 = 10 * (this.currentPage - 1) + num;
					if (num2 < PhotonNetwork.GetRoomList().Length)
					{
						this.items[num].SetActive(value: true);
						this.items[num].GetComponentInChildren<UILabel>().text = this.getServerDataString(PhotonNetwork.GetRoomList()[num2]);
						this.items[num].GetComponentInChildren<BTN_Connect_To_Server_On_List>().roomName = PhotonNetwork.GetRoomList()[num2].name;
					}
					else
					{
						this.items[num].SetActive(value: false);
					}
				}
			}
			else
			{
				for (num = 0; num < 10; num++)
				{
					int num3 = 10 * (this.currentPage - 1) + num;
					if (num3 < this.filterRoom.Count)
					{
						RoomInfo roomInfo = (RoomInfo)this.filterRoom[num3];
						this.items[num].SetActive(value: true);
						this.items[num].GetComponentInChildren<UILabel>().text = this.getServerDataString(roomInfo);
						this.items[num].GetComponentInChildren<BTN_Connect_To_Server_On_List>().roomName = roomInfo.name;
					}
					else
					{
						this.items[num].SetActive(value: false);
					}
				}
			}
			GameObject.Find("LabelServerListPage").GetComponent<UILabel>().text = this.currentPage + "/" + this.totalPage;
		}
		else
		{
			for (int i = 0; i < this.items.Length; i++)
			{
				this.items[i].SetActive(value: false);
			}
			GameObject.Find("LabelServerListPage").GetComponent<UILabel>().text = this.currentPage + "/" + this.totalPage;
		}
	}

	private void Start()
	{
		int num = 0;
		for (num = 0; num < 10; num++)
		{
			this.items[num].SetActive(value: true);
			this.items[num].GetComponentInChildren<UILabel>().text = string.Empty;
			this.items[num].SetActive(value: false);
		}
	}

	private void Update()
	{
		this.elapsedTime += Time.deltaTime;
		if (this.elapsedTime > 1f)
		{
			this.elapsedTime = 0f;
			this.showlist();
		}
	}

	private void updateFilterRooms()
	{
		this.filterRoom = new ArrayList();
		if (!(this.filter != string.Empty))
		{
			return;
		}
		RoomInfo[] roomList = PhotonNetwork.GetRoomList();
		foreach (RoomInfo roomInfo in roomList)
		{
			if (roomInfo.name.ToUpper().Contains(this.filter.ToUpper()))
			{
				this.filterRoom.Add(roomInfo);
			}
		}
	}
}
