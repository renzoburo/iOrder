namespace iOrder.web.Models.DTO
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class ClientDTO
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "firstNames")]
        public string FirstNames { get; set; }

        [DataMember(Name = "surname")]
        public string Surname { get; set; }

        [DataMember(Name = "addressType")]
        public string AddressType { get; set; }

        [DataMember(Name = "streetAddress")]
        public string StreetAddress { get; set; }

        [DataMember(Name = "suburb")]
        public string Suburb { get; set; }

        [DataMember(Name = "city")]
        public string City { get; set; }

        [DataMember(Name = "postalCode")]
        public string PostalCode { get; set; }
    }
}
