namespace DSCC.CW1._12219.MVC.Services
{
    public class UrlInjector
    {
        private readonly IConfiguration _configuration;

        public UrlInjector(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetUrl()
        {
            return _configuration["BaseUrl"];
        }
    }
}
