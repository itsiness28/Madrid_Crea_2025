using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public InputSystem_Actions actions;
    [Header("Past & Present TileMaps")]
    [SerializeField] GameObject pastTileMap;
    [SerializeField] GameObject presentTileMap;

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
            currentTime = TimeZone.PAST;
            presentTileMap.SetActive(false);
            pastTileMap.SetActive(true);
        }
        else
        {
            currentTime = TimeZone.PRESENT;
            presentTileMap.SetActive(true);
            pastTileMap.SetActive(false);
        }
    }


    void Start()
    {
        
    }


    void Update()
    {
        
    }
}
