namespace iOrder.web.Models.DTO
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class OrderProductDTO
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember]
        public string OrderId { get; set; }

        [DataMember]
        public string ProductId { get; set; }

        [DataMember]
        public string Quantity { get; set; }
    }
}
