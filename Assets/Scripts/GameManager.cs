using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject spawnPoints;
    public GameObject raptor1;
    public GameObject raptor2;
    public GameObject raptor3;
    public GameObject raptor4;
    public GameObject raptor5;
    public GameObject raptor6;
    public GameObject raptor7;
    public GameObject raptor8;
    private float number1MaxSpeed = 1.2f;
    private float number1MinSpeed = 1.05f;
    private float number2MaxSpeed = 1.1f;
    private float number2MinSpeed = 0.9f;
    private float number3MaxSpeed = 1.05f;
    private float number3MinSpeed = 0.87f;
    private float othersMaxSpeed = 1.03f;
    private float othersMinSpeed = 0.84f;
    private float[] number1Speeds = { 0.94f, 1f, 1.05f, 1.06f, 1.05f, 1.04f, 0.97f, 1.03f };
    private float[] number2Speeds = { 1.05f, 0.98f, 0.95f, 1.1f, 0.9f, 0.95f, 0.97f, 1.2f };
    private float[] number3Speeds = { 1.03f, 1.05f, 0.95f, 0.98f, 1.1f, 1.04f, 0.97f, 1.03f };
    private float[] otherSpeeds = { 1.03f, 1.05f, 0.95f, 0.98f, 1.1f, 1.04f, 0.97f, 1.03f };

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
            GameObject player = Instantiate(instantiateCharacter, spawnPoints.transform.GetChild(index).transform);
            foreach (int top3Item in APICall.instance.top3)
            {
                if (APICall.instance.quickPlayWinner == item)
                {
                    player.GetComponent<PlayerRacing>().speeds = number1Speeds;
                    player.GetComponent<PlayerRacing>().tokenId = item;
                } 
                else if (APICall.instance.top3[1] == item)
                {
                    player.GetComponent<PlayerRacing>().speeds = number2Speeds;
                    player.GetComponent<PlayerRacing>().tokenId = item;
                }
                else if (APICall.instance.top3[2] == item)
                {
                    player.GetComponent<PlayerRacing>().speeds = number3Speeds;
                    player.GetComponent<PlayerRacing>().tokenId = item;
                }
                else
                {
                    player.GetComponent<PlayerRacing>().speeds = otherSpeeds;
                    player.GetComponent<PlayerRacing>().tokenId = item;
                }
            }
            index++;
        }
    }
}
