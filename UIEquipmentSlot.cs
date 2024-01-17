using UnityEngine;

[AddComponentMenu("NGUI/Examples/UI Equipment Slot")]
public class UIEquipmentSlot : UIItemSlot
{
	public InvEquipment equipment;

	public InvBaseItem.Slot slot;

	protected override InvGameItem observedItem
	{
		get
		{
			if (!(this.equipment == null))
			{
				return this.equipment.GetItem(this.slot);
			}
			return null;
		}
	}

	protected override InvGameItem Replace(InvGameItem item)
	{
		if (!(this.equipment == null))
		{
			return this.equipment.Replace(this.slot, item);
		}
		return item;
	}
}
