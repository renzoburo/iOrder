namespace iOrder.web.Controllers
{
    using System;
    using System.Collections.Generic;
    using dataaccess.Model;
    using Interfaces;
    using Microsoft.AspNetCore.Mvc;
    using Models.DTO;
    using Models.Extensions;

    [Route("api/OrderProduct")]
    public class OrderProductController : Controller
    {
        OrderProduct orderProductDataSource;

        private OrderProduct OrdeProductDataSource => orderProductDataSource ?? (orderProductDataSource = new OrderProduct());

        public IEnumerable<OrderProductDTO> Get()
        {
            var order = (IEnumerable<OrderProduct>)OrdeProductDataSource.Get();
            return order.MapToOrderProductDtoList();
        }

        public OrderProductDTO Get(Guid id)
        {
            var order = (OrderProduct)OrdeProductDataSource.Get(id);
            return order.MapToOrderProductDto();
        }

        public IActionResult Post(ClientDTO clientDto)
        {
            try
            {
                var client = clientDto.MapToClient();
                var result = client.Save();
                return Ok(result);
            }
            catch (Exception exception)
            {
                return RedirectToAction("Error");
            }
        }

        public IActionResult Delete(ClientDTO clientDto)
        {
            try
            {
                var client = clientDto.MapToClient();
                var result = client.Delete();
                return Ok(result);
            }
            catch (Exception exception)
            {
                return RedirectToAction("Error");
            }
        }

        public IActionResult Error()
        {
            return Redirect("Error");
        }
    }
}