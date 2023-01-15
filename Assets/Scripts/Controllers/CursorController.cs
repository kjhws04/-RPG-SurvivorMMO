using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
	int _mask = (1 << (int)Define.Layer.Ground) | (1 << (int)Define.Layer.Monster);
	PlayerController _player;
	Texture2D _attackIcon;
	Texture2D _handIcon;
	Texture2D _attackHand;
	[SerializeField]
	ParticleSystem _cilck;

	public bool _isAttackGround = false;

	enum CursorType
	{
		None,
		Attack,
		Hand,
		AttackGround
	}

	CursorType _cursorType = CursorType.None;

	void Start()
	{
		_player = FindObjectOfType<PlayerController>();
		_attackIcon = Managers.Resource.Load<Texture2D>("Textures/Cursor/Attack");
		_handIcon = Managers.Resource.Load<Texture2D>("Textures/Cursor/Hand");
		_attackHand = Managers.Resource.Load<Texture2D>("Textures/Cursor/AttackHand");
	}

	void Update()
	{
		if (Input.GetMouseButton(0))
			return;

		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		if (Input.GetKeyDown(KeyCode.A)) //A키를 누르면 
        {
			_cursorType = CursorType.AttackGround; 
			_isAttackGround = true;
			Cursor.SetCursor(_attackHand, new Vector2(_attackHand.width / 5, 0), CursorMode.Auto);
			_player._playerToEnemyCol = null;
			_player._destPosToEnemyCol = null;
		}

		if (!_isAttackGround)
		{
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, 100.0f, _mask))
			{
				if (hit.collider.gameObject.layer == (int)Define.Layer.Monster)
				{
					if (_cursorType != CursorType.Attack)
					{
						Cursor.SetCursor(_attackIcon, new Vector2(_attackIcon.width / 5, 0), CursorMode.Auto);
						_cursorType = CursorType.Attack;
					}
				}
				else
				{
					if (_cursorType != CursorType.Hand)
					{
						Cursor.SetCursor(_handIcon, new Vector2(_handIcon.width / 3, 0), CursorMode.Auto);
						_cursorType = CursorType.Hand;
					}
				}
			}
		}
	}
}
