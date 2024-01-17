using UnityEngine;

namespace UI;

internal class MultiplayerFilterPopup : PromptPopup
{
	protected override string Title => UIManager.GetLocaleCommon("Filters");

	protected override int VerticalPadding => 20;

	protected override int HorizontalPadding => 20;

	protected override float VerticalSpacing => 20f;

	protected override float Width => 370f;

	protected override float Height => 250f;

	protected override TextAnchor PanelAlignment => TextAnchor.MiddleCenter;

	public override void Setup(BasePanel parent = null)
	{
		base.Setup(parent);
		MultiplayerRoomListPopup multiplayerRoomListPopup = (MultiplayerRoomListPopup)parent;
		string category = "MainMenu";
		string subCategory = "MultiplayerFilterPopup";
		ElementStyle style = new ElementStyle(this.ButtonFontSize, 120f, this.ThemePanel);
		ElementStyle style2 = new ElementStyle(24, 240f, this.ThemePanel);
		ElementFactory.CreateDefaultButton(base.BottomBar, style, UIManager.GetLocaleCommon("Confirm"), 0f, 0f, delegate
		{
			this.OnButtonClick("Confirm");
		});
		ElementFactory.CreateToggleSetting(base.SinglePanel, style2, multiplayerRoomListPopup._filterShowFull, UIManager.GetLocale(category, subCategory, "ShowFull"));
		ElementFactory.CreateToggleSetting(base.SinglePanel, style2, multiplayerRoomListPopup._filterShowPassword, UIManager.GetLocale(category, subCategory, "ShowPassword"));
	}

	protected void OnButtonClick(string name)
	{
		if (name == "Confirm")
		{
			this.Hide();
			((MultiplayerRoomListPopup)base.Parent).RefreshList();
		}
	}
}
