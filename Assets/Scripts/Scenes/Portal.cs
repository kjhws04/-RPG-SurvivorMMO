using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    GameObject _player;

    [SerializeField]
    private string _goToThisScene;
    [SerializeField]
    private Vector3 _goToThisPos;

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(Input.GetKeyDown(KeyCode.G))
            {
                LoadingScene.LoadScene(_goToThisScene);
                //SceneManager.LoadScene(_goToThisScene);
                _player = Managers.Game.GetPlayer();
                _player.transform.position = _goToThisPos;
            }
        }
    }
}
