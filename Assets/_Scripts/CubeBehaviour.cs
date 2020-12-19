using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Color = UnityEngine.Color;

[System.Serializable]
public class Contact : IEquatable<Contact>
{
    public CubeBehaviour cube;
    public Vector3 face;
    public float penetration;

    public Contact(CubeBehaviour cube)
    {
        this.cube = cube;
        face = Vector3.zero;
        penetration = 0.0f;
    }

    public override bool Equals(object obj)
    {
        if (obj == null) return false;
        Contact objAsContact = obj as Contact;
        if (objAsContact == null) return false;
        else return Equals(objAsContact);
    }

    public override int GetHashCode()
    {
        return this.cube.gameObject.GetInstanceID();
    }

    public bool Equals(Contact other)
    {
        if (other == null) return false;

        return (
            (this.cube.gameObject.name.Equals(other.cube.gameObject.name)) &&
            (this.face == other.face) &&
            (Mathf.Approximately(this.penetration, other.penetration))
            );
    }

    public override string ToString()
    {
        return "Cube Name: " + cube.gameObject.name + " face: " + face.ToString() + " penetration: " + penetration;
    }
}


[System.Serializable]
public class CubeBehaviour : MonoBehaviour
{
    // extra - saves prefab (include file path as well)
    public string prefab;

    [Header("Cube Attributes")]
    public Vector3 size;
    public Vector3 max;
    public Vector3 min;
    public bool isColliding;
    public bool debug;
    public List<Contact> contacts;

    private MeshFilter meshFilter;
    public Bounds bounds;
    public bool isGrounded;

    // EX - death plane for respawning
    private float deathPlaneY = -50.0F;
    private bool useDeathPlane = true;

    // spawn position
    public Vector3 spawnPos;
    public bool startupPosSpawn = true; // if 'ture', the start up position is the spawn position.

    // Start is called before the first frame update
    void Start()
    {
        debug = false;
        meshFilter = GetComponent<MeshFilter>();

        bounds = meshFilter.mesh.bounds;
        size = bounds.size;


        if (startupPosSpawn) // saves current positon as startup spawn
            spawnPos = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        max = Vector3.Scale(bounds.max, transform.localScale) + transform.position;
        min = Vector3.Scale(bounds.min, transform.localScale) + transform.position;

    }

    // death check
    void FixedUpdate()
    {
        DeathCheck();
    }

    private void OnDrawGizmos()
    {
        if (debug)
        {
            Gizmos.color = Color.magenta;

            Gizmos.DrawWireCube(transform.position, Vector3.Scale(new Vector3(1.0f, 1.0f, 1.0f), transform.localScale));
        }
    }

    // checks to see if the player is in the death zone
    private void DeathCheck()
    {
        if (!useDeathPlane)
            return;

        // reset spawn position if hit death plane.
        if (transform.position.y <= deathPlaneY)
            transform.position = spawnPos;
    }
}
