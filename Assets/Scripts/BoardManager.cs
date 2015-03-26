using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using Random = UnityEngine.Random;
using Assets.Scripts.Common;

public class BoardManager : MonoBehaviour
{
    public int Columns = 8;
    public int Rows = 8;
    public Count WallCount = new Count(5, 9);
    public Count FoodCount = new Count(1, 5);

    public GameObject Exit;
    public GameObject[] FloorTiles;
    public GameObject[] WallTiles;
    public GameObject[] FoodTiles;
    public GameObject[] EnemyTiles;
    public GameObject[] OuterWallTiles;

    private Transform boardHolder;
    private List<Vector3> GridPositions = new List<Vector3>();

    public void SetupScene(int level)
    {
        this.BoardSetup();
        this.InitializeList();
        this.LayoutObjectAtRandom(this.WallTiles, this.WallCount.Minimum, this.WallCount.Maximum);
        this.LayoutObjectAtRandom(this.FoodTiles, this.FoodCount.Minimum, this.FoodCount.Maximum);
        var enemyCount = (int)Mathf.Log(level, 2f);
        this.LayoutObjectAtRandom(this.EnemyTiles, enemyCount, enemyCount);
        Instantiate(this.Exit, new Vector3(this.Columns - 1, this.Rows - 1, 0f), Quaternion.identity);
    }

    private void InitializeList()
    {
        this.GridPositions.Clear();

        for (int x = 1; x < this.Columns - 1; x++)
        {
            for (int y = 1; y < this.Rows - 1; y++)
            {
                this.GridPositions.Add(new Vector3(x, y, 0f));
            }
        }
    }

    private void BoardSetup()
    {
        this.boardHolder = new GameObject("Board").transform;

        for (int x = -1; x < this.Columns + 1; x++)
        {
            for (int y = -1; y < this.Rows + 1; y++)
            {
                GameObject toInstantiate = this.FloorTiles[Random.Range(0, this.FloorTiles.Length)];
                if(x == -1 || x == this.Columns || y == -1 || y == this.Rows)
                {
                    toInstantiate = this.OuterWallTiles[Random.Range(0, this.OuterWallTiles.Length)];
                }

                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

                instance.transform.SetParent(this.boardHolder);
            }
        }
    }

    private Vector3 RandomPosition()
    {
        var randomIndex = Random.Range(0, this.GridPositions.Count);
        var randomPosition = this.GridPositions[randomIndex];
        this.GridPositions.RemoveAt(randomIndex);

        return randomPosition;
    }

    private void LayoutObjectAtRandom(GameObject[] tileArray, int min, int max)
    {
        var objectCount = Random.Range(min, max + 1);

        for (int i = 0; i < objectCount; i++)
        {
            var randomPosition = this.RandomPosition();
            var tileChoice = tileArray[Random.Range(0, tileArray.Length)];
            Instantiate(tileChoice, randomPosition, Quaternion.identity);
        }
    }
}
