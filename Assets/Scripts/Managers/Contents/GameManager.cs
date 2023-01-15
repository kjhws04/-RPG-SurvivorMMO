using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // int <=> gameObject
    GameObject _player;

    HashSet<GameObject> _monster = new HashSet<GameObject>();

    public Action<int> OnSpawnEvent1;
    public Action<int> OnSpawnEvent2;
    public Action<int> OnSpawnEvent3;
    public Action<int> OnSpawnEvent4;

    public GameObject GetPlayer() { return _player; }

    public GameObject Spawn(Define.WorldObject type, string path, Transform parent = null)
    {
        GameObject go = Managers.Resource.Instantiate(path, parent);

        switch (type)
        {
            case Define.WorldObject.Monster:
                Stat _stat = go.GetComponent<Stat>();
                MonsterController _name = go.GetComponent<MonsterController>();
                _stat.Hp = _stat.MaxHp;
                _monster.Add(go);
                switch (_name._name)
                {
                    case Define.MonsterName.Ghoul:
                        if (OnSpawnEvent1 != null)
                            OnSpawnEvent1.Invoke(1);
                        break;
                    case Define.MonsterName.GhoulScavenger:
                        if (OnSpawnEvent2 != null)
                            OnSpawnEvent2.Invoke(1);
                        break;
                    case Define.MonsterName.GhoulGrotesque:
                        if (OnSpawnEvent3 != null)
                            OnSpawnEvent3.Invoke(1);
                        break;
                    case Define.MonsterName.GhoulFestering:
                        if (OnSpawnEvent4 != null)
                            OnSpawnEvent4.Invoke(1);
                        break;
                }
                break;
            case Define.WorldObject.Player:
                _player = go;
                break;
        }

        return go;
    }

    public Define.WorldObject GetWorldObjectType(GameObject go)
    {
        BaseController bc = go.GetComponent<BaseController>();

        if (bc == null)
            return Define.WorldObject.Unknown;

        return bc.WorldObjectType;
        
    }

    public void Despawn(GameObject go)
    {
        Define.WorldObject type = GetWorldObjectType(go);

        switch (type)
        {
            case Define.WorldObject.Monster:
                {
                    if (_monster.Contains(go))
                    {
                        MonsterController _name = go.GetComponent<MonsterController>();
                        _monster.Remove(go);
                        switch (_name._name)
                        {
                            case Define.MonsterName.Ghoul:
                                if (OnSpawnEvent1 != null)
                                    OnSpawnEvent1.Invoke(-1);
                                break;
                            case Define.MonsterName.GhoulScavenger:
                                if (OnSpawnEvent2 != null)
                                    OnSpawnEvent2.Invoke(-1);
                                break;
                            case Define.MonsterName.GhoulGrotesque:
                                if (OnSpawnEvent3 != null)
                                    OnSpawnEvent3.Invoke(-1);
                                break;
                            case Define.MonsterName.GhoulFestering:
                                if (OnSpawnEvent4 != null)
                                    OnSpawnEvent4.Invoke(-1);
                                break;
                        }
                    }
                }
                break;
            case Define.WorldObject.Player:
                {
                    if (_player == go)
                        _player = null;
                }
                break;
        }

        Managers.Resource.Destroy(go);
    }
}
