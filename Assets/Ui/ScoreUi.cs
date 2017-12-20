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

    public WinUi win;

    public float backgroundSliderDecaySpeed = 10;

    float time = 60;

    Coroutine magentaFadingRoutine;
    Coroutine cyanFadingRoutine;

	// Use this for initialization
	void Start () {
        cyanPlayer.healthUpdate.AddListener(UpdateSlider);
        magentaPlayer.healthUpdate.AddListener(UpdateSlider);
        
        UpdateSlider(PlayerTeam.CYAN, 100);
    }
	
	void Update () {
        time -= Time.deltaTime;

        if(time < 0)
        {
            time = 0;
            
            if(cyanPlayer.health > magentaPlayer.health) {
                win.Win(PlayerTeam.CYAN);
            } else if(magentaPlayer.health > cyanPlayer.health) {
                win.Win(PlayerTeam.MAGENTA);
            } else {
                win.Win(PlayerTeam.NEITHER);
            }
        }

        countdown.text = Mathf.Floor(time).ToString();
	}

    void UpdateSlider(PlayerTeam team, float health)
    {
        cyanSlider.value = health;
        if(cyanFadingRoutine != null)
            StopCoroutine(cyanFadingRoutine);
        cyanFadingRoutine = StartCoroutine(fadeHealth(cyanBackgroundSlider, health));
        if (health <= 0) win.Win(team);
    }

    IEnumerator fadeHealth(Slider healthbar, float health)
    {
        yield return new WaitForSeconds(1f);
        while(healthbar.value > health) {
            healthbar.value = Mathf.MoveTowards(healthbar.value, health, Time.deltaTime * backgroundSliderDecaySpeed);
            yield return null;
        }
    }
}
