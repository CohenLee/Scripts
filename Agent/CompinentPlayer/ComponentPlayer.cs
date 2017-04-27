//
// /**************************************************************************
//
// ComponentPlayer.cs
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

[RequireComponent(typeof(Agent))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animation))]
[RequireComponent(typeof(AnimSetPlayer))]
[RequireComponent(typeof(AnimComponent))]


public class ComponentPlayer : MonoBehaviour
{
	private Agent Agent;

	private Vector3 MoveDirection;
	
		public Transform Transform;

	// Use this for initialization
	void Start ()
	{
		Agent = GetComponent<Agent>();
		Transform = transform;

		Agent.BlackBoard.IsPlayer = true;
		
				Agent.AddGOAPAction(E_GOAPAction.gotoPos);
				Agent.AddGOAPAction(E_GOAPAction.move);
				Agent.AddGOAPAction(E_GOAPAction.playAnim);
				Agent.AddGOAPAction(E_GOAPAction.weaponHide);
				Agent.AddGOAPAction(E_GOAPAction.weaponShow);
				Agent.AddGOAPAction(E_GOAPAction.orderAttack);

				Agent.AddGoal(E_GOAPGoals.E_PLAY_ANIM);
				Agent.AddGoal(E_GOAPGoals.E_GOTO);
				//Agent.AddGoal(E_GOAPGoals.E_ORDER_ATTACK);

				Agent.InitializeGOAP();
	}
	
	// Update is called once per frame
	void Update ()
	{
		//input_begin
		float h = Input.GetAxisRaw("Horizontal");
		float v = Input.GetAxisRaw("Vertical");

		MoveDirection = new Vector3(h, 0, v);
		MoveDirection.Normalize();

		if (MoveDirection != Vector3.zero)
		{
			//CreateMove(MoveDirection);
						CreateOrderGoto(MoveDirection);
		}
		else
		{
			//CreateIdle();
						CreateOrderStop();
		}

		if (Input.GetMouseButtonDown(0))
		{
			//CreateWeaponShow();
			//CreateAttackX();
			CreateOrderAttack(E_AttackType.X);
		}
	}

		void CreateOrderGoto (Vector3 _moveDirection)
		{
				//		Agent.BlackBoard.DesiredDirection = _moveDirection;
				//		Agent.BlackBoard.DesiredPosition = Agent.Position;
				//		AgentAction _action = AgentActionFactory.Create(AgentActionFactory.E_Type.E_Move);
				//		Agent.BlackBoard.AddAction(_action);

				AgentOrder order = AgentOrderFactory.Create (AgentOrder.E_OrderType.E_GOTO);
				order.Direction = _moveDirection;
				order.MoveSpeedModifier = 1;
				order.Position = Agent.Position;

				Agent.BlackBoard.AddOrder (order);
		}

		void CreateOrderStop()
		{
				AgentOrder order = AgentOrderFactory.Create (AgentOrder.E_OrderType.E_STOPMOVE);
				Agent.BlackBoard.AddOrder (order);
		}

		void CreateOrderAttack(E_AttackType type)
		{
				AgentOrder order = AgentOrderFactory.Create (AgentOrder.E_OrderType.E_ATTACK);
				order.AttackType = type;
				//todo ... 移动方向或者正前方
				order.Direction = Transform.forward;
				//todo ... 攻击动画信息
				order.AnimAttackData = null;
				//todo ... 获取当前最佳攻击对象
				order.Target = null;
				//todo ... 根据条件添加
				Agent.BlackBoard.AddOrder (order);
		}

	void CreateIdle ()
	{
		Agent.BlackBoard.DesiredPosition = Agent.Position;
		AgentAction _action = AgentActionFactory.Create(AgentActionFactory.E_Type.E_Idle);
		Agent.BlackBoard.AddAction(_action);
	}

	void CreateMove (Vector3 _moveDirection)
	{
		Agent.BlackBoard.DesiredDirection = _moveDirection;
		Agent.BlackBoard.DesiredPosition = Agent.Position;
		AgentAction _action = AgentActionFactory.Create(AgentActionFactory.E_Type.E_Move);
		Agent.BlackBoard.AddAction(_action);
	}

	void CreateWeaponShow ()
	{
		AgentActionWeaponShow _action = AgentActionFactory.Create(AgentActionFactory.E_Type.E_Weapon_Show) as AgentActionWeaponShow;
		_action.Show = true;
		Agent.BlackBoard.AddAction(_action);
	}

	void CreateAttackX ()
	{
		AgentActionAttack _action = AgentActionFactory.Create(AgentActionFactory.E_Type.E_Attack) as AgentActionAttack;
		_action.AttackType = E_AttackType.X;
		if (MoveDirection != Vector3.zero)
			_action.AttackDir = MoveDirection;
		else
			_action.AttackDir = Agent.Transform.forward;
		_action.Target = null;

		_action.Data = new AnimAttackData("attackO", 0.5f, 0.1f, 1.0f, 111, 30);

		Agent.BlackBoard.AddAction(_action);
	}
}

