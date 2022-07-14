using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLine : MonoBehaviour
{
    private int counter = 0;
    private IEnumerator OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Raptor" || collision.tag == "MainPlayer")
        {
            counter++;
            print(counter);
        }
        if (counter == 8)
        {
            yield return new WaitForSeconds(10f);
            yield return SceneManager.LoadSceneAsync(0);
            counter = 0;
        }
    }
}
