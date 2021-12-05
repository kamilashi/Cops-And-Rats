using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class Team : MonoBehaviour
{
    public Actor[] Actors { get; set; } // array of 10
    public Actor[] ActorsOnBase { get; set; }   //?
    public Actor[] ActorsOnCurrentTask {get; set;}   //?
    public Actor Boss { get; set; }

    public Operation NextOperation { get; set; }
    public int AdvancePoints { get; set; }
    public Operation EnemyInputNextOp { get; set; }

    // Start is called before the first frame update
    void Awake()
    {
        AdvancePoints = 0;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
