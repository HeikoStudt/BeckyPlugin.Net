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

    private static Assembly ThisAssembly {
        get { return typeof(ModuleInitializer).Assembly; }
    }

    /// <summary>
    /// Initializes the module.
    /// </summary>
    public static void Initialize() {
        // Search for libraries and resources in the same folder of this DLL
        // ./ and ./AssemblyName/*; so it searches in plugins/* and plugins/PluginName/*
        AppDomain currentDomain = AppDomain.CurrentDomain;
        currentDomain.AssemblyResolve += LoadFromSameAndAssemblyNameFolder; 
    }

    /// <summary>
    ///   Tries to find the assembly in the folder where the current assembly resides or a subfolder named as the assemblyname.
    ///   <see cref="http://stackoverflow.com/questions/1373100/how-to-add-folder-to-assembly-search-path-at-runtime-in-net" />
    /// </summary>
    private static Assembly LoadFromSameAndAssemblyNameFolder(object sender, ResolveEventArgs args) {
        string searchedAssemblyName = new AssemblyName(args.Name).Name + ".dll";
        var path = Path.GetDirectoryName(ThisAssembly.Location);
        if (path == null) {
            return null;
        }

        // First as most specialised
        string assemblyNamePath = Path.Combine(path, ThisAssembly.GetName().Name, searchedAssemblyName);
        if (File.Exists(assemblyNamePath)) {
            return Assembly.LoadFrom(assemblyNamePath);
        }

        // some common library like NLog could be there, but having different versions....
        string sameAssemblyPath = Path.Combine(path, searchedAssemblyName);
        if (File.Exists(sameAssemblyPath)) {
            return Assembly.LoadFrom(sameAssemblyPath);
        }

        return null;
    }
}
