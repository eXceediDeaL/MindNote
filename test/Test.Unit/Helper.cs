using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindNote.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Unit
{
    class Helper
    {
        internal static Node SampleNode()
        {
            int id = 100;
            string name = "testname", content = "testcontent";
            DateTimeOffset ct = DateTimeOffset.Now, mt = DateTimeOffset.Now;
            return new Node
            {
                Id = id,
                Name = name,
                Content = content,
                CreationTime = ct,
                ModificationTime = mt,
            };
        }

        internal static Struct SampleStruct()
        {
            int id = 100;
            string name = "testname";
            DateTimeOffset ct = DateTimeOffset.Now, mt = DateTimeOffset.Now;
            return new Struct
            {
                Id = id,
                Name = name,
                CreationTime = ct,
                ModificationTime = mt,
            };
        }

        internal static Tag SampleTag()
        {
            int id = 100;
            string name = "testname";
            return new Tag
            {
                Id = id,
                Name = name,
                Color = "black",
            };
        }

        internal static Relation SampleRelation()
        {
            int id = 100;
            return new Relation
            {
                Id = id,
                From = id,
                To = id + 1,
                Color = "black",
            };
        }

        internal static void NodeEqual(Node expected, Node real, bool ignore = false)
        {
            Assert.AreEqual(expected.Id, real.Id);
            Assert.AreEqual(expected.Name, real.Name);
            if (!ignore)
            {
                Assert.AreEqual(expected.Content, real.Content);
            }

            Assert.AreEqual(expected.Tags?.Length, real.Tags?.Length);
            for (int i = 0; i < expected.Tags?.Length; i++)
            {
                Assert.AreEqual(expected.Tags[i], real.Tags[i]);
            }
        }

        internal static void RelationEqual(Relation expected, Relation real, bool ignore = false)
        {
            Assert.AreEqual(expected.Id, real.Id);
            Assert.AreEqual(expected.Color, real.Color);
            Assert.AreEqual(expected.From, real.From);
            Assert.AreEqual(expected.To, real.To);
        }

        internal static void TagEqual(Tag expected, Tag real, bool ignore = false)
        {
            Assert.AreEqual(expected.Id, real.Id);
            Assert.AreEqual(expected.Color, real.Color);
            Assert.AreEqual(expected.Name, real.Name);
        }

        internal static void StructEqual(Struct expected, Struct real, bool ignore = false)
        {
            Assert.AreEqual(expected.Id, real.Id);
            Assert.AreEqual(expected.Name, real.Name);
            Assert.AreEqual(expected.Tags?.Length, real.Tags?.Length);
            for (int i = 0; i < expected.Tags?.Length; i++)
            {
                Assert.AreEqual(expected.Tags[i], real.Tags[i]);
            }
            if (!ignore)
            {
                Assert.AreEqual(expected.Extra, real.Extra);
                Assert.AreEqual(expected.Relations?.Length, real.Relations?.Length);
                for (int i = 0; i < expected.Relations?.Length; i++)
                {
                    Assert.AreEqual(expected.Relations[i], real.Relations[i]);
                }
            }
        }
    }
}
