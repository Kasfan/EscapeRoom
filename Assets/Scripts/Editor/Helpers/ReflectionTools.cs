using System;
using System.Linq;

namespace EscapeRoom.EditorScripts.Helpers
{
    /// <summary>
    /// Helper methods that use reflection
    /// </summary>
    public abstract class ReflectionTools
    {
        /// <summary>
        /// Returns all the types that can be assigned as provided type
        /// Ignores abstract classes and generic types.
        /// </summary>
        /// <param name="type">target interface</param>
        /// <param name="abstractTypes">include abstract types</param>
        /// <param name="genericTypes">include generic types</param>
        /// <returns>types that implement the interface.</returns>
        public static Type[] GetTypesImplementingInterface(Type type,
            bool abstractTypes = false,
            bool genericTypes = false)
        {
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p)
                            && p.IsClass
                            && (genericTypes || !p.IsGenericType)
                            && (abstractTypes || !p.IsAbstract)).ToArray();
            return types;
        }
    }
}