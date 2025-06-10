
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class BoardManager : MonoBehaviour
{
    public GameObject[] tilePrefab;

    public GameObject housePrefab;
    public GameObject treePrefab;
    public Text score;
    public List<GameObject> spawnedTiles = new List<GameObject>();

    GameObject[] tiles;

    long dirtBB = 0;
    long treeBB = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }
    public void CreateBoard()
    {
        for (int r = 0; r < 8; r++)
        {
            for (int c = 0; c < 8; c++)
            {
                int randomTile = UnityEngine.Random.Range(0, tilePrefab.Length);
                Vector3 pos = new Vector3(c, 0, r);
                GameObject tile = Instantiate(tilePrefab[randomTile], pos, Quaternion.identity);
                tile.name = tile.tag + "_" + r + "_" + c;
                spawnedTiles.Add(tile);
                if (tile.tag == "Dirt")
                {
                    dirtBB = SetCellState(dirtBB, r, c);
                    PrintBB("Dirt", dirtBB);
                }
            }
        }
        Debug.Log("Dirt Cells = " + CellCount(dirtBB));
        InvokeRepeating("PlantTree", 0.25f, 0.25f);
    }
    public void DeleteBoard()
    {
        for (int i = 0; i < spawnedTiles.Count; i++)
        {
            if (spawnedTiles[i] != null)
                DestroyImmediate(spawnedTiles[i].gameObject);
        }
        spawnedTiles.Clear();
    }
    void PrintBB(string name, long BB)
    {
        Debug.Log(name + ": " + Convert.ToString(BB, 2).PadLeft(64, '0'));
    }
    long SetCellState(long BitBoard, int row, int col)
    {
        long newBit = 1L << (row * 8 + col);
        return (BitBoard |= newBit);
    }
    bool GetCellState(long Bitboard, int row, int col)
    {
        long mask = 1L << (row * 8 + col);
        return ((Bitboard & mask) != 0);
    }
    int CellCount(long bitBoard)
    {
        int count = 1;
        long bb = bitBoard;
        while (bb != 0)
        {
            bb &= bb - 1;
            count++;
        }
        return count;
    }
    void PlantTree()
    {
        int rr = UnityEngine.Random.Range(0, 8);
        int rc = UnityEngine.Random.Range(0, 8);
        if (GetCellState(dirtBB, rr, rc))
        {
            GameObject tree = Instantiate(treePrefab);
            tree.transform.parent = spawnedTiles[rr * 8 + rc].transform;
            tree.transform.localPosition = Vector3.zero;
            treeBB = SetCellState(treeBB, rr, rc);
        }
    }
}