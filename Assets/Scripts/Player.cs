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
    public int Trust; //{ get; set; }
    public bool IsReady { get; set; }
    public PlayerState CurrentState;
    public string OutputMessageBuffer { get; set; }
    public string OutputTextBuffer { get; set; }
    public string Log { get; set; }
    public int MaxPingable { get; set; }
    public bool EndTurn;
    public bool Wait;
    // Start is called before the first frame update
    void Awake()
    {
        Trust = 10;
        IsReady = false;
        Log = "";
        MaxPingable = 3;
        EndTurn = false;
        Wait = false;
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

}
