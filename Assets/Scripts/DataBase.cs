using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataBase 
{
    public Actor[] stationActors;
    public Actor Pboss;
    public Actor[] mobActors;
    public Actor Mboss;

    public Operation[] DealOps;

    public Operation[] RaidOps;

    public DataBase()
    {
        //Police station
        stationActors = new Actor[7];
        Actor pactor1 = new Actor("Mary", "Cotton");
        Actor pactor2 = new Actor("Ben", "Waller");
        Actor pactor3 = new Actor("Wendy", "Morgan");
        Actor pactor4 = new Actor("Henry", "Schultz");
        Actor pactor5 = new Actor("Helen", "Ripley");
        Actor pactor6 = new Actor("Michael", "Kelly");
        Pboss = new Actor("Francis", "Verga");

        //add to array
        stationActors[0] = Pboss;
        stationActors[1] = pactor1;
        stationActors[2] = pactor2;
        stationActors[3] = pactor3;
        stationActors[4] = pactor4;
        stationActors[5] = pactor5;
        stationActors[6] = pactor6;

        //assign keys:
        int i;
        for (i = 0; i < stationActors.Length; i++)
        { stationActors[i].Key = i; }


        //Mob
        mobActors = new Actor[7];
        Actor mactor1 = new Actor("Anna", "Cox");
        Actor mactor2 = new Actor("Eric","Sagalski");
        Actor mactor3 = new Actor("Sophie", "Rothstein");
        Actor mactor4 = new Actor("Marcus", "Bell");
        Actor mactor5 = new Actor("James", "Allen");
        Actor mactor6 = new Actor("Matthew", "Hill");
         Mboss = new Actor("Charles", "Hess");

        //add to array
        mobActors[0] = Mboss;
        mobActors[1] = mactor1;
        mobActors[2] = mactor2;
        mobActors[3] = mactor3;
        mobActors[4] = mactor4;
        mobActors[5] = mactor5;
        mobActors[6] = mactor6;

        for (i = 0; i < mobActors.Length; i++)
        { mobActors[i].Key = i; }


        //Operations
        //Deals
        DealOps = new Operation[3];
        Operation operationD0 = new Operation(5,5, 11111, 0);
        Operation operationD1 = new Operation(3, 3, 12222, 0);
        Operation operationD2 = new Operation(3, 3, 14444, 0);
        DealOps[0] = operationD0;
        DealOps[1] = operationD1;
        DealOps[2] = operationD2;

        //should be synchronized time- and location-wise!!!
        //DealOpsPolice = new Operation[3];
        //Operation operationDP0 = new Operation(3, 11111, 1);
        //Operation operationDP1 = new Operation(3, 12222, 1);
        //Operation operationDP2 = new Operation(3, 14444, 1);
        //DealOpsPolice[0] = operationDP0;
        //DealOpsPolice[1] = operationDP1;
        //DealOpsPolice[2] = operationDP2;

        //Raids
        RaidOps = new Operation[3];
        Operation operationR0 = new Operation(3, 3, 13333, 1);
        Operation operationR1 = new Operation(3, 3, 15555, 1);
        Operation operationR2 = new Operation(3, 3, 16666, 1);
        RaidOps[0] = operationR0;
        RaidOps[1] = operationR1;
        RaidOps[2] = operationR2;

        //should be synchronized time- and location-wise!!!
        //RaidOpsMob = new Operation[3];
        //Operation operationRM0 = new Operation(3, 13333, 2);
        //Operation operationRM1 = new Operation(3, 15555, 2);
        //Operation operationRM2 = new Operation(3, 16666, 2);
        //RaidOpsMob[0] = operationRM0;
        //RaidOpsMob[1] = operationRM1;
        //RaidOpsMob[2] = operationRM2;


    }
}
