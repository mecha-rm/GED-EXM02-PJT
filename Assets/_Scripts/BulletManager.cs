using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// original bullet manager
// public class BulletManager : MonoBehaviour
// {
//     public int MaxBullets;
//     public GameObject bullet;
// 
//     public Queue<GameObject> m_playerBulletPool;
// 
//     // Start is called before the first frame update
//     void Start()
//     {
//         _BuildBulletPool();
//     }
// 
//     private void _BuildBulletPool()
//     {
//         // create empty Queue structures
//         m_playerBulletPool = new Queue<GameObject>();
// 
//         for (int count = 0; count < MaxBullets; count++)
//         {
//             var tempPlayerBullet = Instantiate(bullet);
//             tempPlayerBullet.transform.SetParent(transform);
//             tempPlayerBullet.SetActive(false);
//             m_playerBulletPool.Enqueue(tempPlayerBullet);
//         }
//     }
// 
//     public GameObject GetBullet(Vector3 position, Vector3 direction)
//     {
//         GameObject newBullet = null;
//         newBullet = m_playerBulletPool.Dequeue();
//         newBullet.SetActive(true);
//         newBullet.transform.position = position;
//         newBullet.GetComponent<BulletBehaviour>().direction = direction;
// 
//         return newBullet;
//     }
// 
//     public bool HasBullets()
//     {
//         return m_playerBulletPool.Count > 0;
//     }
// 
//     public void ReturnBullet(GameObject returnedBullet)
//     {
//         returnedBullet.SetActive(false);
//         m_playerBulletPool.Enqueue(returnedBullet);
//     }
//     
// }



[System.Serializable]
public class BulletManager
{
    public enum bulletType {sphere, cube};
    public const int BULLET_TYPE_COUNT = 2;

    // instance of bullet pool manager.
    private static BulletManager instance = null;

    public int MaxBullets = 3; // originally 50
    // public GameObject bullet; // current bullet

    // current bullet type
    public bulletType currBulletType;

    // base bullets
    private List<GameObject> bulletBases;

    // parent object
    // TODO: maybe make subparento objects?
    public GameObject parentObject; // main parent object

    // bullet pools
    public List<Queue<GameObject>> bulletPools;

    // constructor
    private BulletManager()
    {
        Start();
    }

    // gets instance of class (for bonus)
    public static BulletManager GetInstance()
    {
        if (instance == null) // no instance generated.
        {
            instance = new BulletManager(); // creates instance
        }

        return instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        _BuildBulletPool();
    }

    private void _BuildBulletPool()
    {
        // creates parent object
        if (parentObject == null)
        {
            // looks for bullet manager object
            parentObject = GameObject.Find("BulletManager");

            // if the bullet manager object was not made, then make it.
            if(parentObject == null)
                parentObject = new GameObject("BulletManager");
        }

        // initialize bullet bases list and pools.
        bulletBases = new List<GameObject>();
        bulletPools = new List<Queue<GameObject>>(); 

        // adding bullet pools and bullets
        for (int i = 0; i < BULLET_TYPE_COUNT; i++)
        {
            bulletPools.Add(new Queue<GameObject>());

            // adds bullet types as bases
            switch (i)
            {
                case ((int)bulletType.sphere): // sphere base
                    bulletBases.Add((GameObject)Resources.Load("Prefabs/Bullet"));
                    break;

                case ((int)bulletType.cube): // cube base
                    bulletBases.Add((GameObject)Resources.Load("Prefabs/Bullet1"));
                    break;
            }
        }

        // creates each bullet
        // for (int b = 0; b < BULLET_TYPE_COUNT; b++)
        for (int b = 0; b < bulletBases.Count; b++)
        {
            // bullet type being generated
            GameObject bullet = null;

            // gets the current bullet base.
            bullet = bulletBases[b];
            
            // if a bullt has been set
            if(bullet != null)
            {
                // makes all bullets for the given queue
                for (int count = 0; count < MaxBullets; count++)
                {
                    GameObject tempPlayerBullet = MonoBehaviour.Instantiate(bullet);
            
                    tempPlayerBullet.transform.SetParent(parentObject.transform);
                    tempPlayerBullet.SetActive(false);

                    bulletPools[(int)b].Enqueue(tempPlayerBullet);
                }
            }
        }
    }

    // returns the current bullet type
    public bulletType GetCurrentBulletType()
    {
        return currBulletType;
    }

    // gets the current bullet type as an integer
    public int GetCurrentBulletTypeAsInt()
    {
        return (int)currBulletType;
    }

    // sets the current bullet type
    public void SetCurrentBulletType(bulletType newType)
    {
        currBulletType = newType;
    }

    // sets the current bullet type
    public void SetCurrentBulletType(int newType)
    {
        
        currBulletType = (bulletType)Mathf.Clamp(newType, 0, BULLET_TYPE_COUNT);
    }

    // gets the current bullet type
    private GameObject GetCurrentBulletBase()
    {
        return bulletBases[(int)currBulletType];
    }

    // gets the current queue
    private Queue<GameObject> GetCurrentBulletQueue()
    {
        return bulletPools[(int)currBulletType];
    }

    // gets a bullet
    public GameObject GetBullet(Vector3 position, Vector3 direction)
    {
        // new bullet being made
        GameObject newBullet = null;
        Queue<GameObject> currQueue = GetCurrentBulletQueue();

        if (currQueue.Count > 0) // elements in queue
        {
            newBullet = currQueue.Dequeue();
        }
        else // no elements in queue - will be added to the queue after the bullet is returned.
        {
            GameObject bullet = GetCurrentBulletBase();
            newBullet = MonoBehaviour.Instantiate(bullet);
            newBullet.transform.SetParent(parentObject.transform);
        }

        
        newBullet.SetActive(true);
        newBullet.transform.position = position;
        newBullet.GetComponent<BulletBehaviour>().direction = direction;

        return newBullet;
    }

    // manager has bullets
    public bool HasBullets()
    {
        return bulletPools[(int)currBulletType].Count > 0;
    }

    // returns the bullets
    public void ReturnBullet(GameObject returnedBullet)
    {
        returnedBullet.SetActive(false);
        //  m_playerBulletPool.Enqueue(returnedBullet);

        BulletManager.bulletType returnedType = returnedBullet.GetComponent<BulletBehaviour>().bulletType;

        // returns the bullet to its queue
        bulletPools[(int)returnedType].Enqueue(returnedBullet);
    }
}