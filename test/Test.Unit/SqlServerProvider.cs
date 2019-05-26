using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MindNote.Data;
using MindNote.Data.Providers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Test.Unit
{
    [TestClass]
    public class SqlServerProvider
    {
        Node SampleNode()
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

        Struct SampleStruct()
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

        void NodeEqual(Node expected, Node real)
        {
            Assert.AreEqual(expected.Id, expected.Id);
            Assert.AreEqual(expected.Name, expected.Name);
            Assert.AreEqual(expected.Content, expected.Content);
            Assert.AreEqual(expected.CreationTime, expected.CreationTime);
            Assert.AreEqual(expected.ModificationTime, expected.ModificationTime);

            Assert.AreEqual(expected.Tags?.Length, expected.Tags?.Length);
            for (int i = 0; i < expected.Tags?.Length; i++)
            {
                Assert.AreEqual(expected.Tags[i], expected.Tags[i]);
            }
        }

        void StructEqual(Struct expected, Struct real)
        {
            Assert.AreEqual(expected.Id, real.Id);
            Assert.AreEqual(expected.Name, real.Name);
            Assert.AreEqual(expected.CreationTime, real.CreationTime);
            Assert.AreEqual(expected.ModificationTime, real.ModificationTime);
            Assert.AreEqual(expected.Tags?.Length, real.Tags?.Length);
            for (int i = 0; i < expected.Tags?.Length; i++)
            {
                Assert.AreEqual(expected.Tags[i], real.Tags[i]);
            }
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

        [TestMethod]
        public void Node()
        {
            var tn = SampleNode();

            var ctn = MindNote.Data.Providers.SqlServer.Models.Node.FromModel(tn).ToModel();

            NodeEqual(tn,ctn);
        }

        [TestMethod]
        public void Struct()
        {
            var tn = SampleStruct();
            var ctn = MindNote.Data.Providers.SqlServer.Models.Struct.FromModel(tn).ToModel();

            StructEqual(tn, ctn);
        }
    }
}
