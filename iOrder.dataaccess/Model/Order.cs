namespace iOrder.dataaccess.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using Attributes;
    using Base;
    using Data;
    using Interfaces;

    [Table("Order", Schema = "dbo")]
    public class Order : EntityBase
    {
        [Direction(DirectionType.Input)]
        public Guid? ClientId { get; set; }

        [Direction(DirectionType.Input)]
        public DateTime DateCreated { get; set; }

        [Direction(DirectionType.Input)]
        public DateTime DateUpdated { get; set; }

        [Direction(DirectionType.Input)]
        public bool Complete { get; set; }

        public override IEnumerable<IEntity> Get()
        {
            return Database.Get<Order>();
        }

        public override IEntity Get(Guid id)
        {
            return Database.GetById<Order>(id);
        }

        public override IEntity Save()
        {
            return Database.Save(this);
        }

        public override bool Delete()
        {
            return Database.Delete(this);
        }

        public static IEnumerable<Order> GetOrders()
        {
            return Database.Get<Order>();
        }

        public static Order GetOrder(Guid id)
        {
            return Database.GetById<Order>(id);
        }
    }
}
