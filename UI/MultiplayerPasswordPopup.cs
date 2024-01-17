using Settings;
using UnityEngine;
using UnityEngine.UI;

namespace UI;

internal class MultiplayerPasswordPopup : PromptPopup
{
	protected StringSetting _enteredPassword = new StringSetting(string.Empty);

	protected string _actualPassword;

	protected string _roomName;

	protected GameObject _incorrectPasswordLabel;

	protected override string Title => UIManager.GetLocaleCommon("Password");

	protected override int VerticalPadding => 10;

	protected override int HorizontalPadding => 20;

	protected override float VerticalSpacing => 10f;

	protected override float Width => 300f;

	protected override float Height => 250f;

	protected override TextAnchor PanelAlignment => TextAnchor.MiddleCenter;

	public override void Setup(BasePanel parent = null)
	{
		base.Setup(parent);
		float elementWidth = 200f;
		ElementStyle style = new ElementStyle(this.ButtonFontSize, 120f, this.ThemePanel);
		ElementStyle style2 = new ElementStyle(20, 120f, this.ThemePanel);
		ElementFactory.CreateDefaultButton(base.BottomBar, style, UIManager.GetLocaleCommon("Confirm"), 0f, 0f, delegate
		{
			this.OnButtonClick("Confirm");
		});
		ElementFactory.CreateDefaultButton(base.BottomBar, style, UIManager.GetLocaleCommon("Back"), 0f, 0f, delegate
		{
			this.OnButtonClick("Back");
		});
		ElementFactory.CreateDefaultLabel(base.SinglePanel, style2, string.Empty);
		ElementFactory.CreateInputSetting(base.SinglePanel, style2, this._enteredPassword, string.Empty, "", elementWidth);
		this._incorrectPasswordLabel = ElementFactory.CreateDefaultLabel(base.SinglePanel, style2, UIManager.GetLocale("MainMenu", "MultiplayerPasswordPopup", "IncorrectPassword"));
		this._incorrectPasswordLabel.GetComponent<Text>().color = Color.red;
	}

	public void Show(string actualPassword, string roomName)
	{
		this._actualPassword = actualPassword;
		this._roomName = roomName;
		this._incorrectPasswordLabel.SetActive(value: false);
		base.Show();
	}

	protected void OnButtonClick(string name)
	{
		if (name == "Confirm")
		{
			try
			{
				if (this._enteredPassword.Value == new SimpleAES().Decrypt(this._actualPassword))
				{
					PhotonNetwork.JoinRoom(this._roomName);
					this.Hide();
				}
				else
				{
					this._incorrectPasswordLabel.SetActive(value: true);
				}
				return;
			}
			catch
			{
				this._incorrectPasswordLabel.SetActive(value: true);
				return;
			}
		}
		if (name == "Back")
		{
			this.Hide();
		}
	}
}
