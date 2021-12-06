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
    public string OutputMessageLeo;
    public string OutputMessageMatt;
    public string OutputTextLeo;
    public string OutputTextMatt;

    private bool errorFlag;
    private Operation currentOP;

    // Start is called before the first frame update
    void Awake()
    {
        dataBase = new DataBase();
        InitializeTeams();
        InitializeActors();
        errorFlag = false;
    }

    void Start()
    {
        OutputMessageLeo = "Choose who to play as";
        OutputMessageMatt = "Choose who to play as";
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
        LeoScreen.OutputMessage.text = OutputMessageLeo;
        LeoScreen.OutputText.text = OutputTextLeo;

        MattScreen.OutputMessage.text = OutputMessageMatt;
        MattScreen.OutputText.text = OutputTextMatt;

        if ((Matt.IsReady) && (Leo.IsReady) && (!errorFlag))
        {
            ManageTurn();
        }
        
    }
    void ManageTurn()
    {
        if (Mob.OperationPlanned.Date < Police.OperationPlanned.Date)
        {
            currentOP = Mob.OperationPlanned;
        }
        else { currentOP = Police.OperationPlanned; }


        //Process Leo

        Leo.IsReady = false;
        switch (Leo.CurrentState)
        {
            case PlayerState.ACTORCHOICE:
                Leo.ChosenActor = Police.Actors[int.Parse(LeoScreen.InputField.text)-1];
               // Mob.NextOp = dataBase.DealOpsMob[0];
               // Mob.EnemyInputNextOp = dataBase.DealOpsPolice[0];
                Leo.CurrentState = PlayerState.PREOPERATION;
                OutputMessageLeo = "Join the first Operation? type yes orno";
                break;
            case PlayerState.PREOPERATION:
                if ((currentOP.type == Operation.OPtype.DEAL) || ((currentOP.type == Operation.OPtype.RAID) &&(Mob.HasIntel)))
                {
                    if (LeoScreen.InputField.text == "yes")
                    {
                        Leo.CurrentState = PlayerState.ONOPERATION;
                        errorFlag = false;
                    }
                    else if (LeoScreen.InputField.text == "no")
                    {
                        Leo.CurrentState = PlayerState.OFFOPERATION;
                        errorFlag = false;
                    }
                    else
                    {
                        OutputMessageLeo = "Type again: yes or no";
                        errorFlag = true;
                    }
                    break;
                }
                else {
                        OutputMessageLeo = "You have no intel on the department/n Meet with your real Boss?";
                        if (LeoScreen.InputField.text == "yes")
                        {
                            Leo.CurrentState = PlayerState.MEETINGBOSS;
                            errorFlag = false;
                        }
                        else if (LeoScreen.InputField.text == "no")
                        {
                            Leo.CurrentState = PlayerState.OFFOPERATION;
                            errorFlag = false;
                        }
                        else
                        {
                            OutputMessageLeo = "Type again: yes or no";
                            errorFlag = true;
                        }
                    }
                    break;

            case PlayerState.ONOPERATION:

                break;
            case PlayerState.OFFOPERATION:

                break;
            case PlayerState.POSTOPERATION:

                break;
            case PlayerState.NONE:

                break;

        }
        LeoScreen.InputField.text = " ";
       

        //Process Matt

        Matt.IsReady = false;
        switch (Matt.CurrentState)
        {
            case PlayerState.ACTORCHOICE:
                Matt.ChosenActor = Mob.Actors[int.Parse(MattScreen.InputField.text) - 1];

                Matt.CurrentState = PlayerState.PREOPERATION;
                break;
            case PlayerState.PREOPERATION:
                break;
            case PlayerState.ONOPERATION:

                break;
            case PlayerState.OFFOPERATION:

                break;
            case PlayerState.POSTOPERATION:

                break;
            case PlayerState.NONE:

                break;

        }
        MattScreen.InputField.text = " ";


        Debug.Log(" ");
    }
    void InitializeTeams()
    {
        Police = new Team();
        Police.OperationPlanned = dataBase.RaidOpsPolice[Police.CurrentOPIndex];
        Mob = new Team();
        Mob.OperationPlanned = dataBase.DealOpsMob[Mob.CurrentOPIndex];
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
