namespace iOrder.dataaccess.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using Attributes;
    using Base;
    using Data;
    using Interfaces;

    [Table("Client", Schema = "dbo")]
    public class Client : EntityBase
    {
        [Direction(DirectionType.Input)]
        public string FirstNames { get; set; }

        [Direction(DirectionType.Input)]
        public string Surname { get; set; }

        [Direction(DirectionType.Input)]
        public Guid? AddressType { get; set; }

        [Direction(DirectionType.Input)]
        public string StreetAddress { get; set; }

        [Direction(DirectionType.Input)]
        public string Suburb { get; set; }

        [Direction(DirectionType.Input)]
        public string City { get; set; }

        [Direction(DirectionType.Input)]
        public int PostalCode { get; set; }

        public override IEnumerable<IEntity> Get()
        {
            return Database.Get<Client>();
        }

        public override IEntity Get(Guid id)
        {
            Id = id;
            return Database.GetById<Order>(id);
        }

        public override IEntity Save()
        {
            return Database.SaveProc(this);
        }

        public override bool Delete()
        {
            return Database.DeleteProc(this);
        }

        public static IEnumerable<Client> GetClients()
        {
            return Database.Get<Client>();
        }

        public static Client GetClient(Guid id)
        {
            return Database.GetById<Client>(id);
        }
    }
}
