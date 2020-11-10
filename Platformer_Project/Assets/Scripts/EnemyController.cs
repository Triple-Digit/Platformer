using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    Transform t_player;
    Vector3 v_moveDirection;
    Rigidbody r_body;
    public int i_wayPointIndex;

    [SerializeField] bool b_freeMoving, b_chase, b_waypointMovement, b_explodes;
    [SerializeField] Transform[] t_wayPoints;
    [SerializeField] float f_moveSpeed;
    [SerializeField] GameObject g_ExplosionFX;
    [SerializeField] float f_explosionForce = 300f;
    [SerializeField] float f_radius = 5f;
    private void Awake()
    {
        t_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        
        r_body = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        MovePosition();
    }

    void MovePosition()
    {
        if(b_chase && b_freeMoving)
        {
            v_moveDirection = t_player.position - transform.position;
            r_body.velocity = v_moveDirection * f_moveSpeed;
        }
        else if(b_waypointMovement && b_freeMoving)
        {
            v_moveDirection = t_wayPoints[i_wayPointIndex].position - transform.position;
            if(Vector3.Distance(transform.position, t_wayPoints[i_wayPointIndex].position) < 0.01f)
            {
                i_wayPointIndex++;
                if (i_wayPointIndex >= t_wayPoints.Length)
                {
                    i_wayPointIndex = 0;
                }                                
            }
            r_body.velocity = v_moveDirection * f_moveSpeed;
        }
        
    }

    


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Instantiate(g_ExplosionFX, transform.position, transform.rotation);
            if (b_explodes)
            {
                
                other.GetComponent<PlayerController>().b_dead = true;
                other.GetComponent<Rigidbody>().AddExplosionForce(f_explosionForce, transform.position, f_radius);
                Destroy(gameObject);
            }
            else
            {
                other.GetComponent<PlayerController>().b_dead = true;
            }
            GameManager.instance.Lose();
        }
    }
}
