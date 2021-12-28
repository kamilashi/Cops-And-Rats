using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ClassLib : MonoBehaviour
{
}

public class Operation
{
    public int ActorsCountPolice { get; set; }
    public int ActorsCountMob { get; set; }
    public int Date { get; set; }  //dhhmm
    public int index { get; set; }
    public string Location { get; set; }
    public OPtype type;
    public bool EnemyHasIntel { get; set; } //changed by the player once they leak info to their real team in between the ops (E.g. Leo leaks --> DealOps[next].EnemyHasIntel = true)

    public Operation(){ }
    public Operation(int ActorsCountPolice, int ActorsCountMob, int date, int t)
    {
        this.ActorsCountPolice = ActorsCountPolice;
        this.ActorsCountMob = ActorsCountMob;
        this.Date = date;
        this.type = (OPtype) t;
        EnemyHasIntel = false;
    }
    public Operation(int ActorsCountPolice, int ActorsCountMob, int date, int t, string loc)
    {
        this.ActorsCountPolice = ActorsCountPolice;
        this.ActorsCountMob = ActorsCountMob;
        this.Date = date;
        this.type = (OPtype) t;
        this.Location = loc;
        EnemyHasIntel = false;
    }

    public string ToString()
    { 
        return ( (OPtype) type + ": date: " + Date + " location: " + Location + " participating cops: " + ActorsCountPolice + " participating gangsters: " + ActorsCountMob);
    }
    public enum OPtype 
    {   
        DEAL ,
        RAID 
    }

}

    public class Actor 
{
    public string FName { get; set; }
        public string LName { get; set; }
        public bool IsAlive;
    public int Key { get; set; }

    public Actor(string fname, string lname)
    {this.FName = fname; 
     this.LName = lname;
        IsAlive = true;
    }

    
    public string ToString()
    {
        return FName + " " + LName;
    }
}

public class Team
{
    public Actor[] Actors { get; set; } // array of 10
    public int[] ActorsVisInvis { get; set; }   // indices of participating actors to be randomized each time
    public int playerIndex { get; set; }
    public int split { get; set; }              //index so that for all i < split all the entries ActorsVisInvis[i] are "visible" and for i > split - "invisible"
    public Actor Boss { get; set; }

    public int Points { get; set; }
    public bool LeakDetected { get; set; } //shanged by the opposite team if the enemy player leaks info during op (Leo leaks --> Mob.LeakDetected = true)

    // Start is called before the first frame update
    public Team()
    {
        Points = 0;
        Actors = new Actor[7];
        ActorsVisInvis = new int[7];
        split = 0;
        playerIndex = 0;
    }

    public string ActorsWOBossToString()
    {
        string OutputString = "";
        int i;
        for (i = 1; i < Actors.Length; i++)
        {
            OutputString += i + ". " + Actors[i].ToString() + "\n";
        }

        return OutputString;
    }
    public string ActorsWBossToString()
    {
        string OutputString = "";
        //string OutputString = "1. " + Boss.ToString() + " (Boss)\n";
        int i;
        for (i = 0; i < Actors.Length; i++)
        {
            OutputString += (i) + ". " + Actors[i].ToString() + "\n";
        }

        return OutputString;
    }
    public void ShuffleFillActors(int actorsOnOpCount, int playerActorKey) //first fill the vis part excluding the player - the player will be the first in the invis part (ActorsVisInvis[split])
    {                                                                                //if they want to join swap the ActorsVisInvis[split-1] with ActorsVisInvis[split]
        int i, swap;

        foreach (Actor actor in Actors)             //copy indices as is (Key = index)
        { ActorsVisInvis[actor.Key] = actor.Key; }

        //first move the player actor index into the position
        
        ActorsVisInvis[playerActorKey] = ActorsVisInvis[actorsOnOpCount];
        ActorsVisInvis[actorsOnOpCount] = playerActorKey;

        for (i = 0; i < ActorsVisInvis.Length; i++)
        { 
            int randomIndex = Random.Range(0, Actors.Length); //includes min value but not max value (7 is the length of the array)
            if ((i != actorsOnOpCount) && (randomIndex != actorsOnOpCount) )         //only possible bc the key is the same as the actor's index in array 
                    {
                swap = ActorsVisInvis[i];
                ActorsVisInvis[i] = ActorsVisInvis[randomIndex];
                ActorsVisInvis[randomIndex] = swap;
            }
        }
       // Debug.Log("Shuffeled list:");
        foreach (int index in ActorsVisInvis)
        {
            Debug.Log(index);
        }

        split = actorsOnOpCount;
        playerIndex = playerActorKey;
    }

    public void PlayerToVis()
    {
        ActorsVisInvis[split] = ActorsVisInvis[split - 1];
        ActorsVisInvis[split - 1] = playerIndex;
    }

    public bool IsVisible(int indexKey)
    {

        int i;
        for (i = 0; i < split; i++)  //look for a match in the visible part
        {
            if (ActorsVisInvis[i]== indexKey)
            {
                return true;
            }
        }
        return false;
    }

    public string ActorsVisPreOpToString()
    {
        string OutputString = "";
        int i;
        for (i = 0; i < split - 1; i++)
        {
            OutputString += Actors[ActorsVisInvis[i]].ToString() + "\n";
        }
        return OutputString;
    }

    public string ActorsVisToString()
    {
        string OutputString = "";
        int i;
        for (i = 0; i < split; i++)
        {
            OutputString += Actors[ActorsVisInvis[i]].ToString() + "\n";
        }
        return OutputString;
    }

    public string ActorsInvisToString()
    {
        string OutputString = "";
        int i;
        for (i = split; i < ActorsVisInvis.Length; i++)
        {
            OutputString += Actors[ActorsVisInvis[i]].ToString() + "\n";
        }
        return OutputString;
    }
}



public enum PlayerState
{ 
        ACTORCHOICE, //choose a character
        PREOPERATION, //Join or not
        PINGING, //which opposite chars to ping (if at all)
        LEAKINGINFO,  //leak info to the real team or no (on/off op)
        PROCESSLEAK, //only for on op - register leak, manage state transition
        POSTOPERATION,  //view log, end turn
        ELIMINATING,
         GAMEENDEVAL,
        MEETINGBOSS,
        GAMEOVER
}