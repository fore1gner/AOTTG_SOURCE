using System.Collections.Generic;
using Settings;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI;

internal class WheelPopup : BasePopup
{
	private Text _centerText;

	private List<GameObject> _buttons = new List<GameObject>();

	public int SelectedItem;

	private UnityAction _callback;

	protected override float AnimationTime => 0.2f;

	protected override PopupAnimation PopupAnimationType => PopupAnimation.Fade;

	public override void Setup(BasePanel parent = null)
	{
		this._centerText = base.transform.Find("Panel/Center/Label").GetComponent<Text>();
		for (int i = 0; i < 8; i++)
		{
			this._buttons.Add(ElementFactory.InstantiateAndBind(base.transform.Find("Panel/Buttons"), "WheelButton").gameObject);
			int index = i;
			this._buttons[index].GetComponent<Button>().onClick.AddListener(delegate
			{
				this.OnButtonClick(index);
			});
		}
		ElementFactory.SetAnchor(this._buttons[0], TextAnchor.MiddleCenter, TextAnchor.LowerCenter, new Vector2(0f, 180f));
		ElementFactory.SetAnchor(this._buttons[1], TextAnchor.MiddleCenter, TextAnchor.LowerLeft, new Vector2(135f, 90f));
		ElementFactory.SetAnchor(this._buttons[2], TextAnchor.MiddleCenter, TextAnchor.MiddleLeft, new Vector2(180f, 0f));
		ElementFactory.SetAnchor(this._buttons[3], TextAnchor.MiddleCenter, TextAnchor.UpperLeft, new Vector2(135f, -90f));
		ElementFactory.SetAnchor(this._buttons[4], TextAnchor.MiddleCenter, TextAnchor.UpperCenter, new Vector2(0f, -180f));
		ElementFactory.SetAnchor(this._buttons[5], TextAnchor.MiddleCenter, TextAnchor.UpperRight, new Vector2(-135f, -90f));
		ElementFactory.SetAnchor(this._buttons[6], TextAnchor.MiddleCenter, TextAnchor.MiddleRight, new Vector2(-180f, 0f));
		ElementFactory.SetAnchor(this._buttons[7], TextAnchor.MiddleCenter, TextAnchor.LowerRight, new Vector2(-135f, 90f));
	}

	public void Show(string openKey, List<string> options, UnityAction callback)
	{
		if (base.gameObject.activeSelf)
		{
			base.StopAllCoroutines();
			base.SetTransformAlpha(this.MaxFadeAlpha);
		}
		this.SetCenterText(openKey);
		this._callback = callback;
		for (int i = 0; i < options.Count; i++)
		{
			this._buttons[i].SetActive(value: true);
			KeybindSetting keybindSetting = (KeybindSetting)SettingsManager.InputSettings.Interaction.Settings["QuickSelect" + (i + 1)];
			this._buttons[i].transform.Find("Text").GetComponent<Text>().text = keybindSetting.ToString() + " - " + options[i];
		}
		for (int j = options.Count; j < this._buttons.Count; j++)
		{
			this._buttons[j].SetActive(value: false);
		}
		base.Show();
	}

	private void SetCenterText(string openKey)
	{
		this._centerText.text = SettingsManager.InputSettings.Interaction.MenuNext.ToString() + " - " + UIManager.GetLocaleCommon("Next") + "\n";
		Text centerText = this._centerText;
		centerText.text = centerText.text + openKey + " - " + UIManager.GetLocaleCommon("Cancel");
	}

	private void OnButtonClick(int index)
	{
		this.SelectedItem = index;
		this._callback();
	}

	private void Update()
	{
		for (int i = 0; i < 8; i++)
		{
			if (((KeybindSetting)SettingsManager.InputSettings.Interaction.Settings["QuickSelect" + (i + 1)]).GetKeyDown())
			{
				this.OnButtonClick(i);
			}
		}
	}
}
