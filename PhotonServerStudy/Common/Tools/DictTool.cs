
using System;
using System.Collections.Generic;
using System.Reflection;
using Google.Protobuf;

namespace Common.Tools
{
    public class DictTool
    {
        public static R GetValue<T, R>(Dictionary<T, R> dict, T key)
        {
            R value;
            bool isSuccess = dict.TryGetValue(key, out value);
            if (isSuccess)
            {
                return value;
            }
            else
            {
                return default(R);
            }
        }

        public static T GetProtoByDtoData<T>(Dictionary<byte, object> dict, ParameterCode key) where T : IMessage<T>
        {
            object value;
            bool isSuccess = dict.TryGetValue((byte)key, out value);
            if (isSuccess)
            {
                byte[] param = (byte[])value;
                Type dataType = typeof(T);
                PropertyInfo property = dataType.GetProperty("Parser", BindingFlags.Static | BindingFlags.Public);
                if (property != null && property.PropertyType == typeof(MessageParser<T>))
                {
                    MessageParser<T> Parser = (MessageParser<T>)dataType.InvokeMember("Parser", BindingFlags.GetProperty, null, null, null);
                    T proto = Parser.ParseFrom(param);
                    return proto;
                }

                return default(T);
            }
            else
            {
                return default(T);
            }
        }

        public static Dictionary<byte, object> GetDtoDataByProto<T>(IMessage<T> dtoObject,ParameterCode parameterCode) where T:IMessage<T>
        {
            Dictionary<byte,object> data = new Dictionary<byte, object>();
            if (dtoObject != null)
            {
                byte[] byteAtrr = dtoObject.ToByteArray();
                data.Add((byte)parameterCode, byteAtrr);
            }

            return data;
        }

    }
}
