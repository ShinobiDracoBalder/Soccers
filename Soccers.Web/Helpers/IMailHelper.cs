using Soccers.Common.Models;

namespace Soccers.Web.Helpers
{
    public interface IMailHelper
    {
        Response SendMail(string to, string subject, string body);
    }
}
