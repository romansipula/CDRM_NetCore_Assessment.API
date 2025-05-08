using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.ComponentModel.DataAnnotations;

namespace CDRM_NetCore_Assessment.API.Models
{
    public class Document : IXmlSerializable
    {
        [Required]
        public string Id { get; set; } = string.Empty;

        [Required]
        [MinLength(1, ErrorMessage = "At least one tag is required.")]
        public List<string> Tags { get; set; } = new();

        [Required]
        public Dictionary<string, object> Data { get; set; } = new();

        // IXmlSerializable implementation
        #nullable enable
        public XmlSchema? GetSchema() => null;
        #nullable disable

        public void ReadXml(XmlReader reader)
        {
            reader.MoveToContent();
            Id = reader.GetAttribute("Id") ?? string.Empty;
            reader.ReadStartElement();
            // Tags
            Tags = new List<string>();
            if (reader.IsStartElement("Tags"))
            {
                reader.ReadStartElement("Tags");
                while (reader.IsStartElement("Tag"))
                {
                    Tags.Add(reader.ReadElementContentAsString());
                }
                reader.ReadEndElement();
            }
            // Data
            if (reader.IsStartElement("Data"))
            {
                var dataJson = reader.ReadElementContentAsString();
                Data = JsonSerializer.Deserialize<Dictionary<string, object>>(dataJson) ?? new();
            }
            reader.ReadEndElement();
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("Id", Id);
            // Tags
            writer.WriteStartElement("Tags");
            if (Tags != null)
            {
                foreach (var tag in Tags)
                {
                    writer.WriteElementString("Tag", tag);
                }
            }
            writer.WriteEndElement();
            // Data
            writer.WriteStartElement("Data");
            writer.WriteString(JsonSerializer.Serialize(Data));
            writer.WriteEndElement();
        }
    }
}
