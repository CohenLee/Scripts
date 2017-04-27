//
// /**************************************************************************
//
// BlackBoard.cs
//
// Author: xiaohong  <704872627@qq.com>
//
// Unity课程讨论群:  152767675
//
// Date: 15-11-21
//
// Description:Provide  functions  to connect Oracle
//
// Copyright (c) 2015 xiaohong
//
// **************************************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IActionHandle
{
	void HandleAction(AgentAction _action);
}

[System.Serializable]
public class BlackBoard
{
	private List<AgentAction> _ActiveActions = new List<AgentAction>(); 
	private List<IActionHandle> _ActionHandles = new List<IActionHandle>();
	// 黑板拥有者
	public Agent Owner;
	[System.NonSerialized]
	public GameObject myGameObject;

		public bool IsPlayer = false;

	// 最大冲刺速度
	public float MaxSprintSpeed = 8;
	// 最大奔跑速度
	public float MaxRunSpeed = 4;
	// 最大行走速度
	public float MaxWalkSpeed = 1.5f;

	public float SpeedSmooth = 2.0f;
	public float RotationSmooth = 2.0f;
	public float MoveSpeedModifier = 1;

	/// <summary>
	/// The speed.
	/// 角色移动速度
	/// </summary>
	public float   Speed = 0;
	public Vector3 MoveDir = Vector3.zero;

	// 运动类型
	[System.NonSerialized]
	public E_MotionType MotionType = E_MotionType.None;
	[System.NonSerialized]
	public E_WeaponState WeaponState = E_WeaponState.NotInHands; // 武器状态
	[System.NonSerialized]
	public E_WeaponType WeaponSelected = E_WeaponType.Katana;  // 武器类型
	[System.NonSerialized]
	public E_WeaponType WeaponToSelect = E_WeaponType.None;

	public float WeaponRange = 3;

	/// <summary>
	/// The desired position.
	/// 目标位置
	/// </summary>
	[System.NonSerialized]
	public Vector3 DesiredPosition;
	/// <summary>
	/// The desired direction.
	/// 目标方向
	/// </summary>
	[System.NonSerialized]
	public Vector3 DesiredDirection;

		[System.NonSerialized]
		public E_AttackType DesiredAttackType;

		[System.NonSerialized]
		public Agent DesiredTarget;

		[System.NonSerialized]
		public AnimAttackData DesiredAttackPhase;


		[System.NonSerialized]
		public string DesiredAnimation;


		public float IdleTimer = 0.0f;



		///////////////// GOAP Settings /////////////////////////////
		public float GOAP_AlertRelevancy = 0.8f;
		public float GOAP_CalmRelevancy = 0.2f;
		public float GOAP_BlockRelevancy = 0.7f;
		public float GOAP_DodgeRelevancy = 0.9f;
		public float GOAP_GoToRelevancy = 0.5f;
		public float GOAP_CombatMoveBackwardRelevancy = 0.7f;
		public float GOAP_CombatMoveForwardRelevancy = 0.75f;
		public float GOAP_CombatMoveLeftRelevancy = 0.6f;
		public float GOAP_CombatMoveRightRelevancy = 0.6f;
		public float GOAP_LookAtTargetRelevancy = 0.7f;
		public float GOAP_KillTargetRelevancy = 0.8f;
		public float GOAP_PlayAnimRelevancy = 0.95f;
		public float GOAP_UseWorlObjectRelevancy = 0.9f;
		public float GOAP_ReactToDamageRelevancy = 1.0f;
		public float GOAP_IdleActionRelevancy = 0.4f;
		public float GOAP_TeleportRelevancy = 0.9f;

		public float GOAP_AlertDelay = 2.0f;
		public float GOAP_CalmDelay = 2.2f;
		public float GOAP_BlockDelay = 2.7f;
		public float GOAP_DodgeDelay = 5.0f;
		public float GOAP_GoToDelay = 0.5f;
		public float GOAP_CombatMoveDelay = 3.5f;
		public float GOAP_CombatMoveLeftDelay = 3.6f;
		public float GOAP_CombatMoveRightDelay = 3.6f;
		public float GOAP_LookAtTargetDelay = 0.4f;
		public float GOAP_KillTargetDelay = 2.8f;
		public float GOAP_PlayAnimDelay = 0.0f;
		public float GOAP_UseWorlObjectDelay = 5.0f;
		public float GOAP_CombatMoveBackwardDelay = 3.5f;
		public float GOAP_CombatMoveForwardDelay = 3.5f;
		public float GOAP_IdleActionDelay = 10;
		public float GOAP_TeleportDelay = 4;


	
//	[System.NonSerialized]
//	public bool DontUpdate = true;
//	

	public void AddAction(AgentAction _action)
	{
		Debug.Log(_action.ToString());
		_ActiveActions.Add(_action);

		// todo: call HandleAction...
		for (int i=0; i<_ActionHandles.Count; i++)
			_ActionHandles[i].HandleAction(_action);
	}

	public AgentAction GetAction(int index)
	{
		return _ActiveActions[index];
	}

	public int ActionCount
	{
		get { return _ActiveActions.Count; }
	}

	public void AddActionHandle(IActionHandle _handle)
	{
		for (int i=0; i<_ActionHandles.Count; i++)
			if (_ActionHandles[i] == _handle)
				return;
		_ActionHandles.Add(_handle);
	}

	public void RemoveActionHandle(IActionHandle _handle)
	{
		_ActionHandles.Remove(_handle);
	}

	public void Update()
	{
		for (int i=0; i<_ActiveActions.Count; i++)
		{
			if (_ActiveActions[i].IsActive())
				continue;
			// todo: Action Done...
			ActionDone(_ActiveActions[i]);
			_ActiveActions.RemoveAt(i);

			return;
		}
	}

	private void ActionDone(AgentAction _action)
	{
		// todo: ....

		AgentActionFactory.Return(_action);
	}

		public void AddOrder(AgentOrder order)
		{
				Owner.WorldState.SetWSProperty (E_PropKey.E_ORDER, order.Type);
				switch (order.Type) 
				{
				case AgentOrder.E_OrderType.E_GOTO:
						Owner.WorldState.SetWSProperty (E_PropKey.E_AT_TARGET_POS, false);
						DesiredDirection = order.Direction;
						DesiredPosition = order.Position;
						MoveSpeedModifier = order.MoveSpeedModifier;
						break;
				case AgentOrder.E_OrderType.E_STOPMOVE:
						Owner.WorldState.SetWSProperty (E_PropKey.E_AT_TARGET_POS, true);
						DesiredPosition = order.Position;
						break;
				case AgentOrder.E_OrderType.E_ATTACK:
						bool inRange = order.Target == null || (order.Target.Position - Owner.Position).magnitude <= WeaponRange;
						Owner.WorldState.SetWSProperty (E_PropKey.E_IN_WEAPONS_RANGE, inRange);
						DesiredTarget = order.Target;
						DesiredAttackType = order.AttackType;
						DesiredAttackPhase = order.AnimAttackData;
						break;
				default:
						break;
				}

				AgentOrderFactory.Return (order);
		}
}

