using UnityEngine;

[AddComponentMenu("NGUI/Examples/Equipment")]
public class InvEquipment : MonoBehaviour
{
	private InvAttachmentPoint[] mAttachments;

	private InvGameItem[] mItems;

	public InvGameItem[] equippedItems => this.mItems;

	public InvGameItem Equip(InvGameItem item)
	{
		if (item != null)
		{
			InvBaseItem baseItem = item.baseItem;
			if (baseItem != null)
			{
				return this.Replace(baseItem.slot, item);
			}
			Debug.LogWarning("Can't resolve the item ID of " + item.baseItemID);
		}
		return item;
	}

	public InvGameItem GetItem(InvBaseItem.Slot slot)
	{
		if (slot != 0)
		{
			int num = (int)(slot - 1);
			if (this.mItems != null && num < this.mItems.Length)
			{
				return this.mItems[num];
			}
		}
		return null;
	}

	public bool HasEquipped(InvBaseItem.Slot slot)
	{
		if (this.mItems != null)
		{
			int i = 0;
			for (int num = this.mItems.Length; i < num; i++)
			{
				InvBaseItem baseItem = this.mItems[i].baseItem;
				if (baseItem != null && baseItem.slot == slot)
				{
					return true;
				}
			}
		}
		return false;
	}

	public bool HasEquipped(InvGameItem item)
	{
		if (this.mItems != null)
		{
			int i = 0;
			for (int num = this.mItems.Length; i < num; i++)
			{
				if (this.mItems[i] == item)
				{
					return true;
				}
			}
		}
		return false;
	}

	public InvGameItem Replace(InvBaseItem.Slot slot, InvGameItem item)
	{
		InvBaseItem ınvBaseItem = item?.baseItem;
		if (slot != 0)
		{
			if (ınvBaseItem != null && ınvBaseItem.slot != slot)
			{
				return item;
			}
			if (this.mItems == null)
			{
				int num = 8;
				this.mItems = new InvGameItem[num];
			}
			InvGameItem result = this.mItems[(int)(slot - 1)];
			this.mItems[(int)(slot - 1)] = item;
			if (this.mAttachments == null)
			{
				this.mAttachments = base.GetComponentsInChildren<InvAttachmentPoint>();
			}
			int i = 0;
			for (int num2 = this.mAttachments.Length; i < num2; i++)
			{
				InvAttachmentPoint ınvAttachmentPoint = this.mAttachments[i];
				if (ınvAttachmentPoint.slot != slot)
				{
					continue;
				}
				GameObject gameObject = ınvAttachmentPoint.Attach(ınvBaseItem?.attachment);
				if (ınvBaseItem != null && gameObject != null)
				{
					Renderer renderer = gameObject.renderer;
					if (renderer != null)
					{
						renderer.material.color = ınvBaseItem.color;
					}
				}
			}
			return result;
		}
		if (item != null)
		{
			Debug.LogWarning("Can't equip \"" + item.name + "\" because it doesn't specify an item slot");
		}
		return item;
	}

	public InvGameItem Unequip(InvBaseItem.Slot slot)
	{
		return this.Replace(slot, null);
	}

	public InvGameItem Unequip(InvGameItem item)
	{
		if (item != null)
		{
			InvBaseItem baseItem = item.baseItem;
			if (baseItem != null)
			{
				return this.Replace(baseItem.slot, null);
			}
		}
		return item;
	}
}
