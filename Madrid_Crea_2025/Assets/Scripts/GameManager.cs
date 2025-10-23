using System;
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

    [Header("Sprite Mask")]
    [SerializeField] SpriteMask spriteMask;

    public enum TimeZone { PAST, PRESENT}
    TimeZone currentTime;

    private void Awake()
    {
        actions = new InputSystem_Actions();

        currentTime = TimeZone.PRESENT;
        presentTileMap.SetActive(true);
        pastTileMap.SetActive(false);
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
        if(currentTime == TimeZone.PRESENT)
        {
            spriteMask.SetGoToPastAnimTrigger();

            currentTime = TimeZone.PAST;
            //presentTileMap.SetActive(false);
            pastTileMap.SetActive(true);
        }
        else
        {
            spriteMask.SetGoToPresentTrigger();

            currentTime = TimeZone.PRESENT;
            presentTileMap.SetActive(true);
            //pastTileMap.SetActive(false);
        }
    }

    public void GoToPreviousScene()
    {
        SceneManager.LoadScene(previousScene);
    }

    public void GoToNextScene()
    {
        SceneManager.LoadScene(nextScene);
    }


    void Start()
    {
        
    }


    void Update()
    {
        
    }
}
