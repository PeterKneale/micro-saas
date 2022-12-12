namespace Tests;

public static class Uris
{
    public static class FrontEnd
    {
        public static Uri Home => new("http://localhost:8010");
        public static Uri Login => new Uri(Home, new Uri("/Account/Login", UriKind.Relative));
    }

    public static class Registration
    {
        public static Uri Home => new("http://localhost:8030");
        public static Uri Register => new Uri(Home, new Uri("/Register", UriKind.Relative));
        public static Uri Claim => new Uri(Home, new Uri("/Claim", UriKind.Relative));
    }

    public static class Email
    {
        public static Uri Home => new("http://localhost:8025");
    }

}