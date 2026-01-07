namespace ParserCommon.Extensions
{
    public static class StringExtension
    {
        extension(string str)
        {
            public string TrimNullEnd()
            {
                return str.TrimEnd('\0');
            }
        }
    }
}