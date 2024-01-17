using UnityEngine;
using UnityEngine.UI;

namespace UI;

internal class EmoteEmojiPopup : EmoteTextPopup
{
	protected RawImage _emojiImage;

	protected override Vector3 offset => Vector3.up * 3f;

	public override void Setup(BasePanel parent = null)
	{
		this._emojiImage = base.transform.Find("Panel/Emoji").GetComponent<RawImage>();
		base._transform = base.transform;
	}

	protected override void SetEmote(string text)
	{
		this._emojiImage.texture = GameMenu.EmojiTextures[text];
	}
}
