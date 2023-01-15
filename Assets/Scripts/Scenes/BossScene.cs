using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScene : BaseScene
{
    GameObject _player;
    [SerializeField]
    GameObject _bossPos;

    protected override void Init()
    {
        base.Init();

        //Managers.UI.ShowPopupUI<UI_Profile>();

        SceneType = Define.Scene.Boss;
        Managers.Sound.Play("Bgm/BossBgm", Define.Sound.Bgm);

        Dictionary<int, Data.Stat> dict = Managers.Data.StatDict;

        _player = Managers.Game.GetPlayer();
        Camera.main.gameObject.GetOrAddComponent<CameraController>().SetPlayer(_player);
        StartCoroutine(BossSpawnCoroutine());
    }

    IEnumerator BossSpawnCoroutine()
    {
        yield return new WaitForSeconds(7f);

        GameObject go = Managers.Resource.Instantiate("GhoulBoss");
        go.transform.position = _bossPos.transform.position;
    }

    public override void Clear()
    {

    }
}