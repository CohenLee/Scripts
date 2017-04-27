using System;
using UnityEngine;

public class GOAPGoalPlayAnim : GOAPGoal
{
		public GOAPGoalPlayAnim(Agent owner) : base(E_GOAPGoals.E_PLAY_ANIM, owner)
		{
		}
		#region implemented abstract members of GOAPGoal

		public override void SetWSSatisfactionForPlanning (WorldState worldState)
		{
				worldState.SetWSProperty(E_PropKey.E_PLAY_ANIM, false);
		}

		public override bool IsWSSatisfiedForPlanning (WorldState worldState)
		{
				WorldStateProp prop = worldState.GetWSProperty(E_PropKey.E_PLAY_ANIM);
				if (prop != null && prop.GetBool() == false)
						return true;
				return false;
		}

		public override float GetMaxRelevancy ()
		{
				return Owner.BlackBoard.GOAP_PlayAnimRelevancy;
		}

		public override void CalculateGoalRelevancy ()
		{
				WorldStateProp prop = Owner.WorldState.GetWSProperty(E_PropKey.E_PLAY_ANIM);
				if (prop != null && prop.GetBool() == true)
				{
						GoalRelevancy = Owner.BlackBoard.GOAP_PlayAnimRelevancy;
				}
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
				NextEvaluationTime = Owner.BlackBoard.GOAP_PlayAnimDelay + Time.timeSinceLevelLoad;
		}


}
