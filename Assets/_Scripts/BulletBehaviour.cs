using UnityEngine;

// behaviour for bullets.
[System.Serializable]
public class BulletBehaviour : MonoBehaviour
{
    public float speed;
    public Vector3 direction;
    public float range;
    public float radius;
    public bool debug;
    public bool isColliding;
    public Vector3 collisionNormal;
    public float penetration;

    // public BulletManager bulletManager;
    public BulletManager.bulletType bulletType;

    // Start is called before the first frame update
    void Start()
    {
        isColliding = false;
        radius = Mathf.Max(transform.localScale.x, transform.localScale.y, transform.localScale.z) * 0.5f;
        // bulletManager = FindObjectOfType<BulletManager>();
    }

    // Update is called once per frame
    void Update()
    {
        _Move();
        _CheckBounds();
    }

    private void _Move()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void _CheckBounds()
    {
        if (Vector3.Distance(transform.position, Vector3.zero) > range)
        {
            // bulletManager.ReturnBullet(this.gameObject);
            BulletManager.GetInstance().ReturnBullet(this.gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        if (debug)
        {
            Gizmos.color = Color.magenta;

            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }

    // called on collision
    // private void OnCollisionEnter(Collision collision)
    // {
    //     // bullet collided with block
    //     if (collision.gameObject.tag == "Block")
    //     {
    //         collision.gameObject.SetActive(false);
    //     }
    // }

    // gets the type of the bullet
    public BulletManager.bulletType GetBulletType()
    {
        return bulletType;
    }

    // sets the type of the bullet
    public void SetBulletType(BulletManager.bulletType type)
    {
        this.bulletType = type;
    }
}
