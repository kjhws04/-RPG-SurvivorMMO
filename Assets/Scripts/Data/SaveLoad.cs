using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable] //�����͸� ����ȭ�ϸ�, ���ٷ� �����Ͱ� �����Ǿ� ���� ��ġ�� �а� ���� ������
public class SaveData
{
    public Vector3 _playerPos;

}

public class SaveLoad : MonoBehaviour
{
    private SaveData _save = new SaveData();

    private string SAVE_DATA_DIRECTORY;
    private string SAVE_FILENAME = "/SaveFile.txt";

    GameObject _player;

    void Start()
    {
        SAVE_DATA_DIRECTORY = Application.dataPath + "/Saves/";

        if (!Directory.Exists(SAVE_DATA_DIRECTORY))
            Directory.CreateDirectory(SAVE_DATA_DIRECTORY);

        _player = Managers.Game.GetPlayer();
    }

    public void SaveData()
    {
        _save._playerPos = _player.transform.position;

        string _json = JsonUtility.ToJson(_save);

        File.WriteAllText(SAVE_DATA_DIRECTORY + SAVE_FILENAME, _json);

        Debug.Log("����");
        Debug.Log(_json);
    }

    public void LoadData()
    {

    }
}
