using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;
using System;
using System.Linq;
using System.Runtime.InteropServices;

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

[System.Serializable]
public class RaptorJsonData
{
    [JsonProperty("Token ID")]
    public string TokenID;
    public string description;
    public string image;
    public string external_url;
    public string name;
    public List<Attribute> attributes;

    public static RaptorJsonData CreateFromJson(string jsonString)
    {
        return JsonUtility.FromJson<RaptorJsonData>(jsonString);
    }
}

[System.Serializable]
public class Attribute
{
    public string trait_type;
    public string value;
}

public static class ArrayExt
{
    public static T[] GetRow<T>(this T[,] array, int row)
    {
        if (!typeof(T).IsPrimitive)
            throw new InvalidOperationException("Not supported for managed types.");

        if (array == null)
            throw new ArgumentNullException("array");

        int cols = array.GetUpperBound(1) + 1;
        T[] result = new T[cols];

        int size;

        if (typeof(T) == typeof(bool))
            size = 1;
        else if (typeof(T) == typeof(char))
            size = 2;
        else
            size = Marshal.SizeOf<T>();

        Buffer.BlockCopy(array, row * cols * size, result, 0, cols * size);

        return result;
    }
}


public class APICall : MonoBehaviour
{
    public static APICall instance;
    InputField outputArea;
    Data data;
    int[] result = new int[8];
    [SerializeField] GameObject raptor0 = null;
    [SerializeField] GameObject raptor1 = null;
    [SerializeField] GameObject raptor2 = null;
    [SerializeField] GameObject raptor3 = null;
    [SerializeField] GameObject raptor4 = null;
    [SerializeField] GameObject raptor5 = null;
    [SerializeField] GameObject raptor6 = null;
    [SerializeField] GameObject raptor7 = null;
    [SerializeField] GameObject raptor8 = null;
    [SerializeField] GameObject raptor9 = null;
    [SerializeField] RuntimeAnimatorController runtimeControllerRaptor0 = null;
    [SerializeField] RuntimeAnimatorController raptorSprite1 = null;
    [SerializeField] RuntimeAnimatorController raptorSprite2 = null;
    [SerializeField] RuntimeAnimatorController raptorSprite3 = null;
    [SerializeField] RuntimeAnimatorController raptorSprite4 = null;
    [SerializeField] RuntimeAnimatorController raptorSprite5 = null;
    [SerializeField] RuntimeAnimatorController raptorSprite6 = null;
    [SerializeField] RuntimeAnimatorController raptorSprite7 = null;
    [SerializeField] RuntimeAnimatorController raptorSprite8 = null;
    [SerializeField] RuntimeAnimatorController raptorSprite9 = null;
    GameObject spawnPoints = null;
    public int[] raptorsInPlay = new int[8];
    public int injuredRaptor;
    public int fightWinner;
    public int[] fighters = new int[2];
    public int[] top3 = new int[3];
    public int[] theRestRacer = new int[5];
    public int quickPlayWinner;
    public int competitiveWinner;
    public int deathRaceWinner;
    public int ripRaptor;
    public bool hasStarted = false;
    private int[] currentQueue = new int[8];
    private GameObject[] currentGameObjectRaptors = new GameObject[8];
    public Dictionary<int, RuntimeAnimatorController> tokenIdToSprite = new Dictionary<int, RuntimeAnimatorController>();

    public double GetAverage(int[] arr)
    {
        double average = 0.0;
        for (int i = 0; i < arr.Length; i++)
        {
            average += arr[i];
        }
        return average / arr.Length;
    }

    public int GetWeight(double[] arr, int position, int index)
    {
        int below = 0;
        int above = 0;

        for (int i = 0; i < arr.Length; i++)
        {
            if (i == index)
            {
                continue;
            }
            else if (arr[i] < arr[index])
            {
                below += 1;
            }
            else if (arr[i] > arr[index])
            {
                above += 1;
            }
        }

        if (position < arr.Length - below)
        {
            return 1; //faster speed
        }
        else
        {
            return -1; //slower speed
        }
    }

