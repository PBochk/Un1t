using System;
using UnityEngine;

[RequireComponent(typeof(SlimeFollowState),
   typeof(DeadState),
   typeof(IdleState))]
[RequireComponent(typeof(BlueSlimeView),
   typeof(SlimeMeleeAttackState))]
public class BlueSlimeController : EnemyController
{
   private SlimeFollowState followState;
   private SlimeMeleeAttackState meleeState;
   protected override void Awake()
   {
      base.Awake();
      
      //TODO: Make methods in base class for initialization steps
      //this includes: State binding, entry state
      
      followState = GetComponent<SlimeFollowState>();
      meleeState = GetComponent<SlimeMeleeAttackState>();
      view = GetComponent<BlueSlimeView>();
      
      idleState.OnStateExit.AddListener((b) => ChangeState(followState));
      followState.OnStateExit.AddListener(FollowExit);
   }

   private void FollowExit(bool inRange)
   {
      if (!inRange)
      {
         followState.EnterState(target,  model);
      }
      ChangeState(meleeState);
   }
}