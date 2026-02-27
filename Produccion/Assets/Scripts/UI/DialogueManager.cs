using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Animations;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] AudioClip clip;
    public TextMeshProUGUI dialogueText;
    public PlayerController player;
    Animator animator;

    public bool disableAfter;
    [SerializeField] GameObject collisionEvent;
    [SerializeField] GameObject objectToDisable;

    public string[] lines;

    public float textSpeed = 0.05f;

    int index;

    float lastSpeed;
    
    public event EventHandler OnDialogueEnd;

    public void Start()
    {
        player = GameManager.instance.Player.GetComponent<PlayerController>();
        animator = GameManager.instance.Player.GetComponent<Animator>();
        dialogueText.text = string.Empty;
        StartDialogue();
        if(disableAfter)
            collisionEvent.SetActive(false);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            if (dialogueText.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                dialogueText.text = lines[index];
            }
        }
    }

    public void StartDialogue()
    {
        lastSpeed = player.MoveSpeed;
        player.MoveSpeed = 0;
        MenuManager.instance.menu.SetActive(false);
        GameManager.instance.Chatting = true;
        //animator.enabled = false;
        dialogueText.text = string.Empty;
        index = 0;
        StartCoroutine(WriteLine());
    }

    IEnumerator WriteLine()
    {
        foreach (char letter in lines[index].ToCharArray())
        {
            dialogueText.text += letter;
            AudioManager.instance.PlaySound(clip);
            yield return new WaitForSeconds(textSpeed);
        }
    }

    public void NextLine()
    {
        if(index < lines.Length - 1) 
        {
            index++;
            dialogueText.text = string.Empty;
            StartCoroutine(WriteLine());
        }
        else
        {
            EndDialogue();
        }
    }

    void EndDialogue()
    {
        OnDialogueEnd?.Invoke(this, EventArgs.Empty);
        //dialogueText.text = string.Empty;
        gameObject.SetActive(false);
        if (objectToDisable != null)
            objectToDisable.SetActive(false);
        player.MoveSpeed = lastSpeed;
        GameManager.instance.Chatting = false;
        //animator.enabled = true;
    }
}
