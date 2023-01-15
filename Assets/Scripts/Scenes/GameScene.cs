using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    //private int maxSpawnCount = 1;
    GameObject _player;

    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.Game;
        Managers.Sound.Play("Bgm/TownBgm", Define.Sound.Bgm);

        Dictionary<int, Data.Stat> dict = Managers.Data.StatDict;
        //CursorController _cursor = gameObject.GetOrAddComponent<CursorController>();

        //플레이어 체크 & 스폰
        GameObject _checkPlayer = Managers.Game.GetPlayer();
        if (_checkPlayer == null)
        {
            _player = Managers.Game.Spawn(Define.WorldObject.Player, "UnityChan");
            _player.transform.position = new Vector3(0.3f, 0, -225f);
            DontDestroyOnLoad(_player);
        }
        else
            _player = Managers.Game.GetPlayer();

        Camera.main.gameObject.GetOrAddComponent<CameraController>().SetPlayer(_player);
        //GameObject go = new GameObject { name = "SpawningPool" };
        //SpawningPool pool = go.GetOrAddComponent<SpawningPool>();
        //pool.SetKeepMonsterCount(maxSpawnCount);
    }

    public override void Clear()
    {
        
    }
}