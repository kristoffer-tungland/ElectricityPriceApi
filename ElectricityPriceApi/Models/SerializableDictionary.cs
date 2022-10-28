using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace ElectricityPriceApi.Models;

[XmlRoot("dictionary")]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IXmlSerializable where TKey : notnull
{
    public XmlSchema? GetSchema()
    {
        return null;
    }

    public void ReadXml(XmlReader reader)
    {
        var valueSerializer = new XmlSerializer(typeof(TValue));

        var wasEmpty = reader.IsEmptyElement;
        reader.Read();

        if (wasEmpty)
            return;

        while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
        {
            var type = typeof(TKey);
            reader.ReadStartElement();

            var key = (TKey)reader.ReadElementContentAs(type, new XmlNamespaceManager(reader.NameTable));
            var value = (TValue)valueSerializer.Deserialize(reader)!;

            reader.ReadEndElement();
            
            Add(key, value);

            reader.ReadEndElement();
            reader.MoveToContent();
        }

        reader.ReadEndElement();
    }

    public void WriteXml(XmlWriter writer)
    {
        foreach (var key in Keys)
        {
            writer.WriteStartElement(key.ToString()!);
            var value = this[key];
            writer.WriteValue(value!);

            writer.WriteEndElement();
        }
    }
}