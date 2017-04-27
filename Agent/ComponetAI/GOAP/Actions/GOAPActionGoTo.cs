using System;

public class GOAPActionGoTo : GOAPAction
{
		public GOAPActionGoTo(Agent owner) : base(E_GOAPAction.gotoPos, owner)
		{
		}

		#region implemented abstract members of GOAPAction

		public override void InitAction ()
		{

		}

		#endregion


}

