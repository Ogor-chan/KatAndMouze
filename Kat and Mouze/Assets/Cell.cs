using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CellState
{
    Available,
    Current,
    Completed
}

public class Cell : MonoBehaviour
{
    public GameObject[] walls;
    public SpriteRenderer background;

    public void RemoveWall(int Wall)
    {
        walls[Wall].gameObject.SetActive(false);
    }
    public void SetState(CellState state)
    {
        switch (state)
        {
            case CellState.Available:
                background.color = Color.black;
                break;
            case CellState.Current:
                background.color = Color.gray;
                break;
            case CellState.Completed:
                background.color = Color.white;
                break;

        }
    }
}
