using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// We want to using this Dialogue class as and object to pass on to the DialogueManager

// The Serializable attribute lets you embed a class with sub properties in the inspector.
//[System.Serializable] // Serializable make this variable inside the class show up in the inspector.
[System.Serializable]
public class Dialogue
{
    public string name; //NPC Name

    public int indextAnim = 0;

    public bool rightNamePlacement;

    public Color textColor;

  // Attribute to make a string be edited with a height-flexible and scrollable text area.
    public Sentences[] sentences;

  

}

[System.Serializable]
public class Sentences
{
    [TextArea(3, 10)]
    public string sentences;

    public AudioClip sentencesAudioClip;

    [Range(0, 1)]
    public float textWaitSpeed = 0.1f;
}
