using System;


public class AgentActionPlayAnim : AgentAction
{
		public string AnimName;
		/// <summary>
		/// Initializes a new instance of the <see cref="AgentActionIdle"/> class.
		/// </summary>
		public AgentActionPlayAnim() : base(AgentActionFactory.E_Type.E_Play_Anim)
		{
		}
}
