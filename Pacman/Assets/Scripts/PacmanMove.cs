using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PacmanMove : MonoBehaviour
{

    [SerializeField] float speed = 0.4f;
    [SerializeField] int lives;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] ControlsController controls;
    [SerializeField] LivesController livesController;
    Vector2 dest = Vector2.zero;

    public int Score { get; set; }


    // Use this for initialization
    void Start()
    {
        dest = transform.position;
        livesController.SetLivesAmount(lives);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 p = Vector2.MoveTowards(transform.position, dest, speed);
        GetComponent<Rigidbody2D>().MovePosition(p);
        if (controls.Control == MovementDirection.Up && valid(Vector2.up))
        {
            dest = (Vector2)transform.position + Vector2.up;
        }
        else if (controls.Control == MovementDirection.Down && valid(-Vector2.up))
        {
            dest = (Vector2)transform.position - Vector2.up;
        }
        else if (controls.Control == MovementDirection.Right && valid(Vector2.right))
        {
            dest = (Vector2)transform.position + Vector2.right;
        }
        else if (controls.Control == MovementDirection.Left && valid(-Vector2.right))
        {
            dest = (Vector2)transform.position - Vector2.right;
        }
        Vector2 dir = dest - (Vector2)transform.position;
        GetComponent<Animator>().SetFloat("DirX", dir.x);
        GetComponent<Animator>().SetFloat("DirY", dir.y);
    }

    bool valid(Vector2 dir)
    {
        Vector2 pos = transform.position;
        RaycastHit2D hit = Physics2D.Linecast(pos + dir, pos);
        return (hit.collider == GetComponent<Collider2D>());
    }

    public void AddScore()
    {
        Score++;
        scoreText.text = "Score: " + Score;
    }

    public void MinusLive()
    {
        lives -= 1;
        livesController.SetLivesAmount(lives);

        if (lives == 0)
            Destroy(gameObject);
    }
}