//
// /**************************************************************************
//
// GOAPGoalOrderAttack.cs
//
// Author: xiaohong  <704872627@qq.com>
//
// Unity课程讨论群:  152767675
//
// Date: 2016/3/20
//
// Description:Provide  functions  to connect Oracle
//
// Copyright (c) 2016 xiaohong
//
// **************************************************************************/
using UnityEngine;
using System.Collections;

public class GOAPGoalOrderAttack : GOAPGoal
{
		public GOAPGoalOrderAttack(Agent owner) : base(E_GOAPGoals.E_ORDER_ATTACK, owner)
		{
		}
	#region implemented abstract members of GOAPGoal

	public override void SetWSSatisfactionForPlanning (WorldState worldState)
	{
				worldState.SetWSProperty(E_PropKey.E_ATTACK_TARGET, true);
	}

	public override bool IsWSSatisfiedForPlanning (WorldState worldState)
	{
				WorldStateProp prop = worldState.GetWSProperty(E_PropKey.E_ATTACK_TARGET);
				if (prop != null && prop.GetBool() == true)
						return true;
				return false;
	}

	public override float GetMaxRelevancy ()
	{
				return Owner.BlackBoard.GOAP_KillTargetRelevancy;
	}

	public override void CalculateGoalRelevancy ()
	{
				WorldStateProp prop = Owner.WorldState.GetWSProperty(E_PropKey.E_ORDER);
				if (prop != null && prop.GetOrder() == AgentOrder.E_OrderType.E_ATTACK)
						GoalRelevancy = Owner.BlackBoard.GOAP_KillTargetRelevancy;
				else
						GoalRelevancy = 0;
	}

	public override bool IsSatisfied ()
	{
				return IsPlanFinished();
	}

	public override void InitGoal ()
	{
	}

	#endregion

		public override void SetDisableTime ()
	{
				NextEvaluationTime = Owner.BlackBoard.GOAP_KillTargetDelay + Time.timeSinceLevelLoad;
	}

		public override bool ReplanRequired ()
		{
				if (IsPlanFinished() && Owner.WorldState.GetWSProperty(E_PropKey.E_ORDER).GetOrder() == AgentOrder.E_OrderType.E_ATTACK)
						return true;
				return false;
		}

		public override void Deactivate ()
		{
				WorldStateProp prop = Owner.WorldState.GetWSProperty(E_PropKey.E_ORDER);
				if (prop.GetOrder() == AgentOrder.E_OrderType.E_ATTACK)
						Owner.WorldState.SetWSProperty(E_PropKey.E_ORDER, AgentOrder.E_OrderType.E_NONE);
				base.Deactivate ();
		}

}

