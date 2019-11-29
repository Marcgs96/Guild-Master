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
