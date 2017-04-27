using System;
using UnityEngine;

public class GOAPActionMove : GOAPAction
{
		private AgentActionMove Action;

		public GOAPActionMove(Agent owner) : base(E_GOAPAction.move, owner)
		{
		}
		#region implemented abstract members of GOAPAction

		public override void InitAction ()
		{
				WorldEffects.SetWSProperty (E_PropKey.E_AT_TARGET_POS, true);
				Cost = 5;
				Precedence = 30;
		}

		#endregion

		public override void Update ()
	{
				if (WorldEffects.GetWSProperty (E_PropKey.E_AT_TARGET_POS).GetBool () == true) {
						Action = AgentActionFactory.Create (AgentActionFactory.E_Type.E_Move) as AgentActionMove;
						Action.MoveType = AgentActionMove.E_MoveType.E_MT_Forward;
						Owner.BlackBoard.AddAction (Action);
				} else {
						AgentActionIdle a = AgentActionFactory.Create (AgentActionFactory.E_Type.E_Idle) as AgentActionIdle;
						Owner.BlackBoard.AddAction (a);
				}
	}

		public override void Activate ()
	{
		base.Activate ();
				Action = AgentActionFactory.Create (AgentActionFactory.E_Type.E_Move) as AgentActionMove;
				Action.MoveType = AgentActionMove.E_MoveType.E_MT_Forward;
				Owner.BlackBoard.AddAction (Action);

	}

		public override void Deactivate ()
	{
		base.Deactivate ();
				Owner.WorldState.SetWSProperty (E_PropKey.E_AT_TARGET_POS, true);

				AgentActionIdle a = AgentActionFactory.Create (AgentActionFactory.E_Type.E_Idle) as AgentActionIdle;
				Owner.BlackBoard.AddAction (a);
	}

		public override bool IsActionComplete ()
	{
				if (Action != null && Action.IsActive () == false)
						return true;
				return false;
	}

		public override bool ValidateAction ()
	{
				if (Action != null && Action.IsFailed () == true)
						return false;
				return true;
	}


}

