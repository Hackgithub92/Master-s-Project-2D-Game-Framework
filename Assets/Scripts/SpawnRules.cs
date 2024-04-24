using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnRules : MonoBehaviour
{
    ProceduralGeneration procGen;
    public Collider2D[] colliders;
    public float radius;

    //Define the Spawn Rules for Procedurally Generated Game Objects/Obstacles.
    public bool SpawnAvailability(Vector3 spawnPos, float mapSize)
    {
        //Get the transform position of the player because it is at 0,0,0 from the Procedural Generation
        //Script/Class.
        procGen = GameObject.FindGameObjectWithTag("ProcGen").GetComponent<ProceduralGeneration>();
        transform.position = procGen.GetStartPos();

        radius = mapSize;

        //OverlapCircleAll grabs all the colliders in a circle with a radius that we pass in
        //as "radius" and a center point of "transform.position" which is our player sprite.
        colliders = Physics2D.OverlapCircleAll(transform.position, radius);

        //Loop through all of the colliders currently found in the array and get there center
        //point and left and right extents. Then check to make sure the coordinates passed in
        //from ProcGen do not fall inside of any of those colliders bounds. Return true if 
        //coordinates are outside of all colliders bounds, otherwise return false.
        for (int i = 0; i < colliders.Length; i++)
        {
           // Debug.Log("Collider Array Size: " + colliders.Length);

            Vector3 centerPoint = colliders[i].bounds.center;
            float width = colliders[i].bounds.extents.x;
            float height = colliders[i].bounds.extents.y;

            float leftExtent = centerPoint.x - width;
            float rightExtent = centerPoint.x + width;

            float lowerExtent = centerPoint.y - height;
            float upperExtent = centerPoint.y + height;

            if (spawnPos.x >= (leftExtent - 3f) && spawnPos.x <= (rightExtent + 3f))
            {
                if (spawnPos.y >= lowerExtent && spawnPos.y <= upperExtent)
                {
                    return false;
                }
            }
        }
        return true;

    }

    
}
