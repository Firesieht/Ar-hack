using System.Collections;
using System.Collections.Generic;
using UnityEngine;


class Person : IEPersonData
{
    public int id { get; }
    public Person(int[] theVelocity, int[] thePosition)
    {
        velocity = theVelocity;
        position = thePosition;
    }
}

public class PlayerAccelerator : MonoBehaviour
{
    [SerializeField]
    GameObject ChildPrefab;

    public IEPersonData[] persons;
    void Start()
    {
        MainServer.Connect3("188.68.221.63", 10000);
        MainServer.getTerrainData();
        System.Threading.Thread.Sleep(1000);
        setPersons();
        GenerateFunc();


        MainServer.getPeopleRegistration();

    }
    void GenerateFunc()
    {
        for (int i = 0; i < persons.Length; ++i)
        {
            GameObject player = Instantiate(ChildPrefab, new Vector3(persons[i].position[0], 3, persons[i].position[1]), Quaternion.identity);
            player.GetComponent<PlayerMovement>().PlayerVelocity = new Vector2(persons[i].velocity[0], persons[i].velocity[1]);
        }
    }

    void setPersons()
    {
        persons = MainServer.getPeopleRegistration();
    }
}
