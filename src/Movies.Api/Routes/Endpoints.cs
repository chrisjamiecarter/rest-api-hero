namespace Movies.Api.Routes;

public static class Endpoints
{
    private const string Root = "api";

    public static class Movies
    {
        private const string Base = $"{Root}/movies";

        public const string Create = Base;
        public const string Delete = $"{Base}/{{id:guid}}";
        public const string Get = $"{Base}/{{idOrSlug}}";
        public const string GetAll = Base;
        public const string Update = $"{Base}/{{id:guid}}";

        public const string RateMovie = $"{Base}/{{id:guid}}/ratings";
        public const string DeleteMovieRating = $"{Base}/{{id:guid}}/ratings";
    }

    public static class Ratings
    {
        private const string Base = $"{Root}/ratings";

        public const string GetUserRatings = $"{Base}/me";
    }
}
