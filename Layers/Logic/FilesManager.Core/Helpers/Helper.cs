using FilesManager.Core.DTOs;
using FilesManager.Core.ExtensionMethods;
using FilesManager.Core.Validation;
using System.Text.RegularExpressions;

namespace FilesManager.Core.Helpers
{
    /// <summary>
    /// Helper miscellaneous methods.
    /// </summary>
    public static class Helper
    {
        /// <summary>
        /// Gets the length of the number group (extracted from a file name).
        /// </summary>
        public static NumberLengthDto GetNumberLength(string oldFilePath)
        {
            const int NotFound = 0;  // There are no digits in the file name

            Match pathMatch = Validate.IsFilePathValid(oldFilePath);

            if (pathMatch.Success)
            {
                string fileName = pathMatch.Value(Validate.NameGroup);

                Match numberMatch = Regex.Match(fileName, Validate.LeadingZerosPattern);
                GroupCollection numberGroups = numberMatch.Groups;

                return numberMatch.Success
                    ? new NumberLengthDto(numberGroups.Value(Validate.ZerosGroup).Length + numberGroups.Value(Validate.DigitsGroup).Length,
                                          pathMatch.Groups, numberGroups)
                    : new NumberLengthDto(NotFound, pathMatch.Groups, numberGroups);
            }
            else
            {
                return new NumberLengthDto(NotFound, pathMatch.Groups, null);
            }
        }
    }
}
