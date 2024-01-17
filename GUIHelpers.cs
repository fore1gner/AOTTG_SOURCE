using UnityEngine;

public static class GUIHelpers
{
	public enum Alignment
	{
		TOPLEFT,
		TOPCENTER,
		TOPRIGHT,
		RIGHT,
		BOTTOMRIGHT,
		BOTTOMCENTER,
		BOTTOMLEFT,
		LEFT,
		CENTER
	}

	public static Vector2 mousePos => Event.current.mousePosition;

	public static Vector2 mousePosInvertY => GUIHelpers.FlipY(GUIHelpers.mousePos);

	public static Rect screenRect => new Rect(0f, 0f, Screen.width, Screen.height);

	public static Rect AlignRect(float width, float height, Alignment alignment)
	{
		return GUIHelpers.AlignRect(width, height, new Rect(0f, 0f, Screen.width, Screen.height), alignment, 0f, 0f);
	}

	public static Rect AlignRect(float width, float height, Rect parentRect, Alignment alignment)
	{
		return GUIHelpers.AlignRect(width, height, parentRect, alignment, 0f, 0f);
	}

	public static Rect AlignRect(float width, float height, Alignment alignment, float xOffset, float yOffset)
	{
		return GUIHelpers.AlignRect(width, height, new Rect(0f, 0f, Screen.width, Screen.height), alignment, xOffset, yOffset);
	}

	public static Rect AlignRect(float width, float height, Rect parentRect, Alignment alignment, float xOffset, float yOffset)
	{
		Rect result = alignment switch
		{
			Alignment.TOPLEFT => new Rect(0f, 0f, width, height), 
			Alignment.TOPCENTER => new Rect(parentRect.width * 0.5f - width * 0.5f, 0f, width, height), 
			Alignment.TOPRIGHT => new Rect(parentRect.width - width, 0f, width, height), 
			Alignment.RIGHT => new Rect(parentRect.width - width, parentRect.height * 0.5f - height * 0.5f, width, height), 
			Alignment.BOTTOMRIGHT => new Rect(parentRect.width - width, parentRect.height - height, width, height), 
			Alignment.BOTTOMCENTER => new Rect(parentRect.width * 0.5f - width * 0.5f, parentRect.height - height, width, height), 
			Alignment.BOTTOMLEFT => new Rect(0f, parentRect.y + parentRect.height - height, width, height), 
			Alignment.LEFT => new Rect(0f, parentRect.height * 0.5f - height * 0.5f, width, height), 
			Alignment.CENTER => new Rect(parentRect.width * 0.5f - width * 0.5f, parentRect.height * 0.5f - height * 0.5f, width, height), 
			_ => new Rect(0f, 0f, width, height), 
		};
		result.x += parentRect.x + xOffset;
		result.y += parentRect.y + yOffset;
		return result;
	}

	public static Rect ClampPosition(this Rect r, Rect borderRect)
	{
		return new Rect(Mathf.Clamp(r.x, borderRect.x, borderRect.x + borderRect.width - r.width), Mathf.Clamp(r.y, borderRect.y, borderRect.y + borderRect.height - r.height), r.width, r.height);
	}

	public static Vector2 FixedTouchDelta(this Touch aTouch)
	{
		float num = Time.deltaTime / aTouch.deltaTime;
		if (num == 0f || float.IsNaN(num) || float.IsInfinity(num))
		{
			num = 1f;
		}
		return aTouch.deltaPosition * num;
	}

	public static Vector2 FlipY(Vector2 inPos)
	{
		inPos.y = (float)Screen.height - inPos.y;
		return inPos;
	}

	public static Vector2 GetGUIPosition(this Touch aTouch)
	{
		Vector2 position = aTouch.position;
		position.y = (float)Screen.height - position.y;
		return position;
	}

	public static bool GetKeyDown(this Event aEvent, KeyCode aKey)
	{
		if (aEvent.type == EventType.KeyDown)
		{
			return aEvent.keyCode == aKey;
		}
		return false;
	}

	public static bool GetKeyUp(this Event aEvent, KeyCode aKey)
	{
		if (aEvent.type == EventType.KeyUp)
		{
			return aEvent.keyCode == aKey;
		}
		return false;
	}

	public static bool GetMouseDown(this Event aEvent, int aButton)
	{
		if (aEvent.type == EventType.MouseDown)
		{
			return aEvent.button == aButton;
		}
		return false;
	}

	public static bool GetMouseDown(this Event aEvent, int aButton, Rect aRect)
	{
		if (aEvent.type == EventType.MouseDown && aEvent.button == aButton)
		{
			return aRect.Contains(aEvent.mousePosition);
		}
		return false;
	}

	public static bool GetMouseUp(this Event aEvent, int aButton)
	{
		if (aEvent.type == EventType.MouseUp)
		{
			return aEvent.button == aButton;
		}
		return false;
	}

	public static bool GetMouseUp(this Event aEvent, int aButton, Rect aRect)
	{
		if (aEvent.type == EventType.MouseUp && aEvent.button == aButton)
		{
			return aRect.Contains(aEvent.mousePosition);
		}
		return false;
	}

	public static Rect Grow(this Rect r, float nbPixels)
	{
		return r.Shrink(0f - nbPixels, 0f - nbPixels);
	}

	public static Rect Grow(this Rect r, float nbPixelX, float nbPixelY)
	{
		return r.Shrink(0f - nbPixelX, 0f - nbPixelY);
	}

	public static Rect InverseTransform(this Rect r, Rect from)
	{
		return new Rect(r.x + from.x, r.y + from.y, r.width, r.height);
	}

	public static Vector2 InverseTransformPoint(Rect rect, Vector3 inPos)
	{
		return new Vector2(rect.x + inPos.x, rect.y + inPos.y);
	}

	public static Vector2 MouseRelativePos(Rect rect)
	{
		return GUIHelpers.RelativePos(rect, GUIHelpers.mousePos.x, GUIHelpers.mousePos.y);
	}

	public static Rect Move(this Rect r, Vector2 movement)
	{
		return r.Move(movement.x, movement.y);
	}

	public static Rect Move(this Rect r, float xMovement, float yMovement)
	{
		return new Rect(r.x + xMovement, r.y + yMovement, r.width, r.height);
	}

	public static Rect MoveX(this Rect r, float xMovement)
	{
		return r.Move(xMovement, 0f);
	}

	public static Rect MoveY(this Rect r, float yMovement)
	{
		return r.Move(0f, yMovement);
	}

	public static Vector2 RelativePos(Rect rect, Vector2 inPos)
	{
		return GUIHelpers.RelativePos(rect, inPos.x, inPos.y);
	}

	public static Vector2 RelativePos(Rect rect, Vector3 inPos)
	{
		return GUIHelpers.RelativePos(rect, inPos.x, inPos.y);
	}

	public static Vector2 RelativePos(Rect rect, float x, float y)
	{
		return new Vector2(x - rect.x, y - rect.y);
	}

	public static Rect RelativeTo(this Rect r, Rect to)
	{
		return new Rect(r.x - to.x, r.y - to.y, r.width, r.height);
	}

	public static Rect Shrink(this Rect r, float nbPixels)
	{
		return r.Shrink(nbPixels, nbPixels);
	}

	public static Rect Shrink(this Rect r, float nbPixelX, float nbPixelY)
	{
		return new Rect(r.x + nbPixelX, r.y + nbPixelY, r.width - nbPixelX * 2f, r.height - nbPixelY * 2f);
	}
}
