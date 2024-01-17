using System;

namespace ExitGames.Client.DemoParticle;

public class TimeKeeper
{
	private int lastExecutionTime = Environment.TickCount;

	private bool shouldExecute;

	public int Interval { get; set; }

	public bool IsEnabled { get; set; }

	public bool ShouldExecute
	{
		get
		{
			if (this.IsEnabled)
			{
				if (!this.shouldExecute)
				{
					return Environment.TickCount - this.lastExecutionTime > this.Interval;
				}
				return true;
			}
			return false;
		}
		set
		{
			this.shouldExecute = value;
		}
	}

	public TimeKeeper(int interval)
	{
		this.IsEnabled = true;
		this.Interval = interval;
	}

	public void Reset()
	{
		this.shouldExecute = false;
		this.lastExecutionTime = Environment.TickCount;
	}
}
