using Microsoft.AspNetCore.Http;

namespace SharedLibrary.Interfaces
{
    public interface IJWTWebLoginHandler
    {
        public Task Login(HttpContext context, string token, string name, string surname);
    }
}
