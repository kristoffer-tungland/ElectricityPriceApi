using System.Xml.Serialization;
// ReSharper disable IdentifierTypo

namespace ElectricityPriceApi.XMLSchemas;

[XmlRoot(ElementName = "Acknowledgement_MarketDocument", Namespace = "urn:iec62325.351:tc57wg16:451-1:acknowledgementdocument:7:0")]
public class AcknowledgementMarketDocument
{
    [XmlElement(ElementName = "mRID", Namespace = "urn:iec62325.351:tc57wg16:451-1:acknowledgementdocument:7:0")]
    public string? Mrid { get; set; }
    [XmlElement(ElementName = "createdDateTime", Namespace = "urn:iec62325.351:tc57wg16:451-1:acknowledgementdocument:7:0")]
    public string? CreatedDateTime { get; set; }
    [XmlElement(ElementName = "sender_MarketParticipant.mRID", Namespace = "urn:iec62325.351:tc57wg16:451-1:acknowledgementdocument:7:0")]
    public SenderMarketParticipantmRid? SenderMarketParticipantmRid { get; set; }
    [XmlElement(ElementName = "sender_MarketParticipant.marketRole.type", Namespace = "urn:iec62325.351:tc57wg16:451-1:acknowledgementdocument:7:0")]
    public string? SenderMarketParticipantmarketRoletype { get; set; }
    [XmlElement(ElementName = "receiver_MarketParticipant.mRID", Namespace = "urn:iec62325.351:tc57wg16:451-1:acknowledgementdocument:7:0")]
    public ReceiverMarketParticipantmRid? ReceiverMarketParticipantmRid { get; set; }
    [XmlElement(ElementName = "receiver_MarketParticipant.marketRole.type", Namespace = "urn:iec62325.351:tc57wg16:451-1:acknowledgementdocument:7:0")]
    public string? ReceiverMarketParticipanmarketRoletype { get; set; }
    [XmlElement(ElementName = "received_MarketDocument.createdDateTime", Namespace = "urn:iec62325.351:tc57wg16:451-1:acknowledgementdocument:7:0")]
    public string? ReceivedMarketDocumentcreatedDateTime { get; set; }
    [XmlElement(ElementName = "Reason", Namespace = "urn:iec62325.351:tc57wg16:451-1:acknowledgementdocument:7:0")]
    public Reason? Reason { get; set; }
    [XmlAttribute(AttributeName = "xmlns")]
    public string? Xmlns { get; set; }
}

[XmlRoot(ElementName = "sender_MarketParticipant.mRID", Namespace = "urn:iec62325.351:tc57wg16:451-1:acknowledgementdocument:7:0")]
public class SenderMarketParticipantmRid
{
    [XmlAttribute(AttributeName = "codingScheme")]
    public string? CodingScheme { get; set; }

    [XmlText]
    public string? Text { get; set; }
}

[XmlRoot(ElementName = "receiver_MarketParticipant.mRID", Namespace = "urn:iec62325.351:tc57wg16:451-1:acknowledgementdocument:7:0")]
public class ReceiverMarketParticipantmRid
{
    [XmlAttribute(AttributeName = "codingScheme")]
    public string? CodingScheme { get; set; }
    [XmlText]
    public string? Text { get; set; }
}

[XmlRoot(ElementName = "Reason", Namespace = "urn:iec62325.351:tc57wg16:451-1:acknowledgementdocument:7:0")]
public class Reason
{
    [XmlElement(ElementName = "code", Namespace = "urn:iec62325.351:tc57wg16:451-1:acknowledgementdocument:7:0")]
    public string? Code { get; set; }
    [XmlElement(ElementName = "text", Namespace = "urn:iec62325.351:tc57wg16:451-1:acknowledgementdocument:7:0")]
    public string? Text { get; set; }
}