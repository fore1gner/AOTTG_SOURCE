using GameProgress;
using UnityEngine;
using UnityEngine.UI;

namespace UI;

internal class QuestDailyPanel : QuestCategoryPanel
{
	public override void Setup(BasePanel parent = null)
	{
		base.Setup(parent);
		ElementFactory.CreateDefaultLabel(base.SinglePanel, new ElementStyle(24, 120f, this.ThemePanel), QuestHandler.GetTimeToQuestReset(daily: true), FontStyle.Normal, TextAnchor.MiddleLeft).GetComponent<Text>().color = UIManager.GetThemeColor(this.ThemePanel, "QuestHeader", "ResetTextColor");
		base.CreateQuestItems(GameProgressManager.GameProgress.Quest.DailyQuestItems.Value);
	}
}
