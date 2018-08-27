namespace iOrder.web.Models.DTO
{
    using System.Runtime.Serialization;

    [DataContract]
    public class AddressTypeDTO
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember]
        public string AddressTypeName { get; set; }
    }
}
