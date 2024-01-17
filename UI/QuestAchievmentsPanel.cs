using System.Collections.Generic;
using GameProgress;
using UnityEngine;
using UnityEngine.UI;

namespace UI;

internal class QuestAchievmentsPanel : QuestCategoryPanel
{
	protected override bool ScrollBar => true;

	public override void Setup(BasePanel parent = null)
	{
		base.Setup(parent);
		base.SinglePanel.GetComponent<VerticalLayoutGroup>().padding = new RectOffset(10, 25, this.VerticalPadding, this.VerticalPadding);
		Transform transform = ElementFactory.InstantiateAndBind(base.SinglePanel, "AchievmentHeader").transform;
		transform.GetComponent<LayoutElement>().preferredWidth = base.QuestItemWidth;
		transform.GetComponent<HorizontalLayoutGroup>().padding = new RectOffset(10, 10, 0, 0);
		QuestPopup questPopup = (QuestPopup)parent;
		questPopup.CreateAchievmentDropdowns(transform.Find("LeftPanel"));
		AchievmentCount achievmentCount = GameProgressManager.GameProgress.Achievment.GetAchievmentCount();
		transform.Find("RightPanel/TrophyCountBronze/Label").GetComponent<Text>().text = achievmentCount.FinishedBronze + "/" + achievmentCount.TotalBronze;
		transform.Find("RightPanel/TrophyCountSilver/Label").GetComponent<Text>().text = achievmentCount.FinishedSilver + "/" + achievmentCount.TotalSilver;
		transform.Find("RightPanel/TrophyCountGold/Label").GetComponent<Text>().text = achievmentCount.FinishedGold + "/" + achievmentCount.TotalGold;
		transform.Find("RightPanel/TrophyCountBronze/Image").GetComponent<Image>().color = UIManager.GetThemeColor(this.ThemePanel, "Trophy", "BronzeColor");
		transform.Find("RightPanel/TrophyCountSilver/Image").GetComponent<Image>().color = UIManager.GetThemeColor(this.ThemePanel, "Trophy", "SilverColor");
		transform.Find("RightPanel/TrophyCountGold/Image").GetComponent<Image>().color = UIManager.GetThemeColor(this.ThemePanel, "Trophy", "GoldColor");
		transform.Find("RightPanel/TrophyCountBronze/Label").GetComponent<Text>().color = UIManager.GetThemeColor(this.ThemePanel, "Trophy", "TextColor");
		transform.Find("RightPanel/TrophyCountSilver/Label").GetComponent<Text>().color = UIManager.GetThemeColor(this.ThemePanel, "Trophy", "TextColor");
		transform.Find("RightPanel/TrophyCountGold/Label").GetComponent<Text>().color = UIManager.GetThemeColor(this.ThemePanel, "Trophy", "TextColor");
		List<QuestItem> list = new List<QuestItem>();
		foreach (AchievmentItem item in GameProgressManager.GameProgress.Achievment.AchievmentItems.Value)
		{
			if (!(questPopup.TierSelection.Value != item.Tier.Value) && (!(questPopup.CompletedSelection.Value == "Completed") || item.Finished()) && (!(questPopup.CompletedSelection.Value == "In Progress") || !item.Finished()))
			{
				list.Add(item);
			}
		}
		base.CreateQuestItems(list);
	}
}
