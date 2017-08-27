using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour {


    public PlayerTeam team;
    public float gravity;
    public float speed;
    public GameObject bullet;
    public Material cyanMaterial;
    public Material magentaMaterial;
    public float chargeSpeed;
    public Player otherPlayer;
    public float damage;
    public Animator anim;

    [HideInInspector]public UnityEvent healthUpdate;

    [HideInInspector]public float health = 100;
    CharacterController controller;
    float fallSpeed;
    Vector3 direction = Vector3.forward;
    float shooting = 0;
    Transform shotInProgress;
    Bullet shotInProgressBullet;


    // Use this for initialization
    void Awake () {
        controller = GetComponent<CharacterController>();

       // Time.timeScale = 0.1f;
	}
	
	// Update is called once per frame
	void Update () {
        string vertical = "", horizontal = "", shoot = "";
        if(team == PlayerTeam.CYAN)
        {
            vertical = "Vertical_Cyan";
            horizontal = "Horizontal_Cyan";
            shoot = "Fire_Cyan";
        } else if(team == PlayerTeam.MAGENTA)
        {
            vertical = "Vertical_Magenta";
            horizontal = "Horizontal_Magenta";
            shoot = "Fire_Magenta";
        } else
        {
            Debug.Log("Player without team needs help");
        }

        Move(horizontal, vertical);
        Shoot(shoot);
	}

    void Move(string horizontal, string vertical)
    {
        fallSpeed += gravity * Time.deltaTime;
        Vector3 velocity = Vector3.down * fallSpeed;

        velocity.x = Input.GetAxis(horizontal) * speed / (1 + Mathf.Min(shooting, 5f)*0.5f);
        velocity.z = Input.GetAxis(vertical) * speed / (1 + Mathf.Min(shooting, 5f) * 0.5f);

        controller.Move(velocity * Time.deltaTime);

        if (controller.isGrounded)
            fallSpeed = 0;

        velocity.Scale(new Vector3(1, 0, 1));
        anim.SetFloat("Speed", velocity.magnitude / speed);
        if (velocity.magnitude > 0.1f)
        {
            direction = velocity.normalized;
            transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(velocity.x, velocity.z) * Mathf.Rad2Deg, Vector3.up);
        }
    }

    void Shoot(string shoot)
    {
        if (direction == Vector3.zero)
            return;
        if (Input.GetButtonDown(shoot))
        {
            if (shotInProgress)
            {
                shotInProgressBullet.Kill();
            }
            shotInProgress = Instantiate(bullet, transform.position + direction, Quaternion.identity).transform;
            shotInProgressBullet = shotInProgress.GetComponent<Bullet>();
            shotInProgressBullet.controller.enabled = false;
            shotInProgressBullet.team = team;
            shotInProgressBullet.goal = otherPlayer;
            shotInProgress.GetComponentInChildren<Renderer>().material = team == PlayerTeam.CYAN ? cyanMaterial:magentaMaterial;
            shotInProgress.GetComponentInChildren<Light>().color = team == PlayerTeam.CYAN ? new Color(0, 1, 1) : new Color(1, 0, 1);

            anim.SetBool("Shooting", true);
        }
        if (shotInProgress != null)
        {
            shooting = shooting + Time.deltaTime * chargeSpeed;
            shotInProgress.position = transform.position + direction * (2f + Mathf.Min(shooting, 5f)*shotInProgressBullet.baseControllerRadius);
            shotInProgressBullet.SetSize(Mathf.Min(shooting, 5f));

            if(shooting > 4)
            {
                Screenshake.current.shaking = true;
            }


            if (!Input.GetButton(shoot) || shooting > 10f)
            {
                anim.SetFloat("ShotIntensity", shooting / 5);
                anim.SetBool("Shooting", false);

                if(shooting < 0.01f)
                {
                    shotInProgressBullet.Kill();
                    shotInProgress = null;
                    shotInProgressBullet = null;
                    shooting = 0;
                    return;
                }

                shotInProgressBullet.damage = Mathf.Min(shooting, 5f) * Mathf.Pow(damage, 1.2f);
                shotInProgressBullet.direction = direction;
                shotInProgressBullet.controller.enabled = true;
                shotInProgressBullet.SetSize(Mathf.Clamp(shooting,0.5f,5f));
                shotInProgress = null;
                shotInProgressBullet = null;
                shooting = 0;
            }
        }
    }

    public void Hit(float damage)
    {
        if(damage > 10)
        {
            anim.SetTrigger("Hit");
        }
        health -= damage;
        healthUpdate.Invoke();
    }
}

public enum PlayerTeam{
    CYAN, MAGENTA, NEITHER
}