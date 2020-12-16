using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BulletFactory
{
    // instance of bullet pool manager.
    private static BulletFactory instance = null;

    // constructor
    private BulletFactory()
    {
        Start();
    }

    // gets instance of class (for bonus)
    public static BulletFactory GetInstance()
    {
        if (instance == null) // no instance generated.
        {
            instance = new BulletFactory(); // creates instance
        }

        return instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
