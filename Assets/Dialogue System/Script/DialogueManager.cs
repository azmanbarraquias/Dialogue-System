// 1st Script
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    #region Variables

    /* A Queue is just a collection of objects (like an array or List). 
        -It just stores a bunch of objects.
        -The difference is that you can only enqueue (put something in the queue) 
        at the end and dequeue (get something out) at the beginning of the queue. 
        It's a FIFO(FirstIn-FirstOut). */
    private Queue<Sentences> _sentences;
    private Queue<Dialogue> _dialogue;

    [Header("Dialogue Setup")]
    [Tooltip("Drag Text object that you want name located at the left side")] public TextMeshProUGUI nameTextLeft;
    [Tooltip("Drag Text object that you want name located at the right side")] public TextMeshProUGUI nameTextRight;
    [Tooltip("Drag dialogue text for object")] public TextMeshProUGUI dialogueText;
    [Tooltip("text named continue")] public TextMeshProUGUI statusText;

    [Header("Animation")]
    [Tooltip("Character with animation IsTalking and Idle ")] public Animator[] animators;
    [Tooltip("Dialogue animation close and open")] public Animator dialogBoxAnimator; //Attached to the gameObject that has Animator Component

    [Header("Sound")]
    [Tooltip("Audio source for the voice sound")] public AudioSource audioSource;

    private int currentAnimIndext = 0;
    private bool displayingSentences; // check if finsish displaying the setences
    private Sentences sentence;

    #endregion Variables

    private void Start()
    {
        nameTextLeft.text = "";
        nameTextRight.text = "";
        dialogueText.text = "";

        _sentences = new Queue<Sentences>();
        _dialogue = new Queue<Dialogue>();
    }

    private void Update()
    {
        //check if the dialogue setences still displaying letter
        Debug.Log("Dialogue text still displaying each sentence: " + displayingSentences);
    }

    public void StartDialogues(Dialogue[] dialogues) // Receive a Dialogue object or class
    {
        // Clear any data last load.
        _dialogue.Clear();
        _sentences.Clear();

        dialogBoxAnimator.SetBool("IsStarted", true);

        // Store each dialogue in the QUEUE list
        foreach (var dialogueItem in dialogues)
        {
            this._dialogue.Enqueue(dialogueItem);
        }

        NextDialogue();
    }

    private void NextDialogue()
    {
        // check if the dialogue is empty
        if (_dialogue.Count == 0)
        {
            // Do something when dialogue loaded all the category
            EndDialogue();
            return;
        }

        // unloaded the first dialogue from the Queue
        var dialogue = _dialogue.Dequeue();

        // Expose the variable
        currentAnimIndext = dialogue.indextAnim;

        // Displaying name left or right
        if (dialogue.rightNamePlacement == true)
        {
            nameTextRight.text = dialogue.name;
            nameTextRight.color = dialogue.textColor;
        }
        else
        {
            nameTextLeft.text = dialogue.name;
            nameTextLeft.color = dialogue.textColor;
        }

        // load sentences to queue list
        foreach (var sentence in dialogue.sentences)
        {
            _sentences.Enqueue(sentence);
        }

        // Add color to the sentences or dialogue text
        dialogueText.color = dialogue.textColor;

        NextSentence();
    }

    public void NextSentence()
    {
        // Stop Coroutines if still playing
        StopAllCoroutines();

        if (displayingSentences == true)
        {
            animators[currentAnimIndext].SetBool("Talking", false);
            dialogueText.text = sentence.sentences;
            displayingSentences = false;
        }
        else
        {
            if (_sentences.Count == 0)
            {
                // if true setence is 0 end Dialogue
                animators[currentAnimIndext].SetBool("Talking", false);
                NextDialogue();
                return;
            }

            sentence = _sentences.Dequeue(); // FIFO, "OUT" the first Queue then the next if call again (Queue --)

            audioSource.Stop();
            audioSource.PlayOneShot(sentence.sentencesAudioClip);

            //Start new Coroutine
            StartCoroutine(DisplayEachLetter(sentence));
        }
    }

    IEnumerator DisplayEachLetter(Sentences sentence)
    {
        animators[currentAnimIndext].SetBool("Talking", true);
        statusText.gameObject.SetActive(false);
        displayingSentences = true;

        dialogueText.text = "";

        //ToCharArray is a fuction that convert string in to Char Array
        foreach (char letter in sentence.sentences.ToCharArray())
        {
            dialogueText.text += letter; // dialogueText.text = dialogueText.text + letter;
            yield return new WaitForSeconds(sentence.textWaitSpeed); // Delay
        }

        animators[currentAnimIndext].SetBool("Talking", false);
        displayingSentences = false;
        statusText.gameObject.SetActive(true);
    }

    public void EndDialogue()
    {
        nameTextLeft.text = "";
        nameTextRight.text = "";
        dialogueText.text = "";
        dialogBoxAnimator.SetBool("IsStarted", false);
    }
}