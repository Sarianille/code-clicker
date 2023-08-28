using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberSuffixes : MonoBehaviour
{
    public string FormatNumber(ulong number)
    {
        string numberString = number.ToString();
        string[] suffixes = { "", "M", "B", "T", " Quad", " Quin" };
        ulong divisor = 1000000;
        int counter = 0;

        while ((number / divisor) > 0)
        {
            counter++;
            divisor *= 1000;
        }

        if (counter > 0)
        {
            divisor /= 1000;

            var roundedNumber = Math.Round((double)number / divisor, 2, MidpointRounding.AwayFromZero);
            numberString = roundedNumber.ToString() + suffixes[counter];
        }

        return numberString;
    }
}
