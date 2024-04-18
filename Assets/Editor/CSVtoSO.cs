using UnityEngine;
using UnityEditor;
using System.IO;

public class CSVtoSO
{

    [MenuItem("Utilities/Generate Equips")]
    public static void GenerateEquips()
    {
        string CSVPath = "/1-RawData/EquipData.csv";
        string[] allLines = File.ReadAllLines(Application.dataPath + CSVPath);

        foreach(string allLine in allLines)
        {
            string [] splitData = allLine.Split(',');

            if(splitData.Length != 4)
            {
                Debug.Log(allLine + " Does not have 4 values");
            }

            EquipSO equip = ScriptableObject.CreateInstance<EquipSO>();
            equip.id = int.Parse(splitData[0]);
            equip.equipName = splitData[1];
            equip.star = int.Parse(splitData[3]);

            for(int i = 4; i < 7; i++)
            {
                if(!splitData[i].Equals("0"))
                {
                    string [] keypair = splitData[i].Split('(');
                    splitData[i] = keypair[0];
                }
            }


            equip.hp = int.Parse(splitData[4]);
            equip.atk = int.Parse(splitData[5]);
            equip.def = int.Parse(splitData[6]);
            equip.upgradeLevel = int.Parse(splitData[7]);

            for(int i =  8; i < 12; i++)
            {
                if(!splitData[i].Equals("-"))
                {
                    string [] keypair = splitData[i].Split('x');
                    equip.upgradeMaterials.Add(int.Parse(keypair[0]), int.Parse(keypair[1]));
                }
            }

            equip.gold = int.Parse(splitData[12]);

            AssetDatabase.CreateAsset(equip, $"Assets/0-AddressableResources/ScriptableObjects/EquipSO/EquipSO_{equip.id}.asset");
        }

        AssetDatabase.SaveAssets();
    }

    [MenuItem("Utilities/Generate Items")]
    public static void GenerateItems()
    {
        string CSVPath = "/1-RawData/ItemData.csv";
        string[] allLines = File.ReadAllLines(Application.dataPath + CSVPath);

        foreach(string allLine in allLines)
        {
            string [] splitData = allLine.Split(',');

            if(splitData.Length != 2)
            {
                Debug.Log(allLine + " Does not have 4 values");
            }

            RankUp_equipSO item = ScriptableObject.CreateInstance<RankUp_equipSO>();
            item.id = int.Parse(splitData[0]);
            item.itemName = splitData[1];

            AssetDatabase.CreateAsset(item, $"Assets/0-AddressableResources/ScriptableObjects/ItemSO/ItemSO_{item.id}.asset");
        }

        AssetDatabase.SaveAssets();
    }
}