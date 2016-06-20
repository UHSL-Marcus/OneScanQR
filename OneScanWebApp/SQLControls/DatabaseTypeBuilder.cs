using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace SQLControls
{
    class DatabaseTypeBuilder
    {
        public class Field
        {
            public string Name;
            public Type Type;

            public Field(string name, Type type)
            {
                Name = name;
                Type = type;
            }
        }
        public static Type GetType(string name, Field[] fields)
        {
            TypeBuilder tb = GetTypeBuilder(name);
            ConstructorBuilder constructor = tb.DefineDefaultConstructor(MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName);

            tb.SetParent(typeof(DatabaseTableObject));

            foreach (Field field in fields)
                tb.DefineField(field.Name, field.Type, FieldAttributes.Public);

            Type objectType = tb.CreateType();
            return objectType;
        }

        private static TypeBuilder GetTypeBuilder(string name)
        {
            var typeSignature = name;
            var an = new AssemblyName(typeSignature);
            AssemblyBuilder assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(an, AssemblyBuilderAccess.RunAndSave);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("MainModule");
            TypeBuilder tb = moduleBuilder.DefineType(typeSignature
                                , TypeAttributes.Public |
                                TypeAttributes.Class |
                                TypeAttributes.AutoClass |
                                TypeAttributes.AnsiClass |
                                TypeAttributes.BeforeFieldInit |
                                TypeAttributes.AutoLayout
                                , null);
            return tb;
        } 
    }
}
