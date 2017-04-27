using System;


public class GOAPActionWeaponShow : GOAPAction
{
		private AgentActionWeaponShow Action;

		public GOAPActionWeaponShow(Agent owner) : base(E_GOAPAction.weaponShow, owner)
		{
		}
		#region implemented abstract members of GOAPAction

		public override void InitAction ()
		{
				WorldEffects.SetWSProperty (E_PropKey.E_WEAPON_IN_HANDS, true);
				Cost = 1;
				Interruptible = false;

		}

		#endregion

		public override void Activate ()
	{
		base.Activate ();
				Owner.BlackBoard.WeaponState = E_WeaponState.Ready;
				Action = AgentActionFactory.Create (AgentActionFactory.E_Type.E_Weapon_Show) as AgentActionWeaponShow;
				Action.Show = true;
				Owner.BlackBoard.AddAction (Action);
	}

		public override void Deactivate ()
	{
		base.Deactivate ();
				Owner.WorldState.SetWSProperty (E_PropKey.E_WEAPON_IN_HANDS, true);
	}

		public override bool IsActionComplete ()
	{
				if (Action.IsActive () == false)
						return true;
				return false;
	}
}
