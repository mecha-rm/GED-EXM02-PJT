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
    public bool active; // if object was active or not

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
    public bool loadAsChildren = false;

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
        so.active = go.activeSelf;

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
        object prefab = Resources.Load("Prefabs/" + so.prefab);
        GameObject newObject;

        if (prefab != null)
            newObject = Instantiate((GameObject)prefab); // instantiate game object
        else
            newObject = new GameObject(); // empty game object otherwise
        
        newObject.SetActive(so.active);

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

    // loads content from file
    public void LoadContent()
    {
        // loads content
        SetManagerFile(file);

        // if the file is not available, do not save content.
        if (!FileAvailable())
        {
            Debug.LogError("File Not Available. Load Failed.");
            return;
        }

        // loads the data records
        if (LoadDataRecords())
        {
            Debug.Log("Content Loaded");
        }
        else
        {
            Debug.LogError("Error. Content Not Loaded");
            return;
        }

        // gets the amount of data
        int dataCount = GetDataRecordAmount();

        // loads all objects
        for (int i = 0; i < dataCount; i++)
        {
            // gets the data, unpacks it, then generates a game object.

            DataRecord dr = GetDataRecordFromManager(i);
            object unpacked = DeserializeObject(dr.data);
            SerializablePrefabObject so = (SerializablePrefabObject)unpacked;
            GameObject newObject = UnpackPrefabGameObject(so);

            // if the game object should be loaded as a child of this game object.
            if (loadAsChildren)
                newObject.transform.parent = gameObject.transform;
        }


        // refreshes the collision manager
        CollisionManager cm = FindObjectOfType<CollisionManager>();

        if (cm != null)
            cm.RefreshCubeList();

    }


    // saves content
    public void SaveContent()
    {
        // sets the manager file
        SetManagerFile(file);

        // the file will be generated if it doesn't exist
        // if the file is not available, do not save content.
        // if (!FileAvailable())
        // {
        //     Debug.LogError("File Not Accessible. Save Failed.");
        //     return;
        // }
            
        // if 'true', it finds all active blicks
        if (findBlocks)
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

        string getfile = GetManagerFile();
        int recordAmnt = GetDataRecordAmount();

        Debug.Log("File: " + getfile);
        Debug.Log("Record Count: " + recordAmnt);

        // saves the data
        if (SaveDataRecords())
            Debug.Log("Content Saved");
        else
            Debug.LogError("Error. Content Not Saved");

        // deletes all records - calling this function causes the game to crash, and I don't know why.
        // either way, this function shouldn't be used.
        // DeleteAllDataRecordsFromManager();
        ClearAllDataRecordsFromManager();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
