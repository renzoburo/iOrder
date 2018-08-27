namespace iOrder.dataaccess.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using Attributes;
    using Base;
    using Data;
    using Interfaces;

    [Table("OrderProduct", Schema = "dbo")]
    public class OrderProduct : EntityBase
    {
        [Direction(DirectionType.Input)]
        public Guid? OrderId { get; set; }

        [Direction(DirectionType.Input)]
        public Guid? ProductId { get; set; }

        [Direction(DirectionType.Input)]
        public int? Quantity { get; set; }

        public override IEnumerable<IEntity> Get()
        {
            return Database.Get<OrderProduct>();
        }

        public override IEntity Get(Guid id)
        {
            return Database.GetById<OrderProduct>(id);
        }

        public override IEntity Save()
        {
            return Database.Save(this);
        }

        public override bool Delete()
        {
            return Database.Delete(this);
        }

        public static IEnumerable<OrderProduct> GetOrderProducts()
        {
            return Database.Get<OrderProduct>();
        }

        public static OrderProduct GetOrderProduct(Guid id)
        {
            return Database.GetById<OrderProduct>(id);
        }
    }
}
