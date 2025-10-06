using System;
using UnityEngine;

[RequireComponent(typeof(SlimeFollowState),
   typeof(DeadState),
   typeof(IdleState))]
[RequireComponent(typeof(BlueSlimeView),
   typeof(SlimeMeleeAttackState))]
public class BlueSlimeController : EnemyController
{
   private DeadState deadState;
   private IdleState idleState;
   private SlimeFollowState followState;
   private SlimeMeleeAttackState meleeState;
   protected override void Awake()
   {
      base.Awake();
      deadState = GetComponent<DeadState>();
      idleState = GetComponent<IdleState>();
      followState = GetComponent<SlimeFollowState>();
      meleeState = GetComponent<SlimeMeleeAttackState>();
      view = GetComponent<BlueSlimeView>();
      
      currentState = followState;
   }
}