//
// /**************************************************************************
//
// AgentActionMove.cs
//
// Author: xiaohong  <704872627@qq.com>
//
// Unity课程讨论群:  152767675
//
// Date: 15-11-17
//
// Description:角色移动命令
//
// Copyright (c) 2015 xiaohong
//
// **************************************************************************/
using UnityEngine;
using System.Collections;

public class AgentActionMove : AgentAction
{
	/// <summary>
	/// 角色移动类型枚举.
	/// </summary>
	public enum E_MoveType
	{
		/// <summary>
		/// 向前移动.
		/// </summary>
		E_MT_Forward,
		/// <summary>
		/// 向后移动.
		/// </summary>
		E_MT_Backward,
		/// <summary>
		/// 向左移动.
		/// </summary>
		E_Strafe_Left,
		/// <summary>
		/// 向右移动.
		/// </summary>
		E_Strafe_Right,
	}
	
	/// <summary>
	/// 角色移动类型.
	/// </summary>
	public E_MoveType MoveType = E_MoveType.E_MT_Forward;

	/// <summary>
	/// Initializes a new instance of the <see cref="AgentActionMove"/> class.
	/// </summary>
	public AgentActionMove() : base(AgentActionFactory.E_Type.E_Move)
	{

	}  
}

