using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject cubePrefab;
    Vector3 cubePosition;
    GameObject activeCube;
    public int gridX = 16, gridY = 9;
    int airplaneX, airplaneY, startX, startY;
    int depotX, depotY;
    GameObject[,] grid;
    bool airplaneActive;
    float turnLength, turnTimer;
    int airplaneCargo, airplaneCargoMax;
    int cargoGain;
    int score = 0;

    // Start is called before the first frame update
    void Start()
    {
        turnLength = 1.5f;
        turnTimer = turnLength;

        airplaneCargo = 0;
        airplaneCargoMax = 90;
        cargoGain = 10;

        grid = new GameObject[gridX, gridY];

        for (int y = 0; y < gridY; y++)
        {
            for (int x = 0; x < gridX; x++)
            {
                cubePosition = new Vector3(x * 2, y * 2, 0);
                grid[x,y] = Instantiate(cubePrefab, cubePosition, Quaternion.identity);
                grid[x, y].GetComponent<CubeController>().myX = x;
                grid[x, y].GetComponent<CubeController>().myY = y;

            }
        }

        // airplane starts in upper left
        startX = 0;
        startY = gridY - 1;
        airplaneX = startX;
        airplaneY = startY;
        grid[airplaneX, airplaneY].GetComponent<Renderer>().material.color = Color.red;
        airplaneActive = false;
        depotX = gridX - 1;
        depotY = 0;
        grid[depotX, depotY].GetComponent<Renderer>().material.color = Color.black;
    }

    public void ProcessClick(GameObject clickedCube, int x, int y)
    {
        if (x == airplaneX && y == airplaneY)
        {
            if (airplaneActive)
            {
                airplaneActive = false;
                clickedCube.transform.localScale /= 1.5f;
            }
            else
            {
                airplaneActive = true;
                clickedCube.transform.localScale *= 1.5f;
            }
        }

        else
        {
            if (airplaneActive)
            {
                // remove plane from old spot
                if (airplaneX == depotX && airplaneY == depotY)
                {
                    grid[depotX, depotY].GetComponent<Renderer>().material.color = Color.black;
                   
                }
                else
                {
                    grid[airplaneX, airplaneY].GetComponent<Renderer>().material.color = Color.white;
                }
                grid[airplaneX, airplaneY].transform.localScale /= 1.5f;


                // move plane to new spot
                airplaneX = x;
                airplaneY = y;
                grid[x, y].GetComponent<Renderer>().material.color = Color.red;
                grid[x, y].transform.localScale *= 1.5f;
            }
        }
    }

    void LoadCargo ()
    {
        if (airplaneX == startX && airplaneY == startY)
        {
            airplaneCargo += cargoGain;
            if (airplaneCargo > airplaneCargoMax)
            {
                airplaneCargo = airplaneCargoMax;
            }
        }
    }

    void DeliverCargo()
    {
        if (airplaneX == depotX && airplaneY == depotY)
        {
            score += airplaneCargo;
            airplaneCargo = 0;
        }
    }

// Update is called once per frame
    void Update()
    {
        if (Time.time > turnTimer)
        {
            //take a turn
            // see if it's in the upper left and give cargo
            LoadCargo ();
            DeliverCargo ();
            // check if its in the lowwer right and score points
            print("Cargo: " + airplaneCargo + "  Score: " + score);

            turnTimer += turnLength;
        }
    }
}