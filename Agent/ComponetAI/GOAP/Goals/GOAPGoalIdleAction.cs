﻿using System;
using UnityEngine;

public class GOAPGoalIdleAction : GOAPGoal
{
		public GOAPGoalIdleAction(Agent owner) : base(E_GOAPGoals.E_IDLE_ANIM, owner)
		{
		}
		#region implemented abstract members of GOAPGoal

		public override void SetWSSatisfactionForPlanning (WorldState worldState)
		{
				worldState.SetWSProperty(E_PropKey.E_IDLING, false);
		}

		public override bool IsWSSatisfiedForPlanning (WorldState worldState)
		{
				WorldStateProp prop = worldState.GetWSProperty(E_PropKey.E_IDLING);
				if (prop.GetBool() == false)
						return true;
				return false;
		}

		public override float GetMaxRelevancy ()
		{
				return Owner.BlackBoard.GOAP_IdleActionRelevancy;
		}

		public override void CalculateGoalRelevancy ()
		{
				WorldStateProp prop1 = Owner.WorldState.GetWSProperty(E_PropKey.E_IDLING);
				WorldStateProp prop2 = Owner.WorldState.GetWSProperty(E_PropKey.E_WEAPON_IN_HANDS);
				if (prop1 != null && prop1.GetBool() == true && prop2.GetBool() == true && Owner.BlackBoard.IdleTimer > 5)
						GoalRelevancy = Owner.BlackBoard.GOAP_IdleActionRelevancy;
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
				NextEvaluationTime = Owner.BlackBoard.GOAP_IdleActionDelay + Time.timeSinceLevelLoad;
		}


}


