using System.Reflection;

namespace OOD_project1;

public interface utils
{
   
    public static List<Type> GetDerivedTypes<T>()
    {
        Type baseType = typeof(T);

        return Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => 
                    t.IsClass && !t.IsAbstract && baseType.IsAssignableFrom(t) 
            )
            .ToList();
    }

    public static string FormatItem(Item item)
    {
        return item != null ? item.GetName().PadRight(10) : "None".PadRight(10);
    }

}