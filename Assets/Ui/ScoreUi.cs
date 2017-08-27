using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUi : MonoBehaviour {

    public Player cyan;
    public Player magenta;

    public Slider cyanSlider;
    public Slider cyanBackgroundSlider;
    public Slider magentaSlider;
    public Slider magentaBackgroundSlider;

    public Text countdown;

    public WinUi win;

    public float backgroundSliderDecaySpeed = 10;

    float time = 60;

    Coroutine magentaRoutine;
    Coroutine cyanRoutine;

	// Use this for initialization
	void Start () {
        cyan.healthUpdate.AddListener(UpdateCyan);
        magenta.healthUpdate.AddListener(UpdateMagenta);
        UpdateCyan();
        UpdateMagenta();
    }
	
	// Update is called once per frame
	void Update () {
        time -= Time.deltaTime;

        if(time < 0)
        {
            time = 0;
            
            if(cyan.health > magenta.health)
            {
                win.Win(PlayerTeam.CYAN);
            } else if(magenta.health > cyan.health)
            {
                win.Win(PlayerTeam.MAGENTA);
            } else
            {
                win.Win(PlayerTeam.NEITHER);
            }
        }

        countdown.text = Mathf.Floor(time).ToString();
	}

    void UpdateCyan()
    {
        cyanSlider.value = cyan.health;
        if(cyanRoutine != null)
            StopCoroutine(cyanRoutine);
        cyanRoutine = StartCoroutine(takeHealth(cyanBackgroundSlider, cyan.health));
        if (cyan.health <= 0) win.Win(PlayerTeam.MAGENTA);
    }

    void UpdateMagenta()
    {
        magentaSlider.value = magenta.health;
        if(magentaRoutine != null)
            StopCoroutine(magentaRoutine);
        magentaRoutine = StartCoroutine(takeHealth(magentaBackgroundSlider, magenta.health));
        if (magenta.health <= 0) win.Win(PlayerTeam.CYAN);
    }

    IEnumerator takeHealth(Slider healthbar, float health)
    {
        yield return new WaitForSeconds(1f);
        while(healthbar.value > health)
        {
            healthbar.value = Mathf.MoveTowards(healthbar.value, health, Time.deltaTime * backgroundSliderDecaySpeed);
            yield return null;
        }
    }
}
