using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralGeneration : MonoBehaviour
{
    //declare variables.
    private int width;
    private int sawSpawn;
    private int randObstacle;
    private int diffPlus;

    private float spawnPosX;
    private float spawnPosY;
    private float obstacleYPos;

    private GameObject selectedObstacle;

    //difficulty assignment for obstacles.
    private int obstacleDifficulty;
    private int dSaw = 4;
    private int dSpikeBlock = 3;
    private int dHighJump = 2;
    private int dLowJump = 1;
    private int dSpikeHighJump = 5;


    //GameObject variable to bring in SpawnRules script.
    SpawnRules spawnRules;

    //Spawn Position Variable to be passed to SpawnRules Method
    // and a variable for the returned boolean from that method.
    private Vector3 spawnPos = new Vector3(0, 0, 0);
    private bool spawnable;

    //Game Objects and Modifiable Fields from UI.
    [SerializeField] int difficulty;
    [SerializeField] private Transform Player;
    [SerializeField] GameObject block;
    [SerializeField] GameObject saw;
    [SerializeField] GameObject finish;
    [SerializeField] GameObject spikeBlock;
    [SerializeField] GameObject highJump;
    [SerializeField] GameObject lowJump;
    [SerializeField] GameObject spikeHighJump;

    //Obstacle Array
    public GameObject[] obstacleArr;



    // Start is called before the first frame update
    void Start()
    {
        GenerateBase();
    }

    // This is called to spawn tiles on the x-axis based on the set width.
    void GenerateBase()
    {
        width = difficulty * 3;
        GetStartPos();

        //Make sure that a zero difficulty is not left without a level being generated.
        if (width == 0)
        {
            width = 3;
        }

        //loop through and place a block on the ground for the length of the level
        //determined by the width.
        for (int x = (int)transform.position.x; x <= width; x++)
        {
            //Always create a base for the level.
            spawnObj(block, x, transform.position.y - 1f);

            //Always Spawn a Finish.
            if (x == width)
            {
                spawnObj(finish, x, transform.position.y + .5f);
            }
        }

        //issues with placing obstacles in low space areas. This prevents that
        //from causing crashes.
        if (difficulty >= 2)
        {
            GenerateObstacles();
        }
    }


    //This generates obstacles by calling the obstacles game object and using the SpawnRules
    //script attached to it. The spawn rules script checks to make sure overlapping spawns
    //are not occuring.
    void GenerateObstacles()
    {
        GetStartPos();
        spawnRules = GameObject.FindGameObjectWithTag("Obstacles").GetComponent<SpawnRules>();
        diffPlus = 0;

        //Loop until the difficulty target is met. Incremented by the difficulty value of the 
        //obstacle that is spawned.
        for (int x = 0; x < difficulty; x += diffPlus)
        {
            int preventInf = 0;
            spawnable = false;
            diffPlus = 0;

            while (!spawnable)
            {
                spawnPosX = Random.Range(0, width);
                spawnPosY = 0f;
                spawnPos = new Vector3(spawnPosX, spawnPosY, 0f);

                spawnable = spawnRules.SpawnAvailability(spawnPos, width);

                if (spawnable)
                {
                    GetObstacle();
                    GetObstacleDifficulty(selectedObstacle);
                    GetObstacleYPos(selectedObstacle);

                    diffPlus = obstacleDifficulty;

                    spawnObj(selectedObstacle, spawnPosX, obstacleYPos);
                    Debug.Log("Spawned " + selectedObstacle + " at " + spawnPosX + "," + spawnPosY);

                    break;
    
                }

                //prevent bad luck infinite loops.
                if (preventInf > 50)
                {
                    Debug.Log("Can't Find A Spawn Spot");
                    diffPlus = difficulty;
                    break;
                }

                preventInf++;
            }

        }
    }

    //Used to Un-clutter the Hierarchy by making Procedurally Generated game objects children
    //of the Procedural Generation Game Object.
    void spawnObj(GameObject obj, float width, float height)
    {
        obj = Instantiate(obj, new Vector2(width, height), Quaternion.identity);
        obj.transform.parent = this.transform;
    }

    //Get the start x,y,z coordinates for the player character.
    public Vector3 GetStartPos()
    {
        transform.position = new Vector3(Player.position.x, Player.position.y, Player.position.z);

        return transform.position;
    }

    //Get a random obstacles from the obstacle array.
    public GameObject GetObstacle()
    {
        randObstacle = Random.Range(0, obstacleArr.Length);
        
        selectedObstacle = obstacleArr[randObstacle];

        return selectedObstacle;

    }

    //Certain Obstacles may need a non-zero spawn location on the y-axis.
    //This is used for the saw currently, but other obstacles could be added
    //to this method.
    public float GetObstacleYPos(GameObject selectedObj)
    {
        if (selectedObj == saw)
        {
            obstacleYPos = Random.Range(0f, 3f);

        }
        else
        {
            obstacleYPos = 0f;
        }

        return obstacleYPos;
    }

    //Getter method for obstacle difficulty. Currently these values
    //are arbitrary based on feel during testing.
    public int GetObstacleDifficulty(GameObject selectedObj)
    {
        if (selectedObj == saw)
        {
            obstacleDifficulty = dSaw;
        }
        else if (selectedObj == spikeBlock)
        {
            obstacleDifficulty = dSpikeBlock;

        }
        else if (selectedObj == spikeHighJump)
        {
            obstacleDifficulty = dSpikeHighJump;
        }
        else if (selectedObj == lowJump) {

            obstacleDifficulty = dLowJump;
        }
        else
        {
            obstacleDifficulty = dHighJump;
        }

        return obstacleDifficulty;

    }
}

