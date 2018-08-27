using Microsoft.AspNetCore.Mvc;

namespace iOrder.web.Controllers
{
    using System;
    using System.Collections.Generic;
    using dataaccess.Model;
    using Interfaces;
    using Models.DTO;
    using Models.Extensions;

    [Route("api/Order")]
    public class OrderController : Controller
    {
        Order orderDataSource;

        private Order OrderDataSource => orderDataSource ?? (orderDataSource = new Order());

        public IEnumerable<OrderDTO> Get()
        {
            var order = (IEnumerable<Order>)OrderDataSource.Get();
            return order.MapToOrderDtoList();
        }

        public OrderDTO Get(Guid id)
        {
            var order = (Order)OrderDataSource.Get(id);
            return order.MapTOrderDto();
        }

        public IActionResult Post(OrderDTO orderDto)
        {
            try
            {
                var order = orderDto.MapToOrder();
                var result = order.Save();
                return Ok(result);
            }
            catch (Exception exception)
            {
                return RedirectToAction("Error");
            }
        }

        public IActionResult Delete(OrderDTO orderDto)
        {
            try
            {
                var order = orderDto.MapToOrder();
                var result = order.Delete();
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