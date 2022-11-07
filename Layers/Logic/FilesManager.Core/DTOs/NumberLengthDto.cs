using System.Text.RegularExpressions;

namespace FilesManager.Core.DTOs
{
    /// <summary>
    /// The result of calculating the length of a number component from a file name.
    /// </summary>
    public class NumberLengthDto
    {
        public int NumberLength { get; }

        public GroupCollection? FileGroups { get; }

        public GroupCollection? NumberGroups { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NumberLengthDto"/> class.
        /// </summary>
        public NumberLengthDto(int numberLength, GroupCollection? fileGroups, GroupCollection? numberGroups)
        {
            this.NumberLength = numberLength;
            this.FileGroups = fileGroups;
            this.NumberGroups = numberGroups;
        }
    }
}
