using Settings;

namespace UI;

internal class EditProfileProfilePanel : BasePanel
{
	protected override float Width => 720f;

	protected override float Height => 520f;

	public override void Setup(BasePanel parent = null)
	{
		base.Setup(parent);
		ProfileSettings profileSettings = SettingsManager.ProfileSettings;
		ElementStyle style = new ElementStyle(24, 120f, this.ThemePanel);
		ElementFactory.CreateInputSetting(base.SinglePanel, style, profileSettings.Name, UIManager.GetLocaleCommon("Name"), "", 200f);
		ElementFactory.CreateInputSetting(base.SinglePanel, style, profileSettings.Guild, UIManager.GetLocaleCommon("Guild"), "", 200f);
	}
}
