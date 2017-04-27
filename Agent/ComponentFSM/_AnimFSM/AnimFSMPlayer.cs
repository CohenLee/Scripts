//
// /**************************************************************************
//
// AnimFSMPlayer.cs
//
// Author: xiaohong  <704872627@qq.com>
//
// Unity课程讨论群:  152767675
//
// Date: 15-11-17
//
// Description:主角玩家状态机
//
// Copyright (c) 2015 xiaohong
//
// **************************************************************************/
using UnityEngine;
using System.Collections;

public class AnimFSMPlayer : AnimFSM
{
	/// <summary>
	/// 该状态机所有状态枚举
	/// </summary>
	enum E_AnimState
	{
		E_Idle,
		E_Move,
		E_Attack,
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="AnimFSMPlayer"/> class.
	/// </summary>
	/// <param name="_anims">_anims.</param>
	/// <param name="_owner">_owner.</param>
	public AnimFSMPlayer(Animation _anims, Agent _owner) : base(_anims, _owner)
	{

	}

	/// <summary>
	/// Initialize this instance.
	/// </summary>
	public override void Initialize ()
	{
		// todo: add Anim State
		// 添加角色状态到列表
		AnimStates.Add(new AnimStateIdle(AnimEngine, Owner));
		AnimStates.Add(new AnimStateMove(AnimEngine, Owner));
		AnimStates.Add(new AnimStateAttackMelee(AnimEngine, Owner));

		DefaultAnimState = AnimStates[(int)E_AnimState.E_Idle];
		base.Initialize ();
	}

	/// <summary>
	/// 子类实现该方法，每个角色在做一个操作时会传人操作对应的AgentAction.
	/// 该方法内执行状态切换和状态选择
	/// </summary>
	/// <param name="_action">_action.</param>
	public override void DoAction (AgentAction _action)
	{
		// 当前状态是否执行新的AgentAction，不执行就返回false，然后切换状态
		if (CurrentAnimState.HandleNewAction(_action))
		{
			NextAnimState = null;
		}
		else
		{
			// todo: switch AgentAction -> NextAnimState
			// 根据AgentAction选择下一个状态
//			if (_action is AgentActionIdle)
//				NextAnimState = AnimStates[(int)E_AnimState.E_Idle];
			if (_action is AgentActionMove)
				NextAnimState = AnimStates[(int)E_AnimState.E_Move];
			else if (_action is AgentActionAttack)
				NextAnimState = AnimStates[(int)E_AnimState.E_Attack];
//			else
//				Debug.Log("Not Find AnimState!!!");

			// 有下一个状态就切换到下一个状态
			if (null != NextAnimState)
			{
				ProgressToNextStage(_action);
			}
		}
	}
}

