﻿using Nikse.SubtitleEdit.Core.Common;
using Nikse.SubtitleEdit.Core.Common.TextLengthCalculator;
using Nikse.SubtitleEdit.Core.SubtitleFormats;
using System;

namespace Tests.Core
{
    [TestClass]
    public class StringExtensionsTest
    {

        [TestMethod]
        public void LineStartsWithHtmlTagEmpty()
        {
            var test = string.Empty;
            Assert.IsFalse(test.LineStartsWithHtmlTag(true));
        }

        [TestMethod]
        public void LineStartsWithHtmlTagNull()
        {
            string? test = null;
            Assert.IsFalse(test.LineStartsWithHtmlTag(true));
        }

        [TestMethod]
        public void LineStartsWithHtmlTagItalic()
        {
            const string test = "<i>";
            Assert.IsTrue(test.LineStartsWithHtmlTag(true));
        }

        [TestMethod]
        public void EndsWithEmpty()
        {
            var test = string.Empty;
            Assert.IsFalse(test.EndsWith('?'));
        }

        [TestMethod]
        public void EndsWithHtmlTagEmpty()
        {
            var test = string.Empty;
            Assert.IsFalse(test.LineEndsWithHtmlTag(true));
        }

        [TestMethod]
        public void EndsWithHtmlTagItalic()
        {
            const string test = "<i>Hej</i>";
            Assert.IsTrue(test.LineEndsWithHtmlTag(true));
        }

        [TestMethod]
        public void LineBreakStartsWithHtmlTagEmpty()
        {
            var test = string.Empty;
            Assert.IsFalse(test.LineBreakStartsWithHtmlTag(true));
        }

        [TestMethod]
        public void LineBreakStartsWithHtmlTagItalic()
        {
            var test = "<i>Hej</i>" + Environment.NewLine + "<i>Hej</i>";
            Assert.IsTrue(test.LineBreakStartsWithHtmlTag(true));
        }

        [TestMethod]
        public void LineBreakStartsWithHtmlTagFont()
        {
            var test = "Hej!" + Environment.NewLine + "<font color=FFFFFF>Hej!</font>";
            Assert.IsTrue(test.LineBreakStartsWithHtmlTag(true, true));
        }

        //QUESTION: fix three lines?
        //[TestMethod]
        //public void LineBreakStartsWithHtmlTagFontThreeLines()
        //{
        //    string test = "Hej!" + Environment.NewLine + "Hej!" + Environment.NewLine + "<font color=FFFFFF>Hej!</font>";
        //    Assert.IsTrue(test.LineBreakStartsWithHtmlTag(true, true));
        //}

        [TestMethod]
        public void LineBreakStartsWithHtmlTagFontFalse()
        {
            const string test = "<font color=FFFFFF>Hej!</font>";
            Assert.IsFalse(test.LineBreakStartsWithHtmlTag(true, true));
        }

        [TestMethod]
        public void SplitToLines1()
        {
            var input = "Line1" + Environment.NewLine + "Line2";
            Assert.AreEqual(2, input.SplitToLines().Count);
        }

        [TestMethod]
        public void SplitToLinesEmptyLines1()
        {
            var input = "\n\nLine3\r\n\r\nLine5\r";
            var res = input.SplitToLines();
            Assert.AreEqual(6, res.Count);
            Assert.AreEqual(string.Empty, res[0]);
            Assert.AreEqual(string.Empty, res[1]);
            Assert.AreEqual("Line3", res[2]);
            Assert.AreEqual(string.Empty, res[3]);
            Assert.AreEqual("Line5", res[4]);
            Assert.AreEqual(string.Empty, res[5]);
        }

        [TestMethod]
        public void SplitToLines0A0A0D()
        {
            var input = "a\n\n\rb";
            var res = input.SplitToLines();
            Assert.AreEqual(4, res.Count);
            Assert.AreEqual("a", res[0]);
            Assert.AreEqual(string.Empty, res[1]);
            Assert.AreEqual("b", res[3]);
        }

