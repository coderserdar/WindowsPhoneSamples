// ------------------------------------------------------------------------------
// Copyright 2014 Microsoft Corporation
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ------------------------------------------------------------------------------

namespace Microsoft.Live.UnitTests
{
    using System;
    using System.IO;

#if NETFX_CORE
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#elif WINDOWS_PHONE || DESKTOP || WEB
    using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
#error This platform needs to be handled.
#endif

    using Microsoft.Live.Serialization;

    [TestClass]
    public class IndentedTextWriterTest
    {
        [TestMethod]
        public void TestWriteString()
        {
            var stringWriter = new StringWriter();
            var indentedTextWriter = new IndentedTextWriter(stringWriter, true);

            indentedTextWriter.Write("foo");
            indentedTextWriter.Indent = 1;
            indentedTextWriter.Write("bar");

            Assert.AreEqual("foobar", stringWriter.ToString());
        }

        [TestMethod]
        public void TestWriteBoolean()
        {
            var stringWriter = new StringWriter();
            var indentedTextWriter = new IndentedTextWriter(stringWriter, true);

            indentedTextWriter.Write(true);
            indentedTextWriter.Indent = 1;
            indentedTextWriter.Write(false);

            Assert.AreEqual("TrueFalse", stringWriter.ToString());
        }

        [TestMethod]
        public void TestWriteChar()
        {
            var stringWriter = new StringWriter();
            var indentedTextWriter = new IndentedTextWriter(stringWriter, true);

            indentedTextWriter.Write('f');
            indentedTextWriter.Indent = 1;
            indentedTextWriter.Write('b');

            Assert.AreEqual("fb", stringWriter.ToString());
        }

        [TestMethod]
        public void TestWriteCharArray()
        {
            var stringWriter = new StringWriter();
            var indentedTextWriter = new IndentedTextWriter(stringWriter, true);

            indentedTextWriter.Write(new char[]{'f', 'o', 'o'});
            indentedTextWriter.Indent = 1;
            indentedTextWriter.Write(new char[]{'b', 'a', 'r'});

            Assert.AreEqual("foobar", stringWriter.ToString());
        }

        [TestMethod]
        public void TestWriteCharArrayIndexed()
        {
            var stringWriter = new StringWriter();
            var indentedTextWriter = new IndentedTextWriter(stringWriter, true);

            indentedTextWriter.Write(new char[]{'f', 'o', 'o', 'b', 'a', 'r'}, 0, 3);
            indentedTextWriter.Indent = 1;
            indentedTextWriter.Write(new char[]{'f', 'o', 'o', 'b', 'a', 'r'}, 3, 3);

            Assert.AreEqual("foobar", stringWriter.ToString());
        }

        [TestMethod]
        public void TestWriteDouble()
        {
            var stringWriter = new StringWriter();
            var indentedTextWriter = new IndentedTextWriter(stringWriter, true);

            indentedTextWriter.Write(0.5d);
            indentedTextWriter.Indent = 1;
            indentedTextWriter.Write(-0.5d);

            Assert.AreEqual("0.5-0.5", stringWriter.ToString());
        }

        [TestMethod]
        public void TestWriteFloat()
        {
            var stringWriter = new StringWriter();
            var indentedTextWriter = new IndentedTextWriter(stringWriter, true);

            indentedTextWriter.Write(0.5f);
            indentedTextWriter.Indent = 1;
            indentedTextWriter.Write(-0.5f);

            Assert.AreEqual("0.5-0.5", stringWriter.ToString());
        }

        [TestMethod]
        public void TestWriteInt()
        {
            var stringWriter = new StringWriter();
            var indentedTextWriter = new IndentedTextWriter(stringWriter, true);

            indentedTextWriter.Write(10);
            indentedTextWriter.Indent = 1;
            indentedTextWriter.Write(-10);

            Assert.AreEqual("10-10", stringWriter.ToString());
        }

        [TestMethod]
        public void TestWriteLong()
        {
            var stringWriter = new StringWriter();
            var indentedTextWriter = new IndentedTextWriter(stringWriter, true);

            indentedTextWriter.Write(10L);
            indentedTextWriter.Indent = 1;
            indentedTextWriter.Write(-10L);

            Assert.AreEqual("10-10", stringWriter.ToString());
        }

