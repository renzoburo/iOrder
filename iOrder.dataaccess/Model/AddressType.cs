namespace iOrder.dataaccess.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using Attributes;
    using Base;
    using Data;
    using Interfaces;

    [Table("AddressType", Schema = "dbo")]
    public class AddressType : EntityBase
    {
        [Direction(DirectionType.Input)]
        public string AddressTypeName { get; set; }

        public override IEnumerable<IEntity> Get()
        {
            return Database.Get<AddressType>();
        }

        public override IEntity Get(Guid id)
        {
            return Database.GetById<AddressType>(id);
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
