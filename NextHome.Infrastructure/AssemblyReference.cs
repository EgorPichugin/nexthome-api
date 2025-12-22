using System.Reflection;

namespace NextHome.Infrastructure;

public static class AssemblyReference
{
    public static Assembly CurrentAssembly => typeof(AssemblyReference).Assembly;
}