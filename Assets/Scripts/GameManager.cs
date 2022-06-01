using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject spawnPoints;
    public GameObject raptor1;
    public GameObject raptor2;
    private float number1Speed = 1.15f;
    private float number2Speed = 1.1f;
    private float number3Speed = 1.05f;
    private float othersSpeed = 1.03f;

    private void Start()
    {
        int index = 0;
        foreach (int item in APICall.instance.raptorsInPlay)
        {
            if (item == 0) continue;
            GameObject instantiateCharacter = item == 1 ? raptor1 : raptor2;
            if (spawnPoints.transform.GetChild(index).gameObject.transform.childCount > 0)
            {
                Destroy(spawnPoints.transform.GetChild(index).gameObject.transform.GetChild(0).gameObject);
            }
            GameObject player = Instantiate(instantiateCharacter, spawnPoints.transform.GetChild(index).GetComponent<Transform>());
            foreach (int top3Item in APICall.instance.top3)
            {
                if (APICall.instance.quickPlayWinner == item)
                {
                    player.GetComponent<PlayerRacing>().speed = number1Speed;
                    player.GetComponent<PlayerRacing>().tokenId = item;
                } 
                else if (APICall.instance.top3[1] == item)
                {
                    player.GetComponent<PlayerRacing>().speed = number2Speed;
                    player.GetComponent<PlayerRacing>().tokenId = item;
                }
                else if (APICall.instance.top3[2] == item)
                {
                    player.GetComponent<PlayerRacing>().speed = number3Speed;
                    player.GetComponent<PlayerRacing>().tokenId = item;
                }
                else
                {
                    player.GetComponent<PlayerRacing>().speed = othersSpeed;
                    player.GetComponent<PlayerRacing>().tokenId = item;
                }
            }
            index++;
        }
    }
}
