using System.Collections;
using System.Collections.Generic;
using Settings;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI;

internal class InputSettingElement : BaseSettingElement
{
	protected InputField _inputField;

	protected int _inputFontSizeOffset = -4;

	protected bool _fixedInputField;

	protected UnityAction _onValueChanged;

	protected UnityAction _onEndEdit;

	protected Transform _caret;

	protected bool _finishedSetup;

	protected object[] _setupParams;

	protected override HashSet<SettingType> SupportedSettingTypes => new HashSet<SettingType>
	{
		SettingType.Float,
		SettingType.Int,
		SettingType.String
	};

	public void Setup(BaseSetting setting, ElementStyle style, string title, string tooltip, float elementWidth, float elementHeight, bool multiLine, UnityAction onValueChanged, UnityAction onEndEdit)
	{
		this._onValueChanged = onValueChanged;
		this._onEndEdit = onEndEdit;
		this._inputField = base.transform.Find("InputField").gameObject.GetComponent<InputField>();
		if (this._inputField == null)
		{
			this._inputField = base.transform.Find("InputField").gameObject.AddComponent<InputFieldPasteable>();
			this._inputField.textComponent = this._inputField.transform.Find("Text").GetComponent<Text>();
			this._inputField.transition = Selectable.Transition.ColorTint;
			this._inputField.targetGraphic = this._inputField.GetComponent<Image>();
			this._inputField.text = "Default";
			this._inputField.text = string.Empty;
		}
		this._inputField.colors = UIManager.GetThemeColorBlock(style.ThemePanel, "DefaultSetting", "Input");
		this._inputField.transform.Find("Text").GetComponent<Text>().color = UIManager.GetThemeColor(style.ThemePanel, "DefaultSetting", "InputTextColor");
		this._inputField.selectionColor = UIManager.GetThemeColor(style.ThemePanel, "DefaultSetting", "InputSelectionColor");
		this._inputField.transform.Find("Text").GetComponent<Text>().fontSize = style.FontSize + this._inputFontSizeOffset;
		this._inputField.transform.Find("Text").GetComponent<Text>().color = UIManager.GetThemeColor(style.ThemePanel, "DefaultSetting", "InputTextColor");
		this._inputField.GetComponent<LayoutElement>().preferredWidth = elementWidth;
		this._inputField.GetComponent<LayoutElement>().preferredHeight = elementHeight;
		this._inputField.lineType = (multiLine ? InputField.LineType.MultiLineNewline : InputField.LineType.SingleLine);
		base._settingType = base.GetSettingType(setting);
		if (base._settingType == SettingType.Float)
		{
			this._inputField.contentType = InputField.ContentType.DecimalNumber;
			this._inputField.characterLimit = 20;
		}
		else if (base._settingType == SettingType.Int)
		{
			this._inputField.contentType = InputField.ContentType.IntegerNumber;
			this._inputField.characterLimit = 10;
		}
		else if (base._settingType == SettingType.String)
		{
			this._inputField.contentType = InputField.ContentType.Standard;
			int maxLength = ((StringSetting)setting).MaxLength;
			if (maxLength == int.MaxValue)
			{
				this._inputField.characterLimit = 0;
			}
			else
			{
				this._inputField.characterLimit = maxLength;
			}
		}
		this._inputField.onValueChange.AddListener(delegate(string value)
		{
			this.OnValueChanged(value);
		});
		this._inputField.onEndEdit.AddListener(delegate(string value)
		{
			this.OnInputFinishEditing(value);
		});
		if (multiLine)
		{
			this._setupParams = new object[4] { setting, style, title, tooltip };
			base.StartCoroutine(this.WaitAndFinishSetup());
		}
		else
		{
			base.Setup(setting, style, title, tooltip);
			base.StartCoroutine(this.WaitAndFixInputField());
			this._finishedSetup = true;
		}
	}

	private void OnEnable()
	{
		if (this._inputField != null && !this._finishedSetup)
		{
			base.StartCoroutine(this.WaitAndFinishSetup());
		}
		else if (this._inputField != null && !this._fixedInputField)
		{
			base.StartCoroutine(this.WaitAndFixInputField());
		}
	}

	private IEnumerator WaitAndFinishSetup()
	{
		yield return new WaitForEndOfFrame();
		base.Setup((BaseSetting)this._setupParams[0], (ElementStyle)this._setupParams[1], (string)this._setupParams[2], (string)this._setupParams[2]);
		base.StartCoroutine(this.WaitAndFixInputField());
		this._finishedSetup = true;
	}

	private IEnumerator WaitAndFixInputField()
	{
		yield return new WaitForEndOfFrame();
		this._inputField.gameObject.SetActive(value: false);
		this._inputField.gameObject.SetActive(value: true);
		this.SyncElement();
		this._fixedInputField = true;
	}

	protected void OnValueChanged(string value)
	{
		if (!this._finishedSetup)
		{
			return;
		}
		if (base._settingType == SettingType.String)
		{
			((StringSetting)base._setting).Value = this._inputField.text;
		}
		else if (value != string.Empty)
		{
			int result2;
			if (base._settingType == SettingType.Float)
			{
				if (float.TryParse(value, out var result))
				{
					((FloatSetting)base._setting).Value = result;
				}
			}
			else if (base._settingType == SettingType.Int && int.TryParse(value, out result2))
			{
				((IntSetting)base._setting).Value = result2;
			}
		}
		if (this._onValueChanged != null)
		{
			this._onValueChanged();
		}
	}

	protected void OnInputFinishEditing(string value)
	{
		if (this._finishedSetup)
		{
			this.SyncElement();
			if (this._onEndEdit != null)
			{
				this._onEndEdit();
			}
		}
	}

	public override void SyncElement()
	{
		if (this._finishedSetup)
		{
			if (base._settingType == SettingType.Float)
			{
				this._inputField.text = ((FloatSetting)base._setting).Value.ToString();
			}
			else if (base._settingType == SettingType.Int)
			{
				this._inputField.text = ((IntSetting)base._setting).Value.ToString();
			}
			else if (base._settingType == SettingType.String)
			{
				this._inputField.text = ((StringSetting)base._setting).Value;
			}
			this._inputField.transform.Find("Text").GetComponent<Text>().text = this._inputField.text;
		}
	}

	private void Update()
	{
		if (!this._caret && this._inputField != null)
		{
			this._caret = this._inputField.transform.Find(this._inputField.transform.name + " Input Caret");
			if ((bool)this._caret && !this._caret.GetComponent<Graphic>())
			{
				this._caret.gameObject.AddComponent<Image>();
			}
		}
	}
}
