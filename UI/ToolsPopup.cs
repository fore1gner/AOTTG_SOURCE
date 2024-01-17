using UnityEngine;

namespace UI;

internal class ToolsPopup : BasePopup
{
	protected override string Title => UIManager.GetLocale("MainMenu", "ToolsPopup", "Title");

	protected override float Width => 280f;

	protected override float Height => 355f;

	protected override float VerticalSpacing => 20f;

	protected override int VerticalPadding => 20;

	public override void Setup(BasePanel parent = null)
	{
		base.Setup(parent);
		string category = "MainMenu";
		string subCategory = "ToolsPopup";
		float elementWidth = 210f;
		ElementStyle style = new ElementStyle(this.ButtonFontSize, 120f, this.ThemePanel);
		ElementFactory.CreateDefaultButton(base.BottomBar, style, UIManager.GetLocaleCommon("Back"), 0f, 0f, delegate
		{
			this.OnButtonClick("Back");
		});
		ElementFactory.CreateDefaultButton(base.SinglePanel, style, UIManager.GetLocale(category, subCategory, "ButtonMapEditor"), elementWidth, 0f, delegate
		{
			this.OnButtonClick("MapEditor");
		});
		ElementFactory.CreateDefaultButton(base.SinglePanel, style, UIManager.GetLocale(category, subCategory, "ButtonCharacterEditor"), elementWidth, 0f, delegate
		{
			this.OnButtonClick("CharacterEditor");
		});
		ElementFactory.CreateDefaultButton(base.SinglePanel, style, UIManager.GetLocale(category, subCategory, "ButtonSnapshotViewer"), elementWidth, 0f, delegate
		{
			this.OnButtonClick("SnapshotViewer");
		});
	}

	protected void OnButtonClick(string name)
	{
		switch (name)
		{
		case "MapEditor":
			FengGameManagerMKII.settingsOld[64] = 101;
			Application.LoadLevel(2);
			break;
		case "CharacterEditor":
			Application.LoadLevel("characterCreation");
			break;
		case "SnapshotViewer":
			Application.LoadLevel("SnapShot");
			break;
		case "Back":
			this.Hide();
			break;
		}
	}
}
