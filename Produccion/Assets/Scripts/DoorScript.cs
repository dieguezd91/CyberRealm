using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public string place;
    public bool pjNearby;
    public LayerMask layer;
    Inventory inventory;

    private void Start()
    {
        inventory = GameManager.instance.GetComponent<Inventory>();
        //gameObject.SetActive(inventory.hasCompletedDinniesTutorial);
    }

    void Update()
    {
        pjNearby = Physics2D.OverlapBox(transform.position, transform.localScale, 0f, layer);

        if (pjNearby && Input.GetKeyDown(KeyCode.Space))
        {
            if(place != "Ciudad") GameManager.instance.lastPosition = GameManager.instance.player.transform.position - new Vector3(0f, 0.5f,0f);
            SceneManagerScript.instance.LoadScene(place);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position, transform.localScale);
    }
}
