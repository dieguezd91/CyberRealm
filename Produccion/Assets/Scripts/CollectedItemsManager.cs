using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectedItemsManager : MonoBehaviour
{
    public static CollectedItemsManager instance;
    [SerializeField] GameObject[] items;
    public string[] scene;
    public bool[] collected;

    private void Awake()
    {
        if (instance != null && instance != this) Destroy(gameObject);
        else instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        Debug.Log("Start");
        items = GameObject.FindGameObjectsWithTag("Item");
        collected = new bool[items.Length];
    }

    private void Update()
    {
        for (int i = 0; i < collected.Length; i++)
        {
            if (items[i] != null)
            {
                if (collected[i]) items[i].SetActive(false);
                else items[i].SetActive(true);
            }
        }
    }

    public int GetItemNumber(GameObject item)
    {
        for(int i = 0; i < items.Length; i++)
        {
            if (items[i] == item) return i;
        }

        Debug.LogWarning(item.name + " does not exist");
        return 0;
    }
}
