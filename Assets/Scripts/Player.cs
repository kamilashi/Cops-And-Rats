using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{

    public PlayerScreenManager Screen;
    public Actor ChosenActor { get; set; }
    public Team RealTeam { get; set; }
    public Team PseudoTeam { get; set; }
    public int Trust { get; set; }
    public bool IsReady { get; set; }
    public PlayerState CurrentState;
    public string OutputMessageBuffer { get; set; }
    public string OutputTextBuffer { get; set; }
    public bool OnOperation;
    // Start is called before the first frame update
    void Awake()
    {
        Trust = 10;
        IsReady = false;
        onOperation = false;
    }
    void Start()
    {
        CurrentState = PlayerState.ACTORCHOICE;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Ping()
    { 
        
    }

    public void ProcessCommand(string command)
    {
        Debug.Log("Processing command: " + command);
       // if (CurrentState == ACTORCHOICE)
       // {
       //     ChosenActor = DataBase.Ac
       // }

    }
}
