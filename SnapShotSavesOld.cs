using UnityEngine;

public class SnapShotSavesOld
{
	private static int currentIndex;

	private static int[] dmg;

	private static Texture2D[] img;

	private static int index;

	private static bool inited;

	private static int maxIndex;

	public static void addIMG(Texture2D t, int d)
	{
		SnapShotSavesOld.init();
		SnapShotSavesOld.img[SnapShotSavesOld.index] = t;
		SnapShotSavesOld.dmg[SnapShotSavesOld.index] = d;
		SnapShotSavesOld.currentIndex = SnapShotSavesOld.index;
		SnapShotSavesOld.index++;
		if (SnapShotSavesOld.index >= SnapShotSavesOld.img.Length)
		{
			SnapShotSavesOld.index = 0;
		}
		SnapShotSavesOld.maxIndex = Mathf.Max(SnapShotSavesOld.index, SnapShotSavesOld.maxIndex);
	}

	public static int getCurrentDMG()
	{
		if (SnapShotSavesOld.maxIndex == 0)
		{
			return 0;
		}
		return SnapShotSavesOld.dmg[SnapShotSavesOld.currentIndex];
	}

	public static Texture2D getCurrentIMG()
	{
		if (SnapShotSavesOld.maxIndex == 0)
		{
			return null;
		}
		return SnapShotSavesOld.img[SnapShotSavesOld.currentIndex];
	}

	public static int getCurrentIndex()
	{
		return SnapShotSavesOld.currentIndex;
	}

	public static int getLength()
	{
		return SnapShotSavesOld.maxIndex;
	}

	public static int getMaxIndex()
	{
		return SnapShotSavesOld.maxIndex;
	}

	public static Texture2D GetNextIMG()
	{
		SnapShotSavesOld.currentIndex++;
		if (SnapShotSavesOld.currentIndex >= SnapShotSavesOld.maxIndex)
		{
			SnapShotSavesOld.currentIndex = 0;
		}
		return SnapShotSavesOld.getCurrentIMG();
	}

	public static Texture2D GetPrevIMG()
	{
		SnapShotSavesOld.currentIndex--;
		if (SnapShotSavesOld.currentIndex < 0)
		{
			SnapShotSavesOld.currentIndex = SnapShotSavesOld.maxIndex - 1;
		}
		return SnapShotSavesOld.getCurrentIMG();
	}

	public static void init()
	{
		if (!SnapShotSavesOld.inited)
		{
			SnapShotSavesOld.inited = true;
			SnapShotSavesOld.index = 0;
			SnapShotSavesOld.maxIndex = 0;
			SnapShotSavesOld.img = new Texture2D[99];
			SnapShotSavesOld.dmg = new int[99];
		}
	}
}
