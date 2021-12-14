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
        currentOP.EnemyHasIntel = true;
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
            if (currentOpFinished) { currentOP = operations[++currentOPIndex]; currentOpFinished = false; } //if the flag is up - next op
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
                        currentPlayer.CurrentState = PlayerState.LEAKINGINFO;
                        currentPlayer.OutputMessageBuffer = "You decided to join the operation. Leak info to your real team?";
                        currentPlayer.OutputTextBuffer = "type yes or no";
                        //add to vis
                        //fill the rest to invis
                        errorFlag = false;
                    }
                    else if (currentPlayer.Screen.InputField.text == "no")      //you decided to lay low         //add branch to base(-1 trust) /invis (-2 trust)?
                    {
                        currentPlayer.CurrentState = PlayerState.POSTOPERATION;
                        //add to invis, fill
                        //fill on op
                        currentPlayer.Trust--;
                        currentPlayer.PseudoTeam.Points++;
                        errorFlag = false;

                        currentPlayer.OutputMessageBuffer = "You decided to lay low for now. Type anything to continue.";
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
                    currentPlayer.CurrentState = PlayerState.POSTOPERATION;
                    currentPlayer.OutputMessageBuffer = "Thanks to your opponent, your pseudo team didn't have any intel on their enemy's next move. You were unprepared when they struck \nYou can view your log instead.\nType anything to continue ";
                    currentPlayer.OutputTextBuffer = "Here be log";
                }
                break;

            case PlayerState.LEAKINGINFO:  //onop part 1

                    //if (currentPlayer.OnOperation)
               // {

                    if (currentPlayer.Screen.InputField.text == "yes")
                    {
                        currentPlayer.Trust--;
                        currentPlayer.PseudoTeam.LeakDetected = true;
                        errorFlag = false;
                    }
                    else if (currentPlayer.Screen.InputField.text == "no")
                    {
                        currentPlayer.PseudoTeam.Points++;
                        errorFlag = false;
                    }
                    else
                    {
                        currentPlayer.Screen.OutputMessage.text = "Mistype";
                        errorFlag = true;
                    }


                    if (currentOP.EnemyHasIntel)
                    {

                        currentPlayer.CurrentState = PlayerState.PROCESSLEAK;

                    currentPlayer.OutputMessageBuffer = "Type anything to continue.";
                    currentPlayer.OutputTextBuffer = " ";

                }
                    else
                    {
                        currentPlayer.CurrentState = PlayerState.POSTOPERATION;
                        currentPlayer.OutputMessageBuffer = "No one is watching, so no one to ping. The op is over. Type anything to continue.";
                        currentPlayer.OutputTextBuffer = "here be log";
                    }

               // }
                break;


            case PlayerState.PROCESSLEAK: //you know how many participants to ping if your opponent has leaked the info during op

                currentPlayer.CurrentState = PlayerState.PINGING;

                if(currentPlayer.RealTeam.LeakDetected)
                {
                    //add leak to opponents log
                    currentPlayer.Screen.OutputMessage.text = "\nYou have intel on the current operation:" + currentOP.toString();
                    currentPlayer.Screen.OutputText.text = Police.ActorsWBossToString();
                    //flush
                    currentPlayer.RealTeam.LeakDetected = false;
                }
                else 
                {
                    currentPlayer.Screen.OutputMessage.text = "\nYou don't have any intel on the current operation";
                    currentPlayer.Screen.OutputText.text = Police.ActorsWBossToString();
                }

                currentPlayer.Screen.OutputMessage.text += "Time to choose who to ping from your real team";



                break;

            case PlayerState.PINGING: //onop part 2

                if (currentOP.EnemyHasIntel)
                {

                    //ping enemy team 
                    //Add to log
                }
                else {
                    // currentPlayer.OutputMessageBuffer = "No one's watching."; //shouldn't reach this
                }
                currentPlayer.CurrentState = PlayerState.POSTOPERATION;
                currentPlayer.OutputMessageBuffer = "You ping output was added to your log. Type anything to continue.";
                currentPlayer.OutputTextBuffer = "here be log";
                break;
            case PlayerState.POSTOPERATION:

                currentOpFinished = true;
                currentPlayer.CurrentState = PlayerState.ELIMINATING;
                currentPlayer.OutputMessageBuffer = "Who would you like to try to eliminate from your team? Try to identify the rat";
                currentPlayer.OutputTextBuffer = "0.No one (Skip to next step)\n";
                currentPlayer.OutputTextBuffer += currentPlayer.RealTeam.ActorsWOBossToString();
                break;

            case PlayerState.ELIMINATING:
                currentPlayer.CurrentState = PlayerState.GAMEENDEVAL;
                break;

            case PlayerState.GAMEENDEVAL:
                currentPlayer.OutputMessageBuffer = "";
                bool gameOver = false;
                if (currentPlayer.Trust<0)
                { currentPlayer.OutputMessageBuffer += "You raised too much suspicion. Your fake boss decided to get rid of you.";
                    gameOver = true;
                }
                if(currentPlayer.PseudoTeam.Points == 10) 
                { currentPlayer.OutputMessageBuffer += "You were too inefficient and your real team failed"; gameOver = true; }
                if (!currentPlayer.ChosenActor.IsAlive)
                { currentPlayer.OutputMessageBuffer += "You were killed. Presumably by one of your own"; gameOver = true; }
                if (gameOver)
                {
                    currentPlayer.CurrentState = PlayerState.GAMEOVER;
                    currentPlayer.OutputTextBuffer = "Type anything to continue.";
                }
                else { currentPlayer.CurrentState = PlayerState.MEETINGBOSS;
                    currentPlayer.OutputMessageBuffer += "You were lucky to still be alive and kicking. Would you like to meet your real boss in order ot update them on your fake team's plans?";
                    currentPlayer.OutputTextBuffer = "Type yes or no";
                }

                break;

            case PlayerState.MEETINGBOSS:
                if (currentPlayer.Screen.InputField.text == "yes")
                {
                    currentPlayer.Trust--;
                    //add a slider index to each team's ops array!! EnemyHasIntel = true;
                    if (currentPlayer == Leo)
                    {
                        int i;
                        for (i = currentOPIndex; i < operations.Length; i++)
                        { if (operations[i].type == Operation.OPtype.DEAL)
                            { operations[i].EnemyHasIntel = true; } }
                    }
                    else if (currentPlayer == Matt)
                    {
                        int i;
                        for (i = currentOPIndex; i < operations.Length; i++)
                        {
                            if (operations[i].type == Operation.OPtype.RAID)
                            { operations[i].EnemyHasIntel = true; }
                        }
                    }
                    errorFlag = false;
                }
                else if (currentPlayer.Screen.InputField.text == "no")
                {
                    currentPlayer.PseudoTeam.Points++;
                    errorFlag = false;
                }
                else
                {
                    currentPlayer.Screen.OutputMessage.text = "Mistype";
                    errorFlag = true;
                }

                currentPlayer.CurrentState = PlayerState.PREOPERATION;
                currentPlayer.OutputMessageBuffer = "Join the next Operation?";
                currentPlayer.OutputTextBuffer = "type yes or no";
                break;

            case PlayerState.GAMEOVER:

                currentPlayer.OutputMessageBuffer = "You and your team lost!";
                currentPlayer.OutputTextBuffer = " ";
                break;

        }
        currentPlayer.Screen.InputField.text = " ";
        Debug.Log("End turn");
    }
    void InitializeTeams()
    {
        Police = new Team();
        Mob = new Team();

        //Mob.HasIntel = true;
        //Police.HasIntel = true;
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
