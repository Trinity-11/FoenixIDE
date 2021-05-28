using FoenixIDE.GameGenerator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace FoenixIDETester
{
    [TestClass]
    public class LexerTests
    {
        [TestMethod]
        public void CodeDetectionTest()
        {
            string code = @"ASSET Test '1234'
                MY_FUNCTION
                {
                    there is code in here
                }
            ";
            List<TokenMatch> matches = new List<TokenMatch>();
            TokenDefinition td = new TokenDefinition(TokenType.SUB, @"(\S+)\s*{([^}]*)}", 1);
            matches.AddRange(td.FindMatches(code));
            Assert.IsTrue(matches.Count == 1);
        }
    }
}
