using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System;

public class StructBytesInterop : MonoBehaviour
{
    /// <summary>
    /// 结构体转字节数组（按小端模式）
    /// </summary>
    /// <param name="obj">struct type</param>
    /// <returns></returns>
    public static byte[] StructureToByteArray(object obj)
    {
        int len = Marshal.SizeOf(obj);
        byte[] arr = new byte[len];
        IntPtr ptr = Marshal.AllocHGlobal(len);
        Marshal.StructureToPtr(obj, ptr, true);
        Marshal.Copy(ptr, arr, 0, len);
        Marshal.FreeHGlobal(ptr);
        return arr;
    }

    /// <summary>
    /// 结构体转字节数组（按小端模式）
    /// </summary>
    /// <param name="obj">struct type</param>
    /// <returns></returns>
    public static byte[] StructureToByteArraySmallEndian(object obj)
    {
        object thisBoxed = obj;   //copy ，将 struct 装箱
        Type test = thisBoxed.GetType();

        int offset = 0;
        int size = Marshal.SizeOf(thisBoxed);
        byte[] data = new byte[size];

        object fieldValue;
        TypeCode typeCode;
        byte[] temp;
        // 列举结构体的每个成员，并Reverse
        foreach (var field in test.GetFields())
        {
            fieldValue = field.GetValue(thisBoxed); // Get value

            typeCode = Type.GetTypeCode(fieldValue.GetType());  // get type

            switch (typeCode)
            {
                case TypeCode.Single: // float
                    {
                        temp = BitConverter.GetBytes((Single)fieldValue);
                       
                        Array.Copy(temp, 0, data, offset, sizeof(Single));
                        break;
                    }
                case TypeCode.Int32:
                    {
                        temp = BitConverter.GetBytes((Int32)fieldValue);
                     
                        Array.Copy(temp, 0, data, offset, sizeof(Int32));
                        break;
                    }
                case TypeCode.UInt32:
                    {
                        temp = BitConverter.GetBytes((UInt32)fieldValue);
                       
                        Array.Copy(temp, 0, data, offset, sizeof(UInt32));
                        break;
                    }
                case TypeCode.Int16:
                    {
                        temp = BitConverter.GetBytes((Int16)fieldValue);
                      
                        Array.Copy(temp, 0, data, offset, sizeof(Int16));
                        break;
                    }
                case TypeCode.UInt16:
                    {
                        temp = BitConverter.GetBytes((UInt16)fieldValue);
                     
                        Array.Copy(temp, 0, data, offset, sizeof(UInt16));
                        break;
                    }
                case TypeCode.Int64:
                    {
                        temp = BitConverter.GetBytes((Int64)fieldValue);
                       
                        Array.Copy(temp, 0, data, offset, sizeof(Int64));
                        break;
                    }
                case TypeCode.UInt64:
                    {
                        temp = BitConverter.GetBytes((UInt64)fieldValue);
                     
                        Array.Copy(temp, 0, data, offset, sizeof(UInt64));
                        break;
                    }
                case TypeCode.Double:
                    {
                        temp = BitConverter.GetBytes((Double)fieldValue);
                     
                        Array.Copy(temp, 0, data, offset, sizeof(Double));
                        break;
                    }
                case TypeCode.Byte:
                    {
                        data[offset] = (Byte)fieldValue;
                        break;
                    }
                case TypeCode.Boolean:
                    {
                        data[offset] = (Byte)((Boolean)fieldValue ? 1 : 0);
                        break;
                    }
                default:
                    {
                        // Debug.LogError("No conversion provided for this type : " + typeCode.ToString());
                        break;
                    }
            }; // switch
            if (typeCode == TypeCode.Object)
            {
                int length = ((byte[])fieldValue).Length;
                Array.Copy(((byte[])fieldValue), 0, data, offset, length);
                offset += length;
            }
            else
            {
                offset += Marshal.SizeOf(fieldValue);
            }
        } // foreach

        return data;
    } // Swap

