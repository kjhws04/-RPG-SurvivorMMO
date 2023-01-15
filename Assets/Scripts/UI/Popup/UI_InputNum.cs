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

    public bool InputNumCheck(string _arg) //���� ���� �������� Ȯ�� �ϴ� �Լ�
    {
        char[] _tempCharArray = _arg.ToCharArray(); //�޾ƿ� ���� 1���ڴ� 1�迭�� ���� 
        bool _isNum = true; //���� ���� ���������� �Ǻ��ϴ� ����

        for (int i = 0; i < _tempCharArray.Length; i++) //�迭�� for������ Ȯ�� ex) 48d7 => [4],[8],[d],[7]
        {
            if (_tempCharArray[i] >= 48 && _tempCharArray[i] <= 57) //i��° �迭�� �����ΰ�? [4]=o,[8]=o,[d]=x
                continue; //�´ٸ� ���
            _isNum = false; //�ƴ϶�� ������ false
        }

        return _isNum; //���ڶ�� true, ���� ���� �ٸ� �ƽ�Ű�ڵ尡 �ִٸ� false
    }

    public void OnDropOk() //ok ��ư�� ������ ��,
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
        // 8�ð��� �������� �ϴ��� �����ϱ�� ����...

        //if (_tmpInput != null)
        //{
        //    if (InputNumCheck(_tmpInput))
        //    {
        //        _slot.CheckNumItemDrop(1);
        //    }
        //    else
        //    {
        //        //�ٹ�����
        //    }

        //    // �ȵǴ� �� Debug.Log("int.Parse �� �� : " + int.Parse(_Input));
        //    Debug.Log("�����ΰ� �����ΰ� : " + InputNumCheck(_Input));
        //    Debug.Log("������ �� int �� �� : " + int.TryParse(_Input, out int result));
        //    Debug.Log($"��� �� : {result}");
        //}
        //else
        //{
        //    //�ٹ�����
        //}
        //StartCoroutine(DropItemCoroutine(num));
    }

    public override void ClosePopupUI()
    {
        base.ClosePopupUI();
    }
}
