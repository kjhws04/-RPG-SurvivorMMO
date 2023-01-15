using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Died : UI_Popup
{
    GameObject _player;
    PlayerStat _Stat;

    enum GameObjects
    {
        Retry
    }

    public override void Init()
    {
        _player = Managers.Game.GetPlayer();
        _Stat = _player.GetComponent<PlayerStat>();
        Bind<GameObject>(typeof(GameObjects));
        GetObject((int)GameObjects.Retry).SetActive(false);
        StartCoroutine(RetryButtonCoroutine());
    }

    public override void ClosePopupUI()
    {
        base.ClosePopupUI(); //팝업 닫기
        LoadingScene.LoadScene("Game");
        _Stat._stat.State = Define.State.Idle; //상태 IDLE
        _player.transform.position = new Vector3(0f, 0f, -226f); //위치 초기
        _Stat.Hp = _Stat.MaxHp; //HP초기화
        _Stat._isDead = false; //죽음 초기화
    }
    
    IEnumerator RetryButtonCoroutine()
    {
        yield return new WaitForSeconds(3f);
        GetObject((int)GameObjects.Retry).SetActive(true);
    }
}
