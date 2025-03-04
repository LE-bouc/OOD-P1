using System.Reflection;

namespace OOD_project1;

public interface utils
{
    public static List<Type> GetDerivedTypes<T>()
    {
        return Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(T)))
            .ToList();
    }
    
    public static string FormatItem(Item item)
    {
        return item != null ? item.GetName().PadRight(10) : "None".PadRight(10);
    }

}