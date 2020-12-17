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

    public bulletType currBulletType;

    // TODO: change these to lists so that they're easier to update
    // private List<GameObject> bullets;
    private GameObject bullet0; // bullet type 0
    private GameObject bullet1; // bullet type 1

    public GameObject parentObject;

    // public Queue<GameObject> m_playerBulletPool;

    // bullet pools
    public Queue<GameObject> bulletPool0;
    public Queue<GameObject> bulletPool1;

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

        // create empty Queue structures
        // m_playerBulletPool = new Queue<GameObject>();
        bulletPool0 = new Queue<GameObject>();
        bulletPool1 = new Queue<GameObject>();

        {
            // loading bullet types
            bullet0 = (GameObject)Resources.Load("Prefabs/Bullet");
            bullet1 = (GameObject)Resources.Load("Prefabs/Bullet1");

            // loads a prefab
            // if (bullet == null)
            //     bullet = (GameObject)Resources.Load("Prefabs/Bullet");

        }

        // bullet count
        // for (int count = 0; count < MaxBullets; count++)
        // {
        //     var tempPlayerBullet = MonoBehaviour.Instantiate(bullet);
        //     tempPlayerBullet.transform.SetParent(parentObject.transform);
        // 
        //     tempPlayerBullet.SetActive(false);
        //     m_playerBulletPool.Enqueue(tempPlayerBullet);
        // }

        // creates each bullet
        for(int b = 0; b < BULLET_TYPE_COUNT; b++)
        {
            // bullet type being generated
            GameObject bullet = null;

            switch(b) // gets the bullet type
            {
                case ((int)bulletType.sphere): // sphere copy
                    bullet = bullet0;
                    break;

                case ((int)bulletType.cube): // cube copy
                    bullet = bullet1;
                    break;
            }

            // if a bullt has been set
            if(bullet != null)
            {
                // makes all bullets for the given queue
                for (int count = 0; count < MaxBullets; count++)
                {
                    GameObject tempPlayerBullet = MonoBehaviour.Instantiate(bullet);

                    tempPlayerBullet.transform.SetParent(parentObject.transform);
                    tempPlayerBullet.SetActive(false);

                    // adds the instantiated bullet to the proper queue
                    switch (b)
                    {
                        case ((int)bulletType.sphere): // sphere copy
                            bulletPool0.Enqueue(tempPlayerBullet);
                            break;

                        case ((int)bulletType.cube): // cube copy
                            bulletPool1.Enqueue(tempPlayerBullet);
                            break;
                    }
                }
            }

            
        }

        

    }

    // gets the current bullet type
    private GameObject GetCurrentBulletBase()
    {
        GameObject currBullet = null;

        // gets the current bullet base
        switch (currBulletType)
        {
            case bulletType.sphere:
                currBullet = bullet0;
                break;

            case bulletType.cube:
                currBullet = bullet1;
                break;
        }

        return currBullet;
    }

    // gets the current queue
    private Queue<GameObject> GetCurrentBulletQueue()
    {
        Queue<GameObject> currBulletPool = null;

        // gets the current bullet queue
        switch (currBulletType)
        {
            case bulletType.sphere:
                currBulletPool = bulletPool0;
                break;

            case bulletType.cube:
                currBulletPool = bulletPool1;
                break;
        }

        // returns the current bullet pool.
        return currBulletPool;

    }

    // gets a bullet
    public GameObject GetBullet(Vector3 position, Vector3 direction)
    {
        // new bullet being made
        GameObject newBullet = null;
        Queue<GameObject> currQueue = GetCurrentBulletQueue();

        // if (m_playerBulletPool.Count > 0) // elements in queue
        // {
        //     newBullet = m_playerBulletPool.Dequeue();
        // }
        // else // no elements in queue - will be added to the queue after the bullet is returned.
        // {
        //     newBullet = MonoBehaviour.Instantiate(bullet);
        //     newBullet.transform.SetParent(parentObject.transform);
        // }

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
        // return m_playerBulletPool.Count > 0;

        bool check = false;

        // checks if there are bullets available in the current queue.
        switch (currBulletType)
        {
            case bulletType.sphere:
                check = bulletPool0.Count > 0;
                break;

            case bulletType.cube:
                check = bulletPool1.Count > 0;
                break;
        }

        return check;
    }

    // returns the bullets
    public void ReturnBullet(GameObject returnedBullet)
    {
        returnedBullet.SetActive(false);
        //  m_playerBulletPool.Enqueue(returnedBullet);

        BulletManager.bulletType returnedType = returnedBullet.GetComponent<BulletBehaviour>().bulletType;

        switch(returnedType)
        {
            case BulletManager.bulletType.sphere: // sphere
                bulletPool0.Enqueue(returnedBullet);
                break;

            case BulletManager.bulletType.cube: // cube
                bulletPool1.Enqueue(returnedBullet);
                break;
        }
    }
}