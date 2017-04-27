//
// /**************************************************************************
//
// Agent.cs
//
// Author: xiaohong  <704872627@qq.com>
//
// Unity课程讨论群:  152767675
//
// Date: 15-11-17
//
// Description:Provide  functions  to connect Oracle
//
// Copyright (c) 2015 xiaohong
//
// **************************************************************************/
using UnityEngine;
using System.Collections;

public class Agent : MonoBehaviour
{
	[System.NonSerialized]
	public Transform Transform;
	[System.NonSerialized]
	public GameObject GameObject;
	[System.NonSerialized]
	public CharacterController CharacterController;
	private Vector3 CollisionCenter;

	public AnimSet AnimSet;
	public BlackBoard BlackBoard = new BlackBoard();

	[System.NonSerialized]
	public WorldState WorldState;

	private GOAPManager GoapManager;
	private Hashtable m_Actions = new Hashtable();
	public GOAPAction GetAction(E_GOAPAction type) { return (GOAPAction)m_Actions[type]; }
	public int GetNumberOfActions() { return m_Actions.Count; }

	public GOAPGoal CurrentGOAPGoal { get { return GoapManager.CurrentGoal; } }
	
	public Vector3 Position { get { return Transform.position; } }

	public bool IsPlayer { get{ return BlackBoard.IsPlayer; } }
		
	public Transform t;



	void Awake()
	{
		Transform = transform;
		GameObject = gameObject;

		AnimSet = GetComponent<AnimSet>();

		WorldState = new WorldState();
		GoapManager = new GOAPManager (this);




		CharacterController = Transform.GetComponent<CharacterController>();
		CollisionCenter = CharacterController.center;

		BlackBoard.Owner = this;
		BlackBoard.myGameObject = GameObject;
		
		
		t = GameObject.Find ("GameObject").transform;
	}

	// Use this for initialization
	void Start () {
		CharacterController.detectCollisions = true;
		CharacterController.center = CollisionCenter;
		
		RaycastHit hit;
		if (Physics.Raycast(t.position + Vector3.up, -Vector3.up, out hit, 5, 1 << 10) == false)
			Transform.position = t.position;
		else
			Transform.position = hit.point;
		
		Transform.rotation = t.rotation;
	}

	//玩家角色更新Agent，非玩家角色设置GOAP目标FindCriticalGoal
	void LateUpdate()
	{
				if (IsPlayer) 
				{
						UpdateAgent ();
				}
				else 
				{
						GoapManager.FindCriticalGoal ();
				}
		//UpdateAgent();
	}
	
	//非玩家角色更新Agent
	void FixedUpdate()
	{
				if (IsPlayer)
						return;
				UpdateAgent ();
				WorldState.SetWSProperty (E_PropKey.E_IDLING, GoapManager.CurrentGoal == null);
	}
	
	void UpdateAgent()
	{
//		if (BlackBoard.DontUpdate == true)
//			return;
//		
		//update blackboard
		BlackBoard.Update();

		GoapManager.UpdateCurrentGoal ();
		GoapManager.ManageGoals ();
		
	}

		void ResetAgent()
		{
				WorldState.Reset ();
				GoapManager.Reset ();
		}

		public GOAPGoal AddGoal(E_GOAPGoals newGoal)
		{
				return GoapManager.AddGoal (newGoal);
		}

		public void InitializeGOAP()
		{
				GoapManager.Initialize ();
		}

		public void AddGOAPAction(E_GOAPAction action)
		{
				m_Actions.Add (action, GOAPActionFactory.Create (action, this));
		}



}

