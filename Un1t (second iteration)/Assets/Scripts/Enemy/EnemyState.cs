using System;
using System.Collections;
using UnityEngine;

//TODO: Consider reanming (EnemyStrategy, perhaps?)
public abstract class EnemyState : MonoBehaviour
{
    public abstract void MakeDecision(IEnemyTarget target, EnemyModel model);
}