        [TestMethod]
        public void SplitToLines650D0D0A650A0A650A650D650D0A650A0D65()
        {
            var input = "e\r\r\ne\n\ne\ne\re\r\ne\n\re";
            var res = input.SplitToLines();
            Assert.AreEqual(10, res.Count);
            Assert.AreEqual("e", res[0]);
            Assert.AreEqual(string.Empty, res[1]);
            Assert.AreEqual("e", res[2]);
            Assert.AreEqual(string.Empty, res[3]);
            Assert.AreEqual("e", res[4]);
            Assert.AreEqual("e", res[5]);
            Assert.AreEqual("e", res[6]);
            Assert.AreEqual("e", res[7]);
            Assert.AreEqual(string.Empty, res[8]);
            Assert.AreEqual("e", res[9]);
        }

        [TestMethod]
        public void FixExtraSpaces()
        {
            var input = "Hallo  world!";
            var res = input.FixExtraSpaces();
            Assert.AreEqual("Hallo world!", res);
        }

        [TestMethod]
        public void FixExtraSpaces2()
        {
            var input = "Hallo   world!";
            var res = input.FixExtraSpaces();
            Assert.AreEqual("Hallo world!", res);
        }

        [TestMethod]
        public void FixExtraSpaces3()
        {
            var input = "Hallo world!  ";
            var res = input.FixExtraSpaces();
            Assert.AreEqual("Hallo world! ", res);
        }

        [TestMethod]
        public void FixExtraSpaces4()
        {
            var input = "Hallo " + Environment.NewLine + " world!";
            var res = input.FixExtraSpaces();
            Assert.AreEqual("Hallo" + Environment.NewLine + "world!", res);
        }


        [TestMethod]
        public void FixExtraSpaces5()
        {
            var input = "a  " + Environment.NewLine + "b";
            var res = input.FixExtraSpaces();
            Assert.AreEqual("a" + Environment.NewLine + "b", res);
        }

        [TestMethod]
        public void FixExtraSpaces6()
        {
            var input = "a" + Environment.NewLine + "   b";
            var res = input.FixExtraSpaces();
            Assert.AreEqual("a" + Environment.NewLine + "b", res);
        }

        [TestMethod]
        public void RemoveRecursiveLineBreakTest()
        {
            Assert.AreEqual("foo\r\nfoo", "foo\r\n\r\nfoo".RemoveRecursiveLineBreaks());
            Assert.AreEqual("foo\r\nfoo", "foo\r\nfoo".RemoveRecursiveLineBreaks());
            Assert.AreEqual("foo\r\nfoo", "foo\r\n\r\n\r\nfoo".RemoveRecursiveLineBreaks());
        }

        [TestMethod]
        public void RemoveRecursiveLineBreakNonWindowsStyleTest()
        {
            Assert.AreEqual("foo\nfoo", "foo\nfoo".RemoveRecursiveLineBreaks());
            Assert.AreEqual("foo\nfoo", "foo\n\n\nfoo".RemoveRecursiveLineBreaks());
            Assert.AreEqual("foo\n.\nfoo", "foo\n.\n\n\nfoo".RemoveRecursiveLineBreaks());
        }

        [TestMethod]
        public void RemoveChar1()
        {
            var input = "Hallo world!";
            var res = input.RemoveChar(' ');
            Assert.AreEqual("Halloworld!", res);
        }

        [TestMethod]
        public void RemoveChar2()
        {
            var input = " Hallo  world! ";
            var res = input.RemoveChar(' ');
            Assert.AreEqual("Halloworld!", res);
        }

        [TestMethod]
        public void RemoveChar3()
        {
            var input = " Hallo  world! ";
            var res = input.RemoveChar(' ', '!');
            Assert.AreEqual("Halloworld", res);
        }

        [TestMethod]
        public void RemoveChar4()
        {
            var input = " Hallo  world! ";
            var res = input.RemoveChar(' ', '!', 'H');
            Assert.AreEqual("alloworld", res);
        }

