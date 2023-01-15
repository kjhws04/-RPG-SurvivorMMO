using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonScene : BaseScene
{
    GameObject _player;
    [SerializeField]
    GameObject _spawnPos;

    protected override void Init()
    {
        base.Init();

        //Managers.UI.ShowPopupUI<UI_Profile>();

        SceneType = Define.Scene.Game;
        Managers.Sound.Play("Bgm/DGBgm", Define.Sound.Bgm);

        Dictionary<int, Data.Stat> dict = Managers.Data.StatDict;
        //gameObject.GetOrAddComponent<CursorController>();
        _player = Managers.Game.GetPlayer();
        Camera.main.gameObject.GetOrAddComponent<CameraController>().SetPlayer(_player);
    }
    //GameObject player = Managers.Game.Spawn(Define.WorldObject.Player, "UnityChan");
    //Managers.Game.Spawn(Define.WorldObject.Monster, "Knight");
    //GameObject go = new GameObject { name = "SpawningPool" };
    //SpawningPool pool = go.GetOrAddComponent<SpawningPool>();
    //pool.SetKeepMonsterCount(maxSpawnCount);

    public override void Clear()
    {

    }
}
