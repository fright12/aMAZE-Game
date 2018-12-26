using UnityEngine;
using System.Collections;

public static class access {
    public static player player1 {
        get { return new player((GameObject)store.findObj(settings.player1), new Color(settings.color1[0], settings.color1[1], settings.color1[2], settings.color1[3])); }
    }

    public static player player2 { 
        get { return new player((GameObject)store.findObj(settings.player2), new Color(settings.color2[0], settings.color2[1], settings.color2[2], settings.color2[3])); }
    }

    public static Material wall {
        get { return (Material)store.findObj(settings.wall); }
    }

    public static Material ground {
        get { return (Material)store.findObj(settings.ground); }
    }

    public static Material skybox {
        get { return (Material)store.findObj(settings.skybox); }
    }
}