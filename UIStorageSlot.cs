using UnityEngine;

[AddComponentMenu("NGUI/Examples/UI Storage Slot")]
public class UIStorageSlot : UIItemSlot
{
	public int slot;

	public UIItemStorage storage;

	protected override InvGameItem observedItem
	{
		get
		{
			if (!(this.storage == null))
			{
				return this.storage.GetItem(this.slot);
			}
			return null;
		}
	}

	protected override InvGameItem Replace(InvGameItem item)
	{
		if (!(this.storage == null))
		{
			return this.storage.Replace(this.slot, item);
		}
		return item;
	}
}
