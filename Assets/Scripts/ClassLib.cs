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

    public Operation(){ }
    public Operation(int ActorsCountPolice, int ActorsCountMob, int date, int t)
    {
        this.ActorsCountPolice = ActorsCountPolice;
        this.ActorsCountMob = ActorsCountMob;
        this.Date = date;
        this.type = (OPtype) t;
    }
    public Operation(int ActorsCountPolice, int ActorsCountMob, int date, int t, string loc)
    {
        this.ActorsCountPolice = ActorsCountPolice;
        this.ActorsCountMob = ActorsCountMob;
        this.Date = date;
        this.type = (OPtype) t;
        this.Location = loc;
    }

    public string toString()
    { 
        return ( (OPtype) type + ": d: " + Date + " l: " + Location + " police count: " + ActorsCountPolice + " mob count: " + ActorsCountMob);
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
    public Actor[] ActorsInvis { get; set; }   //?
    public Actor[] ActorsOnOP { get; set; }   //?
    public Actor[] ActorsOnBase { get; set; }   //?
    public Actor Boss { get; set; }

    public int AdvancePoints { get; set; }
    //public Operation NextOp { get; set; }
    //public Operation OperationPlanned { get; set; }
    //public int CurrentOPIndex { get; set; }
    public bool HasIntel { get; set; }
    //public Operation EnemyInputNextOp { get; set; }

    // Start is called before the first frame update
    public Team()
    {
        AdvancePoints = 0;
        HasIntel = false;
        Actors = new Actor[7];
        ActorsOnBase = new Actor[7]; 
        ActorsInvis = new Actor[7];
        ActorsOnOP = new Actor[7];
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
        ACTORCHOICE,
        PREOPERATION,
        PING,
        ONOPERATION,
        OFFOPERATION,
        MEETINGBOSS,
        POSTOPERATION,
        NONE
}