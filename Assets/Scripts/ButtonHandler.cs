using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonHandler : MonoBehaviour
{
    [SerializeField] GameObject playerToInstantiate = null;
    GameObject spawnPoints = null;
    int index = 0;

    private void Start() {
        spawnPoints = GameObject.FindWithTag("SpawnPoint");
    }

    public void ShowPlayer() {
        if (index == spawnPoints.transform.childCount) {
            SceneManager.LoadScene(1);
        }
        var player = Instantiate(playerToInstantiate, spawnPoints.transform.GetChild(index).GetComponent<Transform>());
        DontDestroyOnLoad(player);
        index++;
    }
}
