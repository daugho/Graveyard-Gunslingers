using System.Collections.Generic;
using UnityEngine;

public enum ItemGrade
{
    Normal,
    Rare,
    Epic,
    Legendary
}

public enum ItemType
{
    Gold,
    HpPotion,
    MpPotion,
    Box
}

public class ItemDropData
{
    public int Gold;
    public int Exp;
    public int HpPotion;
    public int MpPotion;
    public int Box;
    public ItemGrade Grade; // 등급 정보 추가
}

public class DropTable
{
    private static DropTable _instance;
    public static DropTable Instance => _instance ??= new DropTable();

    private Dictionary<int, ItemDropData> _dropItem = new();

    private DropTable()
    {
        LoadDropTable();
    }
    private ItemGrade GetGradeByKey(int key)
    {
        return key switch
        {
            2001 => ItemGrade.Normal,
            2002 => ItemGrade.Rare,
            2003 => ItemGrade.Epic,
            2004 => ItemGrade.Legendary,
            _ => ItemGrade.Normal
        };
    }
    private void LoadDropTable()
    {
        TextAsset csv = Resources.Load<TextAsset>("Data/MonsterData/ZombieItem");
        if (csv == null)
        {
            Debug.LogError("DropTable CSV 파일이 없습니다.");
            return;
        }
        string[] lines = csv.text.Split(new[] { "\r\n", "\n" }, System.StringSplitOptions.RemoveEmptyEntries);
        for (int i = 1; i < lines.Length; i++)
        {
            string[] tokens = lines[i].Split(',');

            int keys = int.Parse(tokens[0]); // 첫 번째 열이 key (RateKey)

            var data = new ItemDropData
            {
                Gold = int.Parse(tokens[1]),
                Exp = int.Parse(tokens[2]),
                HpPotion = int.Parse(tokens[3]),
                MpPotion = int.Parse(tokens[4]),
                Box = int.Parse(tokens[5]),
                Grade = GetGradeByKey(keys)
            };

            _dropItem[keys] = data;
        }
        Debug.Log("[DropTable] 드랍 테이블 데이터 로드 완료");
    }

    public ItemDropData GetItem(int monsterkey)
    {
        return _dropItem.TryGetValue(monsterkey, out var reward) ? reward : null;
    }
}
