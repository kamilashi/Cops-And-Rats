using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataBase 
{
    public Actor[] stationActors;
    public Actor Pboss;
    public Actor[] mobActors;
    public Actor Mboss;

    public Operation[] DealOpsMob;
    public Operation[] DealOpsPolice;

    public Operation[] RaidOpsPolice;
    public Operation[] RaidOpsMob;

    public DataBase()
    {
        //Police station
        stationActors = new Actor[6];
        Actor pactor1 = new Actor("Mary", " Cotton");
        Actor pactor2 = new Actor("Ben", " Waller");
        Actor pactor3 = new Actor("Wendy", " Morgan");
        Actor pactor4 = new Actor("Henry", " Schultz");
        Actor pactor5 = new Actor("Helen", " Ripley");
        Actor pactor6 = new Actor("Michael", " Kelly");
        Pboss = new Actor("Francis", " Verga");

        //add to array
        stationActors[0] = pactor1;
        stationActors[1] = pactor2;
        stationActors[2] = pactor3;
        stationActors[3] = pactor4;
        stationActors[4] = pactor5;
        stationActors[5] = pactor6;


        //Mob
        mobActors = new Actor[6];
        Actor mactor1 = new Actor("Anna", " Cox");
        Actor mactor2 = new Actor("Eric ", "Sagalski");
        Actor mactor3 = new Actor("Sophie ", "Rothstein");
        Actor mactor4 = new Actor("Marcus ", "Bell");
        Actor mactor5 = new Actor("James", " Allen");
        Actor mactor6 = new Actor("Matthew", " Hill");
         Mboss = new Actor("Charles ", "Hess");

        //add to array
        mobActors[0] = mactor1;
        mobActors[1] = mactor2;
        mobActors[2] = mactor3;
        mobActors[3] = mactor4;
        mobActors[4] = mactor5;
        mobActors[5] = mactor6;


        //Operations
        //Deals
        DealOpsMob = new Operation[3];
        Operation operationDM0 = new Operation(3, 11111, 1);
        Operation operationDM1 = new Operation(3, 12222, 1);
        Operation operationDM2 = new Operation(3, 14444, 1);
        DealOpsMob[0] = operationDM0;
        DealOpsMob[1] = operationDM1;
        DealOpsMob[2] = operationDM2;

        //should be synchronized time- and location-wise!!!
        DealOpsPolice = new Operation[3];
        Operation operationDP0 = new Operation(3, 11111, 1);
        Operation operationDP1 = new Operation(3, 12222, 1);
        Operation operationDP2 = new Operation(3, 14444, 1);
        DealOpsPolice[0] = operationDP0;
        DealOpsPolice[1] = operationDP1;
        DealOpsPolice[2] = operationDP2;

        //Raids
        RaidOpsPolice = new Operation[3];
        Operation operationRP0 = new Operation(3, 13333, 2);
        Operation operationRP1 = new Operation(3, 15555, 2);
        Operation operationRP2 = new Operation(3, 16666, 2);
        RaidOpsPolice[0] = operationRP0;
        RaidOpsPolice[1] = operationRP1;
        RaidOpsPolice[2] = operationRP2;

        //should be synchronized time- and location-wise!!!
        RaidOpsMob = new Operation[3];
        Operation operationRM0 = new Operation(3, 13333, 2);
        Operation operationRM1 = new Operation(3, 15555, 2);
        Operation operationRM2 = new Operation(3, 16666, 2);
        RaidOpsMob[0] = operationRM0;
        RaidOpsMob[1] = operationRM1;
        RaidOpsMob[2] = operationRM2;


    }
}
