//
// /**************************************************************************
//
// AgentActionFactory.cs
//
// Author: xiaohong  <704872627@qq.com>
//
// Unity课程讨论群:  152767675
//
// Date: 15-11-17
//
// Description: AgentAction对象缓存池
//              FSM 驱动命令AgentAction对象创建和回收接口
//
// Copyright (c) 2015 xiaohong
//
// **************************************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class AgentActionFactory
{
	/// <summary>
	/// AgentAction 类型枚举， 工厂根据该枚举类型创建AgentAction对象
	/// </summary>
	public enum E_Type
	{
		E_Idle,
		E_Move,
		E_Attack,
		E_Weapon_Show,
		E_Play_Anim,
		E_Count

	}
	// 缓存池队列
	static private Queue<AgentAction>[] _UnusedActions = new Queue<AgentAction>[(int)E_Type.E_Count];

#if DEBUG
	// 测试输出当前激活的命令
	static private List<AgentAction> _ActiveActions = new List<AgentAction>();
#endif

	static AgentActionFactory()
	{
		for(E_Type i=0; i<E_Type.E_Count; i++)
		{
			_UnusedActions[(int)i] = new Queue<AgentAction>();
		}
	}

	/// <summary>
	/// 使用E_Type创建AgentAction命令.
	/// </summary>
	/// <param name="_type">_type.</param>
	static public AgentAction Create(E_Type _type)
	{
		int index = (int)_type;
		AgentAction a;
		if (_UnusedActions[index].Count > 0)
		{
			a = _UnusedActions[index].Dequeue();
		}
		else
		{
			switch (_type)
			{
			case E_Type.E_Idle:
				a = new AgentActionIdle();
				break;
			case E_Type.E_Move:
				a = new AgentActionMove();
				break;
			case E_Type.E_Weapon_Show:
				a = new AgentActionWeaponShow();
				break;
			case E_Type.E_Attack:
				a = new AgentActionAttack();
				break;	
			case E_Type.E_Play_Anim:
				a = new AgentActionPlayAnim();
				break;	

			default:
				Debug.LogError("No AgentAction Create!!! Type: " + _type.ToString());
				return null;
			}
		}
		a.Reset();
		a.SetActive();
#if DEBUG
		_ActiveActions.Add(a);
#endif
		return a;
	}

	/// <summary>
	/// 将命令放回缓存池.
	/// </summary>
	/// <param name="_action">_action.</param>
	public static void Return(AgentAction _action)
	{
		_action.SetUnused();
		_UnusedActions[(int)_action.Type].Enqueue(_action);

#if DEBUG
		_ActiveActions.Remove(_action);
#endif

	}

#if DEBUG
	/// <summary>
	/// Debug输出命令列表.
	/// </summary>
	static public void Report()
	{
		Debug.Log("Action Factory _ActiveActions Count: " + _ActiveActions.Count);
		for (int i = 0; i < _ActiveActions.Count; i++)
			Debug.Log(_ActiveActions[i].Type);
	}
#endif
}

