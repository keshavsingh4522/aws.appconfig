namespace aws.appconfig.core.Helper;

public static class Apihelper
{
    public static string ConvertToProfileName(this string str1, string? env)
    {
        return string.Join("", str1.Split(' ').Select(str1 => char.ToUpper(str1[0]) + str1[1..].ToLower())) + "Config" + "-" + env;
    }
}
