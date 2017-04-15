using AutoAddressBook;
using System;
using System.IO;
using System.Reflection;

/// <summary>
/// Used by the ModuleInit. All code inside the Initialize method is ran as soon as the assembly is loaded.
/// </summary>
public static class ModuleInitializer {

    /* ModuleInit.Fody modifies the IL (via ILDASM/ILASM) like the following:
.method public hidebysig specialname rtspecialname static 
      void  .cctor() cil managed
{
call void ModuleInitializer::Initialize()
ret
}
     */


    /// <summary>
    /// Initializes the module.
    /// </summary>
    public static void Initialize() {
        // Search for libraries and resources in the same folder of this DLL
        // ./ and ./AssemblyName/*; so it searches in plugins/* and plugins/PluginName/*
        AppDomain currentDomain = AppDomain.CurrentDomain;
        currentDomain.AssemblyResolve += LoadFromAssemblyNameSubFolder; // First
        currentDomain.AssemblyResolve += LoadFromSameFolder;
    }

    private static Assembly ThisAssembly {
        get { return typeof(ModuleInitializer).Assembly; }
    }

    /// <summary>
    ///   Tries to find the assembly in the folder where the current assembly resides.
    ///   <see cref="http://stackoverflow.com/questions/1373100/how-to-add-folder-to-assembly-search-path-at-runtime-in-net" />
    /// </summary>
    private static Assembly LoadFromSameFolder(object sender, ResolveEventArgs args) {
        string searchedAssemblyName = new AssemblyName(args.Name).Name + ".dll";
        var path = Path.GetDirectoryName(ThisAssembly.Location);
        if (path == null) {
            return null;
        }
        string assemblyPath = Path.Combine(path, searchedAssemblyName);
        if (!File.Exists(assemblyPath)) return null;
        return Assembly.LoadFrom(assemblyPath);
    }

    /// <summary>
    ///   Tries to find the assembly in the same named subfolder where the current assembly resides with assemblyname.
    ///   <see cref="http://stackoverflow.com/questions/1373100/how-to-add-folder-to-assembly-search-path-at-runtime-in-net" />
    /// </summary>
    private static Assembly LoadFromAssemblyNameSubFolder(object sender, ResolveEventArgs args) {
        string searchedAssemblyName = new AssemblyName(args.Name).Name + ".dll";
        var path = Path.GetDirectoryName(ThisAssembly.Location);
        if (path == null) {
            return null;
        }
        string assemblyPath = Path.Combine(path, ThisAssembly.GetName().Name, searchedAssemblyName);
        if (!File.Exists(assemblyPath)) return null;
        return Assembly.LoadFrom(assemblyPath);
    }
}