        [TestMethod]
        public void TestWriteObject()
        {
            var stringWriter = new StringWriter();
            var indentedTextWriter = new IndentedTextWriter(stringWriter, true);

            indentedTextWriter.Write((object)"foo");
            indentedTextWriter.Indent = 1;
            indentedTextWriter.Write((object)"bar");

            Assert.AreEqual("foobar", stringWriter.ToString());
        }

        [TestMethod]
        public void TestWriteFormatString()
        {
            var stringWriter = new StringWriter();
            var indentedTextWriter = new IndentedTextWriter(stringWriter, true);

            indentedTextWriter.Write("foo {0}", "bar");
            indentedTextWriter.Indent = 1;
            indentedTextWriter.Write("foo {0}", "bar");

            Assert.AreEqual("foo barfoo bar", stringWriter.ToString());
        }

        [TestMethod]
        public void TestWriteUnsignedInt()
        {
            var stringWriter = new StringWriter();
            var indentedTextWriter = new IndentedTextWriter(stringWriter, true);

            indentedTextWriter.Write(10u);
            indentedTextWriter.Indent = 1;
            indentedTextWriter.Write(10u);

            Assert.AreEqual("1010", stringWriter.ToString());
        }

        [TestMethod]
        public void TestWriteLineNoTabs()
        {
            var stringWriter = new StringWriter();
            var indentedTextWriter = new IndentedTextWriter(stringWriter, false);

            indentedTextWriter.WriteLineNoTabs("foo");
            indentedTextWriter.Indent = 1;
            indentedTextWriter.WriteLineNoTabs("bar");

            Assert.AreEqual("foo\rbar\r", stringWriter.ToString());
        }

        [TestMethod]
        public void TestWriteLineNoTabsMinimized()
        {
            var stringWriter = new StringWriter();
            var indentedTextWriter = new IndentedTextWriter(stringWriter, true);

            indentedTextWriter.WriteLineNoTabs("foo");
            indentedTextWriter.Indent = 1;
            indentedTextWriter.WriteLineNoTabs("bar");

            Assert.AreEqual("foobar", stringWriter.ToString());
        }

        [TestMethod]
        public void TestWriteLineString()
        {
            var stringWriter = new StringWriter();
            var indentedTextWriter = new IndentedTextWriter(stringWriter, false);

            indentedTextWriter.WriteLine("foo");
            indentedTextWriter.Indent = 1;
            indentedTextWriter.Write("bar");

            Assert.AreEqual("foo\r   bar", stringWriter.ToString());
        }

        [TestMethod]
        public void TestWriteLineStringMinimized()
        {
            var stringWriter = new StringWriter();
            var indentedTextWriter = new IndentedTextWriter(stringWriter, true);

            indentedTextWriter.WriteLine("foo");
            indentedTextWriter.Indent = 1;
            indentedTextWriter.Write("bar");

            Assert.AreEqual("foobar", stringWriter.ToString());
        }

        [TestMethod]
        public void TestWriteLineBoolean()
        {
            var stringWriter = new StringWriter();
            var indentedTextWriter = new IndentedTextWriter(stringWriter, false);

            indentedTextWriter.WriteLine(true);
            indentedTextWriter.Indent = 1;
            indentedTextWriter.Write(false);

            Assert.AreEqual("True\r   False", stringWriter.ToString());
        }

        [TestMethod]
        public void TestWriteLineBooleanMinimized()
        {
            var stringWriter = new StringWriter();
            var indentedTextWriter = new IndentedTextWriter(stringWriter, true);

            indentedTextWriter.WriteLine(true);
            indentedTextWriter.Indent = 1;
            indentedTextWriter.Write(false);

            Assert.AreEqual("TrueFalse", stringWriter.ToString());
        }

        [TestMethod]
        public void TestWriteLineChar()
        {
            var stringWriter = new StringWriter();
            var indentedTextWriter = new IndentedTextWriter(stringWriter, false);

            indentedTextWriter.WriteLine('f');
            indentedTextWriter.Indent = 1;
            indentedTextWriter.Write('b');

            Assert.AreEqual("f\r   b", stringWriter.ToString());
        }

        [TestMethod]
        public void TestWriteLineCharMinimized()
        {
            var stringWriter = new StringWriter();
            var indentedTextWriter = new IndentedTextWriter(stringWriter, true);

            indentedTextWriter.WriteLine('f');
            indentedTextWriter.Indent = 1;
            indentedTextWriter.Write('b');

            Assert.AreEqual("fb", stringWriter.ToString());
        }

