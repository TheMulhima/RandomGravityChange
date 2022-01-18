using System;

namespace RandomGravityChange;

public static class HeroControllerUtil
{
    //normally we cant store based only on name but since its only for HC it'll do
    private static Dictionary<string, PropertyInfo> HCProperties = new Dictionary<string, PropertyInfo>();
    private static List<string> HCFields = new List<string>();
    private static Dictionary<string, MethodInfo> HCMethods = new Dictionary<string, MethodInfo>();

    static HeroControllerUtil()
    {
        //front load the cost of doing this
        FieldInfo[] fieldInfos = typeof(HeroController).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        foreach (var fieldinfo in fieldInfos)
        {
            HCFields.Add(fieldinfo.Name);
        }
        PropertyInfo[] propertyInfos = typeof(HeroController).GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        foreach (var propertyinfo in propertyInfos)
        {
            HCProperties.Add(propertyinfo.Name, propertyinfo);
        }
    }
    
    public static T Get<T>(this HeroController self, string name)
    {
        if (HCFields.Contains(name))
        {
            return ReflectionHelper.GetField<HeroController, T>(self, name);
        }
        else if (HCProperties.TryGetValue(name, out PropertyInfo info))
        {
            return (T) info.GetValue(self);
        }
        else
        {
            throw new Exception($"DIDNT FIND FIELD OR PROPERTY {name}");
        }
    }

    public static void Set<T>(this HeroController self, string name, T val)
    {
        if (HCFields.Contains(name))
        {
            ReflectionHelper.SetField(self, name, val);
        }
        else if (HCProperties.TryGetValue(name, out PropertyInfo info))
        {
            info.SetValue(self, val);
        }
        else
        {
            throw new Exception($"DIDNT FIND FIELD OR PROPERTY {name}");
        }
    }

    public static int Increment(this HeroController self, string name)
    {
        int tmp = self.Get<int>(name);
        tmp += 1;
        self.Set(name, tmp);
        return tmp;
    }

    public static int Decrement(this HeroController self, string name)
    {
        int tmp = self.Get<int>(name);
        tmp -= 1;
        self.Set(name, tmp);
        return tmp;
    }

    public static void Add(this HeroController self, string name, float val)
    {
        float tmp = self.Get<float>(name);
        tmp += val;
        self.Set(name, tmp);
    }
    public static void Subtract(this HeroController self, string name, float val)
    {
        float tmp = self.Get<float>(name);
        tmp -= val;
        self.Set(name, tmp);
    }

    public static object CallMethod(this HeroController self, string name, object[] parameters)
    {
        return self.CallMethod<object>(name, parameters);
    }
    public static object CallMethod(this HeroController self, string name)
    {
        return self.CallMethod<object>(name, null);
    }
    public static T CallMethod<T>(this HeroController self, string name, object[] parameters)
    {
        if (HCMethods.TryGetValue(name, out MethodInfo FoundmethodInfo))
        {
            return (T) FoundmethodInfo.Invoke(self, parameters);
        }
        else
        {
            //cache it
            MethodInfo methodInfo = self.GetType().GetMethod(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            HCMethods.Add(name,methodInfo);
            return (T) methodInfo.Invoke(self, parameters);
        }
    }

    public static T CallMethod<T>(this HeroController self, string name)
    {
        return self.CallMethod<T>(name, null);
    }

    public static bool RelativeYComparison(this float y_velocity, bool isUpsideDown, float comparison = 0.0f)
    {
        return !isUpsideDown && y_velocity > comparison || isUpsideDown && y_velocity < comparison * -1f;
    }
}