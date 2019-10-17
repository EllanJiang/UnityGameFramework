//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace UnityGameFramework.Editor.DataTableTools
{
    public sealed partial class DataTableProcessor
    {
        private static class DataProcessorUtility
        {
            private static readonly IDictionary<string, DataProcessor> s_DataProcessors = new SortedDictionary<string, DataProcessor>();

            static DataProcessorUtility()
            {
                System.Type dataProcessorBaseType = typeof(DataProcessor);
                Assembly assembly = Assembly.GetExecutingAssembly();
                System.Type[] types = assembly.GetTypes();
                for (int i = 0; i < types.Length; i++)
                {
                    if (!types[i].IsClass || types[i].IsAbstract)
                    {
                        continue;
                    }

                    if (dataProcessorBaseType.IsAssignableFrom(types[i]))
                    {
                        DataProcessor dataProcessor = (DataProcessor)Activator.CreateInstance(types[i]);
                        foreach (string typeString in dataProcessor.GetTypeStrings())
                        {
                            s_DataProcessors.Add(typeString.ToLower(), dataProcessor);
                        }
                    }
                }
            }

            public static DataProcessor GetDataProcessor(string type)
            {
                if (type == null)
                {
                    type = string.Empty;
                }

                DataProcessor dataProcessor = null;
                if (s_DataProcessors.TryGetValue(type.ToLower(), out dataProcessor))
                {
                    return dataProcessor;
                }

                throw new GameFrameworkException(Utility.Text.Format("Not supported data processor type '{0}'.", type));
            }
        }
    }
}
