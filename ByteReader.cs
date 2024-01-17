using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ByteReader
{
	private byte[] mBuffer;

	private int mOffset;

	public bool canRead
	{
		get
		{
			if (this.mBuffer != null)
			{
				return this.mOffset < this.mBuffer.Length;
			}
			return false;
		}
	}

	public ByteReader(byte[] bytes)
	{
		this.mBuffer = bytes;
	}

	public ByteReader(TextAsset asset)
	{
		this.mBuffer = asset.bytes;
	}

	public Dictionary<string, string> ReadDictionary()
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		char[] separator = new char[1] { '=' };
		while (this.canRead)
		{
			string text = this.ReadLine();
			if (text == null)
			{
				return dictionary;
			}
			if (!text.StartsWith("//"))
			{
				string[] array = text.Split(separator, 2, StringSplitOptions.RemoveEmptyEntries);
				if (array.Length == 2)
				{
					string key = array[0].Trim();
					string value = array[1].Trim().Replace("\\n", "\n");
					dictionary[key] = value;
				}
			}
		}
		return dictionary;
	}

	public string ReadLine()
	{
		int num = this.mBuffer.Length;
		while (this.mOffset < num && this.mBuffer[this.mOffset] < 32)
		{
			this.mOffset++;
		}
		int num2 = this.mOffset;
		if (num2 >= num)
		{
			this.mOffset = num;
			return null;
		}
		byte b;
		do
		{
			if (num2 < num)
			{
				b = this.mBuffer[num2++];
				continue;
			}
			num2++;
			break;
		}
		while (b != 10 && b != 13);
		string result = ByteReader.ReadLine(this.mBuffer, this.mOffset, num2 - this.mOffset - 1);
		this.mOffset = num2;
		return result;
	}

	private static string ReadLine(byte[] buffer, int start, int count)
	{
		return Encoding.UTF8.GetString(buffer, start, count);
	}
}
