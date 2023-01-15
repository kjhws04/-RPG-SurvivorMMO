using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum BossState
{
    Null,
    MissileShot,
    Punch,
    SpawnGhoul,
    FastRunning
}

public class BossController : BaseController
{
    Stat _stat;
    [SerializeField]
    float _scanRange = 0;
    [SerializeField]
    float _attackRange = 0;
    [SerializeField]
    float _minAttackRange = 0;
    [SerializeField]
    float _farAttackRange = 0;

    #region Boss Info
    [Header("Boss Info")]
    public string _name;
    public string _aka;
    public BossState _bossState = BossState.Null;
    public bool _near = false;
    public bool _lookAt = true;
    [Header("Boss Patton Info")]
    public bool _pattonCoolTime = false;
    public GameObject _missile;
    public Transform _missilePort;
    public GameObject _fire;
    public Transform _firePort;
    public GameObject _spawnMonster;
    public Transform _spawn1;
    public Transform _spawn2;
    public ParticleSystem _ptcSpawn1;
    public ParticleSystem _ptcSpawn2;
    public Transform _punchRange;
    public GameObject _punchCol;
    public int _punchDam;
    public bool _inPunchRange = false;
    public ParticleSystem _punchDust;
    #endregion

    public override void Init()
    {
        _stat = gameObject.GetComponent<Stat>();
        UI_BossHp _bossHp = Managers.UI.ShowPopupUI<UI_BossHp>();
        _bossHp.keep(_stat, this);
    }

    protected override void UpdateIdle()
    {
        if (_stat.MaxHp / 3 <= _stat.Hp)
        {
            //���� ����ȭ
        }
        
        GameObject player = Managers.Game.GetPlayer();
        if (player == null)
            return;
        
        float distance = (player.transform.position - transform.position).magnitude;
        if (distance <= _scanRange)
        {
            _lockTarget = player;
            State = Define.State.Moving;
            return;
        }
    }

