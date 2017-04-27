﻿using System;

public class GOAPActionWeaponHide : GOAPAction
{
		private AgentActionWeaponShow Action;

		public GOAPActionWeaponHide(Agent owner) : base(E_GOAPAction.weaponHide, owner)
		{
		}
		#region implemented abstract members of GOAPAction

		public override void InitAction ()
		{
				WorldEffects.SetWSProperty (E_PropKey.E_WEAPON_IN_HANDS, false);
				Cost = 1;
				Interruptible = false;

		}

		#endregion

		public override void Activate ()
		{
				base.Activate ();
				Owner.BlackBoard.WeaponState = E_WeaponState.Ready;
				Action = AgentActionFactory.Create (AgentActionFactory.E_Type.E_Weapon_Show) as AgentActionWeaponShow;
				Action.Show = false;
				Owner.BlackBoard.AddAction (Action);
		}

		public override void Deactivate ()
		{
				base.Deactivate ();
				Owner.WorldState.SetWSProperty (E_PropKey.E_WEAPON_IN_HANDS, false);
		}

		public override bool IsActionComplete ()
		{
				if (Action.IsActive () == false)
						return true;
				return false;
		}
}
