using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/Examples/UI Item Storage")]
public class UIItemStorage : MonoBehaviour
{
	public UIWidget background;

	public int maxColumns = 4;

	public int maxItemCount = 8;

	public int maxRows = 4;

	private List<InvGameItem> mItems = new List<InvGameItem>();

	public int padding = 10;

	public int spacing = 128;

	public GameObject template;

	public List<InvGameItem> items
	{
		get
		{
			while (this.mItems.Count < this.maxItemCount)
			{
				this.mItems.Add(null);
			}
			return this.mItems;
		}
	}

	public InvGameItem GetItem(int slot)
	{
		if (slot < this.items.Count)
		{
			return this.mItems[slot];
		}
		return null;
	}

	public InvGameItem Replace(int slot, InvGameItem item)
	{
		if (slot < this.maxItemCount)
		{
			InvGameItem result = this.items[slot];
			this.mItems[slot] = item;
			return result;
		}
		return item;
	}

	private void Start()
	{
		if (!(this.template != null))
		{
			return;
		}
		int num = 0;
		Bounds bounds = default(Bounds);
		for (int i = 0; i < this.maxRows; i++)
		{
			for (int j = 0; j < this.maxColumns; j++)
			{
				GameObject obj = NGUITools.AddChild(base.gameObject, this.template);
				obj.transform.localPosition = new Vector3((float)this.padding + ((float)j + 0.5f) * (float)this.spacing, (float)(-this.padding) - ((float)i + 0.5f) * (float)this.spacing, 0f);
				UIStorageSlot component = obj.GetComponent<UIStorageSlot>();
				if (component != null)
				{
					component.storage = this;
					component.slot = num;
				}
				bounds.Encapsulate(new Vector3((float)this.padding * 2f + (float)((j + 1) * this.spacing), (float)(-this.padding) * 2f - (float)((i + 1) * this.spacing), 0f));
				if (++num >= this.maxItemCount)
				{
					if (this.background != null)
					{
						this.background.transform.localScale = bounds.size;
					}
					return;
				}
			}
		}
		if (this.background != null)
		{
			this.background.transform.localScale = bounds.size;
		}
	}
}
