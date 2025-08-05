using AutoMapper;
using System.Reflection;

namespace Notes.Application.Common.Mappings
{
    public class AssemblyMappingProfile : Profile
    {
        public AssemblyMappingProfile(Assembly assembly)
        {
            ApplyMappingsFromAssembly(assembly);
        }

        private void ApplyMappingsFromAssembly(Assembly assembly)
        {
            var types = assembly.GetExportedTypes()//класс ищет все типы которые реализуют интерфейс imapwith<>
                .Where(type => type.GetInterfaces()
                    .Any(i => i.IsGenericType &&
                    i.GetGenericTypeDefinition() == typeof(IMapWith<>)))
                .ToList();

            foreach (var type in types)
            {
                var instance = Activator.CreateInstance(type);
                var methodInfo = type.GetMethod("Mapping");//класс вызывает метод Mapping из типа или из интерфейса если в типе отсутствует
                methodInfo?.Invoke(instance, [this]);
            }
        }
    }
}
