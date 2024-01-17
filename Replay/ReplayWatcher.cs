using UnityEngine;

namespace Replay;

internal class ReplayWatcher : MonoBehaviour
{
	public bool Playing;

	public float Speed;

	public float CurrentTime;

	public float MaxTime;

	private ReplayScript _script;

	private int _currentEvent;

	public void LoadScript(ReplayScript script)
	{
		this._script = script;
		this._currentEvent = 0;
		this.Playing = false;
		this.CurrentTime = script.Events[0].Time;
		this.MaxTime = script.Events[script.Events.Count - 1].Time;
		this.HandleEvent(script.Events[0]);
	}

	private void FixedUpdate()
	{
		if (!this.Playing)
		{
			return;
		}
		this.CurrentTime += Time.fixedDeltaTime * this.Speed;
		while (this._currentEvent < this._script.Events.Count - 1)
		{
			ReplayScriptEvent replayScriptEvent = this._script.Events[this._currentEvent + 1];
			if (this.CurrentTime < replayScriptEvent.Time)
			{
				break;
			}
			this._currentEvent++;
			this.HandleEvent(replayScriptEvent);
		}
		if (this.CurrentTime >= this.MaxTime)
		{
			this.CurrentTime = this.MaxTime;
			this.Playing = false;
		}
	}

	private void HandleEvent(ReplayScriptEvent currentEvent)
	{
		if (currentEvent.Category == ReplayEventCategory.Map.ToString())
		{
			this.HandleMapEvent(currentEvent);
		}
		else if (currentEvent.Category == ReplayEventCategory.Human.ToString())
		{
			this.HandleHumanEvent(currentEvent);
		}
		else if (currentEvent.Category == ReplayEventCategory.Titan.ToString())
		{
			this.HandleTitanEvent(currentEvent);
		}
		else if (currentEvent.Category == ReplayEventCategory.Camera.ToString())
		{
			this.HandleCameraEvent(currentEvent);
		}
		else if (currentEvent.Category == ReplayEventCategory.Chat.ToString())
		{
			this.HandleChatEvent(currentEvent);
		}
	}

	private void HandleMapEvent(ReplayScriptEvent currentEvent)
	{
		if (currentEvent.Action == ReplayEventMapAction.SetMap.ToString())
		{
			_ = currentEvent.Parameters[0];
		}
	}

	private void HandleHumanEvent(ReplayScriptEvent currentEvent)
	{
	}

	private void HandleTitanEvent(ReplayScriptEvent currentEvent)
	{
	}

	private void HandleCameraEvent(ReplayScriptEvent currentEvent)
	{
	}

	private void HandleChatEvent(ReplayScriptEvent currentEvent)
	{
	}
}
