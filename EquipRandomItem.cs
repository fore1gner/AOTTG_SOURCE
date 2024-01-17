using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/Examples/Equip Random Item")]
public class EquipRandomItem : MonoBehaviour
{
	public InvEquipment equipment;

	private void OnClick()
	{
		if (this.equipment != null)
		{
			List<InvBaseItem> items = InvDatabase.list[0].items;
			if (items.Count != 0)
			{
				int max = 12;
				int num = Random.Range(0, items.Count);
				InvBaseItem ınvBaseItem = items[num];
				InvGameItem item = new InvGameItem(num, ınvBaseItem)
				{
					quality = (InvGameItem.Quality)Random.Range(0, max),
					itemLevel = NGUITools.RandomRange(ınvBaseItem.minItemLevel, ınvBaseItem.maxItemLevel)
				};
				this.equipment.Equip(item);
			}
		}
	}
}
