using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawningPool : MonoBehaviour
{
    [SerializeField]
    string _monsterName;

    [SerializeField]
    int _monsterCount = 0;
    int _reserveCount = 0;

    [SerializeField]
    int _keepMonsterCount = 0;

    [SerializeField]
    Vector3 _spawnPos;

    [SerializeField]
    float _spawnRadius = 0f;

    [SerializeField]
    float _spawnTime = 5.0f;

    public void AddMonsterCount1(int value) { _monsterCount += value; }
    public void AddMonsterCount2(int value) { _monsterCount += value; }
    public void AddMonsterCount3(int value) { _monsterCount += value; }
    public void AddMonsterCount4(int value) { _monsterCount += value; }
    //public void SetKeepMonsterCount(int count,Vector3 _pos,string _name) 
    //{
    //    _keepMonsterCount = count;
    //    _spawnPos = _pos;
    //    _monsterName = _name;
    //}

    void Start()
    {
        switch (_monsterName)
        {
            case "Ghoul":
                Managers.Game.OnSpawnEvent1 -= AddMonsterCount1;
                Managers.Game.OnSpawnEvent1 += AddMonsterCount1;
                break;
            case "GhoulScavenger":
                Managers.Game.OnSpawnEvent2 -= AddMonsterCount2;
                Managers.Game.OnSpawnEvent2 += AddMonsterCount2;
                break;
            case "GhoulGrotesque":
                Managers.Game.OnSpawnEvent3 -= AddMonsterCount3;
                Managers.Game.OnSpawnEvent3 += AddMonsterCount3;
                break;
            case "GhoulFestering":
                Managers.Game.OnSpawnEvent4 -= AddMonsterCount4;
                Managers.Game.OnSpawnEvent4 += AddMonsterCount4;
                break;
        }
    }

    void LateUpdate()
    {
        while (_reserveCount + _monsterCount < _keepMonsterCount)
        {
            StartCoroutine(RecerveSpawn(_monsterName));
        }
    }

    IEnumerator RecerveSpawn(string _name)
    {
        _reserveCount++;
        yield return new WaitForSeconds(Random.Range(0, _spawnTime));
        GameObject obj = Managers.Game.Spawn(Define.WorldObject.Monster, _name);
        NavMeshAgent nma = obj.GetOrAddComponent<NavMeshAgent>();
        Vector3 randPos;

        while(true)
        {
            Vector3 randDir = Random.insideUnitSphere * Random.Range(0, _spawnRadius);

            randDir.y = 0;
            randPos = _spawnPos + randDir;

            obj.transform.position = randPos;
            NavMeshPath path = new NavMeshPath();
            nma.CalculatePath(randPos, path);
                break;
        }

        _reserveCount--;
    }
}
