namespace iOrder.dataaccess.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using Attributes;
    using Base;
    using Data;
    using Interfaces;

    [Table("Product", Schema = "dbo")]
    public class Product : EntityBase
    {
        [Direction(DirectionType.Input)]
        public string Title { get; set; }

        [Direction(DirectionType.Input)]
        public string Description { get; set; }

        [Direction(DirectionType.Input)]
        public decimal Price { get; set; }

        [Direction(DirectionType.Input)]
        public string Image { get; set; }

        public override IEnumerable<IEntity> Get()
        {
            return Database.Get<Product>();
        }

        public override IEntity Get(Guid id)
        {
            return Database.GetById<Product>(id);
        }

        public override IEntity Save()
        {
            return Database.Save(this);
        }

        public override bool Delete()
        {
            return Database.Delete(this);
        }
    }
}
