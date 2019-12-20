using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;

namespace DemoDapper
{
    /// <summary>
    /// Emit IL反建立C#代码
    /// </summary>
    public static class DemoExtensionEmit_5
    {
        public static IEnumerable<T> Query5<T>(this IDbConnection cnn, string sql) where T : new()
        {
            using (var command = cnn.CreateCommand())
            {
                command.CommandText = sql;
                using (var reader = command.ExecuteReader())
                {
                    var func = GetTypeDeserializerImpl(typeof(T), reader);

                    while (reader.Read())
                    {
                        var result = func(reader as DbDataReader);
                        yield return result is T ? (T)result : default(T);
                    }
                }

            }
        }

        public static Func<DbDataReader, object> GetTypeDeserializerImpl(Type type, IDataReader reader, int startBound = 0, int length = -1, bool returnNullIfFirstMissing = false)
        {
            var returnType = type.IsValueType ? typeof(object) : type;

            var dm = new DynamicMethod("Deserialize" + Guid.NewGuid().ToString(), returnType, new[] { typeof(IDataReader) }, type, true);
            var il = dm.GetILGenerator();

            //C# : User user = new User();
            //IL : 
            //IL_0001:  newobj      
            //IL_0006:  stloc.0         
            var constructor = returnType.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)[0]; //这边简化成只会有预设constructor
            il.Emit(OpCodes.Newobj, constructor);
            var returnValueLocal = il.DeclareLocal(type);
            il.Emit(OpCodes.Stloc, returnValueLocal); //User user = new User();

            // C# : 
            //object value = default(object);
            // IL :
            //IL_0007: ldnull
            //IL_0008:  stloc.1     // value  
            var valueLoacl = il.DeclareLocal(typeof(object));
            il.Emit(OpCodes.Ldnull);
            il.Emit(OpCodes.Stloc, valueLoacl);


            int index = startBound;
            var getItem = typeof(IDataRecord).GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    .Where(p => p.GetIndexParameters().Length > 0 && p.GetIndexParameters()[0].ParameterType == typeof(int))
                    .Select(p => p.GetGetMethod()).First();

            foreach (var p in type.GetProperties())
            {
                //C# : value = P_0[0];
                //IL:
                //IL_0009:  ldarg.0      
                //IL_000A: ldc.i4.0
                //IL_000B: callvirt System.Data.IDataRecord.get_Item
                //IL_0010:  stloc.1     // value        
                il.Emit(OpCodes.Ldarg_0); //取得reader参数
                EmitInt32(il, index);
                il.Emit(OpCodes.Callvirt, getItem);
                il.Emit(OpCodes.Stloc, valueLoacl);


                //C#: if (!(value is DBNull)) user.Name = (string)value;
                //IL:
                // IL_0011:  ldloc.1     // value
                // IL_0012:  isinst      System.DBNull
                // IL_0017:  ldnull      
                // IL_0018:  cgt.un      
                // IL_001A:  ldc.i4.0   
                // IL_001B:  ceq         
                // IL_001D:  stloc.2    
                // IL_001E:  ldloc.2     
                // IL_001F:  brfalse.s   IL_002E
                // IL_0021:  ldloc.0     // user
                // IL_0022:  ldloc.1     // value
                // IL_0023:  castclass   System.String
                // IL_0028:  callvirt    UserQuery+User.set_Name      
                il.Emit(OpCodes.Ldloc, valueLoacl);
                il.Emit(OpCodes.Isinst, typeof(System.DBNull));
                il.Emit(OpCodes.Ldnull);

                var tmpLoacl = il.DeclareLocal(typeof(int));
                il.Emit(OpCodes.Cgt_Un);
                il.Emit(OpCodes.Ldc_I4_0);
                il.Emit(OpCodes.Ceq);

                il.Emit(OpCodes.Stloc, tmpLoacl);
                il.Emit(OpCodes.Ldloc, tmpLoacl);


                var labelFalse = il.DefineLabel();
                il.Emit(OpCodes.Brfalse_S, labelFalse);
                il.Emit(OpCodes.Ldloc, returnValueLocal);
                il.Emit(OpCodes.Ldloc, valueLoacl);
                if (p.PropertyType.IsValueType)
                    il.Emit(OpCodes.Unbox_Any, p.PropertyType);
                else
                    il.Emit(OpCodes.Castclass, p.PropertyType);
                il.Emit(OpCodes.Callvirt, p.SetMethod);

                il.MarkLabel(labelFalse);

                index++;
            }

            // IL_0053:  ldloc.0     // user
            // IL_0054:  stloc.s     04  //不需要
            // IL_0056:  br.s        IL_0058
            // IL_0058:  ldloc.s     04  //不需要
            // IL_005A:  ret         
            il.Emit(OpCodes.Ldloc, returnValueLocal);
            il.Emit(OpCodes.Ret);

            var funcType = System.Linq.Expressions.Expression.GetFuncType(typeof(IDataReader), returnType);
            return (Func<IDataReader, object>)dm.CreateDelegate(funcType);
        }

        private static void EmitInt32(ILGenerator il, int value)
        {
            switch (value)
            {
                case -1: il.Emit(OpCodes.Ldc_I4_M1); break;
                case 0: il.Emit(OpCodes.Ldc_I4_0); break;
                case 1: il.Emit(OpCodes.Ldc_I4_1); break;
                case 2: il.Emit(OpCodes.Ldc_I4_2); break;
                case 3: il.Emit(OpCodes.Ldc_I4_3); break;
                case 4: il.Emit(OpCodes.Ldc_I4_4); break;
                case 5: il.Emit(OpCodes.Ldc_I4_5); break;
                case 6: il.Emit(OpCodes.Ldc_I4_6); break;
                case 7: il.Emit(OpCodes.Ldc_I4_7); break;
                case 8: il.Emit(OpCodes.Ldc_I4_8); break;
                default:
                    if (value >= -128 && value <= 127)
                    {
                        il.Emit(OpCodes.Ldc_I4_S, (sbyte)value);
                    }
                    else
                    {
                        il.Emit(OpCodes.Ldc_I4, value);
                    }
                    break;
            }
        }
    }
}
