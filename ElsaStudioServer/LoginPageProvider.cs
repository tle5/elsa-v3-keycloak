using Elsa.Studio.Contracts;
using Microsoft.AspNetCore.Components;

namespace ElsaStudioServer
{
    public class LoginPageProvider : ILoginPageProvider
    {
        /// <inheritdoc />
        public RenderFragment GetLoginPage()
        {
            return builder =>
            {
                builder.OpenComponent<RedirectToLogin>(0);
                builder.CloseComponent();
            };
        }
    }
}
