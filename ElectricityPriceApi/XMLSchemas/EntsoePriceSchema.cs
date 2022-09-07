using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ElectricityPriceApi.XMLSchemas
{
	// using System.Xml.Serialization;
	// XmlSerializer serializer = new XmlSerializer(typeof(PublicationMarketDocument));
	// using (StringReader reader = new StringReader(xml))
	// {
	//    var test = (PublicationMarketDocument)serializer.Deserialize(reader);
	// }

	[XmlRoot(ElementName = "sender_MarketParticipant.mRID")]
	public class SenderMarketParticipantMRID
	{

		[XmlAttribute(AttributeName = "codingScheme")]
		public string CodingScheme { get; set; }

		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName = "receiver_MarketParticipant.mRID")]
	public class ReceiverMarketParticipantMRID
	{

		[XmlAttribute(AttributeName = "codingScheme")]
		public string CodingScheme { get; set; }

		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName = "period.timeInterval")]
	public class PeriodTimeInterval
	{

		[XmlElement(ElementName = "start")]
		public DateTime Start { get; set; }

		[XmlElement(ElementName = "end")]
		public DateTime End { get; set; }
	}

	[XmlRoot(ElementName = "in_Domain.mRID")]
	public class InDomainMRID
	{

		[XmlAttribute(AttributeName = "codingScheme")]
		public string CodingScheme { get; set; }

		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName = "out_Domain.mRID")]
	public class OutDomainMRID
	{

		[XmlAttribute(AttributeName = "codingScheme")]
		public string CodingScheme { get; set; }

		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName = "timeInterval")]
	public class TimeInterval
	{

		[XmlElement(ElementName = "start")]
		public DateTime Start { get; set; }

		[XmlElement(ElementName = "end")]
		public DateTime End { get; set; }
	}

	[XmlRoot(ElementName = "Point")]
	public class Point
	{

		[XmlElement(ElementName = "position")]
		public int Position { get; set; }

		[XmlElement(ElementName = "price.amount")]
		public double PriceAmount { get; set; }
	}

	[XmlRoot(ElementName = "Period")]
	public class Period
	{

		[XmlElement(ElementName = "timeInterval")]
		public TimeInterval TimeInterval { get; set; }

		[XmlElement(ElementName = "resolution")]
		public string Resolution { get; set; }

		[XmlElement(ElementName = "Point")]
		public List<Point> Point { get; set; }
	}

	[XmlRoot(ElementName = "TimeSeries")]
	public class TimeSeries
	{

		[XmlElement(ElementName = "mRID")]
		public int MRID { get; set; }

		[XmlElement(ElementName = "businessType")]
		public string BusinessType { get; set; }

		[XmlElement(ElementName = "in_Domain.mRID")]
		public InDomainMRID InDomainMRID { get; set; }

		[XmlElement(ElementName = "out_Domain.mRID")]
		public OutDomainMRID OutDomainMRID { get; set; }

		[XmlElement(ElementName = "currency_Unit.name")]
		public string CurrencyUnitName { get; set; }

		[XmlElement(ElementName = "price_Measure_Unit.name")]
		public string PriceMeasureUnitName { get; set; }

		[XmlElement(ElementName = "curveType")]
		public string CurveType { get; set; }

		[XmlElement(ElementName = "Period")]
		public Period Period { get; set; }
	}

    [XmlType(Namespace = "urn:iec62325.351:tc57wg16:451-3:publicationdocument:7:0")]
	[XmlRoot(ElementName = "Publication_MarketDocument", Namespace = "urn:iec62325.351:tc57wg16:451-3:publicationdocument:7:0")]
	public class PublicationMarketDocument
	{

		[XmlElement(ElementName = "mRID")]
		public string MRID { get; set; }

		[XmlElement(ElementName = "revisionNumber")]
		public int RevisionNumber { get; set; }

		[XmlElement(ElementName = "type")]
		public string Type { get; set; }

		[XmlElement(ElementName = "sender_MarketParticipant.mRID")]
		public SenderMarketParticipantMRID SenderMarketParticipantMRID { get; set; }

		[XmlElement(ElementName = "sender_MarketParticipant.marketRole.type")]
		public string SenderMarketParticipantMarketRoleType { get; set; }

		[XmlElement(ElementName = "receiver_MarketParticipant.mRID")]
		public ReceiverMarketParticipantMRID ReceiverMarketParticipantMRID { get; set; }

		[XmlElement(ElementName = "receiver_MarketParticipant.marketRole.type")]
		public string ReceiverMarketParticipantMarketRoleType { get; set; }

		[XmlElement(ElementName = "createdDateTime")]
		public DateTime CreatedDateTime { get; set; }

		[XmlElement(ElementName = "period.timeInterval")]
		public PeriodTimeInterval PeriodTimeInterval { get; set; }

		[XmlElement(ElementName = "TimeSeries")]
		public List<TimeSeries> TimeSeries { get; set; }

		[XmlAttribute(AttributeName = "xmlns")]
		public string Xmlns { get; set; }

		[XmlText]
		public string Text { get; set; }
	}


}
