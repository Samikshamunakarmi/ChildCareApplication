using MediatR;

namespace ChildCareApplication.Domain
{
    public class LoginEntities : BaseEntities, IRequest<bool>
    {
        public string Email { get; set; }
       public string Password { get; set; }
        
    }
}
