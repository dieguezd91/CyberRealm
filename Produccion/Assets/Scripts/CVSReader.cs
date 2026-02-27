using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CVSReader : MonoBehaviour
{
    public TextAsset textAssetData;

    [System.Serializable]

    public class Enemy
    {
        [SerializeField] private string enemigo;        
        [SerializeField] private int strength;
        [SerializeField] private int dexterity;
        [SerializeField] private int defence;
        [SerializeField] private float critical;
        [SerializeField] private float evasion;

        public string Enemigo { get => enemigo; set => enemigo = value; }
        public int Strength { get => strength; set => strength = value; }
        public int Dexterity { get => dexterity; set => dexterity = value; }
        public int Defence { get => defence; set => defence = value; }
        public float Critical { get => critical; set => critical = value; }
        public float Evasion { get => evasion; set => evasion = value; }
    }

    [System.Serializable]

    public class EnemyList
    {
        public Enemy[] enemy;
    }

    public EnemyList enemyList = new EnemyList();

    void Start()
    {
        ReadCSV();
    }

    void ReadCSV()
    {
        string[] data = textAssetData.text.Split(new string[] { ",", "\n" }, StringSplitOptions.None);

        int tableSize = data.Length / 6 - 1;

        enemyList.enemy = new Enemy[tableSize];

        for(int i = 0; i < tableSize; i++)
        {
            enemyList.enemy[i] = new Enemy();
            enemyList.enemy[i].Enemigo = data[6 * (i + 1)];
            enemyList.enemy[i].Strength = int.Parse(data[6 * (i + 1) + 1]);
            enemyList.enemy[i].Dexterity = int.Parse(data[6 * (i + 2) + 1]);
            enemyList.enemy[i].Defence = int.Parse(data[6 * (i + 3) + 1]);
            enemyList.enemy[i].Critical = float.Parse(data[6 * (i + 4) + 1]);
            enemyList.enemy[i].Evasion = float.Parse(data[6 * (i + 5) + 1]);
        }
    }
}
