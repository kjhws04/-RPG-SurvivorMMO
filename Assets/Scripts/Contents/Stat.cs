using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat : MonoBehaviour
{
    [SerializeField]
    protected int _level;
    [SerializeField]
    protected int _hp;
    [SerializeField]
    protected int _maxHp;
    [SerializeField]
    protected int _attack;
    [SerializeField]
    protected int _mAttack;
    [SerializeField]
    protected int _defense;
    [SerializeField]
    protected int _mDefense;
    [SerializeField]
    protected float _moveSpeed;
    [Header("Drop Info")]
    [SerializeField]
    protected int _dropExp;
    [SerializeField]
    protected int _dropGoldMin;
    [SerializeField]
    protected int _dropGoldMax;
    [SerializeField]
    protected Define.WeaponType _attackType = Define.WeaponType.Null;
    [SerializeField]
    protected Define.EntityType _type;

    public int Level { get { return _level; } set { _level = value; } }
    public int Hp { get { return _hp; } set { _hp = value; } }
    public int MaxHp { get { return _maxHp; } set { _maxHp = value; } }
    public int Attack { get { return _attack; } set { _attack = value; } }
    public int MAttack { get { return _mAttack; } set { _mAttack = value; } }
    public int Defense { get { return _defense; } set { _defense = value; } }
    public int MDefense { get { return _mDefense; } set { _mDefense = value; } }
    public float MoveSpeed { get { return _moveSpeed; } set { _moveSpeed = value; } }
    public int DropExp { get { return _dropExp; } set { _dropExp = value; } }
    public Define.WeaponType AttackType { get { return _attackType; } set { _attackType = value; } }

    public virtual void OnAttacked(Stat attacker) //플레이어 몬스터 공통 공격 공식
    {
        switch (attacker._attackType)
        {
            case Define.WeaponType.Null:
                int damageNull = Mathf.Max(0, attacker.Attack / 2 - Defense);
                Hp -= damageNull;
                if (Hp <= 0)
                {
                    Hp = 0;
                    OnDead(attacker);
                }
                break;
            case Define.WeaponType.AD:
                int damageAD = Mathf.Max(0, attacker.Attack - Defense);
                Hp -= damageAD;
                if (Hp <= 0)
                {
                    Hp = 0;
                    OnDead(attacker);
                }
                break;
            case Define.WeaponType.AP:
                int damageAP = Mathf.Max(0, attacker.MAttack - MDefense);
                Hp -= damageAP;
                if (Hp <= 0)
                {
                    Hp = 0;
                    OnDead(attacker);
                }
                break;
        }
    }

    protected virtual void OnDead(Stat attacker)
    {
        PlayerStat playerStat = attacker as PlayerStat;
        switch (_type)
        {
            case Define.EntityType.CommonMonster:
                if (playerStat != null)
                {
                    playerStat.Exp += _dropExp;
                    playerStat.Gold += Random.Range(_dropGoldMin, _dropGoldMax);
                }
                //일반 몬스터는 죽으면 풀링에 의해 디스폰됨, 죽음 에니메이션(코루틴)  //TODO
                Managers.Game.Despawn(gameObject);
                break;
            case Define.EntityType.Boss:
                BossController boss = GetComponent<BossController>();
                if (playerStat != null)
                {
                    playerStat.Exp += _dropExp;
                    playerStat.Gold += Random.Range(_dropGoldMin, _dropGoldMax);
                }
                boss.State = Define.State.Die;
                break;
        }

    }
}
//    protected override void OnDead(Stat attacker)
//    {
//        _stat.State = Define.State.Die;
//        Debug.Log("Player Dead");
//    }
//}
