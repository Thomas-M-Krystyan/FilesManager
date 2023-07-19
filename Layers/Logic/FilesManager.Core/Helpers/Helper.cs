using FilesManager.Core.ExtensionMethods;

namespace FilesManager.Core.Helpers
{
    /// <summary>
    /// Helper methods for common usage.
    /// </summary>
    public static class Helper
    {
        /// <summary>
        /// Gets the length of the longest text from the provided input.
        /// </summary>
        /// <param name="inputs">The file names:
        ///   <para>
        ///     <list type="bullet">
        ///       <item>collection cannot be null</item>
        ///       <item>items cannot be null or contain only whitespaces</item>
        ///     </list>
        ///   </para>
        /// </param>
        /// <returns>Length of the longest text input.</returns>
        public static int GetMaxLength(string[] inputs)
        {
            if (inputs.IsNullOrEmpty())
            {
                return 0;
            }

            int count = 0;

            for (int index = 0; index < inputs.Length; index++)
            {
                if (String.IsNullOrWhiteSpace(inputs[index]))
                {
                    continue;
                }

                count = Math.Max(count, inputs[index].Length);
            }

            return count;
        }
    }
}
