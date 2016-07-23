using System;
using AutoMapper;

namespace Gaver.Logic
{
    public class MailMapping : IMapping<Mail,SendGridMail>
    {
        public void ConfigureMapping(IMappingExpression<Mail, SendGridMail> expression)
        {        }

        public void Process(Mail source, SendGridMail destination)
        {
            throw new NotImplementedException();
        }
    }
}