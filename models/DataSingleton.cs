using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CSharp_Discord_Bot.models;

public class DataSingleton
{
    private static DataSingleton instance;

    // Private constructor to prevent instantiation from other classes.
    private DataSingleton() { }

    // Public method to provide access to the Singleton instance.
    public static DataSingleton GetInstance()
    {
        // Check if the instance is null; if it is, create a new instance.
        if (instance == null)
        {
            instance = new DataSingleton();
        }
        return instance;
    }

    // Add your Singleton's methods and properties here.
    public HatList hats;
}