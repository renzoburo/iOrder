namespace iOrder.dataaccess.Attributes
{
    using System;

    public enum DirectionType
    {
        Input,
        Output,
        InputOutput
    }

    public class DirectionAttribute : Attribute
    {
        public DirectionType DirectionType { get; set; }

        public DirectionAttribute(DirectionType directionType)
        {
            DirectionType = directionType;
        }

    }
}
