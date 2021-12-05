using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataBase 
{
    public Actor[] stationActors;
    public Actor Pboss;
    public Actor[] mobActors;
    public Actor Mboss;

    public DataBase()
    {
        //Police station
        stationActors = new Actor[6];
        Actor Pactor1 = new Actor("Mary", " Cotton");
        Actor Pactor2 = new Actor("Ben", " Waller");
        Actor Pactor3 = new Actor("Wendy", " Morgan");
        Actor Pactor4 = new Actor("Henry", " Schultz");
        Actor Pactor5 = new Actor("Helen", " Ripley");
        Actor Pactor6 = new Actor("Michael", " Kelly");
        Pboss = new Actor("Francis", " Verga");

        //add to array
        stationActors[0] = Pactor1;
        stationActors[1] = Pactor2;
        stationActors[2] = Pactor3;
        stationActors[3] = Pactor4;
        stationActors[4] = Pactor5;
        stationActors[5] = Pactor6;


        //Mob
        mobActors = new Actor[6];
        Actor Mactor1 = new Actor("Anna", " Cox");
        Actor Mactor2 = new Actor("Eric ", "Sagalski");
        Actor Mactor3 = new Actor("Sophie ", "Rothstein");
        Actor Mactor4 = new Actor("Marcus ", "Bell");
        Actor Mactor5 = new Actor("James", " Allen");
        Actor Mactor6 = new Actor("Matthew", " Hill");
         Mboss = new Actor("Charles ", "Hess");

        //add to array
        mobActors[0] = Mactor1;
        mobActors[1] = Mactor2;
        mobActors[2] = Mactor3;
        mobActors[3] = Mactor4;
        mobActors[4] = Mactor5;
        mobActors[5] = Mactor6;


    }
}
