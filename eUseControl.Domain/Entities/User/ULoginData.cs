using System;

namespace eUseControl.Domain.Entities.User
{
    public class ULoginData
    {
        public string Credential { get; set; }
        public string Password { get; set; }
        public string LoginIp { get; set; }
        public DateTime LoginDateTime { get; set; }
    }
}
