using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class Data : MonoBehaviour {

    public static Dictionary<String, KeyCode> keys;
    public bool isFirstRun = true;
    public int screenIndex = 1;
    public int resolutionIndex;
    public float masterVolume;
    public float sfxVolume;
    public float musicVolume;
    public int qualityIndex;
    public static float mouseVertical;
    public static float mouseHorizontal;

    /*
    public static int xSize;
    public static int zSize;
    public static float Doors;
    */


    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/settings.dat");

        PlayerData data = new PlayerData();
        //set data to local
        data.isFirstRun = isFirstRun;
        data.screenIndex = screenIndex;
        data.resolutionIndex = resolutionIndex;
        data.masterVolume = masterVolume;
        data.sfxVolume = sfxVolume;
        data.musicVolume = musicVolume;
        data.qualityIndex = qualityIndex;
        data.mouseVertical = mouseVertical;
        data.mouseHorizontal = mouseHorizontal;
        /*
        data.xSize = xSize;
        data.zSize = zSize;
        data.Doors = Doors;
        */
        bf.Serialize(file, data);
        file.Close();
    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/settings.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/settings.dat", FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();

            //set local to data
            isFirstRun = data.isFirstRun;
            screenIndex = data.screenIndex;
            resolutionIndex = data.resolutionIndex;
            masterVolume = data.masterVolume;
            sfxVolume = data.sfxVolume;
            musicVolume = data.musicVolume;
            qualityIndex = data.qualityIndex;
            mouseVertical = data.mouseVertical;
            mouseHorizontal = data.mouseHorizontal;
            /*
            xSize = data.xSize;
            zSize = data.zSize;
            Doors = data.Doors;
            */
        }
    }
}


[Serializable]
class PlayerData
{
    public Dictionary<String, KeyCode> keys;

    public bool isFirstRun;
    public int screenIndex;
    public int resolutionIndex;
    public float masterVolume;
    public float sfxVolume;
    public float musicVolume;
    public int qualityIndex;
    public float mouseVertical;
    public float mouseHorizontal;

    public int xSize;
    public int zSize;
    public float Doors;
}
