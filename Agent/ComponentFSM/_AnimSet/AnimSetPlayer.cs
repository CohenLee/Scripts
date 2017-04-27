//
// /**************************************************************************
//
// AnimSetPlayer.cs
//
// Author: xiaohong  <704872627@qq.com>
//
// Unity课程讨论群:  152767675
//
// Date: 15-11-19
//
// Description:Provide  functions  to connect Oracle
//
// Copyright (c) 2015 xiaohong
//
// **************************************************************************/
using UnityEngine;
using System.Collections;

public class AnimSetPlayer : AnimSet
{
	#region implemented abstract members of AnimSet

	/// <summary>
	/// Gets the idle animation.
	/// 获取Idle动画名字，普通Idle，拿各种武器的Idle
	/// </summary>
	/// <returns>The idle animation name.</returns>
	public override string GetIdleAnim (E_WeaponType _weaponType, E_WeaponState _weaponState)
	{
		if (_weaponState == E_WeaponState.NotInHands)
			return "idle";
		
		return "idleSword";
	}

	/// <summary>
	/// Gets the move animation.
	/// 获取Move动画名字，普通移动， 拿各种武器的移动动画
	/// </summary>
	/// <returns>The move animation name.</returns>
	public override string GetMoveAnim (E_MotionType _motionType, E_MoveType _moveType, E_WeaponType _weaponType, E_WeaponState _weaponState)
	{
		if (E_WeaponState.NotInHands == _weaponState)
		{
			if (E_MotionType.Walk == _motionType)
				return "walk";
			return "run";
		}
		if (E_MotionType.Walk == _motionType)
			return "walkSword";
		return "runSword";
	}

	#endregion

	#region implemented abstract members of AnimSet

	/// <summary>
	/// Gets the show weapon animation.
	/// 获取拔除武器的动画名字
	/// </summary>
	/// <returns>The show weapon animation name.</returns>
	public override string GetShowWeaponAnim ()
	{
		return "showSwordRun";
	}

	/// <summary>
	/// Gets the hide weapon animation.
	/// 获取放回武器的动画名字
	/// </summary>
	/// <returns>The hide weapon animation name.</returns>
	public override string GetHideWeaponAnim ()
	{
		return "hidSwordRun";
	}

	#endregion
}
//
//public override string GetMoveAnim (E_MotionType motion, E_MoveType move, E_WeaponType weapon, E_WeaponState weaponState)
//{
//	if (weaponState == E_WeaponState.NotInHands)
//	{
//		if (motion != E_MotionType.Walk)
//			return "run";
//		else
//			return "walk";
//	}
//	
//	if (motion != E_MotionType.Walk)
//		return "runSword";
//	
//	return "walkSword";
//	//return "move";
//}