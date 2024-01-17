using System.Collections.Generic;
using System.Linq;
using ApplicationManagers;
using Settings;
using UnityEngine;

namespace GameProgress;

internal class AchievmentHandler : QuestHandler
{
	private AchievmentContainer _achievment;

	public AchievmentHandler(AchievmentContainer achievment)
		: base(null)
	{
		this._achievment = achievment;
		this.ReloadAchievments();
	}

	public void ReloadAchievments()
	{
		this.LoadAchievments();
		this.CacheActiveAchievments();
	}

	private void LoadAchievments()
	{
		ListSetting<AchievmentItem> listSetting = new ListSetting<AchievmentItem>();
		Dictionary<string, AchievmentItem> dictionary = new Dictionary<string, AchievmentItem>();
		foreach (AchievmentItem item in this._achievment.AchievmentItems.Value)
		{
			dictionary.Add(item.GetQuestName(), item);
		}
		AchievmentContainer achievmentContainer = new AchievmentContainer();
		achievmentContainer.DeserializeFromJsonString(((TextAsset)AssetBundleManager.MainAssetBundle.Load("AchievmentList")).text);
		foreach (AchievmentItem item2 in achievmentContainer.AchievmentItems.Value)
		{
			if (dictionary.ContainsKey(item2.GetQuestName()))
			{
				AchievmentItem achievmentItem = dictionary[item2.GetQuestName()];
				item2.Progress.Value = achievmentItem.Progress.Value;
			}
			item2.Active.Value = false;
			listSetting.Value.Add(item2);
		}
		this._achievment.AchievmentItems.Copy(listSetting);
	}

	private void CacheActiveAchievments()
	{
		base._activeQuests.Clear();
		Dictionary<string, List<AchievmentItem>> dictionary = new Dictionary<string, List<AchievmentItem>>();
		foreach (AchievmentItem item in this._achievment.AchievmentItems.Value)
		{
			string key = item.Category.Value + item.GetConditionsHash();
			if (!dictionary.ContainsKey(key))
			{
				dictionary.Add(key, new List<AchievmentItem>());
			}
			dictionary[key].Add(item);
		}
		foreach (string key2 in dictionary.Keys)
		{
			List<AchievmentItem> list = dictionary[key2].OrderBy((AchievmentItem x) => x.GetQuestName()).ToList();
			AchievmentItem achievmentItem = null;
			foreach (AchievmentItem item2 in list)
			{
				if (item2.Progress.Value < item2.Amount.Value)
				{
					achievmentItem = item2;
					break;
				}
			}
			if (achievmentItem != null)
			{
				achievmentItem.Active.Value = true;
				base.AddActiveQuest(achievmentItem);
			}
		}
	}
}
