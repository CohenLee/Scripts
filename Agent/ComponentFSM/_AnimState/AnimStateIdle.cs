//
// /**************************************************************************
//
// AnimStateIdle.cs
//
// Author: xiaohong  <704872627@qq.com>
//
// Unity课程讨论群:  152767675
//
// Date: 15-11-17
//
// Description:角色Idle状态，该状态处理角色武器显示和隐藏
//             接受 AgentActonWeaponShow 动作
//
// Copyright (c) 2015 xiaohong
//
// **************************************************************************/
using UnityEngine;
using System.Collections;

public class AnimStateIdle : AnimState
{
	float TimeToFinishWeaponAction;
	AgentAction WeaponAction;

	/// <summary>
	/// Initializes a new instance of the <see cref="AnimStateIdle"/> class.
	/// </summary>
	/// <param name="_anims">_anims.</param>
	/// <param name="_owner">_owner.</param>
	public AnimStateIdle(Animation _anims, Agent _owner) : base(_anims, _owner)
	{
	}

	/// <summary>
	/// 结束释放和重置状态信息
	/// </summary>
	public override void Release ()
	{
		SetFinished(true);
	}

	/// <summary>
	/// 执行一个新的AgentAction，是该状态执行就返回true，不执行该动作就返回false.
	/// </summary>
	/// <returns>true</returns>
	/// <c>false</c>
	/// <param name="_action">_action.</param>
	public override bool HandleNewAction (AgentAction _action)
	{
		// 该状态处理AgentActonWeaponShow
		if (_action is AgentActionWeaponShow)
		{
			string animName;
			float delyTime;
			float fadeInTime;
			if ((_action as AgentActionWeaponShow).Show == true)
			{
				//todo: show Weapon Animation
				// 播放显示武器动画
				animName = Owner.AnimSet.GetShowWeaponAnim();
				delyTime = 0.8f;
				fadeInTime = 0.2f;
			}
			else
			{
				//todo: hide Weapon Animation
				// 播放隐藏武器动画
				animName = Owner.AnimSet.GetHideWeaponAnim();
				delyTime = 0.9f;
				fadeInTime = 0.1f;
			}
			TimeToFinishWeaponAction = Time.realtimeSinceStartup + AnimEngine[animName].length * delyTime;
			CrossFade(animName, fadeInTime);
			WeaponAction = _action;
			return true;
		}
		return false;
	}

	/// <summary>
	/// 每帧更新状态.
	/// </summary>
	public override void Update ()
	{
		// 当WeaponAction不为空时，隐藏和显示武器完成后播放Idle动画
		if (WeaponAction != null && TimeToFinishWeaponAction < Time.timeSinceLevelLoad)
		{
			WeaponAction.SetSuccess();
			WeaponAction = null;
			PlayIdleAnim();
		}
	}

	/// <summary>
	/// 播放Idle动画.
	/// </summary>
	void PlayIdleAnim ()
	{
		//todo: play Idle Animation
		string name = Owner.AnimSet.GetIdleAnim(Owner.BlackBoard.WeaponSelected, Owner.BlackBoard.WeaponState);
		CrossFade(name, 0.2f);
	}

	protected override void Initialize (AgentAction _action)
	{
		base.Initialize (_action);

		// 设置角色数据，运动状态，移动方向，移动速度
		Owner.BlackBoard.MotionType = E_MotionType.None;
		Owner.BlackBoard.MoveDir = Vector3.zero;
		Owner.BlackBoard.Speed = 0;
		
		if (WeaponAction == null)
			PlayIdleAnim();
	}
}

