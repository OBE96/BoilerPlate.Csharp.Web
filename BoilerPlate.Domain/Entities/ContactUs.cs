﻿namespace BoilerPlate.Domain.Entities
{
    public class ContactUs : EntityBase
    {
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Message { get; set; }

    }
}
