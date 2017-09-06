using System;

/// <summary>
/// Summary description for Class1
/// </summary>
public class MainPlayerModel
{
    public static int PkModeIndex = 0;
    public enum PlayerState
    {
        dead,//死亡
        normal
    }

    public static PlayerState playerState = PlayerState.normal;

    public static float speed = 5f;
    public static float pos_x = 0;
    public static float pos_z = 0;
    public static float dir = 0;
    public static long roleID = 0;
    public static string roleName = "";
    public static int mapId = 0;
    public static int lineID = 0;
    public static int prof = 0;
    public static int level = 0;
    public static int fight = 0;
    public static int vipLevel = 0;
    public static long lastLoginTime = 0;
    public static int icon = 0;
    public static int arms = 0;
    public static int dress = 0;
    public static int rideId = 0;
    public static int fashionshead = 0;
    public static int fashionsarms = 0;
    public static int fashionsdress = 0;
    public static int wuhun = 0;
    public static int wing = 0;
    public static int suitflag = 0;
    public static byte shenwuId = 0;
    public static byte transferId = 0;
    public static bool isRide = false;
}
