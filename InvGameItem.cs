using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InvGameItem
{
	public enum Quality
	{
		Broken,
		Cursed,
		Damaged,
		Worn,
		Sturdy,
		Polished,
		Improved,
		Crafted,
		Superior,
		Enchanted,
		Epic,
		Legendary,
		_LastDoNotUse
	}

	public int itemLevel;

	private InvBaseItem mBaseItem;

	[SerializeField]
	private int mBaseItemID;

	public Quality quality;

	public InvBaseItem baseItem
	{
		get
		{
			if (this.mBaseItem == null)
			{
				this.mBaseItem = InvDatabase.FindByID(this.baseItemID);
			}
			return this.mBaseItem;
		}
	}

	public int baseItemID => this.mBaseItemID;

	public Color color
	{
		get
		{
			Color white = Color.white;
			return this.quality switch
			{
				Quality.Broken => new Color(0.4f, 0.2f, 0.2f), 
				Quality.Cursed => Color.red, 
				Quality.Damaged => new Color(0.4f, 0.4f, 0.4f), 
				Quality.Worn => new Color(0.7f, 0.7f, 0.7f), 
				Quality.Sturdy => new Color(1f, 1f, 1f), 
				Quality.Polished => NGUIMath.HexToColor(3774856959u), 
				Quality.Improved => NGUIMath.HexToColor(2480359935u), 
				Quality.Crafted => NGUIMath.HexToColor(1325334783u), 
				Quality.Superior => NGUIMath.HexToColor(12255231u), 
				Quality.Enchanted => NGUIMath.HexToColor(1937178111u), 
				Quality.Epic => NGUIMath.HexToColor(2516647935u), 
				Quality.Legendary => NGUIMath.HexToColor(4287627519u), 
				_ => white, 
			};
		}
	}

	public string name
	{
		get
		{
			if (this.baseItem == null)
			{
				return null;
			}
			return this.quality.ToString() + " " + this.baseItem.name;
		}
	}

	public float statMultiplier
	{
		get
		{
			float num = 0f;
			switch (this.quality)
			{
			case Quality.Broken:
				num = 0f;
				break;
			case Quality.Cursed:
				num = -1f;
				break;
			case Quality.Damaged:
				num = 0.25f;
				break;
			case Quality.Worn:
				num = 0.9f;
				break;
			case Quality.Sturdy:
				num = 1f;
				break;
			case Quality.Polished:
				num = 1.1f;
				break;
			case Quality.Improved:
				num = 1.25f;
				break;
			case Quality.Crafted:
				num = 1.5f;
				break;
			case Quality.Superior:
				num = 1.75f;
				break;
			case Quality.Enchanted:
				num = 2f;
				break;
			case Quality.Epic:
				num = 2.5f;
				break;
			case Quality.Legendary:
				num = 3f;
				break;
			}
			float num2 = (float)this.itemLevel / 50f;
			return num * Mathf.Lerp(num2, num2 * num2, 0.5f);
		}
	}

	public InvGameItem(int id)
	{
		this.quality = Quality.Sturdy;
		this.itemLevel = 1;
		this.mBaseItemID = id;
	}

	public InvGameItem(int id, InvBaseItem bi)
	{
		this.quality = Quality.Sturdy;
		this.itemLevel = 1;
		this.mBaseItemID = id;
		this.mBaseItem = bi;
	}

	public List<InvStat> CalculateStats()
	{
		List<InvStat> list = new List<InvStat>();
		if (this.baseItem != null)
		{
			float num = this.statMultiplier;
			List<InvStat> stats = this.baseItem.stats;
			int i = 0;
			for (int count = stats.Count; i < count; i++)
			{
				InvStat ınvStat = stats[i];
				int num2 = Mathf.RoundToInt(num * (float)ınvStat.amount);
				if (num2 == 0)
				{
					continue;
				}
				bool flag = false;
				int j = 0;
				for (int count2 = list.Count; j < count2; j++)
				{
					InvStat ınvStat2 = list[j];
					if (ınvStat2.id == ınvStat.id && ınvStat2.modifier == ınvStat.modifier)
					{
						ınvStat2.amount += num2;
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					InvStat item = new InvStat
					{
						id = ınvStat.id,
						amount = num2,
						modifier = ınvStat.modifier
					};
					list.Add(item);
				}
			}
			list.Sort(InvStat.CompareArmor);
		}
		return list;
	}
}
