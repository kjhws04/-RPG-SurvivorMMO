using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour
{
    static string _nextScene;

    [SerializeField]
    Image _LoadingBar;

    public static void LoadScene(string _sceneName)
    {
        _nextScene = _sceneName;
        SceneManager.LoadScene("Loading");
    }

    void Start()
    {
        StartCoroutine(LoadSceneProcess());
    }

    IEnumerator LoadSceneProcess()
    {
        //LoadScene => 동기 방식 씬 불러오기 (씬을 다 부르기 전까지는 아무것도 할 수 없다)
        //LoadSceneAsync => 비동기 방식 씬 불러오기 (씬을 불러오는 도중에도 다른 작업을 할 수 있다)
        AsyncOperation _op = SceneManager.LoadSceneAsync(_nextScene);
        _op.allowSceneActivation = false; //씬을 비동기로 불러들일때, 씬의 로딩이 끝나면 자동으로 불러올 씬으로 이동할 것인지 설정
        float timer = 0f; //시간 측정
        while(!_op.isDone) //씬로딩이 끝나지 않을 상태라면,
        {
            yield return null; //반복문이 한번 반복될 때마다 제어권을 유니티에 넘겨줌 (넘겨주지 않는다면, LoadingBar의 fillAmount 변화가 보이지 않음)

            if (_op.progress < 0.9f) //90%가 넘어가면 씬이 바뀌기 때문에 90%까지는 진행을 하고,
            {
                _LoadingBar.fillAmount = _op.progress; //로딩바 갱신
            }
            else
            {
                timer += Time.unscaledDeltaTime;
                _LoadingBar.fillAmount = Mathf.Lerp(0.9f, 1f, timer); //로딩바를 90프로 부터는 1초동안 나머지를 채워줌
                if (_LoadingBar.fillAmount >= 1f) //로딩이 완료되면
                {
                    yield return new WaitForSeconds(0.5f);
                    _op.allowSceneActivation = true; //자동으로 불러올 씬으로 이동
                    yield break;
                }
            }
        }
    }

}
