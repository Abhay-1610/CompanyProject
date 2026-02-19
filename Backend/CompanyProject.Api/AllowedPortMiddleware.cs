using CompanyProject.Application.Interfaces;

namespace CompanyProject.Api
{
    public class AllowedPortMiddleware
    {
        private readonly RequestDelegate _next;

        public AllowedPortMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        // This method runs for every request
        public async Task InvokeAsync(HttpContext context, IAllowedPortRepository allowedPortRepository)
        {

            if (context.Request.Method == "OPTIONS")
            {
                await _next(context);
                return;
            }

            int? allowedPort = await allowedPortRepository.GetAllowedPortAsync();

            string originHeader = context.Request.Headers["Origin"];


            if (!string.IsNullOrEmpty(originHeader))
            {

                Uri originUri = new Uri(originHeader);

                int requestPort = originUri.Port;

                if (requestPort == allowedPort)
                {
                    await _next(context);              // 7. Port matched → continue pipeline
                    return;
                }
            }

                      // 8. Port did not match → block request
            throw new UnauthorizedAccessException("Access denied: invalid port");
        }
    }

}
