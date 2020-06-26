using System;
using System.IO;
using System.Text.RegularExpressions;
using Homework_RegEx;
using NSubstitute;
using Xunit;

namespace HomeworkReg_Ex
{
    public class ProgramTests
    {
        Program program = new Program();
        string testText = "http://www.dowcipy.pl https://www.dowcipy.pl/k/blondynki/ http://www.peakware.com";
        [Fact]
        public void MatchWithDomainRegex_ValidInput_ResultsFound()
        {
            MatchCollection matchCollection = program.MatchWithDomainRegex(testText);

            Assert.Equal(3, matchCollection.Count);
        }
        [Fact]
        public void GetDictionaryFromMatches_ValidInput_ValidResult()
        {
            MatchCollection matchCollection = Regex.Matches(testText, @"(?:[a-z0-9](?:[a-z0-9-]{0,61}[a-z0-9])?\.)+[a-z0-9][a-z0-9-]{0,61}[a-z0-9]");

            var testDictionary = program.GetDictionaryFromMatches(matchCollection);

            testDictionary.TryGetValue("www.dowcipy.pl", out int value);
            Assert.Equal(2, value);
        }
        [Fact]
        public void LoadFile_TargetNonExistingFile_ExceptionThrown()
        {
            Assert.Throws<FileNotFoundException>(() => program.LoadFile("nonExistingPathToFile"));
        }
        [Fact]
        public void LoadFile_EmptyPathString_ExceptionThrown()
        {
            Assert.Throws<ArgumentException>(() => program.LoadFile(""));
        }
    }
}
