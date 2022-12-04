namespace Tests;

public static class Uris
{
    public static class FrontEnd
    {
        public static Uri Home => new("http://localhost:8010");
        public static Uri Login
        {
            get
            {
                return new Uri(Home, new Uri("/Account/Login", UriKind.Relative));
            }
        }
    }
    public static class Registration
    {
        public static Uri Home => new("http://localhost:8030");
    }
}