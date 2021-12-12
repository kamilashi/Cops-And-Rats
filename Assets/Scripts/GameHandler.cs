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

            case PlayerState.PREOPERATIONHASINTEL:
                if ((currentPlayer == Leo && ((currentOP.type == Operation.OPtype.DEAL) || ((currentOP.type == Operation.OPtype.RAID) && (Mob.HasIntel)))) ||
                   (currentPlayer == Matt && ((currentOP.type == Operation.OPtype.RAID) || ((currentOP.type == Operation.OPtype.DEAL) && (Police.HasIntel)))))
                {
                    if (currentPlayer.Screen.InputField.text == "yes")
                    {
                        currentPlayer.CurrentState = PlayerState.PING;
                        //add to on op fill
                        //fill invis
                        errorFlag = false;
                    }
                    else if (currentPlayer.Screen.InputField.text == "no")      //add branch to base(-1 trust) /invis (-2 trust)?
                    {
                        currentPlayer.CurrentState = PlayerState.OFFOPERATION;
                        //add to invis, fill
                        //fill on op
                        currentPlayer.Trust--;
                        errorFlag = false;
                    }
                    else
                    {
                        currentPlayer.Screen.OutputMessage.text = "Mistype";
                        errorFlag = true;
                    }
                    break;
                }
                else {
                    currentPlayer.OutputMessageBuffer = "You have no intel on your opponent's plans\n Meet with your real Boss?";
                    currentPlayer.OutputTextBuffer = "type yes or no";
                    if (currentPlayer.Screen.InputField.text == "yes")
                        {
                        currentPlayer.CurrentState = PlayerState.MEETINGBOSS;
                        //add to invis
                        //fill base/invis (random)
                        //exclude boss from enemy onOp
                        errorFlag = false;
                        }
                        else if (currentPlayer.Screen.InputField.text == "no")
                        {
                        //add to base or invis (random) 
                        currentPlayer.CurrentState = PlayerState.OFFOPERATION;
                            errorFlag = false;
                        }
                        else
                        {
                        currentPlayer.Screen.OutputMessage.text = "Mistype";
                            errorFlag = true;
                        }
                    }
                    break;

            case PlayerState.PING: //onop part 1
                //ping enemy team
                //enemy watching? - move to post?
                currentPlayer.Screen.OutputMessage.text = "Choose who to ping on station base";
                currentPlayer.Screen.OutputText.text = Police.ActorsWBossToString();
                //has info or not 

                //joined
                //convey or not
                break;
            case PlayerState.ONOPERATION:  //onop part 2

                //joined
                //convey or not
                break;
            case PlayerState.MEETINGBOSS:
                //convey or not
                //get info or not (trust check)
                //off base
                break;
            case PlayerState.OFFOPERATION:
                //nothing?
                break;
            case PlayerState.POSTOPERATION:
                //review log
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
