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
            string[] tags = new string[] { name };
            return new Node
            {
                Id = id,
                Name = name,
                Content = content,
                CreationTime = ct,
                ModificationTime = mt,
                Tags = tags,
            };
        }

        internal static Struct SampleStruct()
        {
            int id = 100;
            string name = "testname";
            DateTimeOffset ct = DateTimeOffset.Now, mt = DateTimeOffset.Now;
            string[] tags = new string[] { name };
            Relation[] rs = new Relation[]
            {
                new Relation(new int[]{1,2,3}),
                new Relation(new int[]{4,5,6})
            };
            return new Struct
            {
                Id = id,
                Name = name,
                Data = rs,
                CreationTime = ct,
                ModificationTime = mt,
                Tags = tags,
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
                Assert.AreEqual(expected.Data?.Length, real.Data?.Length);
                for (int i = 0; i < expected.Data?.Length; i++)
                {
                    Assert.AreEqual(expected.Data[i].Ids?.Length, real.Data[i].Ids?.Length);
                    for (int j = 0; j < expected.Data[i].Ids?.Length; j++)
                    {
                        Assert.AreEqual(expected.Data[i].Ids[j], real.Data[i].Ids[j]);
                    }
                }
            }
        }
    }
}