    /// <summary>
    /// 结构体转字节数组（按大端模式）
    /// </summary>
    /// <param name="obj">struct type</param>
    /// <returns></returns>
    public static byte[] StructureToByteArrayBigEndian(object obj)
    {
        object thisBoxed = obj;   //copy ，将 struct 装箱
        Type test = thisBoxed.GetType();

        int offset = 0;
        int size = Marshal.SizeOf(thisBoxed);
        //Debug.LogWarning("byte.size = " + size);
        byte[] data = new byte[size];

        object fieldValue;
        TypeCode typeCode;
        byte[] temp;
        // 列举结构体的每个成员，并Reverse
        foreach (var field in test.GetFields())
        {
            fieldValue = field.GetValue(thisBoxed); // Get value

            typeCode = Type.GetTypeCode(fieldValue.GetType());  // get type

            switch (typeCode)
            {
                case TypeCode.Single: // float
                    {
                        temp = BitConverter.GetBytes((Single)fieldValue);
                        Array.Reverse(temp);
                        Array.Copy(temp, 0, data, offset, sizeof(Single));
                        break;
                    }
                case TypeCode.Int32:
                    {
                        temp = BitConverter.GetBytes((Int32)fieldValue);
                        Array.Reverse(temp);
                        Array.Copy(temp, 0, data, offset, sizeof(Int32));
                        break;
                    }
                case TypeCode.UInt32:
                    {
                        temp = BitConverter.GetBytes((UInt32)fieldValue);
                        Array.Reverse(temp);
                        Array.Copy(temp, 0, data, offset, sizeof(UInt32));
                        break;
                    }
                case TypeCode.Int16:
                    {
                        temp = BitConverter.GetBytes((Int16)fieldValue);
                        Array.Reverse(temp);
                        Array.Copy(temp, 0, data, offset, sizeof(Int16));
                        break;
                    }
                case TypeCode.UInt16:
                    {
                        temp = BitConverter.GetBytes((UInt16)fieldValue);
                        Array.Reverse(temp);
                        Array.Copy(temp, 0, data, offset, sizeof(UInt16));
                        break;
                    }
                case TypeCode.Int64:
                    {
                        temp = BitConverter.GetBytes((Int64)fieldValue);
                        Array.Reverse(temp);
                        Array.Copy(temp, 0, data, offset, sizeof(Int64));
                        break;
                    }
                case TypeCode.UInt64:
                    {
                        temp = BitConverter.GetBytes((UInt64)fieldValue);
                        Array.Reverse(temp);
                        Array.Copy(temp, 0, data, offset, sizeof(UInt64));
                        break;
                    }
                case TypeCode.Double:
                    {
                        temp = BitConverter.GetBytes((Double)fieldValue);
                        Array.Reverse(temp);
                        Array.Copy(temp, 0, data, offset, sizeof(Double));
                        break;
                    }
                case TypeCode.Byte:
                    {
                        data[offset] = (Byte)fieldValue;
                        break;
                    }
                case TypeCode.Boolean:
                    {
                        data[offset] = (Byte)((Boolean)fieldValue ? 1 : 0);
                        break;
                    }
                default:
                    {
                       // Debug.LogError("No conversion provided for this type : " + typeCode.ToString());
                        break;
                    }
            }; // switch
            if (typeCode == TypeCode.Object)
            {
                int length = ((byte[])fieldValue).Length;
                Array.Copy(((byte[])fieldValue), 0, data, offset, length);
                offset += length;
            }
            else
            {
                offset += Marshal.SizeOf(fieldValue);
            }
        } // foreach

        return data;
    } // Swap


    /// <summary>
    /// 字节数组转结构体(按小端模式)
    /// </summary>
    /// <param name="bytearray">字节数组</param>
    /// <param name="obj">目标结构体</param>
    /// <param name="startoffset">bytearray内的起始位置</param>
    public static void ByteArrayToStructure(byte[] bytearray, ref object obj, int startoffset)
    {
        int len = Marshal.SizeOf(obj);
        IntPtr i = Marshal.AllocHGlobal(len);
        // 从结构体指针构造结构体
        obj = Marshal.PtrToStructure(i, obj.GetType());
        try
        {
            // 将字节数组复制到结构体指针
            Marshal.Copy(bytearray, startoffset, i, len);
        }
        catch (Exception ex) { Console.WriteLine("ByteArrayToStructure FAIL: error " + ex.ToString()); }
        obj = Marshal.PtrToStructure(i, obj.GetType());
        Marshal.FreeHGlobal(i);  //释放内存，与 AllocHGlobal() 对应

    }

    /// <summary>
    /// 字节数组转结构体(按大端模式)
    /// </summary>
    /// <param name="bytearray">字节数组</param>
    /// <param name="obj">目标结构体</param>
    /// <param name="startoffset">bytearray内的起始位置</param>
    public static void ByteArrayToStructureEndian(byte[] bytearray, ref object obj, int startoffset)
    {
        int len = Marshal.SizeOf(obj);
        IntPtr i = Marshal.AllocHGlobal(len);
        byte[] temparray = (byte[])bytearray.Clone();
        // 从结构体指针构造结构体
        obj = Marshal.PtrToStructure(i, obj.GetType());
        // 做大端转换
        object thisBoxed = obj;
        Type test = thisBoxed.GetType();
        int reversestartoffset = startoffset;
        // 列举结构体的每个成员，并Reverse
        foreach (var field in test.GetFields())
        {
            object fieldValue = field.GetValue(thisBoxed); // Get value

            TypeCode typeCode = Type.GetTypeCode(fieldValue.GetType());  //Get Type
            if (typeCode != TypeCode.Object)  //如果为值类型
            {
                Array.Reverse(temparray, reversestartoffset, Marshal.SizeOf(fieldValue));
                reversestartoffset += Marshal.SizeOf(fieldValue);
            }
            else  //如果为引用类型
            {
                reversestartoffset += ((byte[])fieldValue).Length;
            }
        }
        try
        {
            //将字节数组复制到结构体指针
            Marshal.Copy(temparray, startoffset, i, len);
        }
        catch (Exception ex) { Console.WriteLine("ByteArrayToStructure FAIL: error " + ex.ToString()); }
        obj = Marshal.PtrToStructure(i, obj.GetType());
        Marshal.FreeHGlobal(i);  //释放内存
    }

}
