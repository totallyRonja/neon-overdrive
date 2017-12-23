using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[SelectionBase]
public class Player : MonoBehaviour {

    [SerializeField] public TeamProperty team;
    [SerializeField] float gravity;
    [SerializeField] float speed;
    [SerializeField] GameObject bullet;
    [SerializeField] float chargeSpeed;
    [SerializeField] float damage;
    [SerializeField] Animator anim;

    [HideInInspector] public HealthUpdate healthUpdate = new HealthUpdate();
    
    [HideInInspector] public float health = 100;
    CharacterController controller;
    float fallSpeed;
    Vector3 direction = Vector3.forward;
    float shooting = 0;
    Transform shotInProgress;
    Transform opponent;
    Bullet shotInProgressBullet;


    void Awake () {
        controller = GetComponent<CharacterController>();
	}

    void Start(){
        //find the opponent to home in later
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < players.Length;i++){
            if (players[i] != gameObject){
                opponent = players[i].transform;
                break;
            }
        }
    }

    void OnEnable(){
        if (!team) {
            Debug.Log("Player without team needs help");
            enabled = false;
        }
    }
	
	void Update () {
        Move();
        Shoot();
	}

    void Move()
    {
        fallSpeed += gravity * Time.deltaTime;
        Vector3 velocity = Vector3.down * fallSpeed;

        velocity.x = Input.GetAxis(team.horizontalInput) * speed / (1 + Mathf.Min(shooting, 5f)*0.5f);
        velocity.z = Input.GetAxis(team.verticalInput) * speed / (1 + Mathf.Min(shooting, 5f) * 0.5f);

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

    void Shoot()
    {
        if (direction == Vector3.zero)
            direction = transform.forward;
        
        if (Input.GetButtonDown(team.shotInput))
        {
            if (shotInProgress)
            {
                shotInProgressBullet.Kill();
            }
            shotInProgress = Instantiate(bullet, transform.position + direction, Quaternion.identity).transform;
            shotInProgressBullet = shotInProgress.GetComponent<Bullet>();
            shotInProgressBullet.controller.enabled = false;
            shotInProgressBullet.team = team;
            shotInProgressBullet.goal = opponent;
            shotInProgress.GetComponentInChildren<Renderer>().material = team.bulletMat;
            shotInProgress.GetComponentInChildren<Light>().color = team.teamColor;

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

            if (!Input.GetButton(team.shotInput) || shooting > 10f)
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
        healthUpdate.Invoke(team, health);
    }
}

public class HealthUpdate : UnityEvent<TeamProperty, float> {

}