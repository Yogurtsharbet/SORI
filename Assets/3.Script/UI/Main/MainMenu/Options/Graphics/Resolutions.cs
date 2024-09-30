using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum ResolutionType {
    FullHD = 0,
    HD,
    QHD,
    HDPlus
}
public class Resolutions {
    private static (int, int)[] resolutionValues = new (int, int)[] {
        (1920,1080),    //FullHD
        (1280, 720),    //HD
        (2560, 1440),   //QHD
        (1600, 900)    //HDPlus
    };

    public static (int, int) GetResolutionByNum(int num) { return resolutionValues[num]; }
    public static ResolutionType GetResolutionTypeByNum(int num) { return (ResolutionType)num; }
    public static (int, int) GetResolutionByName(ResolutionType resolutionType) { return resolutionValues[(int)resolutionType]; }
    public static (int, int) FullHD { get { return resolutionValues[(int)ResolutionType.FullHD]; } }
    public static (int, int) HD { get { return resolutionValues[(int)ResolutionType.HD]; } }
    public static (int, int) QHD { get { return resolutionValues[(int)ResolutionType.QHD]; } }
    public static (int, int) HDPlus { get { return resolutionValues[(int)ResolutionType.HDPlus]; } }
}