        [TestMethod]
        public void TestWriteLineCharBuffer()
        {
            var stringWriter = new StringWriter();
            var indentedTextWriter = new IndentedTextWriter(stringWriter, false);

            indentedTextWriter.WriteLine(new char[]{'f', 'o', 'o'});
            indentedTextWriter.Indent = 1;
            indentedTextWriter.Write(new char[] {'b', 'a', 'r'});

            Assert.AreEqual("foo\r   bar", stringWriter.ToString());
        }

        [TestMethod]
        public void TestWriteLineCharBufferMinimized()
        {
            var stringWriter = new StringWriter();
            var indentedTextWriter = new IndentedTextWriter(stringWriter, true);

            indentedTextWriter.WriteLine(new char[]{'f', 'o', 'o'});
            indentedTextWriter.Indent = 1;
            indentedTextWriter.Write(new char[] {'b', 'a', 'r'});

            Assert.AreEqual("foobar", stringWriter.ToString());
        }

        [TestMethod]
        public void TestWriteLineCharBufferIndexed()
        {
            var stringWriter = new StringWriter();
            var indentedTextWriter = new IndentedTextWriter(stringWriter, false);

            indentedTextWriter.WriteLine(new char[]{'f', 'o', 'o', 'b', 'a', 'r'}, 0, 3);
            indentedTextWriter.Indent = 1;
            indentedTextWriter.Write(new char[] {'f', 'o', 'o', 'b', 'a', 'r'}, 3, 3);

            Assert.AreEqual("foo\r   bar", stringWriter.ToString());
        }

        [TestMethod]
        public void TestWriteLineCharBufferIndexedMinimized()
        {
            var stringWriter = new StringWriter();
            var indentedTextWriter = new IndentedTextWriter(stringWriter, true);

            indentedTextWriter.WriteLine(new char[]{'f', 'o', 'o', 'b', 'a', 'r'}, 0, 3);
            indentedTextWriter.Indent = 1;
            indentedTextWriter.Write(new char[] {'f', 'o', 'o', 'b', 'a', 'r'}, 3, 3);

            Assert.AreEqual("foobar", stringWriter.ToString());
        }

        [TestMethod]
        public void TestWriteLineDouble()
        {
            var stringWriter = new StringWriter();
            var indentedTextWriter = new IndentedTextWriter(stringWriter, false);

            indentedTextWriter.WriteLine(0.5d);
            indentedTextWriter.Indent = 1;
            indentedTextWriter.Write(-0.5d);

            Assert.AreEqual("0.5\r   -0.5", stringWriter.ToString());
        }

        [TestMethod]
        public void TestWriteLineDoubleMinimized()
        {
            var stringWriter = new StringWriter();
            var indentedTextWriter = new IndentedTextWriter(stringWriter, true);

            indentedTextWriter.WriteLine(0.5d);
            indentedTextWriter.Indent = 1;
            indentedTextWriter.Write(-0.5d);

            Assert.AreEqual("0.5-0.5", stringWriter.ToString());
        }

        [TestMethod]
        public void TestWriteLineFloat()
        {
            var stringWriter = new StringWriter();
            var indentedTextWriter = new IndentedTextWriter(stringWriter, false);

            indentedTextWriter.WriteLine(0.5f);
            indentedTextWriter.Indent = 1;
            indentedTextWriter.Write(-0.5f);

            Assert.AreEqual("0.5\r   -0.5", stringWriter.ToString());
        }

        [TestMethod]
        public void TestWriteLineFloatMinimized()
        {
            var stringWriter = new StringWriter();
            var indentedTextWriter = new IndentedTextWriter(stringWriter, true);

            indentedTextWriter.WriteLine(0.5f);
            indentedTextWriter.Indent = 1;
            indentedTextWriter.Write(-0.5f);

            Assert.AreEqual("0.5-0.5", stringWriter.ToString());
        }

        [TestMethod]
        public void TestWriteLineInt()
        {
            var stringWriter = new StringWriter();
            var indentedTextWriter = new IndentedTextWriter(stringWriter, false);

            indentedTextWriter.WriteLine(10);
            indentedTextWriter.Indent = 1;
            indentedTextWriter.Write(-10);

            Assert.AreEqual("10\r   -10", stringWriter.ToString());
        }

