using System;
using System.Collections.Generic;
using System.Reflection;

namespace Utility;

internal class BaseCSVObject
{
	private static Dictionary<Type, FieldInfo[]> _fields = new Dictionary<Type, FieldInfo[]>();

	protected virtual char Delimiter => ',';

	protected virtual char ParamDelimiter => ':';

	protected virtual bool NamedParams => false;

	public virtual string Serialize()
	{
		List<string> list = new List<string>();
		FieldInfo[] fields = this.GetFields();
		for (int i = 0; i < fields.Length; i++)
		{
			string item = this.SerializeField(fields[i], this);
			list.Add(item);
		}
		return string.Join(this.Delimiter.ToString(), list.ToArray());
	}

	public virtual void Deserialize(string csv)
	{
		string[] array = csv.Split(this.Delimiter);
		FieldInfo[] fields = this.GetFields();
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = array[i].Trim();
		}
		if (this.NamedParams)
		{
			string[] array2 = array;
			for (int j = 0; j < array2.Length; j++)
			{
				string[] array3 = array2[j].Split(this.ParamDelimiter);
				FieldInfo fieldInfo = this.FindField(array3[0]);
				if (fieldInfo != null)
				{
					this.DeserializeField(fieldInfo, this, array3[1]);
				}
			}
			return;
		}
		for (int k = 0; k < fields.Length; k++)
		{
			if (this.IsList(fields[k]))
			{
				Type t = fields[k].FieldType.GetGenericArguments()[0];
				List<object> list = (List<object>)fields[k].GetValue(this);
				list.Clear();
				int num = k;
				while (num < array.Length)
				{
					list.Add(this.DeserializeValue(t, array[num]));
					k++;
				}
				break;
			}
			this.DeserializeField(fields[k], this, array[k]);
		}
	}

	protected virtual FieldInfo[] GetFields()
	{
		Type type = base.GetType();
		if (!BaseCSVObject._fields.ContainsKey(type))
		{
			BaseCSVObject._fields.Add(type, type.GetFields());
		}
		return BaseCSVObject._fields[type];
	}

	protected virtual FieldInfo FindField(string name)
	{
		FieldInfo[] array = BaseCSVObject._fields[base.GetType()];
		foreach (FieldInfo fieldInfo in array)
		{
			if (fieldInfo.Name == name)
			{
				return fieldInfo;
			}
		}
		return null;
	}

	protected virtual bool IsList(FieldInfo field)
	{
		if (field.FieldType.IsGenericType)
		{
			return field.FieldType.GetGenericTypeDefinition() == typeof(IList<>);
		}
		return false;
	}

	protected virtual string SerializeField(FieldInfo info, object instance)
	{
		string text = string.Empty;
		if (this.NamedParams)
		{
			text = info.Name + this.ParamDelimiter;
		}
		if (this.IsList(info))
		{
			List<string> list = new List<string>();
			Type t = info.FieldType.GetGenericArguments()[0];
			foreach (object item in (List<object>)info.GetValue(instance))
			{
				list.Add(this.SerializeValue(t, item));
			}
			return text + string.Join(this.Delimiter.ToString(), list.ToArray());
		}
		return text + this.SerializeValue(info.FieldType, info.GetValue(instance));
	}

	protected virtual void DeserializeField(FieldInfo info, object instance, string value)
	{
		info.SetValue(instance, this.DeserializeValue(info.FieldType, value));
	}

	protected virtual string SerializeValue(Type t, object value)
	{
		if (t == typeof(string))
		{
			return (string)value;
		}
		if (t == typeof(int) || t == typeof(float))
		{
			return value.ToString();
		}
		if (t == typeof(bool))
		{
			return Convert.ToInt32(value).ToString();
		}
		if (typeof(BaseCSVObject).IsAssignableFrom(t))
		{
			return ((BaseCSVObject)value).Serialize();
		}
		return string.Empty;
	}

	protected virtual object DeserializeValue(Type t, string value)
	{
		if (t == typeof(string))
		{
			return value;
		}
		if (t == typeof(int))
		{
			return int.Parse(value);
		}
		if (t == typeof(float))
		{
			return float.Parse(value);
		}
		if (t == typeof(bool))
		{
			return Convert.ToBoolean(int.Parse(value));
		}
		if (typeof(BaseCSVObject).IsAssignableFrom(t))
		{
			BaseCSVObject obj = (BaseCSVObject)Activator.CreateInstance(t);
			obj.Deserialize(value);
			return obj;
		}
		return null;
	}
}
