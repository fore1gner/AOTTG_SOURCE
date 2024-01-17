using System.Collections.Generic;

internal class RCEvent
{
	public enum foreachType
	{
		titan,
		player
	}

	public enum loopType
	{
		noLoop,
		ifLoop,
		foreachLoop,
		whileLoop
	}

	private RCCondition condition;

	private RCAction elseAction;

	private int eventClass;

	private int eventType;

	public string foreachVariableName;

	public List<RCAction> trueActions;

	public RCEvent(RCCondition sentCondition, List<RCAction> sentTrueActions, int sentClass, int sentType)
	{
		this.condition = sentCondition;
		this.trueActions = sentTrueActions;
		this.eventClass = sentClass;
		this.eventType = sentType;
	}

	public void checkEvent()
	{
		switch (this.eventClass)
		{
		default:
			return;
		case 0:
		{
			for (int j = 0; j < this.trueActions.Count; j++)
			{
				this.trueActions[j].doAction();
			}
			return;
		}
		case 1:
			if (!this.condition.checkCondition())
			{
				if (this.elseAction != null)
				{
					this.elseAction.doAction();
				}
			}
			else
			{
				for (int j = 0; j < this.trueActions.Count; j++)
				{
					this.trueActions[j].doAction();
				}
			}
			return;
		case 2:
			switch (this.eventType)
			{
			case 0:
			{
				foreach (TITAN titan in FengGameManagerMKII.instance.getTitans())
				{
					if (FengGameManagerMKII.titanVariables.ContainsKey(this.foreachVariableName))
					{
						FengGameManagerMKII.titanVariables[this.foreachVariableName] = titan;
					}
					else
					{
						FengGameManagerMKII.titanVariables.Add(this.foreachVariableName, titan);
					}
					foreach (RCAction trueAction in this.trueActions)
					{
						trueAction.doAction();
					}
				}
				break;
			}
			case 1:
			{
				PhotonPlayer[] playerList = PhotonNetwork.playerList;
				foreach (PhotonPlayer value in playerList)
				{
					if (FengGameManagerMKII.playerVariables.ContainsKey(this.foreachVariableName))
					{
						FengGameManagerMKII.playerVariables[this.foreachVariableName] = value;
					}
					else
					{
						FengGameManagerMKII.titanVariables.Add(this.foreachVariableName, value);
					}
					foreach (RCAction trueAction2 in this.trueActions)
					{
						trueAction2.doAction();
					}
				}
				break;
			}
			}
			return;
		case 3:
			break;
		}
		while (this.condition.checkCondition())
		{
			foreach (RCAction trueAction3 in this.trueActions)
			{
				trueAction3.doAction();
			}
		}
	}

	public void setElse(RCAction sentElse)
	{
		this.elseAction = sentElse;
	}
}
