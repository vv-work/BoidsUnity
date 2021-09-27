// To play sound uSing Clip, the process need to be alive.
// Hence, we use a Swing application.

using UnityEngine;

public class Sound {
    private readonly string _filename;


    public Sound(string filename)
    {
        _filename = filename;
        Debug.Log($"Start Sound {_filename}");
    }
    public void stopSong(){
        Debug.Log($"Stop Sound {_filename}");

    }
}