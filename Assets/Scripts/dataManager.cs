using UnityEngine;
using System.Collections;

public class dataManager : MonoBehaviour {
    public static dataManager instance;

    public static dataFile[] data;

    void Awake()
    {
        //Check if instance already exists
        if (instance == null)
            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)
        {
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad (gameObject);

        dataFile.directory = Application.persistentDataPath;

        data = new dataFile[] {
            new dataFile (new settings(), "userPrefs.up"),
            new dataFile(GameObject.Find("MenuController").GetComponent<store>(), "store.st"),
            new dataFile(new game(), "game.gm"),
        };
    }

    void OnApplicationQuit()
    {
        Debug.Log("saved");

        foreach (cell c in game.currentMaze.grid)
            c.isEmpty = true;

        foreach (dataFile df in data)
            df.save();
    }
}
