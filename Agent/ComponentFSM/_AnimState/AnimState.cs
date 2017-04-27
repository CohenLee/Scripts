//
// /**************************************************************************
//
// AnimFSM.cs
//
// Author: xiaohong  <704872627@qq.com>
//
// Unity课程讨论群:  152767675
//
// Date: 15-11-17
//
// Description:状态机对应状态基类
//
// Copyright (c) 2015 xiaohong
//
// **************************************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimState : System.Object
{
	protected Animation AnimEngine;
	protected Agent Owner;

	/// <summary>
	/// 角色对象Transfrom
	/// </summary>
	protected Transform Transform;

	/// <summary>
	/// 角色骨骼根节点Transfrom
	/// </summary>
	protected Transform RootTransform;
	
	private bool m_IsFinished = true;

	/// <summary>
	/// Initializes a new instance of the <see cref="AnimState"/> class.
	/// </summary>
	/// <param name="_anims">_anims.</param>
	/// <param name="_owner">_owner.</param>
	public AnimState(Animation _anims, Agent _owner)
	{
		AnimEngine = _anims;
		Owner = _owner;
		Transform = Owner.transform;
		// 角色骨骼根节点名字root
		RootTransform = Transform.Find("root");
	}


	protected virtual void Initialize(AgentAction _action)
	{

	}

	/// <summary>
	/// 激活状态
	/// </summary>
	/// <param name="_action">_action.</param>
	public virtual void OnActivate(AgentAction _action)
	{
		
		SetFinished(false);
		
		Initialize(_action);
	}

	/// <summary>
	/// 停用状态
	/// </summary>
	public virtual void OnDeactivate()
	{

	}

	/// <summary>
	/// 结束释放和重置状态信息
	/// </summary>
	public virtual void Release ()
	{

	}

	/// <summary>
	/// 每帧更新状态.
	/// </summary>
	public virtual void Update()
	{

	}

	/// <summary>
	/// 执行一个新的AgentAction，是该状态执行就返回true，不执行该动作就返回false.
	/// </summary>
	/// <returns><c>true</c>, if new action was handled, <c>false</c> otherwise.</returns>
	/// <param name="_action">_action.</param>
	public virtual bool HandleNewAction(AgentAction _action)
	{
		return false;
	}

	/// <summary>
	/// Determines whether this instance is finished.
	/// </summary>
	/// <returns><c>true</c> if this instance is finished; otherwise, <c>false</c>.</returns>
	public virtual bool IsFinished()
	{
		return m_IsFinished;
	}

	/// <summary>
	/// Sets the finished.
	/// </summary>
	/// <param name="_finished">If set to <c>true</c> _finished.</param>
	public virtual void SetFinished(bool _finished)
	{
		m_IsFinished = _finished;
	}

	/// <summary>
	/// Crosses the fade.
	/// </summary>
	/// <param name="anim">Animation.</param>
	/// <param name="fadeInTime">Fade in time.</param>
	protected void CrossFade(string anim, float fadeInTime)
	{
		//if (Owner.debugAnims) Debug.Log(Time.timeSinceLevelLoad + " " + this.ToString() + " cross fade anim: " + anim + " in " + fadeInTime + "s.");
		
		if(AnimEngine.IsPlaying(anim))
			AnimEngine.CrossFadeQueued(anim, fadeInTime, QueueMode.PlayNow);
		else
			AnimEngine.CrossFade(anim, fadeInTime);
	}

	protected bool Move(Vector3 velocity)
	{
		Vector3 old = Transform.position;
		CollisionFlags flags = Owner.CharacterController.Move(velocity);
		return true;		
	}

//	
//	protected bool Move(Vector3 velocity, bool slide = true )
//	{
//		Vector3 old = Transform.position;
//		
//		Transform.position += Vector3.up * Time.deltaTime;
//		
//		velocity.y -= 9 * Time.deltaTime;
//		CollisionFlags flags = Owner.CharacterController.Move(velocity);
//		
//		//Debug.Log("move " + flags.ToString());
//		
//		if (slide == false && (flags & CollisionFlags.Sides) != 0)
//		{
//			Transform.position = old;
//			return false;
//		}
//		
//		if ((flags & CollisionFlags.Below) == 0)
//		{
//			Transform.position = old;
//			return false;
//		}
//		
//		return true;
//	}

}


