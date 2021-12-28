using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.Netcode;


public class GameHandler : MonoBehaviour
{

    private DataBase dataBase;
    private Team Police;
    private Team Mob;
    public Player Matt;
    public Player Leo;
    //public PlayerScreenManager MattScreen;
    //public PlayerScreenManager LeoScreen;
    public string OutputMessageLeo;
    public string OutputMessageMatt;
    public string OutputTextLeo;
    public string OutputTextMatt;

    public bool errorFlag;
    public bool currentOpFinished;
    public Operation currentOP;
    public int currentOPIndex;
    private Operation[] operations;
    private static bool gameStart;

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
        //Debug.Log(printOps());
        //1st op: Deal, both have intel
        currentOPIndex = 0;
        currentOP = operations[currentOPIndex];
        currentOPIndex++;
        currentOP.EnemyHasIntel = true;
        errorFlag = false;
        currentOpFinished = false;
        gameStart = false;

}

    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 300, 300));
        if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
        {
            StartButtons();
        }
        else
        {
            StatusLabels();
            SubmitCommand();
            gameStart = true;
        }

        GUILayout.EndArea();
    }

    static void StartButtons()
    {
        if (GUILayout.Button("Host (Play as Leo)")) { NetworkManager.Singleton.StartHost(); }
        if (GUILayout.Button("Client (Play as Matt)")) { NetworkManager.Singleton.StartClient(); }
        if (GUILayout.Button("Server")) NetworkManager.Singleton.StartServer();
    }

    static void StatusLabels()
    {
        var mode = NetworkManager.Singleton.IsHost ?
            "Host (Play as Leo)" : NetworkManager.Singleton.IsServer ? "Server" : "Client (Play as Matt)";

        GUILayout.Label("Transport: " +
            NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetType().Name);
        GUILayout.Label("Mode: " + mode);
    }

    static void SubmitCommand()
    {
        if (GUILayout.Button(NetworkManager.Singleton.IsServer ? "Submit Command" : "Submit Command (Client)"))
        {
            var playerObject = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();
            var playerScreen = playerObject.GetComponent<PlayerScreenManager>();

            if (gameStart)
            {
                if (playerScreen.PlayerID == LeoScreen)
                {
                    Leo.Command = playerScreen.Screen.InputField.text;
                    Leo.IsReady = true;
                }
                else if (playerScreen.PlayerID == MattScreen)
                {
                    Matt.Command = playerScreen.Screen.InputField.text;
                    Matt.IsReady = true;
                }

                TryUpdate();


            }
            //try to manage turn



            //update output
            if (playerScreen == LeoScreen)
            {
                playerScreen.Screen.OutputMessage.text = Leo.OutputMessageBuffer;
                playerScreen.Screen.OutputText.text = Leo.OutputTextBuffer;
            }
            else if (playerScreen == MattScreen)
            {
                playerScreen.Screen.OutputMessage.text = Matt.OutputMessageBuffer;
                playerScreen.Screen.OutputText.text = Matt.OutputTextBuffer;
            }

        }
    }

    void Start()
    {
        Leo.OutputMessageBuffer = "Choose who to play as";
        Leo.OutputTextBuffer = Leo.PseudoTeam.ActorsWOBossToString();
        Matt.OutputMessageBuffer = "Choose who to play as";
        Matt.OutputTextBuffer = Matt.PseudoTeam.ActorsWOBossToString();

    }

    // Update is called once per frame
    void TryUpdate()
    {
        //if (gameStart)
        //{

            //Leo.Screen.OutputMessage.text = Leo.OutputMessageBuffer;
            //Leo.Screen.OutputText.text = Leo.OutputTextBuffer;
            //Matt.Screen.OutputMessage.text = Matt.OutputMessageBuffer;
            //Matt.Screen.OutputText.text = Matt.OutputTextBuffer;

            if ((Matt.IsReady) && (Leo.IsReady) && (!errorFlag))
            {
                //end of each input here
                if (currentOpFinished)
                {
                    if (currentOPIndex < 10)
                    {
                        Debug.Log(operations[currentOPIndex].ToString());
                        currentOP = operations[currentOPIndex++];
                    }
                    else { currentOPIndex = 0; }     //loop


                    currentOpFinished = false;
                } //if the flag is up - next op
                if ((!Leo.Wait) || ((Leo.Wait) && (Matt.Wait)))
                {                //Add multithresd?
                    ManageTurn(Leo);
                    if (Leo.EndTurn)
                    { Leo.EndTurn = false; Matt.Wait = false; }
                }
                if ((!Matt.Wait) || ((Leo.Wait) && (Matt.Wait)))
                {
                    ManageTurn(Matt); if (Matt.EndTurn)
                    { Matt.EndTurn = false; Leo.Wait = false; }
                }

            }
        //}
    }
    void ManageTurn(Player currentPlayer) //implement later!!!!!
    {
        currentPlayer.IsReady = false;
        int split;
        switch (currentPlayer.CurrentState) //start every  case by getting input
                                            //only one input intake per state!!!
        {
            case PlayerState.ACTORCHOICE:
                currentPlayer.ChosenActor = currentPlayer.PseudoTeam.Actors[int.Parse(currentPlayer.Command)];
                currentPlayer.CurrentState = PlayerState.PREOPERATION;

                currentPlayer.OutputMessageBuffer = "You chose to play as " + currentPlayer.ChosenActor.FName  + currentPlayer.ChosenActor.LName + "\n\n";
                currentPlayer.OutputMessageBuffer += "Operation info: \n";

                if (currentPlayer == Leo)
                {
                    split = currentOP.ActorsCountMob;
                }
                else
                { split = currentOP.ActorsCountPolice; }
                currentPlayer.PseudoTeam.ShuffleFillActors(split, currentPlayer.ChosenActor.Key);
                currentPlayer.OutputTextBuffer += currentOP.ToString() + "\n\nGoing to the op:\n";
                currentPlayer.OutputTextBuffer = currentPlayer.PseudoTeam.ActorsVisPreOpToString() +"\n";
                currentPlayer.OutputTextBuffer += "Join them on your first operation? type yes or no";
                
                break;

            case PlayerState.PREOPERATION:
                if ((currentPlayer == Leo && ((currentOP.type == Operation.OPtype.DEAL) || ((currentOP.type == Operation.OPtype.RAID) && (currentOP.EnemyHasIntel)))) ||
                   (currentPlayer == Matt && ((currentOP.type == Operation.OPtype.RAID) || ((currentOP.type == Operation.OPtype.DEAL) && (currentOP.EnemyHasIntel)))))
                {
                    if (currentPlayer.Command == "yes")
                    {
                        currentPlayer.PseudoTeam.PlayerToVis();
                        currentPlayer.CurrentState = PlayerState.LEAKINGINFO;
                        currentPlayer.OutputMessageBuffer = "You decided to join the operation.";
                        currentPlayer.OutputTextBuffer = "Currently on operation: \n";
                        currentPlayer.OutputTextBuffer += currentPlayer.PseudoTeam.ActorsVisToString() + "\n";
                       currentPlayer.OutputTextBuffer += "Leak info to your real team? type yes or no";


                        currentPlayer.Log += "Joined operation " + currentOP.ToString() + "\n";
                        errorFlag = false;
                    }
                    else if (currentPlayer.Command == "no")      //you decided to lay low         //add branch to base(-1 trust) /invis (-2 trust)?
                    {
                        currentPlayer.CurrentState = PlayerState.POSTOPERATION;
                        currentPlayer.Trust--;
                        currentPlayer.PseudoTeam.Points++;
                        errorFlag = false;

                        currentPlayer.OutputMessageBuffer = "You decided to lay low for now. Type anything to continue.";
                        currentPlayer.OutputTextBuffer = currentPlayer.Log;
                        currentPlayer.Wait = true;
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
                    currentPlayer.OutputMessageBuffer = "Thanks to your opponent, your pseudo team didn't have any intel on their enemy's next move.\nYou were unprepared when they struck \nYou can view your log instead.\nType anything to continue ";
                    currentPlayer.OutputTextBuffer = currentPlayer.Log;
                    currentPlayer.Wait = true;
                }
                break;

            case PlayerState.LEAKINGINFO:  //onop part 1
                    if (currentPlayer.Command == "yes")
                    {
                        currentPlayer.Trust--;
                        currentPlayer.PseudoTeam.LeakDetected = true;
                        errorFlag = false;
                    }
                    else if (currentPlayer.Command == "no")
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
                        currentPlayer.OutputTextBuffer = currentPlayer.Log;
                }
                break;


            case PlayerState.PROCESSLEAK: //you know how many participants to ping if your opponent has leaked the info during op

                currentPlayer.CurrentState = PlayerState.PINGING;

                if(currentPlayer.RealTeam.LeakDetected)
                {
                    //add leak to opponents log
                    if (currentPlayer == Leo)
                    {
                        Matt.Log += "Leak detected! \n";             //add opponent field to player class???
                    }
                    else { Leo.Log += "Leak detected! \n"; }

                    currentPlayer.OutputMessageBuffer = "You have intel on the current operation:\n" + currentOP.ToString() +"\n";  //check if both sides are present!!!!???
                    currentPlayer.OutputTextBuffer = currentPlayer.RealTeam.ActorsWBossToString();
                    //flush
                    currentPlayer.RealTeam.LeakDetected = false;
                }
                else 
                {
                    currentPlayer.OutputMessageBuffer = "\nYou don't have any intel on the current operation\n";
                    currentPlayer.OutputTextBuffer = currentPlayer.RealTeam.ActorsWBossToString();
                }

                currentPlayer.OutputTextBuffer += "Choose " + currentPlayer.MaxPingable + " people to ping from your real team (type indexes space separated)";



                break;

            case PlayerState.PINGING: //onop part 2
               int maxPingableCount = currentPlayer.MaxPingable;           //extendable later??
                currentPlayer.OutputMessageBuffer = "";
                currentPlayer.OutputTextBuffer = "";
                if (currentOP.EnemyHasIntel)
                {

                    string input = currentPlayer.Command;
                    List<int> indexes = input.Split(' ').Select(int.Parse).ToList();


                    foreach (int index in indexes)
                    {

                        if (currentPlayer.RealTeam.IsVisible(index))
                        {
                            currentPlayer.Log += currentPlayer.RealTeam.Actors[index].ToString() + " was there.\n";
                        }
                        else { currentPlayer.Log += currentPlayer.RealTeam.Actors[index].ToString() + " was not there.\n"; }
                        maxPingableCount--;

                        if (maxPingableCount==0)
                        { break; }
                    }

                    currentPlayer.Log += "end of operation\n\n";


                    currentPlayer.OutputMessageBuffer += "You ping output was added to your log. Type anything to continue.";
                    currentPlayer.OutputTextBuffer += currentPlayer.Log;
                }
                else {
                    // currentPlayer.OutputMessageBuffer = "No one's watching."; //shouldn't reach this
                }
                currentPlayer.CurrentState = PlayerState.POSTOPERATION;
                break;
            case PlayerState.POSTOPERATION:

                currentOpFinished = true;
                currentPlayer.EndTurn = true;
                currentPlayer.CurrentState = PlayerState.ELIMINATING;
                currentPlayer.OutputMessageBuffer = "Who would you like to try to eliminate from your team? Try to identify the rat";
                currentPlayer.OutputTextBuffer = "0.No one (Skip to next step)\n";
                currentPlayer.OutputTextBuffer += currentPlayer.RealTeam.ActorsWOBossToString();
                break;

            case PlayerState.ELIMINATING:
                //kill here
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
                if (currentPlayer.Command == "yes")
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
                else if (currentPlayer.Command == "no")
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
                
                currentPlayer.OutputMessageBuffer += "Operation info: \n";;
                if (currentPlayer == Leo)
                {
                    split = currentOP.ActorsCountMob;
                }
                else
                { split = currentOP.ActorsCountPolice; }
                currentPlayer.PseudoTeam.ShuffleFillActors(split, currentPlayer.ChosenActor.Key);
                currentPlayer.OutputTextBuffer += currentOP.ToString() + "\n\nGoing to the op:\n";
                currentPlayer.OutputTextBuffer = currentPlayer.PseudoTeam.ActorsVisPreOpToString() + "\n";
                currentPlayer.OutputTextBuffer += "Join them on your next operation? type yes or no";
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
            opList += (op.ToString() + "\n");
        }
        return opList;
    }
}
