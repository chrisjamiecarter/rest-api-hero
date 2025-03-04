namespace Identity.Api;

public static class ApiEndpoints
{
    private const string ApiBase = "api";

    public static class Token
    {
        private const string Base = $"{ApiBase}/token";

        public const string Generate = Base;
    }
}
