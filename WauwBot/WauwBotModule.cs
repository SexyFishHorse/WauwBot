namespace SexyFishHorse.WauwBot
{
    using Ninject.Extensions.Conventions;
    using Ninject.Modules;

    public class WauwBotModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind(x => x.FromThisAssembly().SelectAllClasses().BindDefaultInterface());
        }
    }
}