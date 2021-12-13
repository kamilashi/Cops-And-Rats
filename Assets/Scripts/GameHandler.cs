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
    //public PlayerScreenManager Matt.Screen;
   // public PlayerScreenManager Leo.Screen;
    public string OutputMessageLeo;
    public string OutputMessageMatt;
    public string OutputTextLeo;
    public string OutputTextMatt;

    private bool errorFlag;
    private bool currentOpFinished;
    private Operation currentOP;
    public int currentOPIndex;
    private Operation[] operations;

    // Start is called before the first frame update
    void Awake()
    {
        dataBase = new DataBase();
        InitializeTeams();
        InitializeActors();
        Matt.RealTeam = Mob;
        Matt.PseudoTeam = Police;
        Leo.RealTeam = Police;
        Leo.PseudoTeam = Mob;

        operations = new Operation[6];
        getOps();
        Debug.Log(printOps());
        //1st op: Deal, both have intel
        currentOPIndex = 0;
        currentOP = operations[currentOPIndex];
        errorFlag = false;
        currentOpFinished = false;
        onOperation = false;

}

void Start()
    {
        Leo.OutputMessageBuffer = "Choose who to play as";
        Leo.OutputTextBuffer = Leo.PseudoTeam.ActorsWOBossToString();
        Matt.OutputMessageBuffer = "Choose who to play as";
        Matt.OutputTextBuffer = Matt.PseudoTeam.ActorsWOBossToString();

    }

    // Update is called once per frame
    void Update()
    {

        Leo.Screen.OutputMessage.text = Leo.OutputMessageBuffer;
        Leo.Screen.OutputText.text = Leo.OutputTextBuffer;
        Matt.Screen.OutputMessage.text = Matt.OutputMessageBuffer;
        Matt.Screen.OutputText.text = Matt.OutputTextBuffer;

        if ((Matt.IsReady) && (Leo.IsReady) && (!errorFlag))  
        {
            //end of each input here
            if (currentOpFinished) { currentOP = operations[++currentOPIndex]; } //if the flag is up - next op
            ManageTurn(Leo);  //Add multithresd?
            ManageTurn(Matt);
        }
        
    }
    void ManageTurn(Player currentPlayer) //implement later!!!!!
    {
        //if current player == Leo

        currentPlayer.IsReady = false;
        switch (currentPlayer.CurrentState) //start every  case by getting input
                                            //only one input intake per state!!!
        {
            case PlayerState.ACTORCHOICE:
                currentPlayer.ChosenActor = currentPlayer.RealTeam.Actors[int.Parse(currentPlayer.Screen.InputField.text)];
                currentPlayer.CurrentState = PlayerState.PREOPERATION;
                currentPlayer.OutputMessageBuffer = "Join the first Operation?";
                currentPlayer.OutputTextBuffer = "type yes or no";
                break;

            case PlayerState.PREOPERATION:
                if ((currentPlayer == Leo && ((currentOP.type == Operation.OPtype.DEAL) || ((currentOP.type == Operation.OPtype.RAID) && (currentOP.EnemyHasIntel)))) ||
                   (currentPlayer == Matt && ((currentOP.type == Operation.OPtype.RAID) || ((currentOP.type == Operation.OPtype.DEAL) && (currentOP.EnemyHasIntel)))))
                {
                    if (currentPlayer.Screen.InputField.text == "yes")
                    {
                        currentPlayer.OnOperation = true;
                        if (currentOP.EnemyHasIntel)
                        {
                            currentPlayer.CurrentState = PlayerState.PINGING;          //you decided to join the op
                            currentPlayer.Screen.OutputMessage.text = "You decided to join the operation. \nTime to choose who to ping from your real team";
                            currentPlayer.Screen.OutputText.text = Police.ActorsWBossToString();
                        }
                        else {
                            currentPlayer.CurrentState = PlayerState.LEAKINGINFO;
                            currentPlayer.OutputMessageBuffer = "No one's watching. No one to ping.";
                            currentPlayer.OutputTextBuffer = "Input anything to continue. ";
                        }
                        //add to vis
                        //fill the rest to invis
                        errorFlag = false;
                    }
                    else if (currentPlayer.Screen.InputField.text == "no")      //you decided to lay low         //add branch to base(-1 trust) /invis (-2 trust)?
                    {
                        currentPlayer.CurrentState = PlayerState.POSTOPERTION;
                        //add to invis, fill
                        //fill on op
                        currentPlayer.Trust--;
                        errorFlag = false;

                        currentPlayer.OutputMessageBuffer = "You decided to lay low for now. Input anything to continue.";
                        currentPlayer.OutputTextBuffer = "Here be log";
                    }
                    else
                    {
                        currentPlayer.Screen.OutputMessage.text = "Mistype";
                        errorFlag = true;
                    }
                }
                else
                {
                    currentPlayer.CurrentState = PlayerState.POSTOPERTION;
                    currentPlayer.OutputMessageBuffer = "Thanks to your opponent, your pseudo team didn't have any intel on their enemy's next move. You were unprepared when they struck \n You can view your log instead:";
                    currentPlayer.OutputTextBuffer = "Here be log";
                }
                break;

            case PlayerState.PINGING: //onop part 1
                if (currentOP.EnemyHasIntel)
                {
                    //ping enemy team 
                    //Add to log
                    //how do you know how many actors to ping???
                }
                else {
                    // currentPlayer.OutputMessageBuffer = "No one's watching."; //shouldn't reach this
                }
                currentPlayer.CurrentState = PlayerState.LEAKINGINFO;
                currentPlayer.OutputMessageBuffer += "Leak info to your real team?";
                currentPlayer.OutputTextBuffer = "type yes or no";

                break;
            case PlayerState.LEAKINGINFO:  //onop part 2
                if (currentPlayer.OnOperation )
                { currentPlayer.OnOperation = false; }
                //joined
                //convey or not
                break;
            case PlayerState.POSTOPERATION:
                //review log
                if (!currentPlayer.PseudoTeam.HasIntel)  //move to preop??
                {
                    currentPlayer.OutputMessageBuffer = "You have no intel on your real team's plans\n Meet with your real Boss?";
                    currentPlayer.OutputTextBuffer = "type yes or no";

                }
                break;
            case PlayerState.NONE:
                //unknown state
                break;

        }
        currentPlayer.Screen.InputField.text = " ";

        //Process Matt

        //Matt.IsReady = false;
        //switch (Matt.CurrentState)
        //{
        //    case PlayerState.ACTORCHOICE:
        //        Matt.ChosenActor = Mob.Actors[int.Parse(Matt.Screen.InputField.text) - 1];

        //        Matt.CurrentState = PlayerState.PREOPERATION;
        //        break;
        //    case PlayerState.PREOPERATION:
        //        break;
        //    case PlayerState.ONOPERATION:

        //        break;
        //    case PlayerState.OFFOPERATION:

        //        break;
        //    case PlayerState.POSTOPERATION:

        //        break;
        //    case PlayerState.NONE:

        //        break;

        //}
        //Matt.Screen.InputField.text = " ";


        Debug.Log("End turn");
    }
    void InitializeTeams()
    {
        Police = new Team();
        Mob = new Team();

        Mob.HasIntel = true;
        Police.HasIntel = true;
    }

    void InitializeActors()
    {
        //Pull from database:

        this.Police.Actors = dataBase.stationActors;
        this.Police.Boss = dataBase.Pboss;

        this.Mob.Actors = dataBase.mobActors;
        this.Mob.Boss = dataBase.Mboss;
    }

    void getOps() //all the ops in chronological order
    {
        operations[0] = dataBase.DealOps[0];
        int i = 1;
        int j = 0;
        int k = 1;
        Debug.Log(i + "  " + j + " " + k);
        while( (k<6) && (i < dataBase.DealOps.Length ) && (j < dataBase.RaidOps.Length ) ) 
        {
            if (dataBase.DealOps[i].Date < dataBase.RaidOps[j].Date)
            {
                operations[k] = dataBase.DealOps[i];
                i++;
                k++;
            }
            else {
                operations[k] = dataBase.RaidOps[j];
                j++;
                k++;
            }
        }
        while (i < dataBase.DealOps.Length )
        {
            operations[k] = dataBase.DealOps[i];
            i++;
            k++;
        }

        while (j < dataBase.RaidOps.Length )
        {
            operations[k] = dataBase.RaidOps[j];
            j++;
            k++;
        }

    }
    string printOps()
    {
        string opList = "";
        foreach (Operation op in operations)
        {
            opList += (op.toString() + "\n");
        }
        return opList;
    }
}
