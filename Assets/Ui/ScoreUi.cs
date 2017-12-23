using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUi : MonoBehaviour {

    public Player cyanPlayer;
    public Player magentaPlayer;

    public Slider cyanSlider;
    public Slider cyanBackgroundSlider;
    public Slider magentaSlider;
    public Slider magentaBackgroundSlider;

    public Text countdown;

    public PauseUi pause;

	public float backgroundSliderDecayDelay = 1;
    public float backgroundSliderDecaySpeed = 10;

	public float maximumMatchTime = 60;

    float time = 60;

    Coroutine magentaFadingRoutine;
    Coroutine cyanFadingRoutine;

	// Use this for initialization
	void Start () {
        cyanPlayer.healthUpdate.AddListener(UpdateSlider);
        magentaPlayer.healthUpdate.AddListener(UpdateSlider);
        
		UpdateSlider(cyanPlayer.team, 100);
		UpdateSlider(magentaPlayer.team, 100);

		time = maximumMatchTime;
    }
	
	void Update () {
        time -= Time.deltaTime;

        if(time < 0)
        {
            time = 0;
            
            if(cyanPlayer.health > magentaPlayer.health) {
				pause.Win(cyanPlayer.team);
            } else if(magentaPlayer.health > cyanPlayer.health) {
				pause.Win(magentaPlayer.team);
            } else {
				pause.Win(null);
            }
        }

        countdown.text = Mathf.Floor(time).ToString();
	}

	void UpdateSlider(TeamProperty team, float health)
    {
		Slider teamSlider = null;
		switch (team.team) {
		case PlayerTeam.CYAN:
			teamSlider = cyanSlider;
			StartFadingRoutine (ref cyanFadingRoutine, ref cyanBackgroundSlider, health);
			break;
		case PlayerTeam.MAGENTA:
			teamSlider = magentaSlider;
			StartFadingRoutine (ref magentaFadingRoutine, ref magentaBackgroundSlider, health);
			break;
		default:
			Debug.Log ("Can't update player!");
			return;
		}

		teamSlider.value = health;
		if (health <= 0) pause.Win(team);
    }

	void StartFadingRoutine(ref Coroutine teamRoutine, ref Slider teamSlider, float health){
		if(teamRoutine != null)
			StopCoroutine(teamRoutine);
		teamRoutine = StartCoroutine(fadeHealth(teamSlider, health));
	}

    IEnumerator fadeHealth(Slider healthbar, float health)
    {
		yield return new WaitForSeconds(backgroundSliderDecayDelay);
        while(healthbar.value > health) {
			healthbar.value = Mathf.MoveTowards(healthbar.value, health, Time.unscaledDeltaTime * backgroundSliderDecaySpeed);
            yield return null;
        }
    }
}
