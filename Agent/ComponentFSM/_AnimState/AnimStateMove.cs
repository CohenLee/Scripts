//
// /**************************************************************************
//
// AnimStateMove.cs
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

public class AnimStateMove : AnimState
{
	AgentActionMove Action;
	float MaxSpeed;

	Quaternion StartRotation = new Quaternion();
	Quaternion FinalRotation = new Quaternion();
	float RotationProgress;

	public AnimStateMove(Animation _anims, Agent _owner) : base(_anims, _owner)
	{
	}

	public override void OnActivate (AgentAction _action)
	{
		base.OnActivate (_action);
		//todo: play Animation...
		PlayAnim(GetMotionType());
	}

	public override void OnDeactivate ()
	{
		if (null != Action)
		{
			Action.SetSuccess();
			Action = null;
		}
		Owner.BlackBoard.Speed = 0;
		base.OnDeactivate ();
	}

	public override void Update ()
	{
		if (Action.IsActive() == false)
		{
			Release();
			return;
		}

		//角色旋转
		RotationProgress += Owner.BlackBoard.RotationSmooth * Time.deltaTime;
		RotationProgress = Mathf.Min(RotationProgress, 1);
		Quaternion q = Quaternion.Slerp(StartRotation, FinalRotation, RotationProgress);
		Owner.Transform.rotation = q;
		//角色速度
		MaxSpeed = Mathf.Max(Owner.BlackBoard.MaxWalkSpeed, Owner.BlackBoard.MaxRunSpeed * Owner.BlackBoard.MoveSpeedModifier);
		float curSmooth = Owner.BlackBoard.SpeedSmooth * Time.deltaTime;

		Owner.BlackBoard.Speed = Mathf.Lerp(Owner.BlackBoard.Speed, MaxSpeed, curSmooth);
		Owner.BlackBoard.MoveDir = Owner.BlackBoard.DesiredDirection;
		//Move
		if (Move(Owner.BlackBoard.MoveDir * Owner.BlackBoard.Speed * Time.deltaTime) == false)
			Release();
		//Play Animation
		E_MotionType motion = GetMotionType();
		if (Owner.BlackBoard.MotionType != motion)
			PlayAnim(motion);
	}

	public override bool HandleNewAction (AgentAction _action)
	{
		if (_action is AgentActionMove)
		{
			if (null != Action)
				Action.SetSuccess();
			SetFinished(false);

			Initialize(_action);
			return true;
		}

		if (_action is AgentActionWeaponShow)
		{
			_action.SetSuccess();
			//todo: play Animation
			PlayAnim(GetMotionType());
			return true;
		}

		if (_action is AgentActionIdle)
		{
			_action.SetSuccess();
			SetFinished(true);
		}
		return false;
	}

	protected override void Initialize (AgentAction _action)
	{
		base.Initialize (_action);

		Action = _action as AgentActionMove;

		StartRotation = Owner.Transform.rotation;
		FinalRotation.SetLookRotation(Owner.BlackBoard.DesiredDirection);

		Owner.BlackBoard.MotionType = GetMotionType();

		RotationProgress = 0;
	}

	/// <summary>
	/// Play the animation.
	/// 根据武器类型，武器状态，移动类型播放动画
	/// </summary>
	/// <param name="_motionType">_motion type.</param>
	private void PlayAnim(E_MotionType _motionType)
	{
		Owner.BlackBoard.MotionType = _motionType;
		CrossFade(Owner.AnimSet.GetMoveAnim(Owner.BlackBoard.MotionType, E_MoveType.Forward, Owner.BlackBoard.WeaponSelected, Owner.BlackBoard.WeaponState), 0.2f);
	}

	/// <summary>
	/// Gets the type of the motion.
	/// 获取移动类型
	/// </summary>
	/// <returns>The motion type.</returns>
	private E_MotionType GetMotionType()
	{
		if (Owner.BlackBoard.Speed > Owner.BlackBoard.MaxRunSpeed * 1.5f)
			return E_MotionType.Sprint;
		else if (Owner.BlackBoard.Speed > Owner.BlackBoard.MaxWalkSpeed * 1.5f)
			return E_MotionType.Run;
		return E_MotionType.Walk;
	}
}







