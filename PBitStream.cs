using System;
using System.Collections.Generic;

public class PBitStream
{
	private int currentByte;

	private List<byte> streamBytes;

	private int totalBits;

	public int BitCount
	{
		get
		{
			return this.totalBits;
		}
		private set
		{
			this.totalBits = value;
		}
	}

	public int ByteCount => PBitStream.BytesForBits(this.totalBits);

	public int Position { get; set; }

	public PBitStream()
	{
		this.streamBytes = new List<byte>(1);
	}

	public PBitStream(int bitCount)
	{
		this.streamBytes = new List<byte>(PBitStream.BytesForBits(bitCount));
	}

	public PBitStream(IEnumerable<byte> bytes, int bitCount)
	{
		this.streamBytes = new List<byte>(bytes);
		this.BitCount = bitCount;
	}

	public void Add(bool val)
	{
		int num = this.totalBits / 8;
		if (num > this.streamBytes.Count - 1 || this.totalBits == 0)
		{
			this.streamBytes.Add(0);
		}
		if (val)
		{
			int num2 = 7 - this.totalBits % 8;
			this.streamBytes[num] |= (byte)(1 << num2);
		}
		this.totalBits++;
	}

	public static int BytesForBits(int bitCount)
	{
		if (bitCount <= 0)
		{
			return 0;
		}
		return (bitCount - 1) / 8 + 1;
	}

	public bool Get(int bitIndex)
	{
		int index = bitIndex / 8;
		int num = 7 - bitIndex % 8;
		return (this.streamBytes[index] & (byte)(1 << num)) > 0;
	}

	public bool GetNext()
	{
		if (this.Position > this.totalBits)
		{
			throw new Exception("End of PBitStream reached. Can't read more.");
		}
		return this.Get(this.Position++);
	}

	public void Set(int bitIndex, bool value)
	{
		int index = bitIndex / 8;
		int num = 7 - bitIndex % 8;
		this.streamBytes[index] |= (byte)(1 << num);
	}

	public byte[] ToBytes()
	{
		return this.streamBytes.ToArray();
	}
}
