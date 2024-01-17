using System;
using System.Collections.Generic;
using UnityEngine;

namespace Settings;

internal class InputKey
{
	protected KeyCode _key;

	protected bool _isSpecial;

	protected SpecialKey _special;

	protected bool _isModifier;

	protected KeyCode _modifier;

	protected HashSet<KeyCode> ModifierKeys = new HashSet<KeyCode>
	{
		KeyCode.LeftShift,
		KeyCode.LeftAlt,
		KeyCode.LeftControl,
		KeyCode.RightShift,
		KeyCode.RightAlt,
		KeyCode.RightControl
	};

	protected HashSet<string> AlphaDigits = new HashSet<string> { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };

	public InputKey()
	{
	}

	public InputKey(string keyStr)
	{
		this.LoadFromString(keyStr);
	}

	public bool MatchesKeyCode(KeyCode key)
	{
		if (!this._isSpecial && !this._isModifier)
		{
			return this._key == key;
		}
		return false;
	}

	public bool ReadNextInput()
	{
		this._isModifier = false;
		foreach (KeyCode modifierKey in this.ModifierKeys)
		{
			if (Input.GetKey(modifierKey))
			{
				this._modifier = modifierKey;
				this._isModifier = true;
			}
		}
		foreach (KeyCode value in Enum.GetValues(typeof(KeyCode)))
		{
			if (this.ModifierKeys.Contains(value) && Input.GetKeyUp(value))
			{
				this._isModifier = false;
				this._key = value;
				this._isSpecial = false;
				return true;
			}
			if (!this.ModifierKeys.Contains(value) && value == KeyCode.Mouse0 && Input.GetKeyUp(value))
			{
				this._key = value;
				this._isSpecial = false;
				return true;
			}
			if (!this.ModifierKeys.Contains(value) && value != KeyCode.Mouse0 && Input.GetKeyDown(value))
			{
				this._key = value;
				this._isSpecial = false;
				return true;
			}
		}
		foreach (SpecialKey value2 in Enum.GetValues(typeof(SpecialKey)))
		{
			if (this.GetSpecial(value2))
			{
				this._special = value2;
				this._isSpecial = true;
				return true;
			}
		}
		return false;
	}

	public bool GetKeyDown()
	{
		if (this._isSpecial)
		{
			if (this.GetModifier())
			{
				return this.GetSpecial(this._special);
			}
			return false;
		}
		if (this.GetModifier())
		{
			return Input.GetKeyDown(this._key);
		}
		return false;
	}

	public bool GetKey()
	{
		if (this._isSpecial)
		{
			if (this.GetModifier())
			{
				return this.GetSpecial(this._special);
			}
			return false;
		}
		if (this.GetModifier())
		{
			return Input.GetKey(this._key);
		}
		return false;
	}

	public bool GetKeyUp()
	{
		if (this._isSpecial)
		{
			if (this.GetModifier())
			{
				return this.GetSpecial(this._special);
			}
			return false;
		}
		if (this.GetModifier())
		{
			return Input.GetKeyUp(this._key);
		}
		return false;
	}

	public bool IsWheel()
	{
		if (this._isSpecial)
		{
			if (this._special != SpecialKey.WheelDown)
			{
				return this._special == SpecialKey.WheelUp;
			}
			return true;
		}
		return false;
	}

	public bool IsNone()
	{
		if (this._isSpecial)
		{
			return this._special == SpecialKey.None;
		}
		return false;
	}

	public override string ToString()
	{
		string text = (this._isSpecial ? this._special.ToString() : this._key.ToString());
		if (text.StartsWith("Alpha"))
		{
			text = text.Substring(5);
		}
		if (this._isModifier)
		{
			text = this._modifier.ToString() + "+" + text;
		}
		return text;
	}

	public override bool Equals(object obj)
	{
		return this.ToString() == obj.ToString();
	}

	public void LoadFromString(string serializedKey)
	{
		this._isModifier = false;
		string[] array = serializedKey.Split('+');
		string text = array[0];
		if (array.Length > 1)
		{
			this._modifier = text.ToEnum<KeyCode>();
			this._isModifier = true;
			text = array[1];
		}
		if (text.Length == 1 && this.AlphaDigits.Contains(text))
		{
			text = "Alpha" + text;
		}
		KeyCode keyCode = text.ToEnum<KeyCode>();
		if (keyCode != 0)
		{
			this._key = keyCode;
			this._isSpecial = false;
		}
		else
		{
			this._special = text.ToEnum<SpecialKey>();
			this._isSpecial = true;
		}
	}

	protected bool GetModifier()
	{
		if (this._isModifier)
		{
			return Input.GetKey(this._modifier);
		}
		return true;
	}

	protected bool GetSpecial(SpecialKey specialKey)
	{
		return specialKey switch
		{
			SpecialKey.WheelUp => Input.GetAxis("Mouse ScrollWheel") > 0f, 
			SpecialKey.WheelDown => Input.GetAxis("Mouse ScrollWheel") < 0f, 
			_ => false, 
		};
	}
}
