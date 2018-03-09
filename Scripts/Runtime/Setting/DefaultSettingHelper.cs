//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using System;
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 默认配置辅助器。
    /// </summary>
    public class DefaultSettingHelper : SettingHelperBase
    {
        /// <summary>
        /// 加载配置。
        /// </summary>
        /// <returns>是否加载配置成功。</returns>
        public override bool Load()
        {
            return true;
        }

        /// <summary>
        /// 保存配置。
        /// </summary>
        /// <returns>是否保存配置成功。</returns>
        public override bool Save()
        {
            PlayerPrefs.Save();
            return true;
        }

        /// <summary>
        /// 检查是否存在指定配置项。
        /// </summary>
        /// <param name="settingName">要检查配置项的名称。</param>
        /// <returns>指定的配置项是否存在。</returns>
        public override bool HasSetting(string settingName)
        {
            return PlayerPrefs.HasKey(settingName);
        }

        /// <summary>
        /// 移除指定配置项。
        /// </summary>
        /// <param name="settingName">要移除配置项的名称。</param>
        public override void RemoveSetting(string settingName)
        {
            PlayerPrefs.DeleteKey(settingName);
        }

        /// <summary>
        /// 清空所有配置项。
        /// </summary>
        public override void RemoveAllSettings()
        {
            PlayerPrefs.DeleteAll();
        }

        /// <summary>
        /// 从指定配置项中读取布尔值。
        /// </summary>
        /// <param name="settingName">要获取配置项的名称。</param>
        /// <returns>读取的布尔值。</returns>
        public override bool GetBool(string settingName)
        {
            return PlayerPrefs.GetInt(settingName) != 0;
        }

        /// <summary>
        /// 从指定配置项中读取布尔值。
        /// </summary>
        /// <param name="settingName">要获取配置项的名称。</param>
        /// <param name="defaultValue">当指定的配置项不存在时，返回此默认值。</param>
        /// <returns>读取的布尔值。</returns>
        public override bool GetBool(string settingName, bool defaultValue)
        {
            return PlayerPrefs.GetInt(settingName, defaultValue ? 1 : 0) != 0;
        }

        /// <summary>
        /// 向指定配置项写入布尔值。
        /// </summary>
        /// <param name="settingName">要写入配置项的名称。</param>
        /// <param name="value">要写入的布尔值。</param>
        public override void SetBool(string settingName, bool value)
        {
            PlayerPrefs.SetInt(settingName, value ? 1 : 0);
        }

        /// <summary>
        /// 从指定配置项中读取整数值。
        /// </summary>
        /// <param name="settingName">要获取配置项的名称。</param>
        /// <returns>读取的整数值。</returns>
        public override int GetInt(string settingName)
        {
            return PlayerPrefs.GetInt(settingName);
        }

        /// <summary>
        /// 从指定配置项中读取整数值。
        /// </summary>
        /// <param name="settingName">要获取配置项的名称。</param>
        /// <param name="defaultValue">当指定的配置项不存在时，返回此默认值。</param>
        /// <returns>读取的整数值。</returns>
        public override int GetInt(string settingName, int defaultValue)
        {
            return PlayerPrefs.GetInt(settingName, defaultValue);
        }

        /// <summary>
        /// 向指定配置项写入整数值。
        /// </summary>
        /// <param name="settingName">要写入配置项的名称。</param>
        /// <param name="value">要写入的整数值。</param>
        public override void SetInt(string settingName, int value)
        {
            PlayerPrefs.SetInt(settingName, value);
        }

        /// <summary>
        /// 从指定配置项中读取浮点数值。
        /// </summary>
        /// <param name="settingName">要获取配置项的名称。</param>
        /// <returns>读取的浮点数值。</returns>
        public override float GetFloat(string settingName)
        {
            return PlayerPrefs.GetFloat(settingName);
        }

        /// <summary>
        /// 从指定配置项中读取浮点数值。
        /// </summary>
        /// <param name="settingName">要获取配置项的名称。</param>
        /// <param name="defaultValue">当指定的配置项不存在时，返回此默认值。</param>
        /// <returns>读取的浮点数值。</returns>
        public override float GetFloat(string settingName, float defaultValue)
        {
            return PlayerPrefs.GetFloat(settingName, defaultValue);
        }

        /// <summary>
        /// 向指定配置项写入浮点数值。
        /// </summary>
        /// <param name="settingName">要写入配置项的名称。</param>
        /// <param name="value">要写入的浮点数值。</param>
        public override void SetFloat(string settingName, float value)
        {
            PlayerPrefs.SetFloat(settingName, value);
        }

        /// <summary>
        /// 从指定配置项中读取字符串值。
        /// </summary>
        /// <param name="settingName">要获取配置项的名称。</param>
        /// <returns>读取的字符串值。</returns>
        public override string GetString(string settingName)
        {
            return PlayerPrefs.GetString(settingName);
        }

        /// <summary>
        /// 从指定配置项中读取字符串值。
        /// </summary>
        /// <param name="settingName">要获取配置项的名称。</param>
        /// <param name="defaultValue">当指定的配置项不存在时，返回此默认值。</param>
        /// <returns>读取的字符串值。</returns>
        public override string GetString(string settingName, string defaultValue)
        {
            return PlayerPrefs.GetString(settingName, defaultValue);
        }

        /// <summary>
        /// 向指定配置项写入字符串值。
        /// </summary>
        /// <param name="settingName">要写入配置项的名称。</param>
        /// <param name="value">要写入的字符串值。</param>
        public override void SetString(string settingName, string value)
        {
            PlayerPrefs.SetString(settingName, value);
        }

        /// <summary>
        /// 从指定配置项中读取对象。
        /// </summary>
        /// <typeparam name="T">要读取对象的类型。</typeparam>
        /// <param name="settingName">要获取配置项的名称。</param>
        /// <returns>读取的对象。</returns>
        public override T GetObject<T>(string settingName)
        {
            return Utility.Json.ToObject<T>(PlayerPrefs.GetString(settingName));
        }

        /// <summary>
        /// 从指定配置项中读取对象。
        /// </summary>
        /// <param name="objectType">要读取对象的类型。</param>
        /// <param name="settingName">要获取配置项的名称。</param>
        /// <returns></returns>
        public override object GetObject(Type objectType, string settingName)
        {
            return Utility.Json.ToObject(objectType, PlayerPrefs.GetString(settingName));
        }

        /// <summary>
        /// 从指定配置项中读取对象。
        /// </summary>
        /// <typeparam name="T">要读取对象的类型。</typeparam>
        /// <param name="settingName">要获取配置项的名称。</param>
        /// <param name="defaultObj">当指定的配置项不存在时，返回此默认对象。</param>
        /// <returns>读取的对象。</returns>
        public override T GetObject<T>(string settingName, T defaultObj)
        {
            string json = PlayerPrefs.GetString(settingName, null);
            if (json == null)
            {
                return defaultObj;
            }

            return Utility.Json.ToObject<T>(json);
        }

        /// <summary>
        /// 从指定配置项中读取对象。
        /// </summary>
        /// <param name="objectType">要读取对象的类型。</param>
        /// <param name="settingName">要获取配置项的名称。</param>
        /// <param name="defaultObj">当指定的配置项不存在时，返回此默认对象。</param>
        /// <returns></returns>
        public override object GetObject(Type objectType, string settingName, object defaultObj)
        {
            string json = PlayerPrefs.GetString(settingName, null);
            if (json == null)
            {
                return defaultObj;
            }

            return Utility.Json.ToObject(objectType, json);
        }

        /// <summary>
        /// 向指定配置项写入对象。
        /// </summary>
        /// <typeparam name="T">要写入对象的类型。</typeparam>
        /// <param name="settingName">要写入配置项的名称。</param>
        /// <param name="obj">要写入的对象。</param>
        public override void SetObject<T>(string settingName, T obj)
        {
            PlayerPrefs.SetString(settingName, Utility.Json.ToJson(obj));
        }

        /// <summary>
        /// 向指定配置项写入对象。
        /// </summary>
        /// <param name="settingName">要写入配置项的名称。</param>
        /// <param name="obj">要写入的对象。</param>
        public override void SetObject(string settingName, object obj)
        {
            PlayerPrefs.SetString(settingName, Utility.Json.ToJson(obj));
        }
    }
}
