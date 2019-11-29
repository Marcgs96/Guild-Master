using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationManager : MonoBehaviour
{
    [System.Serializable]
    public struct Location
    {
        public GameObject location;
        public Dictionary<GameObject, bool> positions;
    }

    [SerializeField]
    List<Location> locations;

    public GameObject locations_parent;
    public GameObject knight_fight_location;
    private Location duel_location;

    private void Awake()
    {
        for (int i = 0; i < locations_parent.transform.childCount; i++)
        {
            Location new_location;
            new_location.location = locations_parent.transform.GetChild(i).gameObject;

            new_location.positions = new Dictionary<GameObject, bool>();
            for (int x = 0; x < new_location.location.transform.childCount; x++)
            {
                new_location.positions.Add(new_location.location.transform.GetChild(x).gameObject, false);
            }

            locations.Add(new_location);
            if (new_location.location == knight_fight_location)
                duel_location = new_location;
        }
    }

    internal void AssignDuelLocations(KnightMember agent_1, KnightMember agent_2)
    {
        foreach (KeyValuePair<GameObject, bool> position in duel_location.positions)
        {
            if (!position.Value)
            {
                Debug.Log("TIME TO FIGHT");
                agent_1.assigned_position = position.Key.transform.GetChild(0).gameObject;
                agent_2.assigned_position = position.Key.transform.GetChild(1).gameObject;

                agent_1.go_duel = true;
                agent_2.go_duel = true;

                agent_1.opponent = agent_2;
                agent_2.opponent = agent_1;
            }
        }
    }

    public GameObject GetAvailablePosition(GameObject location)
    {
        foreach (Location loc in locations)
        {
            if(loc.location == location)
            {
                foreach (KeyValuePair<GameObject,bool> position in loc.positions)
                {
                    if (!position.Value)
                    {
                        loc.positions[position.Key] = true;
                        return position.Key;
                    }

                }
            }
        }

        return location;
    }

    public void ReleasePosition(GameObject position)
    {
        if (position == null)
            return;

        foreach (Location loc in locations)
        {
            if (loc.location != position)
            {
                if(loc.positions.ContainsKey(position))
                    loc.positions[position] = false;
            }
            else
                return;
        }
    }
}
