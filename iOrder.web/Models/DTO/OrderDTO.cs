namespace iOrder.web.Models.DTO
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class OrderDTO
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember]
        public string ClientId { get; set; }

        [DataMember]
        public DateTime DateCreated { get; set; }

        [DataMember]
        public DateTime DateUpdated { get; set; }

        [DataMember]
        public bool Complete { get; set; }
    }
}
