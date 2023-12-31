﻿//namespace SharpFileServiceProg.Operations.Yaml.Descriptor
//{
//    using System;

//    using YamlDotNet.Core;
//    using YamlDotNet.Serialization;

//    public class BetterObjectDescriptor : IObjectDescriptor
//    {
//        public BetterObjectDescriptor(object value, Type type, Type staticType)
//            : this(value, type, staticType, ScalarStyle.Any)
//        {
//        }

//        public BetterObjectDescriptor(object value, Type type, Type staticType, ScalarStyle scalarStyle)
//        {
//            Value = value;
//            Type = type;
//            StaticType = staticType;
//            if (scalarStyle == ScalarStyle.Any)
//            {
//                var s = value as string;
//                if (s != null)
//                {
//                    if (Regexes.BooleanLike.IsMatch(s))
//                    {
//                        scalarStyle = ScalarStyle.DoubleQuoted;
//                    }
//                    else if (Regexes.IntegerLike.IsMatch(s))
//                    {
//                        scalarStyle = ScalarStyle.DoubleQuoted;
//                    }
//                    else if (Regexes.DoubleLike.IsMatch(s))
//                    {
//                        scalarStyle = ScalarStyle.DoubleQuoted;
//                    }
//                    else if (s.StartsWith("'"))
//                    {
//                        scalarStyle = ScalarStyle.DoubleQuoted;
//                    }
//                    else if (s.StartsWith("\""))
//                    {
//                        scalarStyle = ScalarStyle.DoubleQuoted;
//                    }
//                }
//            }
//            ScalarStyle = scalarStyle;
//        }

//        public ScalarStyle ScalarStyle { get; }

//        public Type StaticType { get; }

//        public Type Type { get; }

//        public object Value { get; }
//    }
//}