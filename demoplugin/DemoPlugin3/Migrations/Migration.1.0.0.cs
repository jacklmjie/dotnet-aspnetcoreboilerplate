using DynamicPlugins.Infrastructure;
using DynamicPlugins.Data;
using DynamicPlugins.ViewModels;

namespace DemoPlugin3.Migrations
{
    public class Migration_1_0_0 : BaseMigration
    {
        private static PluginVersion _version = new PluginVersion("1.0.0");
        public Migration_1_0_0(MyContext myContext) : base(myContext, _version)
        {

        }

        public override string UpScripts
        {
            get
            {
                return @"CREATE TABLE [dbo].[Test3](
                        TestId[uniqueidentifier] NOT NULL,
                    );";
            }
        }

        public override string DownScripts
        {
            get
            {
                return @"DROP TABLE [dbo].[Test3]";
            }
        }
    }
}
