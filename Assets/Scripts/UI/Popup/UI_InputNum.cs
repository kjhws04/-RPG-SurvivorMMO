using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI_InputNum : UI_Popup
{
    UI_InvRigthClick _slot;

    [SerializeField]
    TextMeshProUGUI _input;

    enum Buttons
    {
        DropButton,
        CancelButton
    }

    enum Texts
    {
        PreviewText,
        InputText
    }

    public void Call(UI_InvRigthClick slot)
    {
        _slot = slot;
    }

    public override void Init()
    {
        base.Init();

        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Button>(typeof(Buttons));

        GetTextMeshProUGUI((int)Texts.PreviewText).text = _slot.itemCount.ToString();
    }

    public bool InputNumCheck(string _arg) //받은 값이 숫자인지 확인 하는 함수
    {
        char[] _tempCharArray = _arg.ToCharArray(); //받아온 값을 1문자당 1배열로 저장 
        bool _isNum = true; //받은 값이 숫자인지를 판별하는 변수

        for (int i = 0; i < _tempCharArray.Length; i++) //배열을 for문으로 확인 ex) 48d7 => [4],[8],[d],[7]
        {
            if (_tempCharArray[i] >= 48 && _tempCharArray[i] <= 57) //i번째 배열이 숫자인가? [4]=o,[8]=o,[d]=x
                continue; //맞다면 계속
            _isNum = false; //아니라면 변수를 false
        }

        return _isNum; //숫자라면 true, 숫자 외의 다른 아스키코드가 있다면 false
    }

    public void OnDropOk() //ok 버튼을 눌렀을 때,
    {
        GameObject go = transform.GetChild(1).transform.GetChild(0).transform.GetChild(2).gameObject;
        TMP_Text tmp = go.GetComponent<TMP_Text>();
        string test = tmp.text;

        Debug.Log(tmp.text);
        Debug.Log(tmp.text.GetType());
        Debug.Log(int.TryParse(tmp.text, out int result));
        Debug.Log(int.TryParse(test, out int result1));
        Debug.Log(int.TryParse("12", out int result2));
        Debug.Log(result1);
        Debug.Log(result);
        // 8시간의 사투끝에 일단은 유기하기로 결정...

        //if (_tmpInput != null)
        //{
        //    if (InputNumCheck(_tmpInput))
        //    {
        //        _slot.CheckNumItemDrop(1);
        //    }
        //    else
        //    {
        //        //다버리기
        //    }

        //    // 안되는 것 Debug.Log("int.Parse 한 값 : " + int.Parse(_Input));
        //    Debug.Log("숫자인가 문자인가 : " + InputNumCheck(_Input));
        //    Debug.Log("숫자일 때 int 한 값 : " + int.TryParse(_Input, out int result));
        //    Debug.Log($"결과 값 : {result}");
        //}
        //else
        //{
        //    //다버리기
        //}
        //StartCoroutine(DropItemCoroutine(num));
    }

    public override void ClosePopupUI()
    {
        base.ClosePopupUI();
    }
}
