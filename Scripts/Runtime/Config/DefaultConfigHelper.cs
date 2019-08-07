//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using GameFramework.Config;
using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 默认配置辅助器。
    /// </summary>
    public class DefaultConfigHelper : ConfigHelperBase
    {
        private static readonly string[] RowSplitSeparator = new string[] { "\r\n", "\r", "\n" };
        private static readonly string[] ColumnSplitSeparator = new string[] { "\t" };
        private const int ColumnCount = 4;

        private ResourceComponent m_ResourceComponent = null;
        private IConfigManager m_ConfigManager = null;

        /// <summary>
        /// 解析配置。
        /// </summary>
        /// <param name="text">要解析的配置文本。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>是否解析配置成功。</returns>
        public override bool ParseConfig(string text, object userData)
        {
            try
            {
                string[] rowTexts = text.Split(RowSplitSeparator, StringSplitOptions.None);
                for (int i = 0; i < rowTexts.Length; i++)
                {
                    if (rowTexts[i].Length <= 0 || rowTexts[i][0] == '#')
                    {
                        continue;
                    }

                    string[] splitLine = rowTexts[i].Split(ColumnSplitSeparator, StringSplitOptions.None);
                    if (splitLine.Length != ColumnCount)
                    {
                        Log.Warning("Can not parse config '{0}'.", text);
                        return false;
                    }

                    string configName = splitLine[1];
                    string configValue = splitLine[3];
                    if (!AddConfig(configName, configValue))
                    {
                        Log.Warning("Can not add raw string with config name '{0}' which may be invalid or duplicate.", configName);
                        return false;
                    }
                }

                return true;
            }
            catch (Exception exception)
            {
                Log.Warning("Can not parse config '{0}' with exception '{1}'.", text, exception.ToString());
                return false;
            }
        }

        /// <summary>
        /// 解析配置。
        /// </summary>
        /// <param name="bytes">要解析的配置二进制流。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>是否解析配置成功。</returns>
        public override bool ParseConfig(byte[] bytes, object userData)
        {
            using (MemoryStream memoryStream = new MemoryStream(bytes, false))
            {
                return ParseConfig(memoryStream, userData);
            }
        }

        /// <summary>
        /// 解析配置。
        /// </summary>
        /// <param name="stream">要解析的配置二进制流。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>是否解析配置成功。</returns>
        public override bool ParseConfig(Stream stream, object userData)
        {
            try
            {
                using (BinaryReader binaryReader = new BinaryReader(stream, Encoding.UTF8))
                {
                    while (binaryReader.BaseStream.Position < binaryReader.BaseStream.Length)
                    {
                        string configName = binaryReader.ReadString();
                        string configValue = binaryReader.ReadString();
                        if (!AddConfig(configName, configValue))
                        {
                            Log.Warning("Can not add raw string with config name '{0}' which may be invalid or duplicate.", configName);
                            return false;
                        }
                    }
                }

                return true;
            }
            catch (Exception exception)
            {
                Log.Warning("Can not parse config with exception '{0}'.", exception.ToString());
                return false;
            }
        }

        /// <summary>
        /// 释放配置资源。
        /// </summary>
        /// <param name="configAsset">要释放的配置资源。</param>
        public override void ReleaseConfigAsset(object configAsset)
        {
            m_ResourceComponent.UnloadAsset(configAsset);
        }

        /// <summary>
        /// 加载配置。
        /// </summary>
        /// <param name="configName">配置名称。</param>
        /// <param name="configAsset">配置资源。</param>
        /// <param name="loadType">配置加载方式。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>是否加载成功。</returns>
        protected override bool LoadConfig(string configName, object configAsset, LoadType loadType, object userData)
        {
            TextAsset textAsset = configAsset as TextAsset;
            if (textAsset == null)
            {
                Log.Warning("Config asset '{0}' is invalid.", configName);
                return false;
            }

            bool retVal = false;
            switch (loadType)
            {
                case LoadType.Text:
                    retVal = m_ConfigManager.ParseConfig(textAsset.text, userData);
                    break;

                case LoadType.Bytes:
                    retVal = m_ConfigManager.ParseConfig(textAsset.bytes, userData);
                    break;

                case LoadType.Stream:
                    using (MemoryStream stream = new MemoryStream(textAsset.bytes, false))
                    {
                        retVal = m_ConfigManager.ParseConfig(stream, userData);
                    }
                    break;

                default:
                    Log.Warning("Unknown load type.");
                    return false;
            }

            if (!retVal)
            {
                Log.Warning("Config asset '{0}' parse failure.", configName);
            }

            return retVal;
        }

        /// <summary>
        /// 增加指定配置项。
        /// </summary>
        /// <param name="configName">要增加配置项的名称。</param>
        /// <param name="configValue">要增加配置项的值。</param>
        /// <returns></returns>
        protected bool AddConfig(string configName, string configValue)
        {
            bool boolValue = false;
            bool.TryParse(configValue, out boolValue);

            int intValue = 0;
            int.TryParse(configValue, out intValue);

            float floatValue = 0f;
            float.TryParse(configValue, out floatValue);

            return AddConfig(configName, boolValue, intValue, floatValue, configValue);
        }

        /// <summary>
        /// 增加指定配置项。
        /// </summary>
        /// <param name="configName">要增加配置项的名称。</param>
        /// <param name="boolValue">配置项布尔值。</param>
        /// <param name="intValue">配置项整数值。</param>
        /// <param name="floatValue">配置项浮点数值。</param>
        /// <param name="stringValue">配置项字符串值。</param>
        /// <returns>是否增加配置项成功。</returns>
        protected bool AddConfig(string configName, bool boolValue, int intValue, float floatValue, string stringValue)
        {
            return m_ConfigManager.AddConfig(configName, boolValue, intValue, floatValue, stringValue);
        }

        private void Start()
        {
            m_ResourceComponent = GameEntry.GetComponent<ResourceComponent>();
            if (m_ResourceComponent == null)
            {
                Log.Fatal("Resource component is invalid.");
                return;
            }

            m_ConfigManager = GameFrameworkEntry.GetModule<IConfigManager>();
            if (m_ConfigManager == null)
            {
                Log.Fatal("Config manager is invalid.");
                return;
            }
        }
    }
}
