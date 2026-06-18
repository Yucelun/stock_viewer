using System;
using System.Collections.Generic;

public class SingletonFactory
{
    private Dictionary<Type, object> instances = new Dictionary<Type, object>();

    public T getInstance<T>() where T : new()
    {
        Type type = typeof(T);
        object obj;
        if (!this.instances.TryGetValue(type, out obj)) {
            obj = new T();

            IBaseSingleton baseSingleton = obj as IBaseSingleton;
            if (baseSingleton != null)
            {
                baseSingleton.Awake();
                baseSingleton.Start();
            }

            this.instances.Add(type, obj);
        }

        return (T)obj;
    }

    public void setInstance<T>(object obj)
    {
        Type type = typeof(T);
        if (!this.instances.ContainsKey(type))
        {
            this.instances.Add(type, obj);
        }
    }

    public void delete<T>() where T : new()
    {
        Type type = typeof(T);
        object obj;
        if (!this.instances.TryGetValue(type, out obj))
        {
            IBaseSingleton baseSingleton = obj as IBaseSingleton;
            if (baseSingleton != null)
            {
                baseSingleton.OnDestroy();
            }
            this.instances.Remove(type);
        }
    }

    public void deleteGameSingleton() {
        foreach (var item in instances)
        {
            BaseGameSingleton baseGameSingleton = item.Value as BaseGameSingleton;
            if (baseGameSingleton != null)
            {
                baseGameSingleton.OnDestroy();
                this.instances.Remove(item.Key);
            }
        }
    }
}
