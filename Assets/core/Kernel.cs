using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kernel
{
    static SingletonFactory singletonFactory = new SingletonFactory();
    public static T get<T>() where T : new() {
        return singletonFactory.getInstance<T>();
    }
}
