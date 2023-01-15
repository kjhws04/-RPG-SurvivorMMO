using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : BaseController
{
	int _mask = (1 << (int)Define.Layer.Ground) | (1 << (int)Define.Layer.Monster);

	#region need Component Property
	PlayerStat _stat;
	UI_Inventory _inventory;
	public CursorController _cursur;
	[Header("Quick & Socket Info")]
	public UI_Slot _equipment;
	public UI_Slot _armor;
	public UI_Slot _shose;
	public UI_Slot _potion;
	public GameObject LefthandSocket;
	public GameObject RighthandSocket;
	#endregion
	#region Attack Gournd Property
	[Header("Lock Target Info")]
	[SerializeField]
	private float _radius = 10.0f; //플레이어가 공격할 반경 
	[SerializeField]
	public Collider[] _playerToEnemyCol;
	[SerializeField]
	public Collider[] _destPosToEnemyCol;
	#endregion
	#region Potion & Skill Property
	[Header("Potion & Skill Info")]
	[SerializeField]
	bool _stopSkill = false;
	public bool _isCoolTime = false;
	public float _currentCoolTime = 0;
	public Define.WeaponAnim _weaponAnim = Define.WeaponAnim.Null;
	public ParticleSystem _HpPtc;
	public ParticleSystem _MpPtc;
	#endregion

	public override void Init()
	{
		WorldObjectType = Define.WorldObject.Player;
		_stat = gameObject.GetComponent<PlayerStat>();
		_inventory = Util.FindChild<UI_Inventory>(gameObject, "UI_Inventory");
		_cursur = FindObjectOfType<CursorController>();

		Managers.Input.KeyAction -= OnKeyboard;
		Managers.Input.KeyAction += OnKeyboard;
		Managers.Input.MouseAction -= OnMouseEvent;
		Managers.Input.MouseAction += OnMouseEvent;

        #region HP Bar Code
        //플레이어 머리 위에 HpBar를 붙이는 코드
        //if (gameObject.GetComponentInChildren<UI_HPBar>() == null)
        //	Managers.UI.MakeWorldSpaceUI<UI_HPBar>(transform);
        #endregion
    }

    protected override void UpdatePotionCoolTime()
    {
		if (_isCoolTime)
			CoolTimeCalc();
	} //포션 쿨타임 함수

    protected override void UpdateMoving()
	{
		if (_lockTarget != null)
		{
			_destPos = _lockTarget.transform.position;
			float distance = (_destPos - transform.position).magnitude;
			if (distance <= WeaponRange())
			{
				State = Define.State.Skill;
				return;
			}
		} // 몬스터가 내 사정거리보다 가까우면 공격

		Vector3 dir = _destPos - transform.position;
		dir.y = 0;

		if (dir.magnitude < 0.1f)
			State = Define.State.Idle;
		else
		{
			Debug.DrawRay(transform.position + Vector3.up * 0.5f, dir.normalized, Color.green);
			if (Physics.Raycast(transform.position + Vector3.up * 0.5f, dir, 1.0f, LayerMask.GetMask("Block")))
			{
				if (Input.GetMouseButton(0) == false)
					State = Define.State.Idle;
				return;
			}
			float moveDist = Mathf.Clamp(_stat.MoveSpeed * Time.deltaTime, 0, dir.magnitude);
			transform.position += dir.normalized * moveDist;
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20 * Time.deltaTime);
		}
	}

	float WeaponRange() //무기별 평타 사거리를 계산하는 함수
    {
		if (_equipment._item == null)
			return 0.5f;
		else
			return _equipment._item._itemRange;
    }

	protected override void SkillAnim()
	{
		if (_equipment._item == null)
		{
			anim.CrossFade("Punch", 0.1f, -1, 0);
			return;
		}
		//만약 스킬을 사용하지 않았다면 리턴
		//스킬을 사용했다면 애니메이션 재생

		switch (_equipment._item._weaponAnim)
		{
			case Define.WeaponAnim.Null:
				anim.CrossFade("Punch", 0.1f, -1, 0);
				break;
			case Define.WeaponAnim.OneHandSword:
				anim.CrossFade("OneHandSword", 0.1f, -1, 0);
				break;
			case Define.WeaponAnim.Bow:
				anim.CrossFade("Bow", 0.1f, -1, 0);
				break;
			case Define.WeaponAnim.Magic:
				anim.CrossFade("Magic", 0.1f, -1, 0);
				break;
		}
	} //무기별 애니메이션을 변경하는 함수

	protected override void UpdateSkill() //평타 공격
	{
		if (_lockTarget != null)
		{
			Vector3 dir = _lockTarget.transform.position - transform.position;
			Quaternion quat = Quaternion.LookRotation(dir);
			transform.rotation = Quaternion.Lerp(transform.rotation, quat, 20 * Time.deltaTime);
		}
	}

    protected override void UpdateDie()
    {
    }

	void OnHitEvent()
	{
		if(_lockTarget != null)
        {
			//TODO
			Stat targetStat = _lockTarget.GetComponent<Stat>();
			targetStat.OnAttacked(_stat);
		}

		if (_stopSkill)
		{
			State = Define.State.Idle;
		}
		else
		{
			State = Define.State.Skill;
		}
	}

	void OnKeyboard()
	{
		if (_state == Define.State.Die)
            return;

		if (Input.GetKeyDown(KeyCode.Q)) //temp
        {
            if (_stat.Mp <= 0)
                return;

            _stat.UseMana(40);
		}

		if(Input.GetKeyDown(KeyCode.Alpha1))
        {
			if (_potion._item == null) //포션 퀵슬롯이 비어있으면
				return; //비어 있음 리턴

			if (!_isCoolTime) //쿨타임 중이 아니라면,
			{
				switch (_potion._item._potionType) //포션에 맞는 수치 회복
				{
					case Define.HpMpType.Null:
						break;
					case Define.HpMpType.Hp:
						_HpPtc.Play();
						if (_stat.MaxHp <= _stat.Hp + _potion._item._itemEffect)
							_stat.Hp = _stat.MaxHp;
						else
							_stat.Hp += _potion._item._itemEffect;
						break;
					case Define.HpMpType.Mp:
						_MpPtc.Play();
						if (_stat.MaxHp <= _stat.Hp + _potion._item._itemEffect)
							_stat.Mp = _stat.MaxMp;
						else
							_stat.Mp += _potion._item._itemEffect;
						break;
					default:
						break;
				}
				_potion.SetSlotCount(-1); //포션을 1개깜
				_isCoolTime = true;
			}
        } //포션 관련 키보드 함수
	}

    void OnMouseEvent(Define.MouseEvent evt)
	{
		switch (State)
		{
			case Define.State.Idle:
				OnMouseEvent_IdleRun(evt);
				break;
			case Define.State.Moving:
				OnMouseEvent_IdleRun(evt);
				break;
			case Define.State.Skill:
				{
					if (evt == Define.MouseEvent.PointerUp)
						_stopSkill = true;
				}
				break;
		}
	}

	void OnMouseEvent_IdleRun(Define.MouseEvent evt)
	{
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //마우스 클릭 위치
		bool raycastHit = Physics.Raycast(ray, out hit, 100.0f, _mask);
		//Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.red, 1.0f);

		switch (evt) 
		{
			case Define.MouseEvent.PointerDown:
				{
					if (raycastHit)
					{
						_destPos = hit.point; //찍은 땅 좌표
						State = Define.State.Moving;
						_stopSkill = false;
						
						if (_cursur._isAttackGround)
                        {
							_playerToEnemyCol = Physics.OverlapSphere(transform.position, _radius*2, 1 << 8);
							_destPosToEnemyCol = Physics.OverlapSphere(_destPos, _radius, 1 << 8);

							if (hit.collider.gameObject.layer == (int)Define.Layer.Monster) //마우스 댄 레이어가 Monster라면
							{
								_lockTarget = hit.collider.gameObject; //락 타켓은 그 몬스터
								_cursur._isAttackGround = false;
								return;
							}
							else
							{
								if (_playerToEnemyCol != null) //플레이어 반경에 몬스터가 있는지 확인
								{
									foreach (Collider _target in _playerToEnemyCol)
									{
										float _shortDis = Mathf.Infinity;

										float dis = Vector3.SqrMagnitude(transform.position - _target.transform.position);
										if (_shortDis > dis)
										{
											_shortDis = dis;
											_lockTarget = _target.gameObject;
											_cursur._isAttackGround = false;
											return;
										}
									}
								}
								else //플레이어 반경에 몬스터가 없다면, 어택땅을 찍은 포지션 주변에 몬스터가 있는지 확인
								{
									foreach (Collider _target in _destPosToEnemyCol)
									{
										float _shortDis = Mathf.Infinity;

										float dis = Vector3.SqrMagnitude(_destPos - _target.transform.position);
										if (_shortDis > dis)
										{
											_shortDis = dis;
											_lockTarget = _target.gameObject;
											_cursur._isAttackGround = false;
											return;
										}
									}
								}
								_cursur._isAttackGround = false;
							}
						} //어택땅 코드

						if (hit.collider.gameObject.layer == (int)Define.Layer.Monster) //마우스 댄 레이어가 Monster라면
							_lockTarget = hit.collider.gameObject; //락 타켓은 그 몬스터
						else
							_lockTarget = null; //아니면 null
					}
				}
				break;
			case Define.MouseEvent.Press:
				{
					if (_lockTarget == null && raycastHit)
						_destPos = hit.point;
				}
				break;
			case Define.MouseEvent.PointerUp:
				_stopSkill = true;
				break;
		}
	}

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                _inventory.AcquireItem(other.GetComponent<ItemPickUp>()._item);
                Destroy(other.transform.gameObject);
            }
        }
    } //아이템 획득 함수 todo

    private void OnTriggerEnter(Collider other)
    {
		Bullet _bullet = other.GetComponent<Bullet>();
		if (other.CompareTag("Bullet"))
        {
			_stat.Hp = _stat.Hp - _bullet._damage;
			if (_stat.Hp <= 0)
				State = Define.State.Die;
		}
    }

    private void CoolTimeCalc() 
	{
		_stat.PotionCoolTime -= Time.deltaTime; //포션 쿨타임을 초당 1씩 감소
		if (_stat.PotionCoolTime <= 0) //포션 쿨타임이 0이 되면
		{
			_isCoolTime = false; //쿨타임 초기화
			
			if(_potion._item != null) //퀵슬롯에 장비된 아이템이 있다면
				_stat.PotionCoolTime = _potion._item._potionCoolTime; //포션 쿨타임을 퀵슬롯에 장비된 포션 쿨타임으로 바꿈
		}
	} //쿨타임이 감소하는 함수
}