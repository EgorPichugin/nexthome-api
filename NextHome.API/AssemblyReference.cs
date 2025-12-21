using System.Reflection;

namespace NextHome.API;

public static class AssemblyReference
{
    public static Assembly CurrentAssembly => typeof(AssemblyReference).Assembly;
}