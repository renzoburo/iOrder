namespace iOrder.web.Models.DTO
{
    using System.Runtime.Serialization;

    [DataContract]
    public class ProductDTO
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string Price { get; set; }

        [DataMember]
        public string Image { get; set; }
    }
}
