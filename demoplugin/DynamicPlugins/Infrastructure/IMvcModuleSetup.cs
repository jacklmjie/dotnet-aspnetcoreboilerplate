namespace DynamicPlugins.Infrastructure
{
    public interface IMvcModuleSetup
    {
        void DisableModule(string moduleName);

        void EnableModule(string moduleName);

        void DeleteModule(string moduleName);
    }
}
