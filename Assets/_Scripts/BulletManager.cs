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

    // instance of bullet pool manager.
    private static BulletManager instance = null;

    public int MaxBullets = 50;
    public GameObject bullet; // current bullet

    public bulletType m_playerBulletType;

    private GameObject bullet0; // bullet type 0
    private GameObject bullet1; // bullet type 1

    public GameObject parentObject;

    public Queue<GameObject> m_playerBulletPool;

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
        m_playerBulletPool = new Queue<GameObject>();

        {
            // loading bullet types
            bullet0 = (GameObject)Resources.Load("Prefabs/Bullet");
            bullet1 = (GameObject)Resources.Load("Prefabs/Bullet1");


        }

        // loads a prefab
        if (bullet == null)
            bullet = (GameObject)Resources.Load("Prefabs/Bullet");

        // bullet count
        for (int count = 0; count < MaxBullets; count++)
        {
            var tempPlayerBullet = MonoBehaviour.Instantiate(bullet);
            tempPlayerBullet.transform.SetParent(parentObject.transform);

            tempPlayerBullet.SetActive(false);
            m_playerBulletPool.Enqueue(tempPlayerBullet);
        }
    }

    public GameObject GetBullet(Vector3 position, Vector3 direction)
    {
        GameObject newBullet = null;
        newBullet = m_playerBulletPool.Dequeue();
        newBullet.SetActive(true);
        newBullet.transform.position = position;
        newBullet.GetComponent<BulletBehaviour>().direction = direction;

        return newBullet;
    }

    public bool HasBullets()
    {
        return m_playerBulletPool.Count > 0;
    }

    public void ReturnBullet(GameObject returnedBullet)
    {
        returnedBullet.SetActive(false);
        m_playerBulletPool.Enqueue(returnedBullet);
    }
}