namespace Settings;

internal abstract class TypedSetting<T> : BaseSetting
{
	protected T DefaultValue;

	protected T _value;

	public T Value
	{
		get
		{
			return this._value;
		}
		set
		{
			this._value = this.SanitizeValue(value);
		}
	}

	public TypedSetting()
	{
	}

	public TypedSetting(T defaultValue)
	{
		this.DefaultValue = this.SanitizeValue(defaultValue);
		this.SetDefault();
	}

	public override void SetDefault()
	{
		this.Value = this.DefaultValue;
	}

	protected virtual T SanitizeValue(T value)
	{
		return value;
	}
}
