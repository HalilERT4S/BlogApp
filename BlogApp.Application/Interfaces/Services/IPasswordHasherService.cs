using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Application.Interfaces.Services
{
    public interface IPasswordHasherService
    {
        (byte[] PasswordHash, byte[] PasswordSalt) HashPassword(string password);
        bool VerifyPassword(string password, byte[] storedHash, byte[] storedSalt);
    }
}