        [TestMethod]
        public void CountLetters1()
        {
            var input = " Hallo  world! ";
            var res = CalcFactory.MakeCalculator(nameof(CalcAll)).CountCharacters(input, false);
            Assert.AreEqual(" Hallo  world! ".Length, res);
        }

        [TestMethod]
        public void CountLetters2()
        {
            var input = " Hallo " + Environment.NewLine + " world! ";
            var res = CalcFactory.MakeCalculator(nameof(CalcNoSpace)).CountCharacters(input, false);
            Assert.AreEqual("Halloworld!".Length, res);
        }

        [TestMethod]
        public void CountLetters3()
        {
            var input = " Hallo" + Environment.NewLine + "world!";
            var res = CalcFactory.MakeCalculator(nameof(CalcAll)).CountCharacters(input, false);
            Assert.AreEqual(" Halloworld!".Length, res);
        }

        [TestMethod]
        public void CountLetters4Ssa()
        {
            var input = "{\\an1}Hallo";
            var res = CalcFactory.MakeCalculator(nameof(CalcAll)).CountCharacters(input, false);
            Assert.AreEqual("Hallo".Length, res);
        }

        [TestMethod]
        public void CountLetters4Html()
        {
            var input = "<i>Hallo</i>";
            var res = CalcFactory.MakeCalculator(nameof(CalcAll)).CountCharacters(input, false);
            Assert.AreEqual("Hallo".Length, res);
        }

        [TestMethod]
        public void CountLetters5HtmlFont()
        {
            var input = "<font color=\"red\"><i>Hal lo<i></font>";
            var res = CalcFactory.MakeCalculator(nameof(CalcNoSpace)).CountCharacters(input, false);
            Assert.AreEqual("Hallo".Length, res);
        }

        [TestMethod]
        public void CountLetters6HtmlFontMultiLine()
        {
            var input = "<font color=\"red\"><i>Hal lo<i></font>" + Environment.NewLine + "<i>Bye!</i>";
            var res = CalcFactory.MakeCalculator(nameof(CalcNoSpace)).CountCharacters(input, false);
            Assert.AreEqual("HalloBye!".Length, res);
        }

        [TestMethod]
        public void ToggleCasing1()
        {
            var input = "how are you";
            var res = input.ToggleCasing(new SubRip());
            Assert.AreEqual("How Are You", res);
        }

        [TestMethod]
        public void ToggleCasing1WithItalic()
        {
            var input = "how <i>are</i> you";
            var res = input.ToggleCasing(new SubRip());
            Assert.AreEqual("How <i>Are</i> You", res);
        }

        [TestMethod]
        public void ToggleCasing1WithItalicStart()
        {
            var input = "<i>how</i> are you";
            var res = input.ToggleCasing(new SubRip());
            Assert.AreEqual("<i>How</i> Are You", res);
        }

        [TestMethod]
        public void ToggleCasing1WithItalicEnd()
        {
            var input = "how are <i>you</i>";
            var res = input.ToggleCasing(new SubRip());
            Assert.AreEqual("How Are <i>You</i>", res);
        }

        [TestMethod]
        public void ToggleCasing1WithItalicEndAndBold()
        {
            var input = "how are <i><b>you</b></i>";
            var res = input.ToggleCasing(new SubRip());
            Assert.AreEqual("How Are <i><b>You</b></i>", res);
        }

        [TestMethod]
        public void ToggleCasing2()
        {
            var input = "How Are You";
            var res = input.ToggleCasing(new SubRip());
            Assert.AreEqual("HOW ARE YOU", res);
        }

        [TestMethod]
        public void ToggleCasing3()
        {
            var input = "HOW ARE YOU";
            var res = input.ToggleCasing(new SubRip());
            Assert.AreEqual("how are you", res);
        }

        [TestMethod]
        public void ToggleCasingWithFont()
        {
            var input = "<font color=\"Red\">HOW ARE YOU</font>";
            var res = input.ToggleCasing(new SubRip());
            Assert.AreEqual("<font color=\"Red\">how are you</font>", res);
        }

