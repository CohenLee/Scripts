//
// /**************************************************************************
//
// AnimSet.cs
//
// Author: xiaohong  <704872627@qq.com>
//
// Unity课程讨论群:  152767675
//
// Date: 15-11-19
//
// Description: AnimSet提供获取各种动画名称接口，提供动画信息接口
//
// Copyright (c) 2015 xiaohong
//
// **************************************************************************/
using UnityEngine;
using System.Collections;

public abstract class AnimSet : MonoBehaviour
{
	/// <summary>
	/// Gets the idle animation.
	/// 获取Idle动画名字，普通Idle，拿各种武器的Idle
	/// </summary>
	/// <returns>The idle animation name.</returns>
	public abstract string GetIdleAnim(E_WeaponType _weaponType, E_WeaponState _weaponState);

	/// <summary>
	/// Gets the move animation.
	/// 获取Move动画名字，普通移动， 拿各种武器的移动动画
	/// </summary>
	/// <returns>The move animation name.</returns>
	public abstract string GetMoveAnim(E_MotionType _motionType, E_MoveType _moveType, E_WeaponType _weaponType, E_WeaponState _weaponState);

	/// <summary>
	/// Gets the show weapon animation.
	/// 获取拔除武器的动画名字
	/// </summary>
	/// <returns>The show weapon animation name.</returns>
	public abstract string GetShowWeaponAnim();

	/// <summary>
	/// Gets the hide weapon animation.
	/// 获取放回武器的动画名字
	/// </summary>
	/// <returns>The hide weapon animation name.</returns>
	public abstract string GetHideWeaponAnim();
}


public class AnimAttackData : System.Object
{
		//动画名字
		public string AinmName;

		//移动距离
		public float MoveDistance;

		//Timer
		public float AttackMoveStartTime;
		public float AttackMoveEndTime;
		public float AttackEndTime;


		//Hit 
		public float HitTime;
		public float HitDamage;
		public float HitAngle;

		public AnimAttackData(string animName, float moveDistance, float hitTime, float attackEndTime, float hitDamage, float hitAngle)
		{
				AinmName = animName;

				MoveDistance = moveDistance;
				AttackEndTime = attackEndTime;
				AttackMoveStartTime = 0;
				AttackMoveEndTime = AttackEndTime * 0.7f;

				HitTime = hitTime;
				HitDamage = hitDamage;
				HitAngle = hitAngle;
		}
}

