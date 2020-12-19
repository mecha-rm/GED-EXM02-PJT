using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CollisionManager : MonoBehaviour
{
    public CubeBehaviour[] cubes;
    public BulletBehaviour[] spheres;

    private static Vector3[] faces;

    // Start is called before the first frame update
    void Start()
    {
        cubes = FindObjectsOfType<CubeBehaviour>();

        faces = new Vector3[]
        {
            Vector3.left, Vector3.right,
            Vector3.down, Vector3.up,
            Vector3.back , Vector3.forward
        };
    }

    // Update is called once per frame
    void Update()
    {
        spheres = FindObjectsOfType<BulletBehaviour>();

        // check each AABB with every other AABB in the scene
        for (int i = 0; i < cubes.Length; i++)
        {
            for (int j = 0; j < cubes.Length; j++)
            {
                if (i != j)
                {
                    CheckAABBs(cubes[i], cubes[j]);
                }
            }
        }

        // Check each sphere against each AABB in the scene
        foreach (var sphere in spheres) // bullet collision with cubes
        {
            foreach (var cube in cubes)
            {
                // if the object has been deactivated, it shouldn't trigger collisions.
                // if (cube.name != "Player")
                if (cube.name != "Player" && cube.gameObject.activeInHierarchy == true)
                {
                    CheckSphereAABB(sphere, cube);
                }
                
            }
        }
    }

    public static void CheckSphereAABB(BulletBehaviour s, CubeBehaviour b)
    {

        // get box closest point to sphere center by clamping
        var x = Mathf.Max(b.min.x, Mathf.Min(s.transform.position.x, b.max.x));
        var y = Mathf.Max(b.min.y, Mathf.Min(s.transform.position.y, b.max.y));
        var z = Mathf.Max(b.min.z, Mathf.Min(s.transform.position.z, b.max.z));

        var distance = Math.Sqrt((x - s.transform.position.x) * (x - s.transform.position.x) +
                                 (y - s.transform.position.y) * (y - s.transform.position.y) +
                                 (z - s.transform.position.z) * (z - s.transform.position.z));

        if ((distance < s.radius) && (!s.isColliding))
        {
            // determine the distances between the contact extents
            float[] distances = {
                (b.max.x - s.transform.position.x),
                (s.transform.position.x - b.min.x),
                (b.max.y - s.transform.position.y),
                (s.transform.position.y - b.min.y),
                (b.max.z - s.transform.position.z),
                (s.transform.position.z - b.min.z)
            };

            float penetration = float.MaxValue;
            Vector3 face = Vector3.zero;

            // check each face to see if it is the one that connected
            for (int i = 0; i < 6; i++)
            {
                if (distances[i] < penetration)
                {
                    // determine the penetration distance
                    penetration = distances[i];
                    face = faces[i];
                }
            }

            s.penetration = penetration;
            s.collisionNormal = face;
            //s.isColliding = true;

            
            Reflect(s);

            // collision has happened. Uncomment to keep cubes.
            b.gameObject.SetActive(false);
        }

    }
    
    // This helper function reflects the bullet when it hits an AABB face
    private static void Reflect(BulletBehaviour s)
    {
        if ((s.collisionNormal == Vector3.forward) || (s.collisionNormal == Vector3.back))
        {
            s.direction = new Vector3(s.direction.x, s.direction.y, -s.direction.z);
        }
        else if ((s.collisionNormal == Vector3.right) || (s.collisionNormal == Vector3.left))
        {
            s.direction = new Vector3(-s.direction.x, s.direction.y, s.direction.z);
        }
        else if ((s.collisionNormal == Vector3.up) || (s.collisionNormal == Vector3.down))
        {
            s.direction = new Vector3(s.direction.x, -s.direction.y, s.direction.z);
        }
    }


    public static void CheckAABBs(CubeBehaviour a, CubeBehaviour b)
    {
        Contact contactB = new Contact(b);

        if ((a.min.x <= b.max.x && a.max.x >= b.min.x) &&
            (a.min.y <= b.max.y && a.max.y >= b.min.y) &&
            (a.min.z <= b.max.z && a.max.z >= b.min.z))
        {
            // determine the distances between the contact extents
            float[] distances = {
                (b.max.x - a.min.x),
                (a.max.x - b.min.x),
                (b.max.y - a.min.y),
                (a.max.y - b.min.y),
                (b.max.z - a.min.z),
                (a.max.z - b.min.z)
            };

            float penetration = float.MaxValue;
            Vector3 face = Vector3.zero;

            // check each face to see if it is the one that connected
            for (int i = 0; i < 6; i++)
            {
                if (distances[i] < penetration)
                {
                    // determine the penetration distance
                    penetration = distances[i];
                    face = faces[i];
                }
            }
            
            // set the contact properties
            contactB.face = face;
            contactB.penetration = penetration;


            // check if contact does not exist
            if (!a.contacts.Contains(contactB))
            {
                // remove any contact that matches the name but not other parameters
                for (int i = a.contacts.Count - 1; i > -1; i--)
                {
                    if (a.contacts[i].cube.name.Equals(contactB.cube.name))
                    {
                        a.contacts.RemoveAt(i);
                    }
                }

                if (contactB.face == Vector3.down)
                {
                    a.gameObject.GetComponent<RigidBody3D>().Stop();
                    a.isGrounded = true;
                }
                

                // add the new contact
                a.contacts.Add(contactB);
                a.isColliding = true;
                
            }
        }
        else
        {

            if (a.contacts.Exists(x => x.cube.gameObject.name == b.gameObject.name))
            {
                a.contacts.Remove(a.contacts.Find(x => x.cube.gameObject.name.Equals(b.gameObject.name)));
                a.isColliding = false;

                if (a.gameObject.GetComponent<RigidBody3D>().bodyType == BodyType.DYNAMIC)
                {
                    a.gameObject.GetComponent<RigidBody3D>().isFalling = true;
                    a.isGrounded = false;
                }
            }
        }
    }

    // removes a cube from the list via its value
    // if 'keepOrder' is true, then the array is reshuffled. 
    public void RemoveCubeFromList(CubeBehaviour cb, bool keepOrder = false)
    {
        // null provided
        if (cb == null)
            return;

        int index = -1; // index of provided cube behaviour

        // goes through the list of cubes to find the provided one.
        for(int i = 0; i < cubes.Length; i++)
        {
            // if the cube behaviour has been found.
            if(cubes[i] == cb)
            {
                index = i;
                break;
            }
        }

        // item not found
        if (index == -1)
            return;
        
        // switches spots
        cubes[index] = cubes[cubes.Length - 1];
        cubes[cubes.Length - 1] = cb;

        // removes last component
        Array.Resize<CubeBehaviour>(ref cubes, cubes.Length - 1);

        // if the order should be kept
        if(keepOrder)
        {
            // moves swapped item back to its original position
            for(int i = index; i < cubes.Length - 1; i++)
            {
                CubeBehaviour temp = cubes[i];
                cubes[i] = cubes[i + 1];
                cubes[i + 1] = temp;

            }
        }
    }

    // removes a cube from the list via its index
    // if 'keepOrder' is true, then the array is reshuffled. 
    public CubeBehaviour RemoveCubeFromList(int index, bool keepOrder = false)
    {
        // item not found
        if (index < 0 || index >= cubes.Length)
            return null;

        // gets the cube and switches places with last index
        CubeBehaviour cb = cubes[index];
        cubes[index] = cubes[cubes.Length - 1];
        cubes[cubes.Length - 1] = cb;

        // removes last component
        Array.Resize<CubeBehaviour>(ref cubes, cubes.Length - 1);

        // if the order should be kept
        if (keepOrder)
        {
            // moves swapped item back to its original position
            for (int i = index; i < cubes.Length - 1; i++)
            {
                CubeBehaviour temp = cubes[i];
                cubes[i] = cubes[i + 1];
                cubes[i + 1] = temp;

            }
        }

        return cb;
    }

    // clears the cube list
    public void ClearCubeList()
    {
        Array.Clear(cubes, 0, cubes.Length);
        Array.Resize<CubeBehaviour>(ref cubes, 0);
    }

    // destroys all cubes in the cube list.
    public void DestroyCubesInList()
    {
        // while there are cubes to delete - deletes one by one
        // while(cubes.Length != 0)
        // {
        //     CubeBehaviour cb = RemoveCubeFromList(0);
        //     Destroy(cb.gameObject);
        //     Destroy(cb);
        // }

        // destroys all game objects
        foreach (CubeBehaviour cb in cubes)
        {
            cb.enabled = false; // disables cube behaviour so it won't be found again.
            Destroy(cb.gameObject); // destroys game object.
            Destroy(cb); // destroys cube behaviour
        }
            

        Array.Clear(cubes, 0, cubes.Length); // deletes cube behaviour data
        Array.Resize<CubeBehaviour>(ref cubes, 0); // brings it down to 0.
    }

    // refreshes the list of cubes
    public void RefreshCubeList(bool includeInactive = true)
    {
        Array.Resize<CubeBehaviour>(ref cubes, 0);  // clears array (may not be needed?)
        cubes = FindObjectsOfType<CubeBehaviour>(includeInactive); // fills array again

        Array.Sort(cubes);

    }

    // resets the whole round
    // public void ResetScene()
    // {
    //     // will need ot be changed for new input system
    //     if(Input.GetKeyDown(KeyCode.R)) // re-enables all cubes
    //     {
    //         for (int i = 0; i < cubes.Length; i++)
    //             cubes[i].gameObject.SetActive(true);
    //     }
    // }

    // resets the blocks
    public void ResetCubes()
    {
        // will need ot be changed for new input system
        for (int i = 0; i < cubes.Length; i++) // re-enables all cubes
        {
            // this sometimes causes an error of the entity is already visible when stopping the game.
            // i don't know how to fix this but it's not a system breaking issue.
            cubes[i].gameObject.SetActive(true);
        }
    }
}
