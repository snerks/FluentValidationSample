using System;

namespace FluentValidationLib
{
    public class Widget
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Description { get; set; }
    }
}
