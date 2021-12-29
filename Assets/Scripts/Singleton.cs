using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton 
{
    private static Singleton current;
    public static Singleton instance
    {
        get
        {
            if (current==null)
            {
                current = new Singleton();
            }
            return current;
        }
    }
    public int Lifes=5;
}
