//
// /**************************************************************************
//
// AnimStateAttackMelee.cs
//
// Author: xiaohong  <704872627@qq.com>
//
// Unity课程讨论群:  152767675
//
// Date: 15-12-18
//
// Description:角色攻击状态
//
// Copyright (c) 2015 xiaohong
//
// **************************************************************************/
using UnityEngine;
using System.Collections;

public class AnimStateAttackMelee : AnimState
{
	/// <summary>
	/// E state.
	/// 每次攻击的3个阶段
	/// </summary>
	enum E_State
	{
		/// <summary>
		/// The e preparing.
		/// 攻击准备
		/// </summary>
		E_Preparing,
		/// <summary>
		/// The e attacking.
		/// 攻击中
		/// </summary>
		E_Attacking,
		/// <summary>
		/// The e finished.
		/// 攻击完成
		/// </summary>
		E_Finished,
	}

	AgentActionAttack Action;
	//AnimData
	AnimAttackData AnimAttackData;
	//攻击动作时长
	float AttackPhaseTime;
	//击中时间
	float HitTime;
	//当前状态结束时间
	float EndOfStateTime;
	//暴击
	bool Critical = false; 
	//击倒
	bool Knockdown = false;

	#region Position and Rotation
	//攻击开始时的位置
	Vector3 StartPosition;
	//开始攻击时的位置
	Vector3 FinalPosition;
	//攻击开始时的旋转
	Quaternion StartRotation;
	//开始攻击时的旋转
	Quaternion FinalRotation;
	//如果攻击前需要移动，移动的时间
	float MoveTime;
	//当前移动的时间
	float CurrentMoveTime;
	//如果攻击前需要旋转，旋转的时间
	float RotationTime;
	//当前旋转的时间
	float CurrentRotationTime;
	//位置是否合适攻击
	bool PositionOK = false;
	//旋转是否适合攻击
	bool RotationOK = false;
	#endregion

	E_State State;

	/// <summary>
	/// Initializes a new instance of the <see cref="AnimStateAttackMelee"/> class.
	/// </summary>
	/// <param name="_anim">Animation.</param>
	/// <param name="_owner">Owner.</param>
	public AnimStateAttackMelee (Animation _anim, Agent _owner) : base (_anim, _owner)
	{
	}
				
	/// <summary>
	/// 停用状态
	/// </summary>
	public override void OnDeactivate ()
	{
		if (null != Action) {
			Action.SetSuccess ();
			Action = null;
		}
		base.OnDeactivate ();
	}

	/// <summary>
	/// 每帧更新状态.
	/// </summary>
	public override void Update ()
	{
				if (State == E_State.E_Preparing)
				{
						bool dontMove = false;
						if (RotationOK == false)
						{
								CurrentRotationTime += Time.deltaTime;
								if (CurrentRotationTime >= RotationTime)
								{
										CurrentRotationTime = RotationTime;
										RotationOK = true;
								}

								float progress = CurrentRotationTime / RotationTime;
								Quaternion q = Quaternion.Lerp(StartRotation, FinalRotation, progress);
								Owner.Transform.rotation = q;

								if (Quaternion.Angle(q, FinalRotation) > 20.0f)
										dontMove = true;
						}

						if (dontMove == false && PositionOK == false)
						{
								CurrentMoveTime += Time.deltaTime;
								if (CurrentMoveTime >= MoveTime)
								{
										CurrentMoveTime = MoveTime;
										PositionOK = true;
								}

								if (CurrentMoveTime > 0)
								{
										float progress = CurrentMoveTime / MoveTime;
										// 艾米插值
										Vector3 finalPos = Mathfx.Hermite(StartPosition, FinalPosition, progress);
										if (Move(finalPos - Transform.position) == false)
										{
												PositionOK = true;
										}
								}
						}

						if (RotationOK && PositionOK)
						{
								State = E_State.E_Attacking;
								//todo : play Animation
								PlayAnim();
						}
				}
				else if (State == E_State.E_Attacking)
				{
						CurrentMoveTime += Time.deltaTime;

						if (AttackPhaseTime < Time.timeSinceLevelLoad)
						{
								State = E_State.E_Finished;
						}

						if (CurrentMoveTime >= MoveTime)
								CurrentMoveTime = MoveTime;

						if (CurrentMoveTime > 0 && CurrentMoveTime <= MoveTime)
						{
								float progress = CurrentMoveTime / MoveTime;
								// 艾米插值
								Vector3 finalPos = Mathfx.Hermite(StartPosition, FinalPosition, progress);
								if (Move(finalPos - Transform.position) == false)
								{
										CurrentMoveTime = MoveTime;
								}
						}

						// Hit
						if (Action.IsHit == false && HitTime <= Time.timeSinceLevelLoad)
						{
								Action.IsHit = true;
								// todo : hit logic
						}
				}
				else if (State == E_State.E_Finished)
				{
						Action.AttackPhaseDone = true;
						Release();
				}
	}

