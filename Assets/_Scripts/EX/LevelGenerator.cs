using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// level generator class
// this is based on the one I made for lecture 10 - in-class assignment #6.
public class LevelGenerator : MonoBehaviour
{
    // parent for new objects
    public GameObject blockParent = null;

    // if 'true', then this object is used as the default parent.
    public bool defaultParent = true;
    // if 'true', parents are added to spanwed objects
    public bool addParents = true;
    // if 'true', parents of spawned objects are reserved if not set.
    public bool preserveParents = true;

    // the tiles being generated.
    public List<GameObject> blockOptions = new List<GameObject>();

    // the amount of tiles to be made.
    // if there aren't enough spaces, remaining tiles are disabled.
    // if this is 0 or negative, then it moves around existing blocks.
    // if there are no existing blocks, then it creates a random amount.
    public int blockCount = -1; // if negative, then its set to the amount of existing blocks

    // TODO: maybe add randomizations?

    // tile area base position, tile area size, and tile area origin
    public Vector3 areaPosition = new Vector3(0, 0, 0);
    public Vector3Int areaSize = new Vector3Int(10, 10, 10);
    private Vector3 areaOrigin = new Vector3(0.5F, 0.5F, 0.5F);

    // chance of changing direction (out of 1.0F (100%))
    public float direcChangeChance = 0.4F; // out of 1.0F

    // chance of spawning room (out of 1.0F (100%))
    public float spreadChance = 0.2F;

    // amount of cycles in one direction.
    public int cycles = 10;

    // refreshes the stage.
    public CollisionManager colManager;
    private bool resetBlocks = false;
    int refreshCD = 0; // waits one frame to refresh

    // copies the rotates the vector 2
    public Vector2 RotateVector2(Vector2 v, float deg)
    {
        Vector2 vx = new Vector2();

        // calculates rotation
        vx.x = v.x * Mathf.Cos(Mathf.Deg2Rad * deg) - v.y * Mathf.Sin(Mathf.Deg2Rad * deg);
        vx.y = v.x * Mathf.Sin(Mathf.Deg2Rad * deg) + v.y * Mathf.Cos(Mathf.Deg2Rad * deg);

        return vx;
    }

    // copies and rotates the vector 2 int
    public Vector2Int RotateVector2Int(Vector2Int v, float deg)
    {
        // calculates the vector rotation
        Vector2 vx = new Vector2();
        Vector2Int vy = new Vector2Int();

        // calculates rotation
        vx.x = v.x * Mathf.Cos(Mathf.Deg2Rad * deg) - v.y * Mathf.Sin(Mathf.Deg2Rad * deg);
        vx.y = v.x * Mathf.Sin(Mathf.Deg2Rad * deg) + v.y * Mathf.Cos(Mathf.Deg2Rad * deg);

        vy.x = Mathf.RoundToInt(vx.x);
        vy.y = Mathf.RoundToInt(vx.y);

        return vy;
    }


    // Start is called before the first frame update
    void Start()
    {
        // no tiles set, load all the prefabs
        if(blockOptions.Count == 0)
        {
            object prefab; // loaded prefab

            // loads block a
            prefab = Resources.Load("Prefabs/BlockA");
            if (prefab != null)
                blockOptions.Add((GameObject)prefab);

            // loads block b
            prefab = Resources.Load("Prefabs/BlockB");
            if (prefab != null)
                blockOptions.Add((GameObject)prefab);
        }

        // if no level parent exists, the gameObject this script is attached to serves as one.
        if (blockParent == null && defaultParent)
            blockParent = gameObject;

        // finds the collision manager in the scene.
        if (colManager == null)
            colManager = FindObjectOfType<CollisionManager>();

        // generates the level (not called on start)
        // GenerateLevel();
    }

