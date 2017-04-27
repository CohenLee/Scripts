using System;

public class GOAPActionPlayAnim : GOAPAction
{
		private AgentActionPlayAnim Action;

		public GOAPActionPlayAnim(Agent owner) : base(E_GOAPAction.playAnim, owner)
		{
		}
	#region implemented abstract members of GOAPAction

	public override void InitAction ()
	{
				WorldEffects.SetWSProperty (E_PropKey.E_PLAY_ANIM, false);
				Cost = 1;
				Interruptible = false;
	}

	#endregion


		public override void Activate ()
	{
		base.Activate ();
				Action = AgentActionFactory.Create (AgentActionFactory.E_Type.E_Play_Anim) as AgentActionPlayAnim;
				Action.AnimName = Owner.BlackBoard.DesiredAnimation;
				Owner.BlackBoard.AddAction (Action);
	}

		public override void Deactivate ()
	{
				Owner.WorldState.SetWSProperty (E_PropKey.E_PLAY_ANIM, false);
		base.Deactivate ();
	}

		public override bool IsActionComplete ()
	{
				if (Action.IsActive () == false)
						return true;
				return false;
	}


}