	/// <summary>
	/// 执行一个新的AgentAction，是该状态执行就返回true，不执行该动作就返回false.
	/// </summary>
	/// <returns>true</returns>
	/// <c>false</c>
	/// <param name="_action">_action.</param>
	public override bool HandleNewAction (AgentAction _action)
	{
		if (_action as AgentActionAttack != null) {
			if (null != Action) {
				Action.SetSuccess ();
			}
			Initialize (_action);
			return true;
		}
		return false;
	}

	protected override void Initialize (AgentAction _action)
	{
		base.Initialize (_action);
		SetFinished (false);

		State = E_State.E_Preparing;

		Owner.BlackBoard.MotionType = E_MotionType.Attack;
		Action = _action as AgentActionAttack;
		Action.IsHit = false;

		//todo: AnimSet get first Attack Anim
		if (Action.Data == null)
			Action.Data = null;
		AnimAttackData = Action.Data;

		if (AnimAttackData == null)
			Debug.LogError("AnimAttackData is null !!!");

		StartPosition = Transform.position;
		StartRotation = Transform.rotation;

		float angle = 0;
		float distance = 0;
		if (null != Action.Target) {
			Vector3 dir = Action.Target.Position - Transform.position;
			distance = dir.magnitude;

			if (distance > 0.1f) {
				dir.Normalize ();
				angle = Vector3.Angle (Transform.forward, dir);
				// todo : add back logic

			} else {
				dir = Transform.forward;
			}

			FinalRotation.SetLookRotation (dir);

			if (distance < Owner.BlackBoard.WeaponRange) {
				FinalPosition = StartPosition;
			} else {
				FinalPosition = Action.Target.Transform.position - dir * Owner.BlackBoard.WeaponRange;
			}

			MoveTime = (FinalPosition - StartPosition).magnitude / 10.0f;
			RotationTime = angle / 720.0f;
		} else {
			FinalRotation.SetLookRotation (Action.AttackDir);
			RotationTime = Vector3.Angle (Transform.forward, Action.AttackDir) / 720.0f;
			MoveTime = 0;
		}

		PositionOK = MoveTime == 0;
		RotationOK = RotationTime == 0;

		CurrentMoveTime = 0;
		CurrentRotationTime = 0;
	}

		private void PlayAnim()
		{
				string animName = AnimAttackData.AinmName;

				CrossFade(animName, 0.2f);
				//击中时间
				HitTime = Time.timeSinceLevelLoad + AnimAttackData.HitTime;

				StartPosition = Transform.position;
				//攻击位移
				FinalPosition = StartPosition + Transform.forward* AnimAttackData.MoveDistance;

				// 攻击位移结束时间-攻击位移开始时间
				MoveTime = AnimAttackData.AttackMoveEndTime - AnimAttackData.AttackMoveStartTime;

				EndOfStateTime = Time.timeSinceLevelLoad + AnimEngine[animName].length * 0.9f;

				//攻击结束时间
				AttackPhaseTime = Time.timeSinceLevelLoad + AnimAttackData.AttackEndTime;

				//攻击动作前摇时间
				CurrentMoveTime = -AnimAttackData.AttackMoveStartTime;

		}
}

