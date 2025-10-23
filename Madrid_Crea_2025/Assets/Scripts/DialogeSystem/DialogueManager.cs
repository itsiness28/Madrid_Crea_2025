using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue Manager")]
public class DialogueManager : ScriptableObject
{

    public event Action<Story> OnEnterDialogueMode;
    public event Action OnContinuedialog;

    [SerializeField]
    private InputSO inputManager;


    private void OnEnable()
    {
        inputManager.OnJumpAnction += ContinueDialogue;

    }


    public void EnterDialogueMode(TextAsset inkJSON)
    {
        Story currentStory = new Story(inkJSON.text);

        //currentStory.BindExternalFunction("SaveGame", () => SaveGame());
        OnEnterDialogueMode?.Invoke(currentStory);

    }

    public void ContinueDialogue()
    {
        
        OnContinuedialog?.Invoke();
    }

    public Ink.Runtime.Object GetVariableState(string variableName)
    {

        Ink.Runtime.Object variableValue = null;

        return variableValue;
    }

    private void SaveGame()
    {



    }

}
