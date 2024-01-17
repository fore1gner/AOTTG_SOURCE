using UnityEngine;

[AddComponentMenu("NGUI/Examples/Equip Items")]
public class EquipItems : MonoBehaviour
{
	public int[] itemIDs;

	private void Start()
	{
		if (this.itemIDs != null && this.itemIDs.Length != 0)
		{
			InvEquipment ınvEquipment = base.GetComponent<InvEquipment>();
			if (ınvEquipment == null)
			{
				ınvEquipment = base.gameObject.AddComponent<InvEquipment>();
			}
			int max = 12;
			int i = 0;
			for (int num = this.itemIDs.Length; i < num; i++)
			{
				int num2 = this.itemIDs[i];
				InvBaseItem ınvBaseItem = InvDatabase.FindByID(num2);
				if (ınvBaseItem != null)
				{
					InvGameItem item = new InvGameItem(num2, ınvBaseItem)
					{
						quality = (InvGameItem.Quality)Random.Range(0, max),
						itemLevel = NGUITools.RandomRange(ınvBaseItem.minItemLevel, ınvBaseItem.maxItemLevel)
					};
					ınvEquipment.Equip(item);
				}
				else
				{
					Debug.LogWarning("Can't resolve the item ID of " + num2);
				}
			}
		}
		Object.Destroy(this);
	}
}
