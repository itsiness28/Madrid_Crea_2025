using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private InputSO inputSO;
    [Header("Past & Present TileMaps")]
    [SerializeField] GameObject pastTileMap;
    [SerializeField] GameObject presentTileMap;

    [Header("Scene names")]
    [SerializeField] string previousScene;
    [SerializeField] string nextScene;

    [Header("Timer Values")]
    [SerializeField] float stopTimeTimer;
    float actualTimer;
    bool changingTiming;

    [Header("Dependencias")]
    [SerializeField] SpriteMask spriteMask;
    [SerializeField] PlayerMove player;
    [SerializeField] Door door;
    [SerializeField] SceneFader sceneFader;

    [SerializeField]
    Checker[] checkers;

    public enum TimeZone { PAST, PRESENT}
    TimeZone currentTime;

    //private void Awake()
    //{
    //    actions = new InputSystem_Actions();

    //    //presentTileMap.SetActive(true);
    //    //pastTileMap.SetActive(false);
    //}

    private void OnEnable()
    {
        currentTime = TimeZone.PRESENT;
        //actions.Player.Enable();

        inputSO.TimeTrigger += TimeTrigger;
    }
    private void OnDisable()
    {

        inputSO.TimeTrigger -= TimeTrigger;
    }

    private void TimeTrigger()
    {
        if (player.Below)
        {
            foreach (Checker checker in checkers)
            {
                checker.Check();
            }
            inputSO.SwitchMode(InputMode.disableMode);
            if (currentTime == TimeZone.PRESENT)
            {
                inputSO.SwitchMode(InputMode.disableMode);
                changingTiming = true;

                spriteMask.SetGoToPastAnimTrigger();

                currentTime = TimeZone.PAST;

                pastTileMap.SetActive(true);
            }
            else
            {
                inputSO.SwitchMode(InputMode.disableMode);
                changingTiming = true;

                spriteMask.SetGoToPresentTrigger();

                currentTime = TimeZone.PRESENT;

                presentTileMap.SetActive(true);
            }
        }
    }

    public void GoToPreviousScene()
    {
        SceneManager.LoadScene(previousScene);
    }

    public void GoToNextScene()
    {
        StartCoroutine(GoToNextSceneCorrutina());
    }

    IEnumerator GoToNextSceneCorrutina()
    {
        inputSO.SwitchMode(InputMode.disableMode);
        door.SetPliOpenTrigger();
        yield return new WaitForSeconds(1f);
        sceneFader.SetFadeOutTrigger();
        yield return new WaitForSeconds(1.5f);
        inputSO.SwitchMode(InputMode.moveMode);
        SceneManager.LoadScene(nextScene);
    }



    void Start()
    {
        actualTimer = stopTimeTimer;
    }


    void Update()
    {
        if (changingTiming)
        {
            actualTimer -= Time.deltaTime;
            if(actualTimer <= 0)
            {
                actualTimer = stopTimeTimer;
                changingTiming = false;
                inputSO.SwitchMode(InputMode.moveMode);
                if (currentTime == TimeZone.PRESENT)
                {
                    presentTileMap.SetActive(true);
                    pastTileMap.SetActive(false);
                }
                else
                {
                    presentTileMap.SetActive(false);
                    pastTileMap.SetActive(true);
                }
            }
        }
    }
}
