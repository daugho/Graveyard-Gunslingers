using System.Collections.Generic;
using UnityEngine;

public class RoundData
{
    public int RoundNumber;
    public int Zombie1Count;
    public int Zombie2Count;
    public int Zombie3Count;
    public int Zombie4Count;
    public int Skull1Count;
    public int Skull2Count;
    public int Skull3Count;
    public int Skull4Count;
}
public class SpawnDataManager : MonoBehaviour
{
    private static SpawnDataManager _instance;
    public static SpawnDataManager Instance => _instance ??= new GameObject("SpawnDataManager").AddComponent<SpawnDataManager>();

    private Dictionary<int, RoundData> AllRoundData = new();
    public void LoadSpawnData()
    {
        TextAsset csv = Resources.Load<TextAsset>("Data/MonsterData/SponData");

        if (csv == null)
        {
            Debug.LogError("[SpawnDataManager] CSV 파일을 찾을 수 없습니다!");
            return;
        }

        string[] lines = csv.text.Split(new[] { "\r\n", "\n" }, System.StringSplitOptions.RemoveEmptyEntries);

        for (int i = 1; i < lines.Length; i++)
        {
            string[] values = lines[i].Split(',');

            RoundData roundData = new RoundData
            {
                RoundNumber = int.Parse(values[0]),
                Zombie1Count = int.Parse(values[1]),
                Zombie2Count = int.Parse(values[2]),
                Zombie3Count = int.Parse(values[3]),
                Zombie4Count = int.Parse(values[4]),
                Skull1Count = int.Parse(values[5]),
                Skull2Count = int.Parse(values[6]),
                Skull3Count = int.Parse(values[7]),
                Skull4Count = int.Parse(values[8]),
            };

            AllRoundData[roundData.RoundNumber] = roundData;
        }

        Debug.Log($"[SpawnDataManager] {AllRoundData.Count}개의 라운드 데이터를 로드했습니다.");
    }
}
