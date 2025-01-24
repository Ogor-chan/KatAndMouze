using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouzeMovement : MonoBehaviour
{
    [SerializeField] float speed;
    private float horizontal;
    private float vertical;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] GameObject SpecialUI;
    private int cheesesleft;

    public KeyCode Up;
    public KeyCode Right;
    public KeyCode Down;
    public KeyCode Left;
    public KeyCode Sneak;
    public KeyCode Special;

    private float SpecialCooldownTimer;
    [SerializeField] float SpecialCooldown;
    [SerializeField] float SpecialUseTime;
    private float CurretSpecialUseTime;
    private bool InSuper = false;
    private bool HaveSuper;

    private bool Sneaking;

    public void Start()
    {
        SpecialUI = GameObject.FindGameObjectWithTag("SpecialUI");
        CountCheeses();
    }

    public void CountCheeses()
    {
        cheesesleft = 0;
        GameObject[] Cheeses = GameObject.FindGameObjectsWithTag("Cheese");
        foreach (var cheese in Cheeses)
        {
            cheesesleft += 1;
        }
        print(cheesesleft);
    }
    public void ChangeColor()
    {
        if (HaveSuper)
        {
            Image image = SpecialUI.GetComponent<Image>();
            Color color = image.color;
            color.a = 1f;
            image.color = color;
        }
        else
        {
            Image image = SpecialUI.GetComponent<Image>();
            Color color = image.color;
            color.a = 0.5f;
            image.color = color;
        }
    }
    private void FixedUpdate()
    {
        if(cheesesleft < 4)
        {
            GameObject maze = GameObject.FindGameObjectWithTag("Maze");
            MazeGenerator mg = maze.GetComponent<MazeGenerator>();
            mg.KatGameOver();
        }

        SpecialCooldownTimer += Time.deltaTime;

        if (InSuper)
        {
            if(CurretSpecialUseTime > SpecialUseTime)
            {
                CurretSpecialUseTime = 0;
                InSuper = false;
            }
            SpecialCooldownTimer = 0;

            CurretSpecialUseTime += Time.deltaTime;
            Collider2D colider = GetComponent<Collider2D>();
            colider.isTrigger = true;
        }
        else
        {
            Collider2D colider = GetComponent<Collider2D>();
            colider.isTrigger = false;
        }

        if (Input.GetKey(Up))
        {
            vertical += speed;
        }
        if (Input.GetKey(Right))
        {
            horizontal += speed;
        }
        if (Input.GetKey(Down))
        {
            vertical -= speed;
        }
        if (Input.GetKey(Left))
        {
            horizontal -= speed;
        }
        if (Input.GetKey(Sneak) && !InSuper)
        {
            Sneaking = true;
        }
        else
        {
            Sneaking = false;
        }
        if (Sneaking == true)
        {
            horizontal = horizontal / 3;
            vertical = vertical / 3;
        }
        if (SpecialCooldownTimer > SpecialCooldown)
        {
            HaveSuper = true;
            ChangeColor();
        }
        if(HaveSuper)
        {
            if (Input.GetKey(Special))
            {
                InSuper = true;
                HaveSuper = false;
                ChangeColor();
            }
        }
        if (InSuper)
        {
            horizontal = horizontal / 3;
            vertical = vertical / 3;
        }

        rb.velocity = new Vector2(horizontal, vertical);
        horizontal = 0;
        vertical = 0;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Trap" && Sneaking == false)
        {
            GameObject MazeManager = GameObject.FindGameObjectWithTag("Maze");
            MazeGenerator MazeScript = MazeManager.GetComponent<MazeGenerator>();
            MazeScript.MouzeGameOver();
        }
    }
}
