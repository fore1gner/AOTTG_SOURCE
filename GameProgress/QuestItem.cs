using System;
using Settings;

namespace GameProgress;

internal class QuestItem : BaseSettingsContainer
{
	public StringSetting Category = new StringSetting(string.Empty);

	public ListSetting<StringSetting> Conditions = new ListSetting<StringSetting>();

	public IntSetting Amount = new IntSetting(0);

	public StringSetting RewardType = new StringSetting(string.Empty);

	public StringSetting RewardValue = new StringSetting(string.Empty);

	public StringSetting Icon = new StringSetting(string.Empty);

	public IntSetting Progress = new IntSetting(0);

	public BoolSetting Daily = new BoolSetting(defaultValue: true);

	public IntSetting DayCreated = new IntSetting(0);

	public BoolSetting Collected = new BoolSetting(defaultValue: false);

	public string GetQuestName()
	{
		return this.Category.Value + this.GetConditionsHash() + this.Amount.Value;
	}

	public string GetConditionsHash()
	{
		string text = "";
		foreach (StringSetting item in this.Conditions.Value)
		{
			text += item.Value;
		}
		return text;
	}

	public bool Finished()
	{
		return this.Progress.Value >= this.Amount.Value;
	}

	public void AddProgress(int count = 1)
	{
		this.Progress.Value += count;
		this.Progress.Value = Math.Min(this.Progress.Value, this.Amount.Value);
	}

	public void CollectReward()
	{
		if (!this.Collected.Value && this.Progress.Value >= this.Amount.Value)
		{
			this.Collected.Value = true;
			if (this.RewardType.Value == "Exp")
			{
				GameProgressManager.AddExp(int.Parse(this.RewardValue.Value));
			}
		}
	}
}
