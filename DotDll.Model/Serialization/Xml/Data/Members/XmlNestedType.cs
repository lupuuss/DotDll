﻿using System.Runtime.Serialization;
using DotDll.Model.Serialization.Xml.Data.Base;

namespace DotDll.Model.Serialization.Xml.Data.Members
{
    [DataContract(Namespace = "")]
    public class XmlNestedType : XmlMember
    {
        [DataMember]
        public XmlType Type = null!;
    }
}