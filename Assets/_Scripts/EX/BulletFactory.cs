using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BulletFactory
{
    // instance of bullet pool manager.
    private static BulletFactory instance = null;

    public int MaxBullets = 50;
    public GameObject bullet;
    public GameObject parentObject;

    public Queue<GameObject> m_playerBulletPool;

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
        _BuildBulletPool();
    }

    private void _BuildBulletPool()
    {
        // create empty Queue structures
        m_playerBulletPool = new Queue<GameObject>();

        // loads a prefab
        if (bullet == null)
            bullet = (GameObject)Resources.Load("Prefabs/Bullet");

        for (int count = 0; count < MaxBullets; count++)
        {
            var tempPlayerBullet = MonoBehaviour.Instantiate(bullet);

            if(parentObject != null)
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
