/*
 *    描述:
 *          1. AssetBundleXml序列化类
 *
 *    开发人: 邓平
 */
using System.Collections.Generic;
using System.Xml.Serialization;

namespace LtFramework.ResKit
{

    [System.Serializable]
    public class AssetBundleConfig
    {
        [XmlElement("ABList")] public List<ABBase> ABList { get; set; }
    }

    [System.Serializable]
    public class ABBase
    {
        [XmlAttribute("Path")] public string Path { get; set; }
        [XmlAttribute("Crc")] public uint Crc { get; set; }
        [XmlAttribute("ABName")] public string ABName { get; set; }
        [XmlAttribute("AssetName")] public string AssetName { get; set; }
        [XmlElement("ABDependence")] public List<string> ABDependence { get; set; }

    }
}
