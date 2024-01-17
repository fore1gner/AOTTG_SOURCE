using System.Collections.Generic;
using Settings;
using UnityEngine;
using Utility;

namespace Weather;

internal class WeatherScheduleRunner
{
	private const int ScheduleMaxRecursion = 200;

	private int _currentScheduleLine;

	private LinkedList<int> _callerStack = new LinkedList<int>();

	private Dictionary<string, int> _scheduleLabels = new Dictionary<string, int>();

	private Dictionary<int, int> _repeatStartLines = new Dictionary<int, int>();

	private Dictionary<int, int> _repeatCurrentCounts = new Dictionary<int, int>();

	private WeatherManager _manager;

	public WeatherSchedule Schedule = new WeatherSchedule();

	public WeatherScheduleRunner(WeatherManager manager)
	{
		this._manager = manager;
	}

	public void ProcessSchedule()
	{
		for (int i = 0; i < this.Schedule.Events.Count - 1; i++)
		{
			if (this.Schedule.Events[i].Action == WeatherAction.RepeatNext)
			{
				this.Schedule.Events[i].Action = WeatherAction.BeginRepeat;
				WeatherEvent item = new WeatherEvent
				{
					Action = WeatherAction.EndRepeat
				};
				this.Schedule.Events.Insert(i + 2, item);
			}
		}
		int num = -1;
		for (int j = 0; j < this.Schedule.Events.Count; j++)
		{
			WeatherEvent weatherEvent = this.Schedule.Events[j];
			if (weatherEvent.Action == WeatherAction.Label)
			{
				string key = (string)weatherEvent.GetValue();
				if (!this._scheduleLabels.ContainsKey(key))
				{
					this._scheduleLabels.Add(key, j);
				}
			}
			else if (weatherEvent.Action == WeatherAction.BeginRepeat)
			{
				num = j;
				this._repeatCurrentCounts.Add(j, 0);
			}
			else if (weatherEvent.Action == WeatherAction.EndRepeat && num >= 0)
			{
				this._repeatStartLines.Add(j, num);
				num = -1;
			}
		}
	}

	public void ConsumeSchedule()
	{
		int num = 0;
		bool flag = false;
		while (!flag)
		{
			num++;
			if (num > 200)
			{
				Debug.Log("Weather schedule reached max usage (did you forget to use waits?)");
				this.Schedule.Events.Clear();
				break;
			}
			if (this.Schedule.Events.Count == 0 || this._currentScheduleLine < 0 || this._currentScheduleLine >= this.Schedule.Events.Count)
			{
				break;
			}
			WeatherEvent weatherEvent = this.Schedule.Events[this._currentScheduleLine];
			switch (weatherEvent.Action)
			{
			case WeatherAction.SetDefaultAll:
			{
				bool value = this._manager._currentWeather.ScheduleLoop.Value;
				this._manager._currentWeather.SetDefault();
				this._manager._currentWeather.UseSchedule.Value = true;
				this._manager._currentWeather.ScheduleLoop.Value = value;
				this._manager._needSync = true;
				break;
			}
			case WeatherAction.SetDefault:
				((BaseSetting)this._manager._currentWeather.Settings[weatherEvent.Effect.ToString()]).SetDefault();
				this._manager._needSync = true;
				break;
			case WeatherAction.SetValue:
				SettingsUtil.SetSettingValue((BaseSetting)this._manager._currentWeather.Settings[weatherEvent.Effect.ToString()], weatherEvent.GetSettingType(), weatherEvent.GetValue());
				this._manager._needSync = true;
				break;
			case WeatherAction.SetTargetDefaultAll:
				this._manager._targetWeather.SetDefault();
				this._manager._needSync = true;
				break;
			case WeatherAction.SetTargetDefault:
				((BaseSetting)this._manager._targetWeather.Settings[weatherEvent.Effect.ToString()]).SetDefault();
				this._manager._needSync = true;
				break;
			case WeatherAction.SetTargetValue:
				SettingsUtil.SetSettingValue((BaseSetting)this._manager._targetWeather.Settings[weatherEvent.Effect.ToString()], weatherEvent.GetSettingType(), weatherEvent.GetValue());
				this._manager._needSync = true;
				break;
			case WeatherAction.SetTargetTime:
			{
				if (!this._manager._targetWeatherStartTimes.ContainsKey((int)weatherEvent.Effect))
				{
					this._manager._targetWeatherStartTimes.Add((int)weatherEvent.Effect, 0f);
					this._manager._targetWeatherEndTimes.Add((int)weatherEvent.Effect, 0f);
				}
				BaseSetting obj = (BaseSetting)this._manager._startWeather.Settings[weatherEvent.Effect.ToString()];
				BaseSetting other = (BaseSetting)this._manager._currentWeather.Settings[weatherEvent.Effect.ToString()];
				this._manager._targetWeatherStartTimes[(int)weatherEvent.Effect] = this._manager._currentTime;
				this._manager._targetWeatherEndTimes[(int)weatherEvent.Effect] = this._manager._currentTime + (float)weatherEvent.GetValue();
				obj.Copy(other);
				this._manager._needSync = true;
				break;
			}
			case WeatherAction.SetTargetTimeAll:
			{
				this._manager._targetWeatherStartTimes.Clear();
				this._manager._targetWeatherEndTimes.Clear();
				float value2 = this._manager._currentTime + (float)weatherEvent.GetValue();
				foreach (WeatherEffect item in RCextensions.EnumToList<WeatherEffect>())
				{
					this._manager._targetWeatherStartTimes.Add((int)item, this._manager._currentTime);
					this._manager._targetWeatherEndTimes.Add((int)item, value2);
				}
				this._manager._needSync = true;
				break;
			}
			case WeatherAction.Wait:
				this._manager._currentScheduleWait[this] = (float)weatherEvent.GetValue();
				flag = true;
				break;
			case WeatherAction.Goto:
			{
				string text = (string)weatherEvent.GetValue();
				if (text != "NextLine" && this._scheduleLabels.ContainsKey(text))
				{
					this._callerStack.AddLast(this._currentScheduleLine);
					if (this._callerStack.Count > 200)
					{
						this._callerStack.RemoveFirst();
					}
					this._currentScheduleLine = this._scheduleLabels[text];
				}
				break;
			}
			case WeatherAction.Return:
				if (this._callerStack.Count > 0)
				{
					this._currentScheduleLine = this._callerStack.Last.Value;
					this._callerStack.RemoveLast();
				}
				break;
			case WeatherAction.BeginRepeat:
				this._repeatCurrentCounts[this._currentScheduleLine] = (int)weatherEvent.GetValue();
				break;
			case WeatherAction.EndRepeat:
			{
				int num2 = this._repeatStartLines[this._currentScheduleLine];
				if (this._repeatCurrentCounts.ContainsKey(num2) && this._repeatCurrentCounts[num2] > 0)
				{
					this._currentScheduleLine = num2 + 1;
					this._repeatCurrentCounts[num2]--;
				}
				break;
			}
			}
			this._currentScheduleLine++;
			if (this._currentScheduleLine >= this.Schedule.Events.Count && this._manager._currentWeather.ScheduleLoop.Value)
			{
				this._currentScheduleLine = 0;
			}
		}
	}
}
