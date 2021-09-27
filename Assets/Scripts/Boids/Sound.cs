using System;
using UnityEngine;
// To play sound using Clip, the process need to be alive.
// Hence, we use a Swing application.
public class Sound {
    private readonly string _filename;


    public Sound(String filename)
    {
        _filename = filename;
        Debug.Log($"Start Sound {_filename}");
    }
    public void stopSong(){
        Debug.Log($"Stop Sound {_filename}");

    }
}