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
        //LoadScene => ���� ��� �� �ҷ����� (���� �� �θ��� �������� �ƹ��͵� �� �� ����)
        //LoadSceneAsync => �񵿱� ��� �� �ҷ����� (���� �ҷ����� ���߿��� �ٸ� �۾��� �� �� �ִ�)
        AsyncOperation _op = SceneManager.LoadSceneAsync(_nextScene);
        _op.allowSceneActivation = false; //���� �񵿱�� �ҷ����϶�, ���� �ε��� ������ �ڵ����� �ҷ��� ������ �̵��� ������ ����
        float timer = 0f; //�ð� ����
        while(!_op.isDone) //���ε��� ������ ���� ���¶��,
        {
            yield return null; //�ݺ����� �ѹ� �ݺ��� ������ ������� ����Ƽ�� �Ѱ��� (�Ѱ����� �ʴ´ٸ�, LoadingBar�� fillAmount ��ȭ�� ������ ����)

            if (_op.progress < 0.9f) //90%�� �Ѿ�� ���� �ٲ�� ������ 90%������ ������ �ϰ�,
            {
                _LoadingBar.fillAmount = _op.progress; //�ε��� ����
            }
            else
            {
                timer += Time.unscaledDeltaTime;
                _LoadingBar.fillAmount = Mathf.Lerp(0.9f, 1f, timer); //�ε��ٸ� 90���� ���ʹ� 1�ʵ��� �������� ä����
                if (_LoadingBar.fillAmount >= 1f) //�ε��� �Ϸ�Ǹ�
                {
                    yield return new WaitForSeconds(0.5f);
                    _op.allowSceneActivation = true; //�ڵ����� �ҷ��� ������ �̵�
                    yield break;
                }
            }
        }
    }

}
