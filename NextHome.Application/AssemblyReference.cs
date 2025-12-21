using System.Reflection;

namespace NextHome.Application;

public static class AssemblyReference
{
    public static Assembly CurrentAssembly => typeof(AssemblyReference).Assembly;
}