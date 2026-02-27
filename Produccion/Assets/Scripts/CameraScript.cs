using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform player;
    void Start()
    {
        player = GameManager.instance.player.GetComponent<Transform>();
    }
}
