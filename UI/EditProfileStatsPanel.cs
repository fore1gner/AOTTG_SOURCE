using GameProgress;
using UnityEngine;
using UnityEngine.UI;

namespace UI;

internal class EditProfileStatsPanel : BasePanel
{
	protected override float Width => 720f;

	protected override float Height => 520f;

	protected override bool DoublePanel => true;

	protected override bool DoublePanelDivider => true;

	protected override bool ScrollBar => true;

	protected override float VerticalSpacing => 10f;

	public override void Setup(BasePanel parent = null)
	{
		base.Setup(parent);
		GameStatContainer gameStat = GameProgressManager.GameProgress.GameStat;
		AchievmentCount achievmentCount = GameProgressManager.GameProgress.Achievment.GetAchievmentCount();
		ElementStyle style = new ElementStyle(24, 100f, this.ThemePanel);
		this.CreateTitleLabel(base.DoublePanelLeft, style, "General");
		this.CreateStatLabel(base.DoublePanelLeft, style, "Level", gameStat.Level.Value.ToString());
		this.CreateStatLabel(base.DoublePanelLeft, style, "Exp", gameStat.Exp.Value + "/" + GameProgressManager.GetExpToNext());
		this.CreateStatLabel(base.DoublePanelLeft, style, "Hours played", (gameStat.PlayTime.Value / 3600f).ToString("0.00"));
		this.CreateStatLabel(base.DoublePanelLeft, style, "Highest speed", ((int)gameStat.HighestSpeed.Value).ToString());
		base.CreateHorizontalDivider(base.DoublePanelLeft);
		this.CreateTitleLabel(base.DoublePanelLeft, style, "Achievments");
		this.CreateStatLabel(base.DoublePanelLeft, style, "Bronze", achievmentCount.FinishedBronze + "/" + achievmentCount.TotalBronze);
		this.CreateStatLabel(base.DoublePanelLeft, style, "Silver", achievmentCount.FinishedSilver + "/" + achievmentCount.TotalSilver);
		this.CreateStatLabel(base.DoublePanelLeft, style, "Gold", achievmentCount.FinishedGold + "/" + achievmentCount.TotalGold);
		base.CreateHorizontalDivider(base.DoublePanelLeft);
		this.CreateTitleLabel(base.DoublePanelLeft, style, "Damage");
		this.CreateStatLabel(base.DoublePanelLeft, style, "Highest overall", gameStat.DamageHighestOverall.Value.ToString());
		this.CreateStatLabel(base.DoublePanelLeft, style, "Highest blade", gameStat.DamageHighestBlade.Value.ToString());
		this.CreateStatLabel(base.DoublePanelLeft, style, "Highest gun", gameStat.DamageHighestGun.Value.ToString());
		this.CreateStatLabel(base.DoublePanelLeft, style, "Total overall", gameStat.DamageTotalOverall.Value.ToString());
		this.CreateStatLabel(base.DoublePanelLeft, style, "Total blade", gameStat.DamageTotalBlade.Value.ToString());
		this.CreateStatLabel(base.DoublePanelLeft, style, "Total gun", gameStat.DamageTotalGun.Value.ToString());
		this.CreateTitleLabel(base.DoublePanelRight, style, "Titans Killed");
		this.CreateStatLabel(base.DoublePanelRight, style, "Total", gameStat.TitansKilledTotal.Value.ToString());
		this.CreateStatLabel(base.DoublePanelRight, style, "Blade", gameStat.TitansKilledBlade.Value.ToString());
		this.CreateStatLabel(base.DoublePanelRight, style, "Gun", gameStat.TitansKilledGun.Value.ToString());
		this.CreateStatLabel(base.DoublePanelRight, style, "Thunder spear", gameStat.TitansKilledThunderSpear.Value.ToString());
		this.CreateStatLabel(base.DoublePanelRight, style, "Other", gameStat.TitansKilledOther.Value.ToString());
		base.CreateHorizontalDivider(base.DoublePanelRight);
		this.CreateTitleLabel(base.DoublePanelRight, style, "Humans Killed");
		this.CreateStatLabel(base.DoublePanelRight, style, "Total", gameStat.HumansKilledTotal.Value.ToString());
		this.CreateStatLabel(base.DoublePanelRight, style, "Blade", gameStat.HumansKilledBlade.Value.ToString());
		this.CreateStatLabel(base.DoublePanelRight, style, "Gun", gameStat.HumansKilledGun.Value.ToString());
		this.CreateStatLabel(base.DoublePanelRight, style, "Thunder spear", gameStat.HumansKilledThunderSpear.Value.ToString());
		this.CreateStatLabel(base.DoublePanelRight, style, "Titan", gameStat.HumansKilledTitan.Value.ToString());
		this.CreateStatLabel(base.DoublePanelRight, style, "Other", gameStat.TitansKilledOther.Value.ToString());
	}

	protected void CreateStatLabel(Transform panel, ElementStyle style, string title, string value)
	{
		ElementFactory.CreateDefaultLabel(panel, style, title + ": " + value, FontStyle.Normal, TextAnchor.MiddleLeft).GetComponent<Text>();
	}

	protected void CreateTitleLabel(Transform panel, ElementStyle style, string title)
	{
		ElementFactory.CreateDefaultLabel(panel, style, title, FontStyle.Bold, TextAnchor.MiddleLeft).GetComponent<Text>();
	}
}
