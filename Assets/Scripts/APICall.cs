using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;
using System;

[System.Serializable]
public class Data
{
    public int id;
    public string sender;
    public string receiver;
    public string value;

    public static Data CreateFromJson(string jsonString) {
        return JsonUtility.FromJson<Data>(jsonString);
    }
}

[System.Serializable]
public class Data2
{
    public int[] queue;

    public static Data2 CreateFromJson(string jsonString) {
        return JsonUtility.FromJson<Data2>(jsonString);
    }
}

[System.Serializable]
public class DataFightWinner
{
    public int id;
    public int raptor_fight_winner;

    public static DataFightWinner CreateFromJson(string jsonString)
    {
        return JsonUtility.FromJson<DataFightWinner>(jsonString);
    }
}

[System.Serializable]
public class DataFighters
{
    public int id;
    public int[] raptor_fighters;

    public static DataFighters CreateFromJson(string jsonString)
    {
        return JsonUtility.FromJson<DataFighters>(jsonString);
    }
}

[System.Serializable]
public class DataTop3
{
    public int id;
    public int[] raptor_top_3;

    public static DataTop3 CreateFromJson(string jsonString)
    {
        return JsonUtility.FromJson<DataTop3>(jsonString);
    }
}

[System.Serializable]
public class DataRaceWinner
{
    public int id;
    public int qp_winner;

    public static DataRaceWinner CreateFromJson(string jsonString)
    {
        return JsonUtility.FromJson<DataRaceWinner>(jsonString);
    }
}

public class APICall : MonoBehaviour
{
    public static APICall instance;
    InputField outputArea;
    Data data;
    int[] result = new int[8];
    [SerializeField] GameObject raptor1 = null;
    [SerializeField] GameObject raptor2 = null;
    GameObject spawnPoints = null;
    public int[] raptorsInPlay = new int[8];
    public int injuredRaptor;
    public int fightWinner;
    public int[] fighters = new int[2];
    public int[] top3 = new int[3];
    public int quickPlayWinner;
    public int competitiveWinner;
    public int deathRaceWinner;
    public int ripRaptor;
    private int[] currentQueue = new int[8];

    void Awake() {
        instance = this;
        spawnPoints = GameObject.FindWithTag("SpawnPoint");
    }

    // Start is called before the first frame update
    void Start()
    {
        GetData();
        DontDestroyOnLoad(gameObject);
    }

    void GetData() {
        StartCoroutine(GetData_Coroutine());
    } 

    IEnumerator GetData_Coroutine() {
        while (true) {
            raptorsInPlay = new int[8];
            string uri = "http://test-raptor-nft-game.herokuapp.com/test/getCurrentQueue";
            using(UnityWebRequest request = UnityWebRequest.Get(uri)) 
            {
                yield return request.SendWebRequest();
                if (request.isNetworkError || request.isHttpError) {
                    print(request.error);
                } else {
                    var DataResponseObj = Data2.CreateFromJson(request.downloadHandler.text);
                    result = DataResponseObj.queue;
                    int[] value = Array.FindAll(result, element => element != 0);
                    if (value.Length != 0)
                    {
                        currentQueue = result;
                    }
                    int index = 0;
                    int count = 0;
                    foreach (int item in currentQueue)
                    {
                        if (item == 0) {
                            if (spawnPoints.transform.GetChild(index).gameObject.transform.childCount > 0) {
                                Destroy(spawnPoints.transform.GetChild(index).gameObject.transform.GetChild(0).gameObject);
                            }
                        } else {
                            count++;
                            GameObject instantiateCharacter = item == 1 ? raptor1 : raptor2;
                            if (spawnPoints.transform.GetChild(index).gameObject.transform.childCount > 0) {
                                Destroy(spawnPoints.transform.GetChild(index).gameObject.transform.GetChild(0).gameObject);
                            }
                            var player = Instantiate(instantiateCharacter, spawnPoints.transform.GetChild(index).GetComponent<Transform>());
                        }
                        raptorsInPlay[index] = item;
                        index++;
                    }
                    if (count == 8) {
                        string fightWinnerUri = "http://test-raptor-nft-game.herokuapp.com/test/getFightWinner";
                        using (UnityWebRequest fightWinnerRequest = UnityWebRequest.Get(fightWinnerUri))
                        {
                            yield return fightWinnerRequest.SendWebRequest();
                            if (fightWinnerRequest.isNetworkError || fightWinnerRequest.isHttpError)
                            {
                                print(fightWinnerRequest.error);
                            }
                            else
                            {
                                var fightWinnerResponse = DataFightWinner.CreateFromJson(fightWinnerRequest.downloadHandler.text);
                                if (fightWinnerResponse.raptor_fight_winner != 0)
                                {
                                    yield return new WaitForSecondsRealtime(5);
                                    string fightersUri = "https://test-raptor-nft-game.herokuapp.com/test/getFighters";
                                    using (UnityWebRequest fightersRequest = UnityWebRequest.Get(fightersUri))
                                    {
                                        yield return fightersRequest.SendWebRequest();
                                        if (fightersRequest.isNetworkError || fightersRequest.isHttpError)
                                        {
                                            print(fightersRequest.error);
                                        }
                                        else
                                        {
                                            var fightersResponse = DataFighters.CreateFromJson(fightersRequest.downloadHandler.text);
                                            fighters[0] = fightersResponse.raptor_fighters[0];
                                            fighters[1] = fightersResponse.raptor_fighters[1];
                                            fightWinner = fightWinnerResponse.raptor_fight_winner;
                                            string top3Uri = "https://test-raptor-nft-game.herokuapp.com/test/getTop3";
                                            using (UnityWebRequest top3Request = UnityWebRequest.Get(top3Uri))
                                            {
                                                yield return top3Request.SendWebRequest();
                                                if (top3Request.isNetworkError || top3Request.isHttpError)
                                                {
                                                    print(top3Request.error);
                                                }
                                                else
                                                {
                                                    var top3Response = DataTop3.CreateFromJson(top3Request.downloadHandler.text);
                                                    top3[0] = top3Response.raptor_top_3[0];
                                                    top3[1] = top3Response.raptor_top_3[1];
                                                    top3[2] = top3Response.raptor_top_3[2];
                                                    string raceWinnerUri = "https://test-raptor-nft-game.herokuapp.com/test/getQPWinner";
                                                    using (UnityWebRequest raceWinnerRequest = UnityWebRequest.Get(raceWinnerUri))
                                                    {
                                                        yield return raceWinnerRequest.SendWebRequest();
                                                        if (raceWinnerRequest.isNetworkError || raceWinnerRequest.isHttpError)
                                                        {
                                                            print(raceWinnerRequest.error);
                                                        }
                                                        else
                                                        {
                                                            var raceWinnerResponse = DataRaceWinner.CreateFromJson(raceWinnerRequest.downloadHandler.text);
                                                            quickPlayWinner = raceWinnerResponse.qp_winner;
                                                            print("???");
                                                            yield return SceneManager.LoadSceneAsync(1);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            yield return new WaitForSecondsRealtime(30);
        }
    }
}
