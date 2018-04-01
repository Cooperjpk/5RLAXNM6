using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility {

    public static bool IsPositive(this int number)
    {
        return number > 0;
    }

    public static bool IsNegative(this int number)
    {
        return number < 0;
    }

    public static bool IsZero(this int number)
    {
        return number == 0;
    }

}
