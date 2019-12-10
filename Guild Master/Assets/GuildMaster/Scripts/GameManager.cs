using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager manager;

    public MemberManager members;
    public ResourceManager resources;
    public QuestManager quests;
    public UIManager ui;
    public DayNightCicle time;
    public LocationManager locations;

    //make locations/buildings manager ?
    public Building[] buildings;
    public AudioSource[] audios;
    internal bool finished;

    void Awake()
    {
        manager = this;
        PauseGame();
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

    internal void FinishGame(bool state)
    {
        finished = true;
        ui.ShowFinishPanel(state);
        PauseGame();
    }

    internal int GetScore()
    {
        int time_score = ((time.day - 1) * 10) + (time.hour * 60);
        int member_score = members.GetMemberScore();
        int building_score = (int)(buildings[0].GetLevel() + buildings[1].GetLevel()) * 2;
        int resource_score = resources.GetResourcesScore();

        Debug.Log(time_score + " " + member_score + " " + building_score + " " + resource_score);

        int total_score = member_score + building_score + resource_score - time_score;

        if (total_score < 0)
            total_score = 0;

        return total_score;
    }
}
