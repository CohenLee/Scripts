//
// /**************************************************************************
//
// AgentActionIdle.cs
//
// Author: xiaohong  <704872627@qq.com>
//
// Unity课程讨论群:  152767675
//
// Date: 15-11-17
//
// Description:恢复Idle命令
//
// Copyright (c) 2015 xiaohong
//
// **************************************************************************/
using UnityEngine;
using System.Collections;

public class AgentActionIdle : AgentAction
{
	/// <summary>
	/// Initializes a new instance of the <see cref="AgentActionIdle"/> class.
	/// </summary>
	public AgentActionIdle() : base(AgentActionFactory.E_Type.E_Idle)
	{
	}
}