        [TestMethod]
        public void ToggleCasingAssa()
        {
            var input = "{\\i1}This is an example…{\\i0}";
            var res = input.ToggleCasing(new AdvancedSubStationAlpha());
            Assert.AreEqual("{\\i1}THIS IS AN EXAMPLE…{\\i0}", res);
        }

        [TestMethod]
        public void ToggleCasingAssaSoftLineBreak()
        {
            var input = "HOW ARE\\nYOU?";
            var res = input.ToggleCasing(new AdvancedSubStationAlpha());
            Assert.AreEqual("how are\\nyou?", res);
        }

        [TestMethod]
        public void ToggleCasingVoiceTag()
        {
            var input = "<v Johnny>How are you?";
            var res = input.ToggleCasing(null);
            Assert.AreEqual("<v Johnny>HOW ARE YOU?", res);
        }

        [TestMethod]
        public void ToProperCaseFromUpper()
        {
            var input = "HOW ARE YOU?";
            var res = input.ToProperCase(null);
            Assert.AreEqual("How Are You?", res);
        }

        [TestMethod]
        public void ToProperCaseFromLower()
        {
            var input = "how are you?";
            var res = input.ToProperCase(null);
            Assert.AreEqual("How Are You?", res);
        }

        [TestMethod]
        public void ToProperCaseItalic()
        {
            var input = "<i>HOW ARE YOU?</i>";
            var res = input.ToProperCase(null);
            Assert.AreEqual("<i>How Are You?</i>", res);
        }

        [TestMethod]
        public void ToLowercaseButKeepTags1()
        {
            var input = "<i>HOW ARE YOU?</i>";
            var res = input.ToLowercaseButKeepTags();
            Assert.AreEqual("<i>how are you?</i>", res);
        }

        [TestMethod]
        public void ToLowercaseButKeepTags2()
        {
            var input = "{\\c&H0000FF&}Red";
            var res = input.ToLowercaseButKeepTags();
            Assert.AreEqual("{\\c&H0000FF&}red", res);
        }

        [TestMethod]
        public void HasSentenceEndingCultureNeutralTest()
        {
            // language two letter language set to null
            Assert.IsTrue("foobar.".HasSentenceEnding(null)); // this is supposed to use the culture neutral chars

            Assert.IsTrue("foobar.".HasSentenceEnding());
            Assert.IsTrue("foobar?</font>".HasSentenceEnding());
            Assert.IsTrue("foobar!</font>".HasSentenceEnding());
            Assert.IsTrue("foobar.</font>\"".HasSentenceEnding());
            Assert.IsTrue("{\\i1}How are you?{\\i0}".HasSentenceEnding());
            Assert.IsTrue("{\\i1}How are you?{\\i0}</font>".HasSentenceEnding());
            Assert.IsTrue("{\\i1}How are you?</font>{\\i0}".HasSentenceEnding());
            Assert.IsTrue("foobar.\"".HasSentenceEnding());
            Assert.IsTrue("foobar--".HasSentenceEnding());
            Assert.IsTrue("foobar--</i>".HasSentenceEnding());
            Assert.IsTrue("foobar—".HasSentenceEnding()); // em dash
            Assert.IsTrue("foobar—</i>".HasSentenceEnding()); // em dash

            Assert.IsFalse("\"".HasSentenceEnding());
            Assert.IsFalse("foobar>".HasSentenceEnding());
            Assert.IsFalse("How are you{\\i0}".HasSentenceEnding());
            Assert.IsFalse("".HasSentenceEnding());
        }

        [TestMethod]
        public void HasSentenceEndingGreekTest()
        {
            const string greekCultureTwoLetter = "el";
            Assert.IsTrue("foobar)".HasSentenceEnding(greekCultureTwoLetter));
            Assert.IsTrue("foobar\u037E</font>\"".HasSentenceEnding(greekCultureTwoLetter));
            Assert.IsTrue("foobar؟\"".HasSentenceEnding(greekCultureTwoLetter));
            Assert.IsTrue("foobar;".HasSentenceEnding(greekCultureTwoLetter));
        }

    }
}
