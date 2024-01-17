namespace Utility;

internal class BaseCSVContainer : BaseCSVObject
{
	protected override char Delimiter => ';';

	protected virtual bool UseNewlines => true;

	public override string Serialize()
	{
		string text = base.Serialize();
		if (this.UseNewlines)
		{
			text = this.InsertNewlines(text);
		}
		return text;
	}

	public virtual string InsertNewlines(string str)
	{
		string[] value = str.Split(this.Delimiter);
		return string.Join(this.Delimiter + "\n", value);
	}
}
