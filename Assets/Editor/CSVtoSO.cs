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

    [MenuItem("Utilities/Generate Characters")]
    public static void GenerateCharacters()
    {
        string CSVPath = "/1-RawData/CharacterData.csv";
        string[] allLines = File.ReadAllLines(Application.dataPath + CSVPath);

        foreach(string allLine in allLines)
        {
            string [] splitData = allLine.Split(',');

            if(splitData.Length != 20)
            {
                Debug.Log(allLine + " Does not have 20 values");
            }

            CharacterSO character = ScriptableObject.CreateInstance<CharacterSO>();

            character.id = int.Parse(splitData[0]);
            character.characterName = splitData[1];

            if(splitData[2].Equals("근거리"))
            {
                character.attackMethod = Constants.AttackMethod.Melee;
            }
            else
            {
                character.attackMethod = Constants.AttackMethod.Range;
            }

            character.hp = int.Parse(splitData[4]);
            character.atk = int.Parse(splitData[5]);
            character.def = int.Parse(splitData[6]);
            character.mov = int.Parse(splitData[7]);
            character.range = int.Parse(splitData[8]);
            character.hpPerLv = int.Parse(splitData[9]);
            character.atkPerLv = int.Parse(splitData[10]);
            character.defPerLv = int.Parse(splitData[11]);

            character.abilityT1 = int.Parse(splitData[13]);

            string[] pair;

            pair = splitData[14].Split('~');
            Debug.Log(pair[0]);
            character.abilityT2.Add(int.Parse(pair[0]));
            character.abilityT2.Add(int.Parse(pair[1]));

            pair = splitData[15].Split('~');
            character.abilityT3.Add(int.Parse(pair[0]));
            character.abilityT3.Add(int.Parse(pair[1]));

            character.basicClass = int.Parse(splitData[16]);

            pair = splitData[17].Split('~');
            character.superiorClass.Add(int.Parse(pair[0]));
            character.superiorClass.Add(int.Parse(pair[1]));

            pair = splitData[18].Split('~');
            for (int i = int.Parse(pair[0]); i <= int.Parse(pair[1]); i++)
            {
                character.weapon.Add(i);
            }

            pair = splitData[19].Split('~');
            for (int i = int.Parse(pair[0]); i <= int.Parse(pair[1]); i++)
            {
                character.armor.Add(i);
            }

            AssetDatabase.CreateAsset(character, $"Assets/0-AddressableResources/ScriptableObjects/CharacterSO/CharacterSO_{character.id}.asset");
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

    [MenuItem("Utilities/Generate Class")]
    public static void GenerateClass()
    {
        string CSVPath = "/1-RawData/ClassData.csv";
        string[] allLines = File.ReadAllLines(Application.dataPath + CSVPath);

        foreach(string allLine in allLines)
        {
            string [] splitData = allLine.Split(',');

            if(splitData.Length != 3)
            {
                Debug.Log(allLine + " Does not have 3 values");
            }

            ClassSO classSO = ScriptableObject.CreateInstance<ClassSO>();

            classSO.id = int.Parse(splitData[0]);
            classSO.className = splitData[1];
            classSO.baseClass = (Constants.BaseClass)int.Parse(splitData[2]);

            AssetDatabase.CreateAsset(classSO, $"Assets/0-AddressableResources/ScriptableObjects/ClassSO/ClassSO_{classSO.id}.asset");
        }

        AssetDatabase.SaveAssets();
    }

    [MenuItem("Utilities/Generate Ability")]
    public static void GenerateAbility()
    {
        string CSVPath = "/1-RawData/AbilityData.csv";
        string[] allLines = File.ReadAllLines(Application.dataPath + CSVPath);

        foreach(string allLine in allLines)
        {
            string [] splitData = allLine.Split(',');

            if(splitData.Length != 3)
            {
                Debug.Log(allLine + " Does not have 3 values");
            }

            AbilitySO ability = ScriptableObject.CreateInstance<AbilitySO>();

            ability.id = int.Parse(splitData[0]);
            ability.abilityName = splitData[1];

            AssetDatabase.CreateAsset(ability, $"Assets/0-AddressableResources/ScriptableObjects/AbilitySO/AbilitySO_{ability.id}.asset");
        }

        AssetDatabase.SaveAssets();
    }

    [MenuItem("Utilities/Generate PassiveSkill")]
    public static void GeneratePassiveSkill()
    {
        string CSVPath = "/1-RawData/PassiveSkillData.csv";
        string[] allLines = File.ReadAllLines(Application.dataPath + CSVPath);

        foreach(string allLine in allLines)
        {
            string [] splitData = allLine.Split(',');

            if(splitData.Length != 2)
            {
                Debug.Log(allLine + " Does not have 2 values");
            }

            PassiveSkillSO passive = ScriptableObject.CreateInstance<PassiveSkillSO>();

            passive.id = int.Parse(splitData[0]);
            passive.passiveName = splitData[1];

            AssetDatabase.CreateAsset(passive, $"Assets/0-AddressableResources/ScriptableObjects/PassiveSkillSO/PassiveSkillSO_{passive.id}.asset");
        }

        AssetDatabase.SaveAssets();
    }

    [MenuItem("Utilities/Generate ExSkill")]
    public static void GenerateExSkill()
    {
        string CSVPath = "/1-RawData/ExSkillData.csv";
        string[] allLines = File.ReadAllLines(Application.dataPath + CSVPath);

        foreach(string allLine in allLines)
        {
            string [] splitData = allLine.Split(',');

            if(splitData.Length != 2)
            {
                Debug.Log(allLine + " Does not have 2 values");
            }

            ExSkillSO ex = ScriptableObject.CreateInstance<ExSkillSO>();

            ex.skill_ID = int.Parse(splitData[0]);
            ex.skillName = splitData[1];

            AssetDatabase.CreateAsset(ex, $"Assets/0-AddressableResources/ScriptableObjects/ExSkillSO/ExSkillSO_{ex.skill_ID}.asset");
        }

        AssetDatabase.SaveAssets();
    }
}