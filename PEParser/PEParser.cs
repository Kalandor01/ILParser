namespace PEParser
{
    public static class PEParser
    {
        #region Public methods
        public static object Parse(string filePath)
        {
            if (
                !Path.Exists(filePath) ||
                Path.GetExtension(filePath).ToLower() is not (Constants.EXE_EXTENSION or Constants.DLL_EXTENSION)
            )
            {
                throw new ArgumentException("Invalid file path", nameof(filePath));
            }

            return 5;
        }
        #endregion

        #region Private methods
        private static void Hmm()
        {
            
        }
        #endregion
    }
}