    // generates the tiles
    private void GenerateLevel()
    {
        // if there are no tile
        if (blockOptions.Count == 0)
            return;

        // blocks to be positioned.
        GameObject[] blocks = GameObject.FindGameObjectsWithTag("Block"); // finds all blocks

        // block offsets
        Vector3 offset;

        // the amount of available spaces
        int spaces = 0;

        // if negative, it moves aorund the existing blocks only
        if (blockCount <= 0)
            blockCount = blocks.Length;

        // TODO: change this to generate positions first, then  make blocks afterwards.

        // gets block size (originally was in Start(), but I did this to refresh
        // this will need to be moved.
        {
            // this goes off of BlockA. In a more efficient program this would be dependent on each bue.
            // gets the renderer
            MeshRenderer renderer = blockOptions[0].GetComponent<MeshRenderer>();

            // gets the size of the object
            offset = renderer.bounds.size; // / 2.0F; // this assumes the center is the middle. 
        }

        // VARIABLES
        // the value at the given index determines how high the blocks in a given place are
        int[,] grid = new int[areaSize.z, areaSize.x]; // y-axis is filled by height

        // gets the current cell
        Vector2Int currCell = new Vector2Int(Random.Range(0, areaSize.z), Random.Range(0, areaSize.x));

        // randomizes the direction
        Vector2Int dir;

        // finds starting direction.
        {
            // randomizes the direction
            int randNum = Random.Range(0, 5);

            switch (randNum)
            {
                case 1: // left first
                    dir = Vector2Int.left;
                    break;

                case 2: // up first
                    dir = Vector2Int.up;
                    break;

                case 3: // right first
                    dir = Vector2Int.right;
                    break;

                case 4: // down first
                default:
                    dir = Vector2Int.down;
                    break;
            }
        }


        // goes in all four directions using al oop
        for (int i = 0; i < 4; i++)
        {
            Vector2Int cycleDir = dir; // the direction for the current iteration
            Vector2Int cycleCell = currCell; // the cycel cell
            int cycle = 0; // number of cycles

            // while the amount of desired cycles has not been surpassed.
            while (cycle < cycles)
            {
                // gets two random numbers for the direction chance and spread chance
                float r1 = Random.Range(0.0F, 1.0F);
                float r2 = Random.Range(0.0F, 1.0F);

                // if the direction should change
                if (r1 <= direcChangeChance)
                {
                    cycleDir = RotateVector2Int(cycleDir, 90.0F);
                }

                // a spread shot should be used, which fills adjacent tiles.
                if (r2 <= spreadChance)
                {
                    // one left
                    if (cycleCell.x - 1 > 0)
                    {
                        grid[cycleCell.x - 1, cycleCell.y]++;
                        spaces++;
                    }

                    // one right
                    if (cycleCell.x + 1 < areaSize.z)
                    {
                        grid[cycleCell.x + 1, cycleCell.y]++;
                        spaces++;
                    }

                    // one up
                    if (cycleCell.y + 1 < areaSize.x)
                    {
                        grid[cycleCell.x, cycleCell.y + 1]++;
                        spaces++;
                    }

                    // one down
                    if (cycleCell.y - 1 > 0)
                    {
                        grid[cycleCell.x, cycleCell.y - 1]++;
                        spaces++;
                    }

                    // TODO: fill diagonal?
                }

                // increments current cell
                grid[cycleCell.x, cycleCell.y]++;
                spaces++;

                // goes onto new cell, making said cell the current cell.
                cycleCell += cycleDir;

                // TODO: have it change direction instead of breaking?
                // if the cell is out of bounds, break.
                if (cycleCell.x < 0 || cycleCell.x >= areaSize.z || cycleCell.y < 0 || cycleCell.y >= areaSize.x)
                {
                    break;
                }
                else
                {
                    cycle++; // next cycle
                }
            }

            // rotates to go to the next direction
            dir = RotateVector2Int(dir, 90.0F);
        }

        // instatiates objects and places blocks.
        {
            // the current block
            int currBlock = 0;

            // if the block count is less than or equal to 0, it's set to the maximum amount of spaces.
            if(blockCount <= 0)
            {
                // blocks should fill all spaces
                blockCount = spaces;
            }


            // spawns items
            for (int row = 0; row < areaSize.z; row++)
            {
                for (int col = 0; col < areaSize.x; col++)
                {
                    // generates platforms in accordance with tile
                    for (int n = 0; n < grid[row, col] && n < areaSize.y; n++)
                    {
                        // object being generated or altered
                        GameObject go;

                        // generates new base position
                        Vector3 newPos = new Vector3(offset.x * col, offset.y * n, offset.z * row);
                        
                        // moves to align object with origin
                        newPos -= Vector3.Scale(Vector3.Scale(areaSize, areaOrigin), offset);

                        // generates new position
                        // Vector3 newPos = Vector3.Scale(Vector3.Scale(areaSize, areaOrigin), offset);

                        // if there is a block available.
                        if (currBlock < blocks.Length)
                        {
                            go = blocks[currBlock];
                            blocks[currBlock].SetActive(true);
                            blocks[currBlock].transform.position = newPos;
                        }
                        else
                        {
                            int rint = Random.Range(0, blockOptions.Count);
                            go = Instantiate(blockOptions[rint], newPos, new Quaternion(0, 0, 0, 1));

                        }

                        // if the parents should be perserved if not set
                        if(addParents && blockParent != null)
                        {
                            // if there is no parent, or if there is a parent and it should be overridded.
                            if(go.transform.parent == null || (go.transform.parent != null && !preserveParents))
                            {
                                go.transform.parent = blockParent.transform;
                            }
                        }

                        currBlock++;

                        // if the block count has been reached, break.
                        if(currBlock == blockCount)
                            break;
                    }

                    // if the block count has been reached, break.
                    if (currBlock == blockCount)
                        break;
                }
            }


            // if the amount of blocks in the list exceeds how many block were made.
            while (blocks.Length > currBlock)
            {
                int index = blocks.Length - 1; // gets index
                GameObject blk = blocks[index]; // gets block

                System.Array.Resize<GameObject>(ref blocks, index); // removes block
                GameObject.Destroy(blk); // destroys block
            }

            // repositions player, and scales up ground to hold all blocks
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player"); // player object
                GameObject ground = GameObject.FindGameObjectWithTag("Ground"); // ground object

                if (ground != null) // ground exists
                {
                    MeshRenderer groundMr = ground.GetComponent<MeshRenderer>(); // gets the ground's mesh renderer

                    if (groundMr != null) // mesh renderer not set.
                    {
                        // new position for ground
                        Vector3 newPos = Vector3.Scale(Vector3.Scale(areaSize, areaOrigin), offset); // new position

                        // the new scale, which is multiplied by the offset.
                        // scale = desired size / current size
                        Vector3 newScale = new Vector3(
                            areaSize.x * offset.x / groundMr.bounds.size.x,
                            areaSize.y * offset.y / groundMr.bounds.size.y,
                            areaSize.z * offset.z / groundMr.bounds.size.z
                            );

                        // increases scale by 1 to ensure extra space.
                        newScale += new Vector3(1, 1, 1);

                        // sets position and scale
                        ground.transform.position = newPos;
                        ground.transform.localScale = newScale;
                    }
                }
                

                // player found
                if(player != null)
                {
                    Vector3 newPos;

                    // random spot in scene
                    newPos.x = Random.Range(0, areaSize.x) * offset.x;
                    newPos.y = areaSize.y * (offset.y + 1);
                    newPos.z = Random.Range(0, areaSize.z) * offset.z;

                    // offset for origin
                    newPos -= Vector3.Scale(Vector3.Scale(areaSize, areaOrigin), offset);

                    player.transform.position = newPos;
                }
            }


            // tries to find collision manager if not set.
            if (colManager == null)
            {
                colManager = FindObjectOfType<CollisionManager>();

                // does not exist
                if (colManager == null)
                {
                    return;
                }
            }

            // clears the cube list and marks it for refresh.
            colManager.ClearCubeList();
            resetBlocks = true;
            refreshCD = 1;
        }
    }

    // randomizes existing blocks
    public void Randomize()
    {
        int oldBlockCount = blockCount; // old block count
        int existingBlocks; // existing block count

        // gets existing block count
        {
            GameObject[] blocks = GameObject.FindGameObjectsWithTag("Block");

            // no blocks to randomize
            if (blocks.Length == 0)
                return;

            existingBlocks = blocks.Length;
        }

        // generates level using existing blocks only.
        blockCount = existingBlocks; // replaces block count
        GenerateLevel();
        blockCount = oldBlockCount; // retunrs to old block count
    }

    // Update is called once per frame
    void Update()
    {
        // if the blocks should be reset.
        if(resetBlocks)
        {
            if(refreshCD <= 0) // time to refresh
            {
                refreshCD = 0;
                resetBlocks = false;

                // tries to find collision manager if not set.
                if (colManager == null)
                {
                    colManager = FindObjectOfType<CollisionManager>();

                    // does not exist
                    if(colManager == null)
                    {
                        return;
                    }
                }

                colManager.RefreshCubeList();
            }
            else // not refresh time yet, so count down.
            {
                refreshCD--;
            }
        }
    }
}
