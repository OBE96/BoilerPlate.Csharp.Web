using BoilerPlate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoilerPlate.Infrastructure.Services.Interfaces
{
    public interface IEmailService
    {
        public Task<Message> SendEmailMessage(Message message);
    }
}
