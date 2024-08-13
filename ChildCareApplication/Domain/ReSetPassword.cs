using MediatR;

namespace ChildCareApplication.Domain
{
    public class ReSetPassword :IRequest<bool>
    {
        public string Email { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
