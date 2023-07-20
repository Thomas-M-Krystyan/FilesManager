using FilesManager.Core.Services.Strategies;
using NUnit.Framework;

namespace FilesManager.Core.Tests.Services.Strategies
{
    [TestFixture]
    internal class LeadingZerosTests
    {
        [TestCaseSource(nameof(GetTestFileNames_Zeros_MaxLength))]
        public void GetLeadingZeros_ForValidInput_ReturnsExpectedChangedFileNames((string OriginalName, byte ZerosCount, ushort MaxFileLength, string ExpectedName) testData)
        {
            // Act
            string actualName = LeadingZeros.GetDigitsWithLeadingZeros(testData.OriginalName, testData.ZerosCount, testData.MaxFileLength);

            // Assert
            Assert.That(actualName, Is.EqualTo(testData.ExpectedName));
        }

        private static IEnumerable<(string OriginalName, byte ZerosCount, ushort MaxFileLength, string ExpectedName)> GetTestFileNames_Zeros_MaxLength()
        {
            yield return ("1", 2, 1, "001");
            yield return ("1", 1, 1, "01");
            yield return ("1", 0, 1, "1");
            yield return ("", 2, 0, "");
            yield return ("", 1, 0, "");
            yield return ("", 0, 0, "");
        }
    }
}
