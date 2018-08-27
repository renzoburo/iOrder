namespace iOrder.web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using dataaccess.Model;
    using Microsoft.AspNetCore.Mvc;
    using Models.DTO;
    using Models.Extensions;

    public class ProductController : Controller
    {
        Product productDataSource;

        private Product ProductDataSource => productDataSource ?? (productDataSource = new Product());

        static string ClientId { get; set; }

        [Route("Product/Product/{clientId}")]
        public IActionResult Product(string clientId)
        {
            ClientId = clientId;
            return View();
        }

        [Route("Product/Product/api/GetProducts")]
        public IEnumerable<ProductDTO> Get()
        {
            var products = (IEnumerable<Product>)ProductDataSource.Get();
            return products.MapToProductDtoList();
        }

        [Route("Product/Product/api/GetClientBasket")]
        public BasketDTO GetClientBasket()
        {
            try
            {
                var client = Client.GetClient(new Guid(ClientId));
                if (client == null) return null;

                var orders = Order.GetOrders();
                var openOrder = orders.FirstOrDefault(f => f.ClientId == client.Id && !f.Complete);

                if (openOrder == null) return null;

                var orderProducts = OrderProduct.GetOrderProducts();
                var openOrderProducts = orderProducts.Where(w => w.OrderId == openOrder.Id);

                return new BasketDTO
                {
                    OrderInfo = openOrder.MapTOrderDto(),
                    OrderProducts = openOrderProducts.MapToOrderProductDtoList()
                };
            }
            catch (Exception exception)
            {
                return null;
            }
        }

        [HttpPost]
        [Route("Product/Product/api/AddToBasket")]
        public BasketDTO AddToBasket([FromBody]ProductDTO productDto)
        {
            if (productDto == null) return null;

            try
            {
                var client = Client.GetClient(new Guid(ClientId));
                var product = productDto.MapToProduct();
                var openOrder = Order.GetOrders().FirstOrDefault(f => !f.Complete);
                var order = openOrder ?? new Order
                {
                    ClientId = client.Id,
                    DateCreated = DateTime.Now,
                    DateUpdated = DateTime.Now
                };

                if (openOrder == null)
                    order = (Order)order.Save(); //Create an order if none exists so that we can get an Id value back

                var orderProducts = (IList<OrderProduct>)OrderProduct.GetOrderProducts();
                var openOrderProducts = orderProducts?.Where(w => w.OrderId == order.Id);
                var orderProductsList = openOrderProducts?.ToList() ?? new List<OrderProduct>();
//                {
                    //OrderId = order.Id
//                };

                var matchingProduct = orderProductsList.FirstOrDefault(op => op.OrderId == order.Id && order.ClientId == client.Id && op.ProductId == product.Id);

                OrderProduct orderProduct;
                if (matchingProduct != null)
                {
                    var productIndex = orderProductsList.IndexOf(matchingProduct);
                    matchingProduct.Quantity++;
                    orderProduct = (OrderProduct)matchingProduct.Save();
                    orderProductsList[productIndex] = orderProduct;
                }
                else
                {
                    orderProduct = new OrderProduct
                    {
                        OrderId = order.Id,
                        ProductId = product.Id,
                        Quantity = 1
                    };
                    orderProductsList.Add((OrderProduct)orderProduct.Save());
                }

                order.DateUpdated = DateTime.Now;
                order = (Order)order.Save(); //Order was updated so save it now after updating the date.

                var result = new BasketDTO
                {
                    OrderInfo = order.MapTOrderDto(),
                    OrderProducts = orderProductsList.MapToOrderProductDtoList()
                };
                return result;
            }
            catch (Exception exception)
            {
                return null;
            }
        }

        [HttpDelete]
        [Route("Product/Product/api/RemoveFromBasket")]
        public BasketDTO RemoveFromBasket([FromBody] DeleteProductFromBasketDTO parameter)
        {
            try
            {
                var productDTO = parameter.Product;
                var basket = parameter.Basket;
                var basketProducts = basket.OrderProducts.ToList();
                var productoOrderToDelete = basketProducts.FirstOrDefault(f => f.ProductId == productDTO.Id).MapToOrderProduct();
                var productToDeleteDto = productoOrderToDelete.MapToOrderProductDto();

                if (productoOrderToDelete.Quantity > 1)
                {
                    var productIndex = basketProducts.IndexOf(productToDeleteDto);
                    productoOrderToDelete.Quantity--;
                    productoOrderToDelete.Save();
                    productToDeleteDto.Quantity = productoOrderToDelete.Quantity.ToString();
                    basketProducts[productIndex] = productToDeleteDto;
                    basket.OrderProducts = basketProducts;
                }
                else if (productoOrderToDelete.Quantity == 1)
                {
                    if (productoOrderToDelete.Delete())
                    {
                        //basketProducts.Remove(productToDeleteDto);
                        basketProducts.RemoveAll(r => r.Id == productToDeleteDto.Id);
                        basket.OrderProducts = basketProducts.Any() ? basketProducts : null;
                    }
                }

                return basket;
            }
            catch (Exception exception)
            {
                return null;
            }
        }

        [HttpPost]
        [Route("Product/Product/api/Checkout")]
        public bool Checkout([FromBody] BasketDTO basketDto)
        {
            var order = basketDto.OrderInfo.MapToOrder();
            if (order != null)
            {
                order.Complete = true;
                order = (Order)order.Save();
            }
            return order != null;
        }
    }
}
