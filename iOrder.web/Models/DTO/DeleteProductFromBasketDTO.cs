namespace iOrder.web.Models.DTO
{
    using System.Runtime.Serialization;

    [DataContract]
    public class DeleteProductFromBasketDTO
    {
        [DataMember(Name = "basket")]
        public BasketDTO Basket { get; set; }

        [DataMember(Name = "product")]
        public ProductDTO Product { get; set; }
    }
}
