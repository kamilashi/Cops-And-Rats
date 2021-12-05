using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class Actor : MonoBehaviour
{
    public string Name { get; set; }
    public bool IsAlive;

    // Start is called before the first frame update
    void Awake()
    {
        IsAlive = true;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
