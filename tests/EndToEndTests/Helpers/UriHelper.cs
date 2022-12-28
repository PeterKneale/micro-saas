namespace EndToEndTests.Helpers;

public static class UriHelper
{
    public static class FrontEnd
    {
        public static Uri Home => new("http://localhost:8010");
        public static Uri Login => new Uri(Home, new Uri("/Account/Login", UriKind.Relative));
    }

    public static class Registration
    {
        public static Uri Home => new("http://localhost:8010");
        public static Uri Register => new Uri(Home, new Uri("/", UriKind.Relative));
        public static Uri Claim => new Uri(Home, new Uri("/Claim", UriKind.Relative));
    }
}