    protected override void UpdateMoving()
    {
        // �÷��̾ �� �����Ÿ����� ������ ����
        if (_lockTarget != null) //�� Ÿ���� �ΰ��� �ƴ϶��
        {
            if (!_pattonCoolTime)
            {
                _destPos = _lockTarget.transform.position;  //_destPos �� �� Ÿ���� ������
                float distance = (_destPos - transform.position).magnitude; //distance(�Ÿ�)�� �� Ÿ�ϰ��� �ִ� �Ÿ� ��
                if (distance <= _attackRange) //Ÿ���� �ִ� �Ÿ� ���� ���� �������� ª�ٸ�
                {
                    NavMeshAgent nma = gameObject.GetOrAddComponent<NavMeshAgent>(); //AI�� ������ �ٿ��ְ� �ִٸ� ������
                    nma.SetDestination(transform.position); //�� Ÿ���� ���� ��ġ�� ����
                    _near = true; //����� Ʈ��
                    StartCoroutine(PattonCoolTimeCouroutine());
                    State = Define.State.Skill; //���� ����
                    return; //���� (������� �����ߴٸ� State.Skill�� �Ѿ)
                }
                else if (distance > _minAttackRange && distance <= _farAttackRange)
                {
                    NavMeshAgent nma = gameObject.GetOrAddComponent<NavMeshAgent>();
                    nma.SetDestination(transform.position);
                    _near = false; //����� �޽�
                    StartCoroutine(PattonCoolTimeCouroutine());
                    State = Define.State.Skill; //���� ����
                    return;
                }
            }
        }
        Vector3 dir = _destPos - transform.position;  
        if (dir.magnitude < 0.1f)
        {
            State = Define.State.Idle;
        }
        else
        {
            NavMeshAgent nma = gameObject.GetOrAddComponent<NavMeshAgent>();
            nma.SetDestination(_destPos);
            nma.speed = _stat.MoveSpeed;
            _near = false;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20 * Time.deltaTime);
        }

    }

    IEnumerator PattonCoolTimeCouroutine()
    {
        _pattonCoolTime = true;
        yield return new WaitForSeconds(3f);
        _pattonCoolTime = false;

    }
    protected override void UpdateSkill()
    {
        if (_lockTarget != null && _lookAt)
        {
            Vector3 dir = _lockTarget.transform.position - transform.position;
            Quaternion quat = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, quat, 20 * Time.deltaTime);
        }

    }
    protected override void UpdateDie()
    {
        GameObject go = GameObject.Find("BossPunchRange");
        Destroy(go);
        _punchCol.SetActive(false);
        Collider _col = GetComponent<Collider>();
        Destroy(_col);
        BossController _bossCon = GetComponent<BossController>();
        Destroy(_bossCon);
        NavMeshAgent _nav = GetComponent<NavMeshAgent>();
        Destroy(_nav);
    }

    protected override void SpawnEvent()
    {
        base.SpawnEvent();
        //���� �ƾ�
    }

    void OnHitEvent()
    {
        Debug.Log("���� OnHitEvent");

        if (_lockTarget != null)
        {
            Stat targetStat = _lockTarget.GetComponent<Stat>();
            targetStat.OnAttacked(_stat);

            if (targetStat.Hp >= 0)
            {
                float distance = (_lockTarget.transform.position - transform.position).magnitude;
                if (distance <= _attackRange)
                {
                    State = Define.State.Skill;
                }
                else
                {
                    State = Define.State.Moving;
                }
            }
            else
            {
                State = Define.State.Idle;
            }
        }
        else
        {
            State = Define.State.Idle;
            Debug.Log(State);
        }
    }

    protected override void SkillAnim()
    {
        PlayerController _Player = _lockTarget.GetComponent<PlayerController>();
        if (_Player.State != Define.State.Die)
        {
            if (_bossState == BossState.Null)
            {
                if (_near)
                {
                    NearPatton();
                }
                else
                {
                    FarPatton();
                }
            }
            else
                return;
        }
        else
        {
            State = Define.State.Idle;
        }
    }

    public void NearPatton()
    {
        int _ranAction = Random.Range(0, 5);
        switch (_ranAction)
        {
            case 0:
            case 1:
            case 2:
                _lookAt = false;
                _bossState = BossState.Punch;
                StartCoroutine(Punch());
                break;
            case 3:
                _bossState = BossState.MissileShot;
                StartCoroutine(MissileShot());
                break;
            case 4:
                _lookAt = false;
                _bossState = BossState.SpawnGhoul;
                StartCoroutine(SpawnGhoul());
                break;
        }
    }

    public void FarPatton()
    {
        int rAction = Random.Range(0, 5);
        switch (rAction)
        {
            case 0:
            case 1:
                _lookAt = false;
                _bossState = BossState.FastRunning;
                StartCoroutine(FastRunning());
                break;
            case 2:
                _lookAt = false;
                _bossState = BossState.SpawnGhoul;
                StartCoroutine(SpawnGhoul());
                break;
            case 3:
            case 4:
                _bossState = BossState.MissileShot;
                StartCoroutine(MissileShot());
                break;
        }
    }

    IEnumerator MissileShot()
    {
        anim.SetFloat("MissileSpeed", 0.2f);
        anim.Play("Missile");

        yield return new WaitForSeconds(1.5f);
        GameObject go = Managers.Resource.Instantiate("Monster/Boss/BossMissile");
        go.transform.position = _missilePort.position;
        BossMissile bossMissile = go.GetComponent<BossMissile>();
        yield return new WaitForSeconds(1.5f);
        _bossState = BossState.Null;
        State = Define.State.Idle;
    }
    IEnumerator Punch()
    {
        anim.SetFloat("PunchSpeed", 0.2f); //��ġ �ӵ� ����
        anim.Play("Attack"); //��ġ ���

        GameObject go = Managers.Resource.Instantiate("Monster/Boss/BossPunchRange"); //�ٴ� ���� 
        go.transform.position = _punchRange.position; //��ġ
        go.transform.rotation = _punchRange.rotation; //ȸ��

        _punchCol.SetActive(true); //��ġ ���� �ݶ��̴� ��
        yield return new WaitForSeconds(2.5f); //��������
        _punchDust.Play();
        CheckPunchRange(); //Ÿ�� üũ
        yield return new WaitForSeconds(0.5f); //�ĵ�����
        _punchCol.SetActive(false); //��ġ ���� �ݶ��̴� ����

        Destroy(go); // ���� ����

        _bossState = BossState.Null; //���� �ٽ� idle ��� ����
        State = Define.State.Idle;
        _lookAt = true;
    }
    IEnumerator SpawnGhoul()
    {
        anim.SetFloat("SpawnSpeed", 0.3f);
        anim.Play("SpawnGhoul");

        GameObject obj1 = Managers.Game.Spawn(Define.WorldObject.Monster, "Ghoul");
        GameObject obj2 = Managers.Game.Spawn(Define.WorldObject.Monster, "Ghoul");
        _ptcSpawn2.Play();
        _ptcSpawn1.Play();
        obj1.transform.position = _spawn1.position;
        obj2.transform.position = _spawn2.position;
        
        yield return new WaitForSeconds(3f);
        _bossState = BossState.Null;
        State = Define.State.Idle;
        _lookAt = true;
    }
    IEnumerator FastRunning()
    {
        anim.Play("Run");
        Vector3 dir = _destPos - transform.position;
        NavMeshAgent nma = gameObject.GetOrAddComponent<NavMeshAgent>();
        nma.SetDestination(_destPos);
        nma.speed = _stat.MoveSpeed * 10;
        yield return new WaitForSeconds(1.4f);
        nma.speed = _stat.MoveSpeed;
        _bossState = BossState.Null;
        State = Define.State.Idle;
        _lookAt = true;
    }

    public void CheckPunchRange()
    {
        if (_inPunchRange == true)
            HitEvent();
    }

    public void HitEvent()
    {
        PlayerStat targetStat = _lockTarget.GetComponent<PlayerStat>();
        Debug.Log("���� ����");
        Debug.Log(targetStat.Hp);
        Debug.Log(_punchDam);
        targetStat.Hp -= _punchDam;
        if (targetStat.Hp <= 0)
        {
            targetStat.Hp = 0;
            targetStat._stat.State = Define.State.Die; 
            StartCoroutine(WaitTimeCoroutine());
        }
    }

    IEnumerator WaitTimeCoroutine()
    {
        GameObject player = Managers.Game.GetPlayer();
        PlayerStat _stat = player.GetComponent<PlayerStat>();
        _stat._isDead = true;
        yield return new WaitForSeconds(2.0f);
        Managers.UI.ShowPopupUI<UI_Died>();
    }
}
