//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
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
    /// 默认全局配置辅助器。
    /// </summary>
    public class DefaultConfigHelper : ConfigHelperBase
    {
        private static readonly string[] RowSplitSeparator = new string[] { "\r\n", "\r", "\n" };
        private static readonly string[] ColumnSplitSeparator = new string[] { "\t" };
        private static readonly string BytesAssetExtension = ".bytes";
        private const int ColumnCount = 4;

        private ResourceComponent m_ResourceComponent = null;
        private IConfigManager m_ConfigManager = null;

        /// <summary>
        /// 解析全局配置。
        /// </summary>
        /// <param name="configString">要解析的全局配置字符串。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>是否解析全局配置成功。</returns>
        public override bool ParseConfig(string configString, object userData)
        {
            try
            {
                string[] configRowTexts = configString.Split(RowSplitSeparator, StringSplitOptions.None);
                for (int i = 0; i < configRowTexts.Length; i++)
                {
                    if (configRowTexts[i].Length <= 0 || configRowTexts[i][0] == '#')
                    {
                        continue;
                    }

                    string[] splitLine = configRowTexts[i].Split(ColumnSplitSeparator, StringSplitOptions.None);
                    if (splitLine.Length != ColumnCount)
                    {
                        Log.Warning("Can not parse config '{0}'.", configString);
                        return false;
                    }

                    string configName = splitLine[1];
                    string configValue = splitLine[3];
                    if (!AddConfig(configName, configValue))
                    {
                        Log.Warning("Can not add config with config name '{0}' which may be invalid or duplicate.", configName);
                        return false;
                    }
                }

                return true;
            }
            catch (Exception exception)
            {
                Log.Warning("Can not parse config string with exception '{0}'.", exception.ToString());
                return false;
            }
        }

        /// <summary>
        /// 解析全局配置。
        /// </summary>
        /// <param name="configBytes">要解析的全局配置二进制数据。</param>
        /// <param name="startIndex">全局配置二进制数据的起始位置。</param>
        /// <param name="length">全局配置二进制数据的长度。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>是否解析全局配置成功。</returns>
        public override bool ParseConfig(byte[] configBytes, int startIndex, int length, object userData)
        {
            try
            {
                using (MemoryStream memoryStream = new MemoryStream(configBytes, false))
                {
                    using (BinaryReader binaryReader = new BinaryReader(memoryStream, Encoding.UTF8))
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
                }

                return true;
            }
            catch (Exception exception)
            {
                Log.Warning("Can not parse config bytes with exception '{0}'.", exception.ToString());
                return false;
            }
        }

        /// <summary>
        /// 释放全局配置资源。
        /// </summary>
        /// <param name="configAsset">要释放的全局配置资源。</param>
        public override void ReleaseConfigAsset(object configAsset)
        {
            m_ResourceComponent.UnloadAsset(configAsset);
        }

        /// <summary>
        /// 加载全局配置。
        /// </summary>
        /// <param name="configName">全局配置名称。</param>
        /// <param name="configAssetName">全局配置资源名称。</param>
        /// <param name="configObject">全局配置对象。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>是否加载成功。</returns>
        protected override bool LoadConfig(string configName, string configAssetName, object configObject, object userData)
        {
            TextAsset configTextAsset = configObject as TextAsset;
            if (configTextAsset != null)
            {
                if (configAssetName.EndsWith(BytesAssetExtension, StringComparison.Ordinal))
                {
                    return m_ConfigManager.ParseConfig(configTextAsset.bytes, userData);
                }
                else
                {
                    return m_ConfigManager.ParseConfig(configTextAsset.text, userData);
                }
            }

            Log.Warning("Config asset '{0}' is invalid.", configAssetName);
            return false;
        }

        /// <summary>
        /// 加载全局配置。
        /// </summary>
        /// <param name="configName">全局配置名称。</param>
        /// <param name="configAssetName">全局配置资源名称。</param>
        /// <param name="configBytes">全局配置二进制数据。</param>
        /// <param name="startIndex">全局配置二进制数据的起始位置。</param>
        /// <param name="length">全局配置二进制数据的长度。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>是否加载成功。</returns>
        protected override bool LoadConfig(string configName, string configAssetName, byte[] configBytes, int startIndex, int length, object userData)
        {
            if (configAssetName.EndsWith(BytesAssetExtension, StringComparison.Ordinal))
            {
                return m_ConfigManager.ParseConfig(configBytes, startIndex, length, userData);
            }
            else
            {
                return m_ConfigManager.ParseConfig(Utility.Converter.GetString(configBytes, startIndex, length), userData);
            }
        }

        /// <summary>
        /// 增加指定全局配置项。
        /// </summary>
        /// <param name="configName">要增加全局配置项的名称。</param>
        /// <param name="configValue">要增加全局配置项的值。</param>
        /// <returns>是否增加全局配置项成功。</returns>
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
        /// 增加指定全局配置项。
        /// </summary>
        /// <param name="configName">要增加全局配置项的名称。</param>
        /// <param name="boolValue">全局配置项布尔值。</param>
        /// <param name="intValue">全局配置项整数值。</param>
        /// <param name="floatValue">全局配置项浮点数值。</param>
        /// <param name="stringValue">全局配置项字符串值。</param>
        /// <returns>是否增加全局配置项成功。</returns>
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
