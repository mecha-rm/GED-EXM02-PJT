using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

// vector 3 serializable
[System.Serializable]
public struct Vec3 { public float x, y, z; };

// vector 4 serializable
[System.Serializable]
public struct Vec4 { public float x, y, z, w; };

// quaternion serializable
[System.Serializable]
public struct Quat { public float x, y, z, w; };


[System.Serializable]
public struct SerializablePrefabObject
{
    public string prefab; // include path from Prefabs folder.
    public Vec3 position;
    public Quat rotation;
    public Vec3 scale;
}

// data manager extension
public class GameStateLoader : DataManager
{
    // TODO: seperate file from file path at some other time.
    public string file = "";

    // game state object
    public List<CubeBehaviour> blocks = new List<CubeBehaviour>();

    // if 'true', then blocks are added as children.
    public bool importAsChildren = false;

    // seraches for blocks if true.
    public bool findBlocks = true;
    
    // adds children of the object's list
    public bool addBlockChildren = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // serializes an object
    public static byte[] SerializeObject(object entity)
    {
        BinaryFormatter bf = new BinaryFormatter();
        MemoryStream ms = new MemoryStream();

        bf.Serialize(ms, entity);
        return ms.ToArray();
    }

    // deserializes an object
    public static object DeserializeObject(byte[] data)
    {
        BinaryFormatter bf = new BinaryFormatter();
        MemoryStream ms = new MemoryStream();

        ms.Write(data, 0, data.Length); // write data
        ms.Seek(0, 0); // return to start of data

        return bf.Deserialize(ms); // return content
    }

    // serializes a game object
    public SerializablePrefabObject PackPrefabGameObject(string prefab, GameObject go)
    {
        // serializable prefab object
        SerializablePrefabObject so;

        // prefab
        so.prefab = prefab;

        // transformation information
        // position
        so.position.x = go.transform.position.x;
        so.position.y = go.transform.position.y;
        so.position.z = go.transform.position.z;

        // rotation
        so.rotation.x = go.transform.rotation.x;
        so.rotation.y = go.transform.rotation.y;
        so.rotation.z = go.transform.rotation.z;
        so.rotation.w = go.transform.rotation.w;

        // scale
        so.scale.x = go.transform.localScale.x;
        so.scale.y = go.transform.localScale.y;
        so.scale.z = go.transform.localScale.z;

        return so;
    }
    
    // deserializes a game object
    public GameObject UnpackPrefabGameObject(SerializablePrefabObject so)
    {
        // loads the new object
        GameObject newObject = Instantiate((GameObject)Resources.Load("Prefabs/" + so.prefab));

        // transformation information
        // position
        newObject.transform.position = new Vector3(so.position.x, so.position.y, so.position.z);
        newObject.transform.rotation = new Quaternion(so.rotation.x, so.rotation.y, so.rotation.z, so.rotation.w);
        newObject.transform.localScale = new Vector3(so.scale.x, so.scale.y, so.scale.z);

        return newObject;
    }

    // sets the file for the state loader
    public void SetFile(string newFile)
    {
        file = newFile;
        SetManagerFile(newFile);
    }

    // returns the file
    public string GetFile()
    {
        return file;
    }

    // saves content
    public void SaveContent()
    {
        // if 'true', it finds all active blicks
        if(findBlocks)
        {
            // gets all cubes in the scene (active and inactive)
            CubeBehaviour[] cubes = FindObjectsOfType<CubeBehaviour>(true);

            // adds blocks
            for (int i = 0; i < cubes.Length; i++)
            {
                if (!blocks.Contains(cubes[i]))
                    blocks.Add(cubes[i]);
            }
        }

        // adds children of the parent object.
        // if all blocks with the component were added, then this doesn't need to be called
        if(!findBlocks && addBlockChildren)
        {
            // gets cubes that are children of the parent object.
            CubeBehaviour[] cubes = GetComponentsInChildren<CubeBehaviour>(true);

            // adds cubes
            for (int i = 0; i < cubes.Length; i++)
            {
                if (!blocks.Contains(cubes[i]))
                    blocks.Add(cubes[i]);
            }
        }

        // SAVING DATA //
        // for every block
        foreach(CubeBehaviour cube in blocks)
        {
            // packs all objects for the data record
            SerializablePrefabObject spo = PackPrefabGameObject(cube.prefab, cube.gameObject);
            DataRecord dr; // data record
            
            // gets the data, and gives it to the manager
            dr.data = SerializeObject(spo);
            AddDataRecordToManager(dr);
        }

        // sets the file and saves teh data
        SetManagerFile(file);
        SaveDataRecords();
    }

    public void LoadContent()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
