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
        public string enemigo;        
        public int  strength;
        public int dexterity;
        public int defence;
        public float critical;
        public float evasion;
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
            enemyList.enemy[i].enemigo = data[6 * (i + 1)];
            enemyList.enemy[i].strength = int.Parse(data[6 * (i + 1) + 1]);
            enemyList.enemy[i].dexterity = int.Parse(data[6 * (i + 2) + 1]);
            enemyList.enemy[i].defence = int.Parse(data[6 * (i + 3) + 1]);
            enemyList.enemy[i].critical = float.Parse(data[6 * (i + 4) + 1]);
            enemyList.enemy[i].evasion = float.Parse(data[6 * (i + 5) + 1]);
        }
    }
}