    public GameObject GetRaptorGameObject(string value)
    {
        switch (value)
        {
            case "Bolt":
                return raptor0;
            case "Scaled":
                return raptor1;
            case "Undead":
                return raptor2;
            case "Flame":
                return raptor3;
            case "Marble":
                return raptor2;
            case "Blue":
                return raptor5;
            case "Slimy":
                return raptor6;
            case "Jigsaw":
                return raptor7;
            case "Rhodnite":
                return raptor8;
            case "Stripe":
                return raptor9;
            default:
                return raptor0;
        }
    }

    public RuntimeAnimatorController GetRaptorSprite(string value)
    {
        switch (value)
        {
            case "Bolt":
                return runtimeControllerRaptor0;
            case "Scaled":
                return raptorSprite1;
            case "Undead":
                return raptorSprite2;
            case "Flame":
                return raptorSprite3;
            case "Marble":
                return raptorSprite2;
            case "Blue":
                return raptorSprite5;
            case "Slimy":
                return raptorSprite6;
            case "Jigsaw":
                return raptorSprite7;
            case "Rhodnite":
                return raptorSprite8;
            case "Stripe":
                return raptorSprite9;
            default:
                return runtimeControllerRaptor0;
        }
    }

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
        while (!hasStarted) {
            raptorsInPlay = new int[8];
            string uri = "https://test-raptor-nft-game.herokuapp.com/test/getCurrentQueue";
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
                            string raptorSpriteUri = $"https://mrhemp.mypinata.cloud/ipfs/QmQvgt2BgLTYMVXdU8rWXaqi4jTY4CNczsxZQzboLKuxwq/{item.ToString()}.JSON";
                            GameObject instantiateCharacter = null;
                            using (UnityWebRequest raptorSpriteRequest = UnityWebRequest.Get(raptorSpriteUri))
                            {
                                yield return raptorSpriteRequest.SendWebRequest();
                                if (raptorSpriteRequest.isNetworkError || raptorSpriteRequest.isHttpError)
                                {
                                    print(raptorSpriteRequest.error);
                                }
                                else
                                {
                                    RaptorJsonData DataResponse = RaptorJsonData.CreateFromJson(raptorSpriteRequest.downloadHandler.text);
                                    foreach (Attribute attribute in DataResponse.attributes)
                                    {
                                        if (attribute.trait_type == "Body")
                                        {
                                            currentGameObjectRaptors[index] = GetRaptorGameObject(attribute.value);
                                            tokenIdToSprite[item] = GetRaptorSprite(attribute.value);
                                        }
                                    }
                                }
                            }
                            raptorsInPlay[index] = item;
                            index++;
                        }
                    }
                    int spawnPointIndex = 0;
                    foreach (GameObject raptorGameObject in currentGameObjectRaptors)
                    {
                        if (spawnPoints != null)
                        {
                            if (raptorGameObject != null)
                            {
                                if (spawnPoints.transform.GetChild(spawnPointIndex).gameObject.transform.childCount > 0)
                                {
                                    Destroy(spawnPoints.transform.GetChild(spawnPointIndex).gameObject.transform.GetChild(0).gameObject);
                                }

                                Instantiate(raptorGameObject, spawnPoints.transform.GetChild(spawnPointIndex).transform);
                                spawnPointIndex++;
                            }
                        }
                    }
                    if (count == 8) {
                        string fightWinnerUri = "https://test-raptor-nft-game.herokuapp.com/test/getFightWinner";
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
                                                    int[] raptorsInPlayTemp = raptorsInPlay;
                                                    int restRacerIndex = 0;
                                                    int[] endingPositions = new int[8];
                                                    foreach (int raptor in raptorsInPlayTemp)
                                                    {
                                                        if (!top3.Contains(raptor))
                                                        {
                                                            theRestRacer[restRacerIndex] = raptor;
                                                            restRacerIndex++;
                                                        }
                                                    }
                                                    int[] mergeArray = top3.Concat(theRestRacer).ToArray();

                                                    //for (int i = 0; i < currentQueue.Length; i++)
                                                    //{
                                                    //    if (currentQueue[i] == mergeArray[0])
                                                    //    {
                                                    //        endingPositions[i] = 1;
                                                    //    }
                                                    //    else if (currentQueue[i] == mergeArray[1])
                                                    //    {
                                                    //        endingPositions[i] = 2;
                                                    //    }
                                                    //    else if (currentQueue[i] == mergeArray[2])
                                                    //    {
                                                    //        endingPositions[i] = 3;
                                                    //    }
                                                    //    else if (currentQueue[i] == mergeArray[3])
                                                    //    {
                                                    //        endingPositions[i] = 4;
                                                    //    }
                                                    //    else if (currentQueue[i] == mergeArray[4])
                                                    //    {
                                                    //        endingPositions[i] = 5;
                                                    //    }
                                                    //    else if (currentQueue[i] == mergeArray[5])
                                                    //    {
                                                    //        endingPositions[i] = 6;
                                                    //    }
                                                    //    else if (currentQueue[i] == mergeArray[6])
                                                    //    {
                                                    //        endingPositions[i] = 7;
                                                    //    }
                                                    //    else if (currentQueue[i] == mergeArray[7])
                                                    //    {
                                                    //        endingPositions[i] = 8;
                                                    //    }
                                                    //}

                                                    //for (int j = 0; j < endingPositions.Length; j++)
                                                    //{
                                                    //    if (currentQueue[j] == fighters[0])
                                                    //    {
                                                    //        endingPositions[6] = endingPositions[j];
                                                    //        endingPositions[j] = 7;
                                                    //    }
                                                    //    if (currentQueue[j] == fighters[1])
                                                    //    {
                                                    //        endingPositions[7] = endingPositions[j];
                                                    //        endingPositions[j] = 8;
                                                    //    }
                                                    //}
                                                    //2 dimentsional array 5 arrays, 8 values per array
                                                    //int[,] speed = new int[6, 8];

                                                    ////The upper random bound
                                                    //int upperBound = 10;

                                                    ////The lower random bound
                                                    //int lowerBound = 3;

                                                    ////Temporary array for storing data before moving into final array
                                                    //int[] tempArr = new int[8];

                                                    ////Array for holding the average speed for each raptor at a given speed change interval
                                                    //double[,] averageSpeedArr = new double[6, 8];


                                                    ////Iterate 5 times for 5 speed changes
                                                    //for (int j = 0; j < 5; j++)
                                                    //{

                                                    //    //Iterate through the raptors
                                                    //    for (int i = 0; i < endingPositions.Length; i++)
                                                    //    {

                                                    //        //First 2 speed changes are random
                                                    //        if (j < 2)
                                                    //        {

                                                    //            //Get random
                                                    //            System.Random rnd = new System.Random();
                                                    //            int tempSpeed = rnd.Next(lowerBound, upperBound);

                                                    //            //Assign to temp array
                                                    //            tempArr[i] = tempSpeed;
                                                    //        }
                                                    //        else
                                                    //        {
                                                    //            //Temp array
                                                    //            int[] arr = new int[8];

                                                    //            //get an array fo this raptors speeds so far
                                                    //            for (int x = 0; x <= j; x++)
                                                    //            {
                                                    //                arr[x] = speed[x, i];
                                                    //            }

                                                    //            //find the average
                                                    //            double average = GetAverage(arr);

                                                    //            // assign the average to the average speed array for a given raptor
                                                    //            averageSpeedArr[j, i] = average;

                                                    //        }
                                                    //    }

                                                        //If we are past the first 2 speed changes
                                                        //if (j >= 2)
                                                        //{

                                                        //    //Iterate through raptors
                                                        //    for (int y = 0; y < speed.Length; y++)
                                                        //    {

                                                        //        //Find whether the raptor needs to go faster or slower to reach the given finishing positions
                                                        //        int weight = GetWeight(averageSpeedArr.GetRow(j), endingPositions[y], index);

                                                        //        //If raptor go fast
                                                        //        if (weight > 0)
                                                        //        {
                                                        //            //increase average speed by 20% for next speed
                                                        //            speed[j, y] = (int)(averageSpeedArr[j, y] * 1.2);
                                                        //        }
                                                        //        else
                                                        //        {
                                                        //            //decrease average speed by 20%
                                                        //            speed[j, y] = (int)(averageSpeedArr[j, y] * 0.8);
                                                        //        }

                                                        //    }

                                                        //    //If still on first 2 speed changes
                                                        //}
                                                        //else
                                                        //{
                                                        //    //assign speed
                                                        //    for (int i = 0; i < tempArr.Length; i++)
                                                        //    {
                                                        //        speed[j, i] = tempArr[i];
                                                        //    }
                                                        //}

                                                    }

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
                                                            hasStarted = true;
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