        [TestMethod]
        public void TestWriteLineIntMinimized()
        {
            var stringWriter = new StringWriter();
            var indentedTextWriter = new IndentedTextWriter(stringWriter, true);

            indentedTextWriter.WriteLine(10);
            indentedTextWriter.Indent = 1;
            indentedTextWriter.Write(-10);

            Assert.AreEqual("10-10", stringWriter.ToString());
        }

        [TestMethod]
        public void TestWriteLineLong()
        {
            var stringWriter = new StringWriter();
            var indentedTextWriter = new IndentedTextWriter(stringWriter, false);

            indentedTextWriter.WriteLine(10L);
            indentedTextWriter.Indent = 1;
            indentedTextWriter.Write(-10L);

            Assert.AreEqual("10\r   -10", stringWriter.ToString());
        }

        [TestMethod]
        public void TestWriteLineLongMinimized()
        {
            var stringWriter = new StringWriter();
            var indentedTextWriter = new IndentedTextWriter(stringWriter, true);

            indentedTextWriter.WriteLine(10L);
            indentedTextWriter.Indent = 1;
            indentedTextWriter.Write(-10L);

            Assert.AreEqual("10-10", stringWriter.ToString());
        }

        [TestMethod]
        public void TestWriteLineObject()
        {
            var stringWriter = new StringWriter();
            var indentedTextWriter = new IndentedTextWriter(stringWriter, false);

            indentedTextWriter.WriteLine((object) "foo");
            indentedTextWriter.Indent = 1;
            indentedTextWriter.Write((object) "bar");

            Assert.AreEqual("foo\r   bar", stringWriter.ToString());
        }

        [TestMethod]
        public void TestWriteLineObjectMinimized()
        {
            var stringWriter = new StringWriter();
            var indentedTextWriter = new IndentedTextWriter(stringWriter, true);

            indentedTextWriter.WriteLine((object) "foo");
            indentedTextWriter.Indent = 1;
            indentedTextWriter.Write((object) "bar");

            Assert.AreEqual("foobar", stringWriter.ToString());
        }

        [TestMethod]
        public void TestWriteLineStringFormat()
        {
            var stringWriter = new StringWriter();
            var indentedTextWriter = new IndentedTextWriter(stringWriter, false);

            indentedTextWriter.WriteLine("foo {0}", "bar");
            indentedTextWriter.Indent = 1;
            indentedTextWriter.Write("{0} bar", "foo");

            Assert.AreEqual("foo bar\r   foo bar", stringWriter.ToString());
        }

        [TestMethod]
        public void TestWriteLineStringFormatMinimized()
        {
            var stringWriter = new StringWriter();
            var indentedTextWriter = new IndentedTextWriter(stringWriter, true);

            indentedTextWriter.WriteLine("foo {0}", "bar");
            indentedTextWriter.Indent = 1;
            indentedTextWriter.Write("{0} bar", "foo");

            Assert.AreEqual("foo barfoo bar", stringWriter.ToString());
        }

        [TestMethod]
        public void TestWriteLineUnsignedInt()
        {
            var stringWriter = new StringWriter();
            var indentedTextWriter = new IndentedTextWriter(stringWriter, false);

            indentedTextWriter.WriteLine(10u);
            indentedTextWriter.Indent = 1;
            indentedTextWriter.Write(10u);

            Assert.AreEqual("10\r   10", stringWriter.ToString());
        }

        [TestMethod]
        public void TestWriteLineUnsignedIntMinimized()
        {
            var stringWriter = new StringWriter();
            var indentedTextWriter = new IndentedTextWriter(stringWriter, true);

            indentedTextWriter.WriteLine(10u);
            indentedTextWriter.Indent = 1;
            indentedTextWriter.Write(10u);

            Assert.AreEqual("1010", stringWriter.ToString());
        }

        [TestMethod]
        public void TestWriteTrimmed()
        {
            var stringWriter = new StringWriter();
            var indentedTextWriter = new IndentedTextWriter(stringWriter, false);

            indentedTextWriter.WriteTrimmed("   foo   ");

            Assert.AreEqual("   foo   ", stringWriter.ToString());
        }

        [TestMethod]
        public void TestWriteTrimmedMinimized()
        {
            var stringWriter = new StringWriter();
            var indentedTextWriter = new IndentedTextWriter(stringWriter, true);

            indentedTextWriter.WriteTrimmed("   foo   ");

            Assert.AreEqual("foo", stringWriter.ToString());
        }
    }
}
