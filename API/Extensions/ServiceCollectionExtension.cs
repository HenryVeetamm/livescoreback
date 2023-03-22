namespace API.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddDerived(this IServiceCollection collection, Type interfaceType, Type implementationType, ServiceLifetime lifetime)
    {
        collection.Add(new ServiceDescriptor(interfaceType, implementationType, lifetime));

        var derivedImplementations = implementationType.Assembly.GetExportedTypes()
            .Where(s => s.BaseType != typeof(object) && s.BaseType != null && !s.IsAbstract &&
                        (s.BaseType.IsGenericType ? s.BaseType.GetGenericTypeDefinition() : s.BaseType) == implementationType);

        var derivedInterfaces = interfaceType.Assembly.GetExportedTypes()
            .Where(t => t.GetInterfaces()
                .Any(i => (i.IsGenericType ? i.GetGenericTypeDefinition() : i) == interfaceType) && t.IsInterface)
            .ToArray();

        foreach (var derivedImplementationsType in derivedImplementations)
        {
            collection.Add(new ServiceDescriptor(
                derivedInterfaces.First(i =>
                    i.IsAssignableFrom(derivedImplementationsType) || IsAssignableToGenericType(derivedImplementationsType, i)),
                derivedImplementationsType, lifetime));
        }
    }

    private static bool IsAssignableToGenericType(Type givenType, Type genericType)
    {
        var interfaceTypes = givenType.GetInterfaces();
        if (interfaceTypes.Any(it => it.IsGenericType && it.GetGenericTypeDefinition() == genericType)) return true;
        if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType) return true;
        var baseType = givenType.BaseType;

        return baseType != null && IsAssignableToGenericType(baseType, genericType);
    }
}