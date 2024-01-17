using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/Examples/Item Database")]
[ExecuteInEditMode]
public class InvDatabase : MonoBehaviour
{
	public int databaseID;

	public UIAtlas iconAtlas;

	public List<InvBaseItem> items = new List<InvBaseItem>();

	private static bool mIsDirty = true;

	private static InvDatabase[] mList;

	public static InvDatabase[] list
	{
		get
		{
			if (InvDatabase.mIsDirty)
			{
				InvDatabase.mIsDirty = false;
				InvDatabase.mList = NGUITools.FindActive<InvDatabase>();
			}
			return InvDatabase.mList;
		}
	}

	public static InvBaseItem FindByID(int id32)
	{
		InvDatabase database = InvDatabase.GetDatabase(id32 >> 16);
		if (!(database == null))
		{
			return database.GetItem(id32 & 0xFFFF);
		}
		return null;
	}

	public static InvBaseItem FindByName(string exact)
	{
		int i = 0;
		for (int num = InvDatabase.list.Length; i < num; i++)
		{
			InvDatabase ınvDatabase = InvDatabase.list[i];
			int j = 0;
			for (int count = ınvDatabase.items.Count; j < count; j++)
			{
				InvBaseItem ınvBaseItem = ınvDatabase.items[j];
				if (ınvBaseItem.name == exact)
				{
					return ınvBaseItem;
				}
			}
		}
		return null;
	}

	public static int FindItemID(InvBaseItem item)
	{
		int i = 0;
		for (int num = InvDatabase.list.Length; i < num; i++)
		{
			InvDatabase ınvDatabase = InvDatabase.list[i];
			if (ınvDatabase.items.Contains(item))
			{
				return (ınvDatabase.databaseID << 16) | item.id16;
			}
		}
		return -1;
	}

	private static InvDatabase GetDatabase(int dbID)
	{
		int i = 0;
		for (int num = InvDatabase.list.Length; i < num; i++)
		{
			InvDatabase ınvDatabase = InvDatabase.list[i];
			if (ınvDatabase.databaseID == dbID)
			{
				return ınvDatabase;
			}
		}
		return null;
	}

	private InvBaseItem GetItem(int id16)
	{
		int i = 0;
		for (int count = this.items.Count; i < count; i++)
		{
			InvBaseItem ınvBaseItem = this.items[i];
			if (ınvBaseItem.id16 == id16)
			{
				return ınvBaseItem;
			}
		}
		return null;
	}

	private void OnDisable()
	{
		InvDatabase.mIsDirty = true;
	}

	private void OnEnable()
	{
		InvDatabase.mIsDirty = true;
	}
}
