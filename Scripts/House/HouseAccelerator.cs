using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

interface IEHouse
{
    HouseTypes type { get; }
    Vector2 position { get; }
}


class House: Building
{
    public HouseTypes type { get; }

    Dictionary<string, HouseTypes> fromDecodeDataToHouseType = new Dictionary<string, HouseTypes>
    {
        { "home", HouseTypes.Home },
        { "work", HouseTypes.Work },
        { "entertainment", HouseTypes.Entertainment }
    };

    public House (String theType, float[] thePosition)
    {
        type = fromDecodeDataToHouseType[theType];
        position = thePosition;
    }
}

public class HouseAccelerator : MonoBehaviour
{
    Dictionary<HouseTypes, GameObject> houseRelaitonsBetweenTypeAndModel;

    List<House> MockHouses = new List<House>();

    private void Awake()
    {
        houseRelaitonsBetweenTypeAndModel = new Dictionary<HouseTypes, GameObject> {
        {
            HouseTypes.Entertainment, Resources.Load<GameObject>("Entertainment")
        },
        {
            HouseTypes.Home, Resources.Load<GameObject>("Home")
        },
        {
            HouseTypes.Work, Resources.Load<GameObject>("Work")
        }
    };
    }

    void Start()
    {
        GetHousePositions();
        genterateHouse();
    }

    void genterateHouse()
    {
        for (int i = 0; i < MockHouses.Count; ++i)
        {
            Instantiate(houseRelaitonsBetweenTypeAndModel[MockHouses[i].type], new Vector3(MockHouses[i].position[0], 0, MockHouses[i].position[1]), Quaternion.identity);
        }

    }

    void GetHousePositions()
    {
        Building[] notParsedBuildings = MainServer.getBuildingsData();
        Debug.Log(notParsedBuildings);
        foreach (var building in notParsedBuildings)
        {
            MockHouses.Add(new House(building.type, building.position));
        }
    }
}


enum HouseTypes
{
    Home,
    Entertainment,
    Work
}
