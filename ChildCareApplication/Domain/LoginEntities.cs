using MediatR;

namespace ChildCareApplication.Domain
{
    public class LoginEntities : IRequest<bool>
    {
        public string Email { get; set; }
       public string Password { get; set; }
        
    }
}
