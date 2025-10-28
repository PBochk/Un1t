using UnityEngine;

//TODO: Decide if to use native events for reactive view update
public class EnemyModel
{
    private float hp;
    private float speedCoeff;

    public float Hp => hp;
    public float SpeedCoeff => speedCoeff;

    public EnemyModel(float hp, float speedCoeff)
    {
        this.hp = hp;
        this.speedCoeff = speedCoeff;
    }

    public void Damage(AttackData data)
    {
        //May be changed later
        hp -= data.Damage;
    }
    //To be implemented
    //public void Heal()
}