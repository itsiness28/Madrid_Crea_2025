using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public InputSystem_Actions actions;
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
    [SerializeField] Player_Movement player;
    [SerializeField] Door door;

    public enum TimeZone { PAST, PRESENT}
    TimeZone currentTime;

    private void Awake()
    {
        actions = new InputSystem_Actions();

        currentTime = TimeZone.PRESENT;
        //presentTileMap.SetActive(true);
        //pastTileMap.SetActive(false);
    }

    private void OnEnable()
    {
        actions.Player.Enable();

        actions.Player.TimeTrigger.started += TimeTrigger;
    }
    private void OnDisable()
    {
        actions.Player.Disable();

        actions.Player.TimeTrigger.started -= TimeTrigger;
    }

    private void TimeTrigger(InputAction.CallbackContext ctx)
    {
        if (player.IsGrounded)
        {
            actions.Player.Disable();
            if (currentTime == TimeZone.PRESENT)
            {
                player.DisablePlayerMovement();
                changingTiming = true;

                spriteMask.SetGoToPastAnimTrigger();

                currentTime = TimeZone.PAST;

                pastTileMap.SetActive(true);
            }
            else
            {
                player.DisablePlayerMovement();
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
        player.DisablePlayerMovement();
        door.SetPliOpenTrigger();
        yield return new WaitForSeconds(1f);
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
                player.EnablePlayerMovement();
                actions.Player.Enable();
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
