using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    public UnityEngine.Transform[] waypoints;

    public int attackRangeTiles = 1;

    public int fovRangeTiles = 3;

    public static int currentWaypointIndex = 0;
}
