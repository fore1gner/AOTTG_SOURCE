namespace UI;

internal class ElementStyle
{
	public int FontSize;

	public float TitleWidth;

	public string ThemePanel;

	public static ElementStyle Default = new ElementStyle();

	public ElementStyle(int fontSize = 24, float titleWidth = 120f, string themePanel = "DefaultPanel")
	{
		this.FontSize = fontSize;
		this.TitleWidth = titleWidth;
		this.ThemePanel = themePanel;
	}
}
