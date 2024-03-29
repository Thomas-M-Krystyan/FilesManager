﻿using FilesManager.UI.Common.Properties;
using System.Text.RegularExpressions;

namespace FilesManager.Core.Validation
{
    /// <summary>
    /// Validation methods.
    /// </summary>
    internal static class Validate
    {
        /// <summary>
        /// Determines whether the given file path is valid.
        /// </summary>
        /// <param name="filePath">The file path to be validated.</param>
        /// <param name="filePathMatch">
        ///   <inheritdoc cref="Regex.Match(string)" path="/returns"/>.
        /// </param>
        /// <returns>
        ///   <see langword="true"/> if the file path is valid; otherwise, <see langword="false"/>.
        /// </returns>
        internal static bool IsFilePathValid(string filePath, out Match filePathMatch)
        {
            filePathMatch = Match.Empty;

            try
            {
                if (string.IsNullOrWhiteSpace(filePath))
                {
                    return false;
                }

                filePathMatch = RegexPatterns.FileComponentsPattern().Match(filePath);

                return filePathMatch.Success;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Determines whether the given <see langword="string"/> contains illegal characters.
        /// </summary>
        /// <param name="textInputs">The text input to be validated.</param>
        /// <param name="successAction">The action to be executed in case of success.</param>
        /// <param name="failureAction">The action to be executed in case of failure.</param>
        /// <returns>
        ///   <inheritdoc cref="Regex.IsMatch(string)"/>
        /// </returns>
        internal static bool ContainInvalidCharacters(string textInput, Action? successAction = null, Action? failureAction = null)
        {
            try
            {
                bool isFailure = RegexPatterns.InvalidCharactersPattern.IsMatch(textInput);

                if (isFailure)
                {
                    failureAction?.Invoke();
                }
                else
                {
                    successAction?.Invoke();
                }

                return isFailure;
            }
            catch
            {
                return true;
            }
        }

        /// <summary>
        /// Determines whether the given value is a valid <see langword="ushort"/> digit or number.
        /// </summary>
        /// <param name="value">The text value to be checked.</param>
        /// <param name="number">The valid number to be returned.</param>
        /// <param name="successAction">The action to be executed in case of success.</param>
        /// <param name="failureAction">The action to be executed in case of failure.</param>
        /// <returns>
        ///   <inheritdoc cref="ushort.TryParse(string?, out ushort)"/>
        /// </returns>
        internal static bool Is<TNumber>(string value, out TNumber number, Action? successAction = null, Action? failureAction = null)
            where TNumber : struct, IComparable, IComparable<TNumber>, IConvertible, IEquatable<TNumber>, IFormattable  // NOTE: Numeric type
        {
            try
            {
                number = (TNumber)Convert.ChangeType(value, typeof(TNumber));
                successAction?.Invoke();

                return true;
            }
            catch
            {
                number = default;
                failureAction?.Invoke();

                return false;
            }
        }

        /// <summary>
        /// Determines whether the given number fits within the specifiex max limit.
        /// </summary>
        /// <param name="number">The number to be checked.</param>
        /// <param name="maxLimit">The maximum limit for a given number.</param>
        /// <param name="successAction">The action to be executed in case of success.</param>
        /// <param name="failureAction">The action to be executed in case of failure.</param>
        /// <returns>
        ///   <see langword="true"/> if the number is smaller or equal to the max limit; otherwise, <see langword="false"/>.
        /// </returns>
        internal static bool WithinLimit<TNumber>(TNumber number, TNumber maxLimit, Action? successAction = null, Action? failureAction = null)
            where TNumber : struct, IComparable, IComparable<TNumber>, IConvertible, IEquatable<TNumber>, IFormattable  // NOTE: Numeric type
            // NOTE: For two different numeric types (doesn't matter if there are floating point or integer ones)
            // the one fitting for both will be used (e.g., "int + double" => "double", "ushort + byte" => "ushort")
        {
            bool isSuccess = number.CompareTo(maxLimit) <= 0;

            if (isSuccess)
            {
                successAction?.Invoke();
            }
            else
            {
                failureAction?.Invoke();
            }

            return isSuccess;
        }

        /// <summary>
        /// Reports an invalid usage of an event.
        /// <para>
        ///   Used for development purposes to monitor if an event from the XAML side was binded to a command
        ///   subscribing to a method which is using a received object parameter as a proper event argument.
        /// </para>
        /// </summary>
        /// <param name="methodName">The name of the method.</param>
        /// <exception cref="InvalidOperationException">The event argument is invalid.</exception>
        internal static void ReportInvalidCommandUsage(string methodName)
        {
            throw new InvalidOperationException(Resources.ERROR_Internal_WrongCommandSubscribed + $" {methodName}");
        }
    }
}
