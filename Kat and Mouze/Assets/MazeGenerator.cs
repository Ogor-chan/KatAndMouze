using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField] GameObject Kat;
    [SerializeField] GameObject Mouze;
    [SerializeField] GameObject Cheese;
    [SerializeField] GameObject MouzeGameOverText;
    [SerializeField] GameObject KatGameOverText;

    [SerializeField] int AmountOfCheese;

    [SerializeField] Cell Cellpref;
    [SerializeField] Vector2Int MazeSize;

    List<Vector2> CheeseOccupiedCells = new List<Vector2>();

    List<Cell> Cells = new List<Cell>();
    List<Cell> CurrentPath = new List<Cell>();
    List<Cell> FinishedCells = new List<Cell>();

    List<int> PossibleDirections = new List<int>();
    //0 = UP
    //1 = DOWN
    //2 = RIGHT
    //3 = LEFT

    public int WallToRemove;
    private bool gameover = false;

    private void Start()
    {
        GenerateCells();
        StartCoroutine(GenerateMaze());
    }

    public void KatGameOver()
    {
        GameObject Kat = GameObject.FindGameObjectWithTag("Player");
        GameObject Mouze = GameObject.FindGameObjectWithTag("Mouze");
        Destroy(Kat);
        Destroy(Mouze);
        KatGameOverText.SetActive(true);
        gameover = true;
    }

    public void MouzeGameOver()
    {
        GameObject Kat = GameObject.FindGameObjectWithTag("Player");
        GameObject Mouze = GameObject.FindGameObjectWithTag("Mouze");
        Destroy(Kat);
        Destroy(Mouze);
        MouzeGameOverText.SetActive(true);
        gameover = true;
    }

    private void SpawnCheese()
    {
        Vector2 StartingPos = new Vector2(-7, -7);
        CheeseOccupiedCells.Add(StartingPos);
        StartingPos = new Vector2(7, 7);
        CheeseOccupiedCells.Add(StartingPos);
        for (int i = 0; i < AmountOfCheese; i++)
        {
            TryAgain:
            int x = Random.Range(-7, 7);
            int y = Random.Range(-7, 7);
            Vector2 pos = new Vector2(x, y);
            if (CheeseOccupiedCells.Contains(pos))
            {
                goto TryAgain;
            }
            Instantiate(Cheese, pos, Quaternion.identity, transform);
            CheeseOccupiedCells.Add(pos);
        }
    }
    private void SpawnPlayers()
    {
        Vector2 pos = new Vector2(-7, -7);
        Instantiate(Kat, pos, Quaternion.identity, transform);
        pos = new Vector2(7, 7);
        Instantiate(Mouze, pos, Quaternion.identity, transform);
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
        if (gameover && Input.GetKey(KeyCode.Space))
        {
            SceneManager.LoadScene(1);   
        }
    }

    private void GenerateCells()
    {
        for (int x = 0; x < MazeSize.x; x++)
        {
            for (int y = 0; y < MazeSize.y; y++)
            {
                Vector2 CellPos = new Vector2(x - (MazeSize.x / 2f) + 0.5f, y - (MazeSize.y / 2f) + 0.5f);
                Cell newCell = Instantiate(Cellpref, CellPos, Quaternion.identity, transform);
                newCell.name = x + "," + y;
                Cells.Add(newCell);
            }
        }
    }

    IEnumerator GenerateMaze()
    {
        CurrentPath.Add(Cells[Random.Range(0, Cells.Count)]);
        CurrentPath[0].SetState(CellState.Current);

        List<Cell> AvailableCells = new List<Cell>();

        AvailableCells = FindAvailableCells(CurrentPath[CurrentPath.Count - 1]);

        while (true)
        {
            AvailableCells = FindAvailableCells(CurrentPath[CurrentPath.Count - 1]);
            if (AvailableCells.Count == 0)
            {
                while (true)
                {
                    Backtrack();
                    if (FinishedCells.Count >= Cells.Count)
                    {
                        goto TheEnd;
                    }
                    AvailableCells = FindAvailableCells(CurrentPath[CurrentPath.Count - 1]);
                    if (AvailableCells.Count > 0)
                    {
                        break;
                    }
                    yield return new WaitForSecondsRealtime(0.005f);
                }
            }
            int RandomCell = Random.Range(0, AvailableCells.Count);

            int whichDirection = PossibleDirections[RandomCell];

            CurrentPath.Add(AvailableCells[RandomCell]);

            switch (whichDirection)
            {
                case 0:
                    CurrentPath[CurrentPath.Count - 2].RemoveWall(2);
                    CurrentPath[CurrentPath.Count - 1].RemoveWall(3);
                    break;
                case 1:
                    CurrentPath[CurrentPath.Count - 2].RemoveWall(3);
                    CurrentPath[CurrentPath.Count - 1].RemoveWall(2);
                    break;
                case 2:
                    CurrentPath[CurrentPath.Count - 2].RemoveWall(0);
                    CurrentPath[CurrentPath.Count - 1].RemoveWall(1);
                    break;
                case 3:
                    CurrentPath[CurrentPath.Count - 2].RemoveWall(1);
                    CurrentPath[CurrentPath.Count - 1].RemoveWall(0);
                    break;
            }

            PossibleDirections.Clear();
            CurrentPath[CurrentPath.Count - 1].SetState(CellState.Current);
            yield return new WaitForSecondsRealtime(0.005f);
        }

    TheEnd:
        SpawnCheese();
        SpawnPlayers();
    }

    private List<Cell> FindAvailableCells(Cell CurrentCell)
    {
        List<Cell> AvailableCells = new List<Cell>();
        int CurrentCellIndex = Cells.IndexOf(CurrentPath[CurrentPath.Count - 1]);

        //Cell above
        if (CurrentCellIndex % MazeSize.y < MazeSize.y - 1)
        {
            int UpCellIndex = CurrentCellIndex + 1;
            if (FinishedCells.Contains(Cells[UpCellIndex]) || CurrentPath.Contains(Cells[UpCellIndex]))
            {
            }
            else
            {
                AvailableCells.Add(Cells[UpCellIndex]);
                PossibleDirections.Add(0);
            }
        }
        //Cell on the right
        if ((CurrentCellIndex / MazeSize.y) + 1 < MazeSize.y)
        {
            int RightCellIndex = CurrentCellIndex + MazeSize.y;
            if (FinishedCells.Contains(Cells[RightCellIndex]) || CurrentPath.Contains(Cells[RightCellIndex]))
            {
            }
            else
            {
                AvailableCells.Add(Cells[RightCellIndex]);
                PossibleDirections.Add(2);
            }
        }
        //Cell bellow
        if (CurrentCellIndex % MazeSize.y > 0)
        {
            int DownCellIndex = CurrentCellIndex - 1;
            if (FinishedCells.Contains(Cells[DownCellIndex]) || CurrentPath.Contains(Cells[DownCellIndex]))
            {
            }
            else
            {
                AvailableCells.Add(Cells[DownCellIndex]);
                PossibleDirections.Add(1);
            }
        }
        //Cell on the left
        if (CurrentCellIndex > MazeSize.y)
        {
            int LeftCellIndex = CurrentCellIndex - MazeSize.y;
            if (FinishedCells.Contains(Cells[LeftCellIndex]) || CurrentPath.Contains(Cells[LeftCellIndex]))
            {
            }
            else
            {
                AvailableCells.Add(Cells[LeftCellIndex]);
                PossibleDirections.Add(3);
            }
        }
        return AvailableCells;
    }

    private void Backtrack()
    {
        FinishedCells.Add(CurrentPath[CurrentPath.Count - 1]);
        CurrentPath[CurrentPath.Count - 1].SetState(CellState.Completed);
        CurrentPath.RemoveAt(CurrentPath.Count - 1);
    }
}
