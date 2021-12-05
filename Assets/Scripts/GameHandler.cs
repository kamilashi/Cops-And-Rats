using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class GameHandler : MonoBehaviour
{

    private DataBase dataBase;
    private Team Police;
    private Team Mob;
    public Player Matt;
    public Player Leo;
    public PlayerScreenManager MattScreen;
    public PlayerScreenManager LeoScreen;
    public string OutputMessage;
    public string OutputTextLeo;
    public string OutputTextMatt;

    // Start is called before the first frame update
    void Awake()
    {
        dataBase = new DataBase();
        InitializeTeams();
        InitializeActors();
    }

    void Start()
    {
        OutputMessage = "Choose who to play as";
        int i = 1;
        foreach (Actor actor in Police.Actors)
        {
            OutputTextLeo += i++ + ". " + actor.ToString() + "\n";
        }
        i = 1;
        foreach (Actor actor in Mob.Actors)
        {
            OutputTextMatt += i++ + ". " + actor.ToString() + "\n";
        }

    }

    // Update is called once per frame
    void Update()
    {
        LeoScreen.OutputMessage.text = OutputMessage;
        LeoScreen.OutputText.text = OutputTextLeo;

        MattScreen.OutputMessage.text = OutputMessage;
        MattScreen.OutputText.text = OutputTextMatt;

        if ((Matt.IsReady) && (Leo.IsReady))
        {
            ManageTurn();
        }
        
    }
    void ManageTurn()
    {
        //Process Leo
        if (Leo.CurrentState == PlayerState.ACTORCHOICE)
        {
            Leo.ChosenActor = Police.Actors[int.Parse(LeoScreen.InputField.text)-1];
        }
        LeoScreen.InputField.text = " ";
        Leo.IsReady = false;

       
        //Process Matt
        if (Matt.CurrentState == PlayerState.ACTORCHOICE)
        {
            Matt.ChosenActor = Mob.Actors[int.Parse(MattScreen.InputField.text) - 1];
        }
        MattScreen.InputField.text = " ";
        Matt.IsReady = false;


        Debug.Log(" ");
    }
    void InitializeTeams()
    {
        Police = new Team();
        Mob = new Team();
    }

    void InitializeActors()
    {
        //Pull from database:

        this.Police.Actors = dataBase.stationActors;
        this.Police.Boss = dataBase.Pboss;

        this.Mob.Actors = dataBase.mobActors;
        this.Mob.Boss = dataBase.Mboss;
    }
}
