using DSharpPlus.CommandsNext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace PizzaBotGG.App.DiscordSlashCommandModule.Utilities
{
	public static class ModuleTypeLoader
	{
        public static List<Type> GetModuleTypes()
		{
            var entryAssembly = Assembly.GetEntryAssembly();

            var moduleTypes = entryAssembly.ExportedTypes.Where(exportedType =>
            {
                var exportedTypeInfo = exportedType.GetTypeInfo();
                return IsModuleCandidateType(exportedTypeInfo) && !exportedTypeInfo.IsNested;
            });

            return moduleTypes.ToList();
        }

        private static bool IsModuleCandidateType(TypeInfo typeInfo)
        {
            // check if compiler-generated
            if (typeInfo.GetCustomAttribute<CompilerGeneratedAttribute>(false) != null)
                return false;

            // check if derives from the required base class
            var tmodule = typeof(SlashModule);
            var typeInfoModule = tmodule.GetTypeInfo();
            if (!typeInfoModule.IsAssignableFrom(typeInfo))
                return false;

            // check if anonymous
            if (typeInfo.IsGenericType && typeInfo.Name.Contains("AnonymousType") && (typeInfo.Name.StartsWith("<>") || typeInfo.Name.StartsWith("VB$")) && (typeInfo.Attributes & TypeAttributes.NotPublic) == TypeAttributes.NotPublic)
                return false;

            // check if abstract, static, or not a class
            if (!typeInfo.IsClass || typeInfo.IsAbstract)
                return false;

            // check if delegate type
            var typeInfoDelegate = typeof(Delegate).GetTypeInfo();
            if (typeInfoDelegate.IsAssignableFrom(typeInfo))
                return false;

            // qualifies if any method or type qualifies
            return typeInfo.DeclaredMethods.Any(methodInfo => IsCommandCandidate(methodInfo)) || typeInfo.DeclaredNestedTypes.Any(nestedTypeInfo => IsModuleCandidateType(nestedTypeInfo));
        }

        private static bool IsCommandCandidate(this MethodInfo method)
        {
            // check if exists
            if (method == null)
                return false;

            // check if static, non-public, abstract, a constructor, or a special name
            if (method.IsStatic || method.IsAbstract || method.IsConstructor || method.IsSpecialName)
                return false;

            // qualifies
            return true;
        }
    }
}
