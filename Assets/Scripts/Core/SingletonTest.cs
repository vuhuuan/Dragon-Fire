using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonTest : MonoBehaviour
{
    public static int count;
    // Start is called before the first frame update
    void Start()
    {
        ++count;
        Debug.Log(count);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
