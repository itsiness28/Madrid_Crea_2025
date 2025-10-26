using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogueDisplay : MonoBehaviour
{
    [SerializeField]
    private Image p1;

    [SerializeField]
    private Image p2;

    [SerializeField]
    protected TextAsset inkJSON;

    [SerializeField]
    private float typingSpeed;

    [SerializeField]
    private DialogueManager dialogueManager;

    [SerializeField]
    private GameObject dialoguePanel;
    [SerializeField]
    private GameObject continueIcone;
    [SerializeField]
    private TMP_Text dialogueText;

    [SerializeField]
    private string sceneName;

    private Story currentStory;
    private bool dialogueIsPlaying;
    public bool DialogueIsPlaying { get => dialogueIsPlaying; }

    private bool canContinueToNextLine = false;

    private Coroutine displayLineCoroutine;

    //[SerializeField]
    //private GameObject[] choices;
    //[SerializeField]
    //private TMP_Text[] choicesText;

    //[Header("Audio")]
    //[SerializeField]
    //private DialogueAudioInfoSO defaultAudioInfo;
    //[SerializeField]
    //private DialogueAudioInfoSO[] audioInfos;

    //[SerializeField]
    //private bool makePredictable;

    //private Dictionary<string, DialogueAudioInfoSO> audioInfoDictionary;

    //private DialogueAudioInfoSO currentAudioInfo;

    //Ink Tags
    private const string speakerTag = "speaker";
    private const string audioTag = "audio";

    private void OnEnable()
    {

        dialogueManager.OnEnterDialogueMode += EnterDialogueMode;
        dialogueManager.OnContinuedialog += ContinueButtom;

        //currentAudioInfo = defaultAudioInfo;
        //InitializeAudioInfoDictionary();

    }

    private void Start()
    {
        dialogueManager.EnterDialogueMode(inkJSON);
    }

    //private void InitializeAudioInfoDictionary()
    //{

    //    audioInfoDictionary = new Dictionary<string, DialogueAudioInfoSO>();
    //    audioInfoDictionary.Add(defaultAudioInfo.ID, defaultAudioInfo);
    //    foreach (DialogueAudioInfoSO audioInfo in audioInfos)
    //    {

    //        audioInfoDictionary.Add(audioInfo.ID, audioInfo);

    //    }

    //}

    //private void SetCurrentAudioInfo(string iD)
    //{

    //    DialogueAudioInfoSO audioInfo = null;
    //    audioInfoDictionary.TryGetValue(iD, out audioInfo);
    //    if (audioInfo != null)
    //    {

    //        currentAudioInfo = audioInfo;

    //    }

    //}

    public void ContinueButtom()
    {

        if (displayLineCoroutine != null)
        {

            StopCoroutine(displayLineCoroutine);
            DisplayAllLine(currentStory.currentText);

        }
        else if (canContinueToNextLine && currentStory.currentChoices.Count <= 0)
        {

            ContinueStory();

        }

    }


    void EnterDialogueMode(Story currentStory)
    {

        this.currentStory = currentStory;
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);

        ContinueStory();

    }

    void ExitDialogueMode()
    {

        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";

        //SetCurrentAudioInfo(defaultAudioInfo.ID);

        //Cargar la siguiente escena
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    public void ContinueStory()
    {

        if (currentStory.canContinue)
        {
            string nextLine = currentStory.Continue();
            HandleTags(currentStory.currentTags);
            displayLineCoroutine = StartCoroutine(DisplayLine(nextLine));

        }
        else
        {

            ExitDialogueMode();

        }

    }



    IEnumerator DisplayLine(string line)
    {

        dialogueText.text = line;
        dialogueText.maxVisibleCharacters = 0;

        continueIcone.SetActive(false);
        //HideChoices();

        canContinueToNextLine = false;

        bool isAddingRichTextTag = false;

        foreach (char letter in line.ToCharArray())
        {
            if (letter == '<' || isAddingRichTextTag)
            {

                isAddingRichTextTag = true;
                dialogueText.text += letter;
                if (letter == '>')
                {

                    isAddingRichTextTag = false;

                }

            }
            else
            {

                //PlayDialogueSound(dialogueText.maxVisibleCharacters, dialogueText.text[dialogueText.maxVisibleCharacters]);
                dialogueText.maxVisibleCharacters++;

                yield return new WaitForSeconds(typingSpeed);

            }

        }

        continueIcone.SetActive(true);
        //DisplayChoices();

        canContinueToNextLine = true;

        displayLineCoroutine = null;

    }

    //private void PlayDialogueSound(int currentDisplayedCharacterCount, char currentCharacter)
    //{

    //    AudioClip[] clips = currentAudioInfo.Clips;
    //    int frecuencyLevel = currentAudioInfo.FrecuencyLevel;
    //    float minPitch = currentAudioInfo.MinPitch;
    //    float maxPitch = currentAudioInfo.MaxPitch;
    //    bool stopAudioSource = currentAudioInfo.StopAudioSource;

    //    if(currentDisplayedCharacterCount % frecuencyLevel == 0)
    //    {

    //        if (stopAudioSource)
    //        {

    //            AudioManager.Instance.StopSound();

    //        }

    //        AudioClip soundClip = null;

    //        if (makePredictable)
    //        {

    //            int hashCode = currentCharacter.GetHashCode();
    //            int predictableIndex = hashCode % clips.Length;
    //            soundClip = clips[predictableIndex];

    //            int minPitchInt = (int)(minPitch * 100);
    //            int maxPitchInt = (int)(maxPitch * 100);
    //            int pitchRangeInt = maxPitchInt - minPitchInt;

    //            if(pitchRangeInt != 0)
    //            {

    //                int predictablePitchInt = (hashCode % pitchRangeInt) + minPitchInt;
    //                float predictablePitch = predictablePitchInt / 100f;
    //                AudioManager.Instance.SetPitch2Fbx(predictablePitch);

    //            }
    //            else
    //            {

    //                AudioManager.Instance.SetPitch2Fbx(minPitchInt);

    //            }
    //        }
    //        else
    //        {

    //            int randomIndex = Random.Range(0, clips.Length);
    //            soundClip = clips[randomIndex];

    //            AudioManager.Instance.SetPitch2Fbx(Random.Range(minPitch, maxPitch));

    //        }



    //        AudioManager.Instance.PlaySound(soundClip);

    //    }

    //}

    void DisplayAllLine(string line)
    {

        dialogueText.maxVisibleCharacters = line.Length;

        continueIcone.SetActive(true);
        //DisplayChoices();

        canContinueToNextLine = true;

        displayLineCoroutine = null;
    }

    //void HideChoices()
    //{

    //    foreach (GameObject choiceButtom in choices)
    //    {

    //        choiceButtom.SetActive(false);

    //    }

    //}

    void HandleTags(List<string> currentTags)
    {

        foreach (string tag in currentTags)
        {

            string[] splitTag = tag.Split(':');

            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();
            if (tagKey == speakerTag)
            {

                if (tagValue == "1")
                {
                    p1.color = Color.white;
                    p2.color = Color.gray;
                }
                else if (tagValue == "2")
                {
                    p1.color = Color.gray;
                    p2.color = Color.white;
                }
                

            }
            else if(tagKey == audioTag)
            {

                //SetCurrentAudioInfo(tagValue);

            }
        }



    }

    //private void DisplayChoices()
    //{


    //    List<Choice> currentChoices = currentStory.currentChoices;

    //    int index = 0;

    //    foreach (Choice choice in currentChoices)
    //    {

    //        choices[index].SetActive(true);
    //        choicesText[index].text = choice.text;
    //        index++;
    //    }

    //    for (int i = index; i < choices.Length; i++)
    //    {

    //        choices[i].SetActive(false);

    //    }

    //}

    //public void MakeChoice(int choiceIndex)
    //{
    //    if (canContinueToNextLine)
    //    {

    //        currentStory.ChooseChoiceIndex(choiceIndex);
    //        ContinueStory();

    //    }



    //}

}
