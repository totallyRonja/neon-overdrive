using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float speed;

    public Transform ballMesh;
    new public Light light;
    public float baseMeshScale = 0.5f;
    public float baseControllerRadius = 0.25f;
    public float steering = 1;

    public GameObject particles;

    [HideInInspector] public float damage = 1;
    [HideInInspector] public PlayerTeam team;
    [HideInInspector] public Transform goal;
    [HideInInspector] public Vector3 direction;
    [HideInInspector] public CharacterController controller;

    //int reflections = 0;
    float size = 0;
    bool alive = true;


    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (alive)
        {
            if (controller.enabled)
                controller.Move(direction * speed * Time.deltaTime);

            Vector3 directionToGoal = (goal.position - transform.position).normalized;
            direction = Vector3.MoveTowards(direction, directionToGoal, Time.deltaTime * steering).normalized;

            if (size > 2 && alive)
            {
                GridDistortion.current.Distort(team == PlayerTeam.CYAN ? 0 : 1, transform.position, size - 2);
            }
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        bool hitGround = Vector3.Dot(hit.normal, Vector3.up) > 0.5f;

        //don't collide with the ground
        if (hitGround)
            return;

        if (hit.gameObject.CompareTag("Player"))
        {
            //hitting a player
            Player otherPlayer = hit.gameObject.GetComponent<Player>();
            if (otherPlayer == null)
            {
                Kill();
                return;
            }
            if (otherPlayer == this)
            {
                return;
            }

            otherPlayer.Hit(damage);
            if(damage > 20){
                Screenshake.current.Shake(0.5f, 0.1f);
            }
            if (size > 2)
            {
                GridDistortion.current.Distort(team == PlayerTeam.CYAN ? 0 : 1, hit.transform.position, size - 2);
                GridDistortion.current.Expand(team == PlayerTeam.CYAN ? 0 : 1, (size - 2) * -10, 250);

                if (particles) Instantiate(particles, hit.transform.position, Quaternion.identity).GetComponent<HitParticles>().InitSystem(team, (int)(Mathf.Pow(size , 2.5f)*2));
            }
            Kill();
        }
        else if (hit.gameObject.CompareTag("Bullet"))
        {
            //hitting another bullet
            Bullet otherBullet = hit.gameObject.GetComponent<Bullet>();
            if (otherBullet == null)
            {
                Kill();
                return;
            }

            if (otherBullet.damage > damage+0.25f)
            {
                Kill();
            } else if(otherBullet.damage < damage-0.25f)
            {
                otherBullet.Kill();
            } else
            {
                otherBullet.Kill();
                Kill();
            }
        }
        else if(hit.gameObject.CompareTag("Bouncy"))
        {
            Vector3 flatnormal = hit.normal;
            flatnormal.y = 0;
            direction = Vector3.Reflect(direction, flatnormal);

            //tell the bounce object that it was collided with
            Bouncy bounce = hit.gameObject.GetComponent<Bouncy>();
            if (bounce)
            {
                bounce.Bounce();
            }
        } else {
            Kill();
        }
    }

    public void Kill()
    {
        alive = false;
        Destroy(gameObject);
    }

    public void SetSize(float newSize)
    {
        newSize = newSize < 0.5f?0:Mathf.Max(0.5f, newSize);
        size = newSize;
        ballMesh.localScale = Vector3.one * newSize * baseMeshScale;
        controller.radius = newSize * baseControllerRadius;

        if (newSize < 1)
        {
            light.enabled = false;
        }
        else
        {
            light.enabled = true;
            light.intensity = (newSize - 1);
        }
    }
}
