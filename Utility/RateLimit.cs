using UnityEngine;

namespace Utility;

internal class RateLimit
{
	private int _currentUsage;

	private int _maxUsage;

	private float _resetDelay;

	private float _lastResetTime;

	public RateLimit(int maxUsage, float resetDelay)
	{
		this._currentUsage = 0;
		this._lastResetTime = Time.time;
		this._maxUsage = maxUsage;
		this._resetDelay = resetDelay;
	}

	public bool Check(int usage = 1)
	{
		this.TryReset();
		if (this._currentUsage + usage <= this._maxUsage)
		{
			this._currentUsage += usage;
			return true;
		}
		return false;
	}

	private void TryReset()
	{
		if (Time.time >= this._lastResetTime + this._resetDelay)
		{
			this._currentUsage = 0;
			this._lastResetTime = Time.time;
		}
	}
}
