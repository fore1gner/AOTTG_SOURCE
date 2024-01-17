using System.Collections;
using Settings;
using UnityEngine;
using UnityEngine.UI;

namespace UI;

internal class KeybindPopup : PromptPopup
{
	private InputKey _setting;

	private Text _settingLabel;

	private Text _displayLabel;

	private InputKey _buffer;

	private bool _isDone;

	protected override string Title => UIManager.GetLocale("SettingsPopup", "KeybindPopup", "Title");

	protected override float Width => 300f;

	protected override float Height => 250f;

	protected override float VerticalSpacing => 15f;

	protected override int VerticalPadding => 30;

	protected override TextAnchor PanelAlignment => TextAnchor.MiddleCenter;

	public override void Setup(BasePanel parent = null)
	{
		base.Setup(parent);
		ElementStyle style = new ElementStyle(this.ButtonFontSize, 120f, this.ThemePanel);
		ElementFactory.CreateDefaultButton(base.BottomBar, style, UIManager.GetLocale("SettingsPopup", "KeybindPopup", "Unbind"), 0f, 0f, delegate
		{
			this.OnButtonClick("Unbind");
		});
		ElementFactory.CreateDefaultButton(base.BottomBar, style, UIManager.GetLocaleCommon("Cancel"), 0f, 0f, delegate
		{
			this.OnButtonClick("Cancel");
		});
		ElementFactory.CreateDefaultLabel(base.SinglePanel, style, UIManager.GetLocale("SettingsPopup", "KeybindPopup", "CurrentKey") + ":").GetComponent<Text>();
		this._displayLabel = ElementFactory.CreateDefaultLabel(base.SinglePanel, style, string.Empty).GetComponent<Text>();
		this._buffer = new InputKey();
	}

	private void Update()
	{
		if (this._setting != null && !this._isDone && this._buffer.ReadNextInput())
		{
			this._isDone = true;
			if (this._buffer.ToString() == "Mouse0")
			{
				base.StartCoroutine(this.WaitAndUpdateSetting());
			}
			else
			{
				this.UpdateSetting();
			}
		}
	}

	private IEnumerator WaitAndUpdateSetting()
	{
		yield return new WaitForEndOfFrame();
		this.UpdateSetting();
	}

	private void UpdateSetting()
	{
		this._setting.LoadFromString(this._buffer.ToString());
		this._settingLabel.text = this._setting.ToString();
		base.gameObject.SetActive(value: false);
	}

	public void Show(InputKey setting, Text label)
	{
		if (!base.gameObject.activeSelf)
		{
			base.Show();
			this._setting = setting;
			this._settingLabel = label;
			this._displayLabel.text = this._setting.ToString();
			this._isDone = false;
		}
	}

	private void OnButtonClick(string name)
	{
		if (name == "Unbind")
		{
			this._setting.LoadFromString(SpecialKey.None.ToString());
			this._settingLabel.text = SpecialKey.None.ToString();
		}
		this._isDone = true;
		this.Hide();
	}
}
