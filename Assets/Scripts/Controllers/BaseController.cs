using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseController : MonoBehaviour
{
	[Header("Stste & Pos Info")]
	[SerializeField]
	protected Vector3 _destPos;

	[SerializeField]
	protected Define.State _state = Define.State.Spawn;

	[SerializeField]
	protected GameObject _lockTarget;

	protected Animator anim;

	public Define.WorldObject WorldObjectType { get; protected set; } = Define.WorldObject.Unknown;

	public virtual Define.State State
	{
		get { return _state; }
		set
		{
			_state = value;

			anim = GetComponent<Animator>();
			switch (_state)
			{
				case Define.State.Die:
					anim.CrossFade("DIE", 0.1f);
					break;
				case Define.State.Idle:
					anim.CrossFade("WAIT", 0.1f);
					break;
				case Define.State.Moving:
					anim.CrossFade("RUN", 0.1f);
					break;
				case Define.State.Skill:
					SkillAnim();
					break;
				case Define.State.Spawn:
					anim.Play("SPAWN");
					break;
			}
		}
	}

	protected virtual void SpawnEvent()
    {
		_state = Define.State.Idle;
	}

	protected virtual void SkillAnim()
    {
		anim.CrossFade("ATTACK", 0.1f, -1, 0);
	}

	private void Start()
	{
		Init();
	}

	void Update()
	{
		switch (State)
		{
			case Define.State.Die:
				UpdateDie();
				break;
			case Define.State.Moving:
				UpdateMoving();
				break;
			case Define.State.Idle:
				UpdateIdle();
				break;
			case Define.State.Skill:
				UpdateSkill();
				break;
		}
		UpdatePotionCoolTime();
	}

	public abstract void Init();

	protected virtual void UpdateDie() { }
	protected virtual void UpdateMoving() { }
	protected virtual void UpdateIdle() { }
	protected virtual void UpdateSkill() { }

	protected virtual void UpdatePotionCoolTime() { }
}
