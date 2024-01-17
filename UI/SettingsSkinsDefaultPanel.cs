namespace UI;

internal class SettingsSkinsDefaultPanel : SettingsCategoryPanel
{
	protected override bool ScrollBar => true;

	protected override float VerticalSpacing => 20f;

	public override void Setup(BasePanel parent = null)
	{
		base.Setup(parent);
		SettingsSkinsPanel obj = (SettingsSkinsPanel)parent;
		obj.CreateCommonSettings(base.DoublePanelLeft, base.DoublePanelRight);
		base.CreateHorizontalDivider(base.DoublePanelRight);
		obj.CreateSkinStringSettings(base.DoublePanelLeft, base.DoublePanelRight);
	}
}
