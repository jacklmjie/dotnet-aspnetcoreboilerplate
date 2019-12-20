using System;
using System.Reflection;
using System.Reflection.Emit;

namespace DemoDapper
{
    public class EmitShow
    {
        public void SyaHello()
        {
            Console.WriteLine("Hello World");
        }

        /// <summary>
        /// emit反编译查看生成的代码
        /// </summary>
        public void EmitSay()
        {
//#if NET461
//            //使用MethodBuilder查看别人已经写好的Emit IL
//            //1. 建立MethodBuilder
//            var type = typeof(void);
//            AppDomain ad = AppDomain.CurrentDomain;
//            AssemblyName am = new AssemblyName();
//            am.Name = "TestAsm";
//            AssemblyBuilder ab = ad.DefineDynamicAssembly(am, AssemblyBuilderAccess.Save);
//            ModuleBuilder mb = ab.DefineDynamicModule("Testmod", "TestAsm.exe");
//            TypeBuilder tb = mb.DefineType("TestType", TypeAttributes.Public);
//            MethodBuilder dm = tb.DefineMethod("TestMeThod", MethodAttributes.Public |
//            MethodAttributes.Static, type, new[] { typeof(string) });   

//            // 2. 填入IL代码
//            var il = ab.GetILGenerator();
//            il.Emit(OpCodes.Ldstr, "Hello World");
//            Type[] types = new Type[1]
//            {
//    typeof(string)
//            };
//            MethodInfo method = typeof(Console).GetMethod("WriteLine", types);
//            il.Emit(OpCodes.Call, method);
//            il.Emit(OpCodes.Ret);

//            ab.SetEntryPoint(dm);

//            // 3. 生成静态档案
//            tb.CreateType();
//            ab.Save("TestAsm.exe");
//#endif
        }

        /// <summary>
        /// 先写好C#代码后 > 反编译查看IL(或resharp) > 使用Emit建立动态方法
        /// </summary>
        public void EmitSayHello()
        {
            // 1. 建立 void 方法()
            DynamicMethod methodbuilder = new DynamicMethod("Deserialize" + Guid.NewGuid().ToString(), typeof(void), null);

            // 2. 建立方法Body内容,借由Emit
            var il = methodbuilder.GetILGenerator();
            il.Emit(OpCodes.Ldstr, "Hello World");
            Type[] types = new Type[1]
            {
    typeof(string)
            };
            MethodInfo method = typeof(Console).GetMethod("WriteLine", types);
            il.Emit(OpCodes.Call, method);
            il.Emit(OpCodes.Ret);

            // 3. 转换指定类型的Func or Action
            var action = (Action)methodbuilder.CreateDelegate(typeof(Action));

            action();
        }
    }
}
