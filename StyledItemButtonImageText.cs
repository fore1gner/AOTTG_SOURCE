using UnityEngine;
using UnityEngine.UI;

public class StyledItemButtonImageText : StyledItem
{
	public class Data
	{
		public Texture2D image;

		public string text;

		public Data(string t, Texture2D tex)
		{
			this.text = t;
			this.image = tex;
		}
	}

	public Button buttonCtrl;

	public RawImage rawImageCtrl;

	public Text textCtrl;

	public override Button GetButton()
	{
		return this.buttonCtrl;
	}

	public override RawImage GetRawImage()
	{
		return this.rawImageCtrl;
	}

	public override Text GetText()
	{
		return this.textCtrl;
	}

	public override void Populate(object o)
	{
		Texture2D texture2D = o as Texture2D;
		if (texture2D != null)
		{
			if (this.rawImageCtrl != null)
			{
				this.rawImageCtrl.texture = texture2D;
			}
			return;
		}
		if (!(o is Data data))
		{
			if (this.textCtrl != null)
			{
				this.textCtrl.text = o.ToString();
			}
			return;
		}
		if (this.rawImageCtrl != null)
		{
			this.rawImageCtrl.texture = data.image;
		}
		if (this.textCtrl != null)
		{
			this.textCtrl.text = data.text;
		}
	}
}
