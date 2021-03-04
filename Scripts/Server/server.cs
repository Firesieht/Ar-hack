using UnityEngine;
using System.Net.Sockets;
using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using Newtonsoft.Json;

public static class MainServer 
{
    private static Socket _connectionInstance = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    public static void Connect3(string host, int port)
    {
        _connectionInstance = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        _connectionInstance.Connect(host, port);
    }
    public static string send(String text)
    {
        string ans = "";
        byte[] msg = Encoding.UTF8.GetBytes(text);
        byte[] bytes = new byte[1024];
        try
        {
            int i = _connectionInstance.Send(msg);
            i = _connectionInstance.Receive(bytes);
            Close();
            Connect3("188.68.221.63", 10000);
            ans = Encoding.UTF8.GetString(bytes);
        }
        catch (SocketException e)
        {
            Debug.Log("{0} Error code: {1}." + e.Message + e.ErrorCode);
        }
        return ans;
    }
    private static string getData(MapTypes data)
    {
        Dictionary<MapTypes, string> EncodeMapTypes = new Dictionary<MapTypes, string>
        {
            { MapTypes.Person, "people" },
            { MapTypes.House, "town" },
            { MapTypes.Terrain, "terrain" }
        };


        string ans = "";
        var readText = Resources.Load<TextAsset>("JSONs/" + EncodeMapTypes[data]);
        Debug.Log(readText);
        try
        {
            

        } catch { }
        string ans_serv = MainServer.send(readText.text);
        Close();
        Connect3("188.68.221.63", 10000);
        Debug.Log(ans_serv);
        return ans_serv;
    }

    public static IEPersonData[] getPeopleRegistration()
    {

        string data = getData(MapTypes.Person);
        Debug.Log(data);
        
        IEPersonAnswer decodedAnswer = JsonUtility.FromJson<IEPersonAnswer>(data);
        foreach(var i in decodedAnswer.people) {
            Debug.Log(i.velocity);
            Debug.Log(i.position);
            Debug.Log(i.id);
        }

        return decodedAnswer.people;
    }
    

    public static List<int[]> getTerrainData()
    {
        List<int[]> res = new List<int[]>();

        string data = getData(MapTypes.Terrain);
        Debug.Log(data);
        ITerrainData decodedData = JsonUtility.FromJson<ITerrainData>(data);

        foreach(var line in decodedData.data)
        {
            res.Add(line.line);
        }

        Debug.Log(decodedData.data.Length);

        return res;
    }


    public static Building[] getBuildingsData()
    {
        return JsonUtility.FromJson<AnotherBuildingProvider>(getData(MapTypes.House)).data.buildings;
    }

    public static void Close()
    {
        try
        {
            _connectionInstance.Shutdown(SocketShutdown.Both);
            _connectionInstance.Close();
        }
        catch
        {
        }

    }

}

[System.Serializable]
public class IEPersonAnswer
{
    public IEPersonData[] people;
}
[System.Serializable]
public class IEPersonData
{
    public int[] velocity;
    public int[] position;
    public int id;
}

[System.Serializable]
public class ITerrainData
{
    public TerrainDataLine[] data;
}
[System.Serializable]
public class TerrainDataLine
{
    public int[] line;
}


[System.Serializable]
public class AnotherBuildingProvider
{
    public BuildingProvider data;
}
[System.Serializable]
public class BuildingProvider
{
    public Building[] buildings;
}

[System.Serializable]
public class Building
{
    public string type;
    public float[] position;

}

enum MapTypes
{
    Person,
    House,
    Terrain
}

