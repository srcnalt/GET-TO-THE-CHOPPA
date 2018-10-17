public static class Global
{
    public static void OpenNyaarghWebpage()
    {
        OpenURL("http://nyaargh.com?utm_source=GETTOTHECHOPPA");
    }

    private static void OpenURL(string url)
    {
        UnityEngine.Application.OpenURL(url);
    }
}