using System.Collections.Generic;
using System.Text;
using UnityEngine;

[AddComponentMenu("NGUI/UI/Text List")]
public class UITextList : MonoBehaviour
{
	protected class Paragraph
	{
		public string[] lines;

		public string text;
	}

	public enum Style
	{
		Text,
		Chat
	}

	public int maxEntries = 50;

	public float maxHeight;

	public float maxWidth;

	protected List<Paragraph> mParagraphs = new List<Paragraph>();

	protected float mScroll;

	protected bool mSelected;

	protected char[] mSeparator = new char[1] { '\n' };

	protected int mTotalLines;

	public Style style;

	public bool supportScrollWheel = true;

	public UILabel textLabel;

	public void Add(string text)
	{
		this.Add(text, updateVisible: true);
	}

	protected void Add(string text, bool updateVisible)
	{
		Paragraph paragraph = null;
		if (this.mParagraphs.Count < this.maxEntries)
		{
			paragraph = new Paragraph();
		}
		else
		{
			paragraph = this.mParagraphs[0];
			this.mParagraphs.RemoveAt(0);
		}
		paragraph.text = text;
		this.mParagraphs.Add(paragraph);
		if (this.textLabel != null && this.textLabel.font != null)
		{
			paragraph.lines = this.textLabel.font.WrapText(paragraph.text, this.maxWidth / this.textLabel.transform.localScale.y, this.textLabel.maxLineCount, this.textLabel.supportEncoding, this.textLabel.symbolStyle).Split(this.mSeparator);
			this.mTotalLines = 0;
			int i = 0;
			for (int count = this.mParagraphs.Count; i < count; i++)
			{
				this.mTotalLines += this.mParagraphs[i].lines.Length;
			}
		}
		if (updateVisible)
		{
			this.UpdateVisibleText();
		}
	}

	private void Awake()
	{
		if (this.textLabel == null)
		{
			this.textLabel = base.GetComponentInChildren<UILabel>();
		}
		if (this.textLabel != null)
		{
			this.textLabel.lineWidth = 0;
		}
		Collider collider = base.collider;
		if (collider != null)
		{
			if (this.maxHeight <= 0f)
			{
				this.maxHeight = collider.bounds.size.y / base.transform.lossyScale.y;
			}
			if (this.maxWidth <= 0f)
			{
				this.maxWidth = collider.bounds.size.x / base.transform.lossyScale.x;
			}
		}
	}

	public void Clear()
	{
		this.mParagraphs.Clear();
		this.UpdateVisibleText();
	}

	private void OnScroll(float val)
	{
		if (this.mSelected && this.supportScrollWheel)
		{
			val *= ((this.style != Style.Chat) ? (-10f) : 10f);
			this.mScroll = Mathf.Max(0f, this.mScroll + val);
			this.UpdateVisibleText();
		}
	}

	private void OnSelect(bool selected)
	{
		this.mSelected = selected;
	}

	protected void UpdateVisibleText()
	{
		if (!(this.textLabel != null) || !(this.textLabel.font != null))
		{
			return;
		}
		int num = 0;
		int num2 = ((this.maxHeight <= 0f) ? 100000 : Mathf.FloorToInt(this.maxHeight / this.textLabel.cachedTransform.localScale.y));
		int num3 = Mathf.RoundToInt(this.mScroll);
		if (num2 + num3 > this.mTotalLines)
		{
			num3 = Mathf.Max(0, this.mTotalLines - num2);
			this.mScroll = num3;
		}
		if (this.style == Style.Chat)
		{
			num3 = Mathf.Max(0, this.mTotalLines - num2 - num3);
		}
		StringBuilder stringBuilder = new StringBuilder();
		int i = 0;
		for (int count = this.mParagraphs.Count; i < count; i++)
		{
			Paragraph paragraph = this.mParagraphs[i];
			int j = 0;
			for (int num4 = paragraph.lines.Length; j < num4; j++)
			{
				string value = paragraph.lines[j];
				if (num3 > 0)
				{
					num3--;
					continue;
				}
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append("\n");
				}
				stringBuilder.Append(value);
				num++;
				if (num >= num2)
				{
					break;
				}
			}
			if (num >= num2)
			{
				break;
			}
		}
		this.textLabel.text = stringBuilder.ToString();
	}
}
