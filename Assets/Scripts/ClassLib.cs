using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassLib : MonoBehaviour
{
}

public class Operation
{
    public int ActorsCount { get; set; }
    public int Date { get; set; }
    public string Location { get; set; }

    public Operation(){ }

    public Operation( int ActorsCount, int date, string loc)
    {
        this.ActorsCount = ActorsCount;
        this.Date = date;
        this.Location = loc;
    }
}

    public class Actor 
{
    public string FName { get; set; }
        public string LName { get; set; }
        public bool IsAlive;

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
    public Actor[] ActorsOnBase { get; set; }   //?
    public Actor[] ActorsOnCurrentOP { get; set; }   //?
    public Actor Boss { get; set; }

    public int AdvancePoints { get; set; }
    public Operation NextOperation { get; set; }
    public Operation EnemyInputNextOp { get; set; }

    // Start is called before the first frame update
    public Team()
    {
        AdvancePoints = 0;
        Actors = new Actor[6];
    }

}



public enum PlayerState
{ 
        ACTORCHOICE,
        READY,
        THINKING,
        NONE
}