//
// /**************************************************************************
//
// AgentActionAttack.cs
//
// Author: xiaohong  <704872627@qq.com>
//
// Unity课程讨论群:  152767675
//
// Date: 15-12-18
//
// Description:角色攻击命令
//
// Copyright (c) 2015 xiaohong
//
// **************************************************************************/
using UnityEngine;
using System.Collections;



public class AgentActionAttack : AgentAction
{
	//攻击的目标对象
	public Agent Target;
	//攻击的类型，X，O。。。
	public E_AttackType AttackType = E_AttackType.None;
	//AnimData
	public AnimAttackData Data;
	//攻击方向
	public Vector3 AttackDir;
	public bool IsHit = false;
	//攻击是否完成
	public bool AttackPhaseDone = false;

	public AgentActionAttack() : base(AgentActionFactory.E_Type.E_Attack)
	{
	}

	public override void Reset ()
	{
		base.Reset ();
		Target = null;
		Data = null;
		IsHit = false;
		AttackPhaseDone = false;
		AttackType = E_AttackType.None;
	}

	public override string ToString ()
	{
		return "[AgentActionAttack]" + ((null != Target) ? Target.ToString() : "No Target.") + " " + AttackType.ToString() + " " + Status.ToString();
	}
}

