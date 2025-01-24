using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KatMovement : MonoBehaviour
{
    [SerializeField] float speed;
    private float horizontal;
    private float vertical;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] GameObject Trap;
    [HideInInspector] public int AmountOfTraps;
    [SerializeField] float TrapCooldown;
    private float TimeCounter;
    [SerializeField] float CastCooldown;
    private float CastTimeCounter;
    [SerializeField] GameObject TrapUI;

    public KeyCode Up;
    public KeyCode Right;
    public KeyCode Down;
    public KeyCode Left;
    public KeyCode TrapKey;


    public void ChangeColor()
    {
        if(AmountOfTraps > 0)
        {
            Image image = TrapUI.GetComponent<Image>();
            Color color = image.color;
            color.a = 1f;
            image.color = color;
        }
        else
        {
            Image image = TrapUI.GetComponent<Image>();
            Color color = image.color;
            color.a = 0.5f;
            image.color = color;
        }
    }
    public void PlaceTrap()
    {
        float x = Mathf.Round(this.transform.position.x);
        float y = Mathf.Round(this.transform.position.y);
        Vector2 TrapPosition = new Vector2(x,y);
        Instantiate(Trap, TrapPosition, Quaternion.identity);
    }

    private void Start()
    {
        TrapUI = GameObject.FindGameObjectWithTag("TrapText");
        GameObject[] Cheeses = GameObject.FindGameObjectsWithTag("Cheese");
        foreach (var cheese in Cheeses)
        {
            Physics2D.IgnoreCollision(cheese.GetComponent<Collider2D>(), GetComponent<Collider2D>(), true);
        }
    }

    private void FixedUpdate()
    {
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
        if (Input.GetKey(TrapKey))
        {
            if (AmountOfTraps > 0 && CastTimeCounter > CastCooldown)
            {
                PlaceTrap();
                AmountOfTraps -= 1;
                ChangeColor();
                CastTimeCounter = 0;
            }
        }

        TimeCounter += Time.deltaTime;
        CastTimeCounter += Time.deltaTime;
        if (TimeCounter > TrapCooldown)
        {
            AmountOfTraps += 1;
            ChangeColor();
            TimeCounter = 0;
        }
        rb.velocity = new Vector2(horizontal, vertical);
        horizontal = 0;
        vertical = 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Mouze")
        {
            GameObject MazeManager = GameObject.FindGameObjectWithTag("Maze");
            MazeGenerator MazeScript = MazeManager.GetComponent<MazeGenerator>();
            MazeScript.MouzeGameOver();
        }
    }
}
