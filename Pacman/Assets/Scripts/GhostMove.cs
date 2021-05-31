using UnityEngine;
using System.Collections;

public class GhostMove : MonoBehaviour
{
    public Transform[] waypoints;
    int cur = 0;

    public float speed = 0.3f;

    void FixedUpdate()
    {
        
        if (!IsCurReached())
        {
            Vector2 p = Vector2.MoveTowards(transform.position,
                                            waypoints[cur].position,
                                            speed);
            GetComponent<Rigidbody2D>().MovePosition(p);
        }
        // Waypoint reached, select next one
        else cur = (cur + 1) % waypoints.Length;

        Vector2 dir = waypoints[cur].position - transform.position;
        GetComponent<Animator>().SetFloat("dirX", dir.x);
        GetComponent<Animator>().SetFloat("dirY", dir.y);
    }

    void OnTriggerEnter2D(Collider2D co)
    {
        if (co.name == "Pacman")
            co.gameObject.GetComponent<PacmanMove>().MinusLive();
    }

    bool IsCurReached()
    {
        return (transform.position.x > waypoints[cur].position.x - 0.01f
                && transform.position.x < waypoints[cur].position.x + 0.01f)
            && (transform.position.y > waypoints[cur].position.y - 0.01f
                && transform.position.y < waypoints[cur].position.y + 0.01f);
    }
}