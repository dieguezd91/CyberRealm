using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCScript : MonoBehaviour
{
    [SerializeField] Transform referencePoint;
    [SerializeField] GameObject MissionPanelUI;
    float lastSpeed;

    void Update()
    {
        if (Physics2D.OverlapBox(referencePoint.position, referencePoint.localScale, 0).CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.Space) && !MissionPanelUI.activeInHierarchy)
            {
                lastSpeed = PlayerController.instance.MoveSpeed;
                PlayerController.instance.MoveSpeed = 0;
                MissionPanelUI.SetActive(true);
            }
            else if (Input.GetKeyDown(KeyCode.Escape) && MissionPanelUI.activeInHierarchy)
            {
                PlayerController.instance.MoveSpeed = lastSpeed;
                MissionPanelUI.SetActive(false);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(referencePoint.position, referencePoint.localScale);
    }
}
