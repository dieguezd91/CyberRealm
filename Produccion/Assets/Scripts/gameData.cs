using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class GameData
{
    [SerializeField] private Vector3 position;
    [SerializeField] private int lifePoints;
    [SerializeField] private int dexterity;
    [SerializeField] private int strength;
    [SerializeField] private int defence;
    [SerializeField] private int xp;
    [SerializeField] private int level;
    [SerializeField] private int credits;
    [SerializeField] private string scene;
    [SerializeField] private bool[] completedQuests;

    public Vector3 Position { get => position; set => position = value; }
    public int LifePoints { get => lifePoints; set => lifePoints = value; }
    public int Dexterity { get => dexterity; set => dexterity = value; }
    public int Strength { get => strength; set => strength = value; }
    public int Defence { get => defence; set => defence = value; }
    public int Xp { get => xp; set => xp = value; }
    public int Level { get => level; set => level = value; }
    public int Credits { get => credits; set => credits = value; }
    public string Scene { get => scene; set => scene = value; }
    public bool[] CompletedQuests { get => completedQuests; set => completedQuests = value; }
}
