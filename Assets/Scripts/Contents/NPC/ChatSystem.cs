using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChatSystem : MonoBehaviour
{
    public Queue<string> _text; //string을 담을 큐 선언
    public string _currentText; //큐에서 tmp로 넣어줄 임시 
    public TextMeshPro _tmp; 
    public GameObject _quad; //뒷배경의 크기 조절
    private Camera _cam;

    private void Start()
    {
        _cam = Camera.main;
    }

    public void Ondialogue(string[] _lines, GameObject _spawnPos) //NPC의 대사를 받아 큐에 저장
    {
        transform.position = _spawnPos.transform.position; //말풍선 위치 조정
        transform.rotation = _spawnPos.transform.rotation;
        _text = new Queue<string>(); //초기화
        _text.Clear();
        foreach (var _line in _lines) //string 배열의 값을 큐에 차례로 넣어줌
        {
            _text.Enqueue(_line);
        }
        StartCoroutine(DialogueFlow(_spawnPos));
    }

    IEnumerator DialogueFlow(GameObject _spawnPos) //큐에 담은 string을 차례대로 말풍선으로 띄우기
    {
        yield return null;
        while (_text.Count > 0)  //큐의 개수반큼 반복대서 tmp에 넣어줌
        {
            _currentText = _text.Dequeue();  //큐의 데이터를 한개씩 _currentText에 담았다가 _currentText를 tmp에 넣어줌
            _tmp.text = _currentText;
            float x = _tmp.preferredWidth; //_quad의 x 값을 밑에 조건을 붙여가공
            x = (x > 3) ? 3 : x + 0.3f; //x값이 3보다크면 3 작으면 0.3f
            _quad.transform.localScale = new Vector2(x, _tmp.preferredHeight); //_quad의 크기를 text에 알맞게 맞춤
            transform.position = new Vector3(_spawnPos.transform.position.x, _spawnPos.transform.position.y + _tmp.preferredHeight, _spawnPos.transform.position.z); //위치조정
            transform.Rotate(new Vector3(_spawnPos.transform.rotation.x, _spawnPos.transform.rotation.y, _spawnPos.transform.rotation.z));
            yield return new WaitForSeconds(3f);
        }
        Destroy(gameObject);
    }
}

//배운것 localScale은 매서드로 사용 할 수 없다. localScale(x,y) => X // localScale = new Vector2(x,y)
//[SerializeField]
//private Transform _lookAt;
//[SerializeField]
//private Vector3 _offset;


//private Camera _cam;

//private void Start()
//{
//    _cam = Camera.main;
//}

//private void Update()
//{
//    Vector3 _pos = _cam.WorldToScreenPoint(_lookAt.position + _offset);
//    if (transform.position != _pos)
//        transform.position = _pos;
//}
