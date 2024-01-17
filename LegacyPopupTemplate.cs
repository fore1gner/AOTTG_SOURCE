using UnityEngine;

internal class LegacyPopupTemplate
{
	private Color BorderColor;

	private Texture2D BackgroundTexture;

	private float PositionX;

	private float PositionY;

	private float Width;

	private float Height;

	private float BorderThickness;

	private float Padding;

	private Color ButtonColor;

	public LegacyPopupTemplate(Color borderColor, Texture2D bgTexture, Color buttonColor, float x, float y, float w, float h, float borderThickness)
	{
		this.BorderColor = borderColor;
		this.BackgroundTexture = bgTexture;
		this.ButtonColor = buttonColor;
		this.PositionX = x - w / 2f;
		this.PositionY = y - h / 2f;
		this.Width = w;
		this.Height = h;
		this.BorderThickness = borderThickness;
	}

	public void DrawPopup(string message, float messageWidth, float messageHeight)
	{
		this.DrawPopupBackground();
		float num = (this.Width - messageWidth) * 0.5f;
		float num2 = (this.Height - messageHeight) * 0.5f;
		GUI.Label(new Rect(this.PositionX + num, this.PositionY + num2, messageWidth, messageHeight), message);
	}

	public bool DrawPopupWithButton(string message, float messageWidth, float messageHeight, string buttonMessage, float buttonWidth, float buttonHeight)
	{
		this.DrawPopupBackground();
		float num = (this.Width - messageWidth) * 0.5f;
		float num2 = (this.Width - buttonWidth) * 0.5f;
		float num3 = (this.Height - messageHeight - buttonHeight) / 3f;
		GUI.Label(new Rect(this.PositionX + num, this.PositionY + num3, messageWidth, messageHeight), message);
		float left = this.PositionX + num2;
		float top = this.PositionY + this.Height - buttonHeight - num3;
		GUI.backgroundColor = this.ButtonColor;
		return GUI.Button(new Rect(left, top, buttonWidth, buttonHeight), buttonMessage);
	}

	public bool[] DrawPopupWithTwoButtons(string message, float messageWidth, float messageHeight, string button1Message, float button1Width, string button2Message, float button2Width, float buttonHeight)
	{
		this.DrawPopupBackground();
		float num = (this.Width - messageWidth) * 0.5f;
		float num2 = (this.Width - button1Width - button2Width) / 3f;
		float num3 = (this.Height - messageHeight - buttonHeight) / 3f;
		GUI.Label(new Rect(this.PositionX + num, this.PositionY + num3, messageWidth, messageHeight), message);
		float num4 = this.PositionX + num2;
		float left = num4 + button1Width + num2;
		float top = this.PositionY + this.Height - buttonHeight - num3;
		GUI.backgroundColor = this.ButtonColor;
		return new bool[2]
		{
			GUI.Button(new Rect(num4, top, button1Width, buttonHeight), button1Message),
			GUI.Button(new Rect(left, top, button2Width, buttonHeight), button2Message)
		};
	}

	private void DrawPopupBackground()
	{
		GUI.backgroundColor = this.BorderColor;
		GUI.Box(new Rect(this.PositionX, this.PositionY, this.Width, this.Height), string.Empty);
		GUI.DrawTexture(new Rect(this.PositionX + this.BorderThickness, this.PositionY + this.BorderThickness, this.Width - 2f * this.BorderThickness, this.Height - 2f * this.BorderThickness), this.BackgroundTexture);
	}
}
