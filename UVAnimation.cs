using System.IO;
using UnityEngine;

public class UVAnimation
{
	public int curFrame;

	public Vector2[] frames;

	public int loopCycles;

	public bool loopReverse;

	public string name;

	protected int numLoops;

	protected int stepDir = 1;

	public Vector2[] UVDimensions;

	public void BuildFromFile(string path, int index, float uvTime, Texture mainTex)
	{
		if (!File.Exists(path))
		{
			Debug.LogError("wrong ean file path!");
			return;
		}
		FileStream fileStream = new FileStream(path, FileMode.Open);
		BinaryReader br = new BinaryReader(fileStream);
		EanFile eanFile = new EanFile();
		eanFile.Load(br, fileStream);
		fileStream.Close();
		EanAnimation eanAnimation = eanFile.Anims[index];
		this.frames = new Vector2[eanAnimation.TotalCount];
		this.UVDimensions = new Vector2[eanAnimation.TotalCount];
		int tileCount = eanAnimation.TileCount;
		int num = (eanAnimation.TotalCount + tileCount - 1) / tileCount;
		int num2 = 0;
		int width = mainTex.width;
		int height = mainTex.height;
		for (int i = 0; i < num; i++)
		{
			for (int j = 0; j < tileCount; j++)
			{
				if (num2 >= eanAnimation.TotalCount)
				{
					break;
				}
				Vector2 zero = Vector2.zero;
				zero.x = (float)(int)eanAnimation.Frames[num2].Width / (float)width;
				zero.y = (float)(int)eanAnimation.Frames[num2].Height / (float)height;
				this.frames[num2].x = (float)(int)eanAnimation.Frames[num2].X / (float)width;
				this.frames[num2].y = 1f - (float)(int)eanAnimation.Frames[num2].Y / (float)height;
				this.UVDimensions[num2] = zero;
				this.UVDimensions[num2].y = 0f - this.UVDimensions[num2].y;
				num2++;
			}
		}
	}

	public Vector2[] BuildUVAnim(Vector2 start, Vector2 cellSize, int cols, int rows, int totalCells)
	{
		int num = 0;
		this.frames = new Vector2[totalCells];
		this.UVDimensions = new Vector2[totalCells];
		this.frames[0] = start;
		for (int i = 0; i < rows; i++)
		{
			for (int j = 0; j < cols; j++)
			{
				if (num >= totalCells)
				{
					break;
				}
				this.frames[num].x = start.x + cellSize.x * (float)j;
				this.frames[num].y = start.y - cellSize.y * (float)i;
				this.UVDimensions[num] = cellSize;
				this.UVDimensions[num].y = 0f - this.UVDimensions[num].y;
				num++;
			}
		}
		return this.frames;
	}

	public bool GetNextFrame(ref Vector2 uv, ref Vector2 dm)
	{
		if (this.curFrame + this.stepDir >= this.frames.Length || this.curFrame + this.stepDir < 0)
		{
			if (this.stepDir > 0 && this.loopReverse)
			{
				this.stepDir = -1;
				this.curFrame += this.stepDir;
				uv = this.frames[this.curFrame];
				dm = this.UVDimensions[this.curFrame];
			}
			else
			{
				if (this.numLoops + 1 > this.loopCycles && this.loopCycles != -1)
				{
					return false;
				}
				this.numLoops++;
				if (this.loopReverse)
				{
					this.stepDir *= -1;
					this.curFrame += this.stepDir;
				}
				else
				{
					this.curFrame = 0;
				}
				uv = this.frames[this.curFrame];
				dm = this.UVDimensions[this.curFrame];
			}
		}
		else
		{
			this.curFrame += this.stepDir;
			uv = this.frames[this.curFrame];
			dm = this.UVDimensions[this.curFrame];
		}
		return true;
	}

	public void PlayInReverse()
	{
		this.stepDir = -1;
		this.curFrame = this.frames.Length - 1;
	}

	public void Reset()
	{
		this.curFrame = 0;
		this.stepDir = 1;
		this.numLoops = 0;
	}

	public void SetAnim(Vector2[] anim)
	{
		this.frames = anim;
	}
}
