//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using System;
using System.IO;

/// <summary>
/// 对 BinaryReader 和 BinaryWriter 的扩展方法。
/// </summary>
public static class BinaryExtension
{
    private static readonly byte[] s_CachedBytes = new byte[byte.MaxValue];

    /// <summary>
    /// 从二进制流读取编码后的整数。
    /// </summary>
    /// <param name="binaryReader">要读取的二进制流。</param>
    /// <returns>读取的整数。</returns>
    public static int Read7BitEncodedInt(this BinaryReader binaryReader)
    {
        if (binaryReader == null)
        {
            throw new GameFrameworkException("Binary reader is invalid.");
        }

        int value = 0;
        int shift = 0;
        byte b;
        do
        {
            if (shift >= 35)
            {
                throw new GameFrameworkException("7 bit encoded int is invalid.");
            }

            b = binaryReader.ReadByte();
            value |= (b & 0x7f) << shift;
            shift += 7;
        } while ((b & 0x80) != 0);

        return value;
    }

    /// <summary>
    /// 向二进制流写入编码后的整数。
    /// </summary>
    /// <param name="binaryWriter">要写入的二进制流。</param>
    /// <param name="value">要写入的整数。</param>
    public static void Write7BitEncodedInt(this BinaryWriter binaryWriter, int value)
    {
        if (binaryWriter == null)
        {
            throw new GameFrameworkException("Binary writer is invalid.");
        }

        uint num = (uint)value;
        while (num >= 0x80)
        {
            binaryWriter.Write((byte)(num | 0x80));
            num >>= 7;
        }

        binaryWriter.Write((byte)num);
    }

    /// <summary>
    /// 从二进制流读取加密字符串。
    /// </summary>
    /// <param name="binaryReader">要读取的二进制流。</param>
    /// <param name="encryptBytes">密钥数组。</param>
    /// <returns>读取的字符串。</returns>
    public static string ReadEncryptedString(this BinaryReader binaryReader, byte[] encryptBytes)
    {
        if (binaryReader == null)
        {
            throw new GameFrameworkException("Binary reader is invalid.");
        }

        byte length = binaryReader.ReadByte();
        if (length <= 0)
        {
            return null;
        }

        for (byte i = 0; i < length; i++)
        {
            s_CachedBytes[i] = binaryReader.ReadByte();
        }

        Utility.Encryption.GetSelfXorBytes(s_CachedBytes, encryptBytes, length);
        string value = Utility.Converter.GetString(s_CachedBytes, 0, length);
        Array.Clear(s_CachedBytes, 0, length);
        return value;
    }

    /// <summary>
    /// 向二进制流写入加密字符串。
    /// </summary>
    /// <param name="binaryWriter">要写入的二进制流。</param>
    /// <param name="value">要写入的字符串。</param>
    /// <param name="encryptBytes">密钥数组。</param>
    public static void WriteEncryptedString(this BinaryWriter binaryWriter, string value, byte[] encryptBytes)
    {
        if (binaryWriter == null)
        {
            throw new GameFrameworkException("Binary writer is invalid.");
        }

        if (string.IsNullOrEmpty(value))
        {
            binaryWriter.Write((byte)0);
            return;
        }

        byte[] encryptedStringBytes = Utility.Converter.GetBytes(value);
        if (encryptedStringBytes.Length > byte.MaxValue)
        {
            throw new GameFrameworkException(Utility.Text.Format("String '{0}' is too long.", value));
        }

        Utility.Encryption.GetSelfXorBytes(encryptedStringBytes, encryptBytes);
        binaryWriter.Write((byte)encryptedStringBytes.Length);
        binaryWriter.Write(encryptedStringBytes);
    }
}
