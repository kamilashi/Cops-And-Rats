using System.Collections;
using System.Collections.Generic;
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

    public string toString()
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
     this.LName = lname; }

    
    public string ToString()
    {
        return FName + " " + LName;
    }
}

public class Team 
{
    public Actor[] Actors { get; set; } // array of 10
    public Actor[] ActorsInvisible { get; set; }   //?
    public Actor[] ActorsVisible { get; set; }   //?
    public Actor Boss { get; set; }

    public int Points { get; set; }
    //public bool HasIntel { get; set; }
    public bool LeakDetected { get; set; } //shanged by the opposite team if the enemy player leaks info during op (Leo leaks --> Mob.LeakDetected = true)

    // Start is called before the first frame update
    public Team()
    {
        Points = 0;
        Actors = new Actor[7];
        ActorsVisible = new Actor[7];
        ActorsInvisible = new Actor[7];
    }

    public string ActorsWOBossToString()
    {
        string OutputString = "";
        int i = 1;
        foreach (Actor actor in Actors)
        {
            OutputString += i++ + ". " + actor.ToString() + "\n";
        }

        return OutputString;
    }
    public string ActorsWBossToString()
    {
        string OutputString = "1. " + Boss.ToString() + " (Boss)\n";
        int i = 2;
        foreach (Actor actor in Actors)
        {
            OutputString += i++ + ". " + actor.ToString() + "\n";
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