using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChatSystem : MonoBehaviour
{
    public Queue<string> _text; //string�� ���� ť ����
    public string _currentText; //ť���� tmp�� �־��� �ӽ� 
    public TextMeshPro _tmp; 
    public GameObject _quad; //�޹���� ũ�� ����
    private Camera _cam;

    private void Start()
    {
        _cam = Camera.main;
    }

    public void Ondialogue(string[] _lines, GameObject _spawnPos) //NPC�� ��縦 �޾� ť�� ����
    {
        transform.position = _spawnPos.transform.position; //��ǳ�� ��ġ ����
        transform.rotation = _spawnPos.transform.rotation;
        _text = new Queue<string>(); //�ʱ�ȭ
        _text.Clear();
        foreach (var _line in _lines) //string �迭�� ���� ť�� ���ʷ� �־���
        {
            _text.Enqueue(_line);
        }
        StartCoroutine(DialogueFlow(_spawnPos));
    }

    IEnumerator DialogueFlow(GameObject _spawnPos) //ť�� ���� string�� ���ʴ�� ��ǳ������ ����
    {
        yield return null;
        while (_text.Count > 0)  //ť�� ������ŭ �ݺ��뼭 tmp�� �־���
        {
            _currentText = _text.Dequeue();  //ť�� �����͸� �Ѱ��� _currentText�� ��Ҵٰ� _currentText�� tmp�� �־���
            _tmp.text = _currentText;
            float x = _tmp.preferredWidth; //_quad�� x ���� �ؿ� ������ �ٿ�����
            x = (x > 3) ? 3 : x + 0.3f; //x���� 3����ũ�� 3 ������ 0.3f
            _quad.transform.localScale = new Vector2(x, _tmp.preferredHeight); //_quad�� ũ�⸦ text�� �˸°� ����
            transform.position = new Vector3(_spawnPos.transform.position.x, _spawnPos.transform.position.y + _tmp.preferredHeight, _spawnPos.transform.position.z); //��ġ����
            transform.Rotate(new Vector3(_spawnPos.transform.rotation.x, _spawnPos.transform.rotation.y, _spawnPos.transform.rotation.z));
            yield return new WaitForSeconds(3f);
        }
        Destroy(gameObject);
    }
}

//���� localScale�� �ż���� ��� �� �� ����. localScale(x,y) => X // localScale = new Vector2(x,y)
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
