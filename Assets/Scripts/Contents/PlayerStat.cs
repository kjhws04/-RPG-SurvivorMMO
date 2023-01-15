using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : Stat
{
    public PlayerController _stat;

    [SerializeField]
    protected int _exp;
    [SerializeField]
    protected int _gold;

    [SerializeField]
    protected int _mp;
    [SerializeField]
    protected int _maxMp;

    //스킬 트리 스텟
    [SerializeField]
    protected int _skillTreePoint; //스킬트리 포인트
    [SerializeField]
    protected int _vitality; //생명력
    [SerializeField]
    protected int _mentality; //정신력
    [SerializeField]
    protected int _strength; //근력
    [SerializeField]
    protected int _intellect; //지능
    [SerializeField]
    protected int _ability; //기량

    [SerializeField]
    protected float _potionCoolTime; //포션 쿨타임
    [SerializeField]
    private ParticleSystem _levelUpPtc;

    public int Exp 
    {
        get { return _exp; } 
        set 
        {
            _exp = value;
            //레벨 업 체크

            int level = Level;
            while (true)
            {
                Data.Stat stat;
                if (Managers.Data.StatDict.TryGetValue(level + 1, out stat) == false)
                    break;
                if (_exp < stat.totalExp)
                    break;
                level++;
            }

            if (level != Level)
            {
                for (int i = Level; i < level; i++)
                    _skillTreePoint++;
                Level = level;
                SetStat(Level);
            }
        } 
    }

    public int Gold { get { return _gold; } set { _gold = value; } }

    public int Mp { get { return _mp; } set { _mp = value; } }
    public int MaxMp { get { return _maxMp; } set { _maxMp = value; } }

    public int SkillTreePoint { get { return _skillTreePoint; } set { _skillTreePoint = value; } }
    public int Vitality { get { return _vitality; } set { _vitality = value; } }
    public int Mentality { get { return _mentality; } set { _mentality = value; } }
    public int Strength { get { return _strength; } set { _strength = value; } }
    public int Intellect { get { return _intellect; } set { _intellect = value; } }
    public int Ability { get { return _ability; } set { _ability = value; } }

    public float PotionCoolTime { get { return _potionCoolTime; } set { _potionCoolTime = value; } }

    public bool _isDead = false;

    private void Start()
    {
        _stat = GetComponent<PlayerController>();

        _level = 1;
        _exp = 0;
        _defense = 5;
        _moveSpeed = 5.0f;
        _gold = 0;

        _mp = 100;
        _maxMp = 100;

        _skillTreePoint = 5;
        _vitality = 0;
        _mentality = 0;
        _strength = 0;
        _intellect = 0;
        _ability = 0;

        SetStat(_level);
    }

    private void Update()
    {
        if(_mp < _maxMp)
        {
            _mp +=1;
            Debug.Log((int)(Time.deltaTime * 100));
        }
    }

    public void SetStat(int level)
    {
        Dictionary<int, Data.Stat> dict = Managers.Data.StatDict;
        Data.Stat stat = dict[level];

        if (level > 1)
            _levelUpPtc.Play();

        _hp = stat.maxHp + (Vitality * 20);
        _maxHp = stat.maxHp + (Vitality * 20);
        _mp = stat.maxMp + (Mentality * 20);
        _maxMp = stat.maxMp + (Mentality * 20);

        if (_stat._equipment._item == null)
        {
            _attack = stat.attack + (Strength * 20);
            _mAttack = stat.mAttack + (Intellect * 20);
        }
        else if (_stat._equipment._item._itemADAPSPType == Define.WeaponType.AD)
        {
            _attack = stat.attack + (Strength * 20) + _stat._equipment._item._itemEffect;
            _mAttack = stat.mAttack + (Intellect * 20);
        }
        else if (_stat._equipment._item._itemADAPSPType == Define.WeaponType.AP)
        {
            _attack = stat.attack + (Strength * 20);
            _mAttack = stat.mAttack + (Intellect * 20) + _stat._equipment._item._itemEffect;
        }

        if (_stat._armor._item == null)
        {
            _defense = stat.defence;
            _mDefense = stat.mDefence;
        }
        else if (_stat._armor._item._itemADAPSPType == Define.WeaponType.AD)
        {
            _defense = stat.defence + _stat._armor._item._itemEffect;
            _mDefense = stat.mDefence;
        }
        else if(_stat._armor._item._itemADAPSPType == Define.WeaponType.AP)
        {
            _defense = stat.defence;
            _mDefense = stat.mDefence + _stat._armor._item._itemEffect;
        }
        //_moveSpeed = _moveSpeed + (Ability * 0.2f);
        //if (_stat._shose._item == null)
        //    _moveSpeed = _moveSpeed + (Ability * 0.2f);
        //else
        //{
        //    _moveSpeed = _moveSpeed + (Ability * 0.2f) + _stat._shose._item._itemEffect;
        //}
    }

    int WeaponStat()
    {
        if (_stat._equipment._item == null)
            return 0;
        else
            return _stat._equipment._item._itemEffect;
    }

    public void ResetStat(int level)
    {
        Dictionary<int, Data.Stat> dict = Managers.Data.StatDict;
        Data.Stat stat = dict[level];

        _maxHp = stat.maxHp + (Vitality * 20);
        _maxMp = stat.maxMp + (Mentality * 20);

        if (_stat._equipment._item == null)
        {
            _attack = stat.attack + (Strength * 20);
            _mAttack = stat.mAttack + (Intellect * 20);
        }
        else if (_stat._equipment._item._itemADAPSPType == Define.WeaponType.AD)
        {
            _attack = stat.attack + (Strength * 20) + _stat._equipment._item._itemEffect;
            _mAttack = stat.mAttack + (Intellect * 20);
        }
        else if (_stat._equipment._item._itemADAPSPType == Define.WeaponType.AP)
        {
            _attack = stat.attack + (Strength * 20);
            _mAttack = stat.mAttack + (Intellect * 20) + _stat._equipment._item._itemEffect;
        }

        if (_stat._shose._item == null)
            _moveSpeed = stat.moveSpeed + (_ability * 0.2f);
        else
            _moveSpeed = stat.moveSpeed + (_ability * 0.2f) + _stat._shose._item._itemEffect;
    }

    protected override void OnDead(Stat attacker)
    {
        if (!_isDead)
        {
            _stat.State = Define.State.Die;
            StartCoroutine(WaitTimeCoroutine());
        }
    }  

    IEnumerator WaitTimeCoroutine()
    {
        _isDead = true;
        yield return new WaitForSeconds(2.0f);
        Managers.UI.ShowPopupUI<UI_Died>();
    }

    public virtual void UseMana(int todo)
    {
        Mp -= todo;
    }
}
