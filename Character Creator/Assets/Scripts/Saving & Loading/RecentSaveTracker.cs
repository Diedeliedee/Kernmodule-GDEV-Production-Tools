using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public static class RecentSaveTracker
{
    private const string registerKey        = "Registered_Saves";
    private const int maxRegisteredSaves    = 3;

    public static void RegisterRecentSave(string _location)
    {
        //  Get the list of registered saves.
        var registeredSaves = GetRegisteredSaves();

        //  If there have not been any previous saves. Create a new list.
        registeredSaves ??= new();

        //  Creating a class to register the save.
        var date            = DateTime.Now;
        var registration    = new SaveRegistration()
        {
            location    = _location,
            date        = $"{date.Hour}:{date.Minute} {date.Day}-{date.Month}-{date.Year}",
            version     = GlobalData.version
        };

        //  Adding the registered save to the list.
        registeredSaves.Add(registration);

        //  Keeping the list small.
        var loopBreaker = 0;
        while (registeredSaves.Count > maxRegisteredSaves)
        {
            if (loopBreaker > 100) break;
            registeredSaves.RemoveAt(0);
            loopBreaker++;
        }

        //  Converting the list to JSON, and saving it in the preferences.
        var json = JsonConvert.SerializeObject(registeredSaves);

        PlayerPrefs.SetString(registerKey, json);
        PlayerPrefs.Save();
    }

    public static List<SaveRegistration> GetRegisteredSaves()
    {
        if (!PlayerPrefs.HasKey(registerKey))
        {
            Debug.Log("Player has not previously saved!");
            return null;
        }

        var json = PlayerPrefs.GetString(registerKey);

        try
        {
            return JsonConvert.DeserializeObject<List<SaveRegistration>>(json);
        }
        catch { return null; }
    }
}