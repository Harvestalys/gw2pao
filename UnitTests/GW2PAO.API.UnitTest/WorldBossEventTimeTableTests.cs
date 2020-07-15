﻿using System;
using System.IO;
using System.Linq;
using GW2PAO.API.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GW2PAO.API.UnitTest
{
    [TestClass]
    public class WorldBossEventTimeTableTests
    {
        private static readonly string renamedFilename = "renamedFile.xml";

        [ClassInitialize]
        public static void Init(TestContext context)
        {
            if (File.Exists(WorldBossEventTimeTable.StandardFilename))
                File.Delete(WorldBossEventTimeTable.StandardFilename);
            if (File.Exists(WorldBossEventTimeTable.AdjustedFilename))
                File.Delete(WorldBossEventTimeTable.AdjustedFilename);
            if (File.Exists(renamedFilename))
                File.Delete(renamedFilename);

            File.Copy(Path.Combine("TestResources", WorldBossEventTimeTable.StandardFilename), WorldBossEventTimeTable.StandardFilename);
            File.Copy(Path.Combine("TestResources", WorldBossEventTimeTable.AdjustedFilename), WorldBossEventTimeTable.AdjustedFilename);
        }

        [ClassCleanup]
        public static void Cleanup()
        {
            File.Delete(WorldBossEventTimeTable.StandardFilename);
            File.Delete(WorldBossEventTimeTable.AdjustedFilename);
            File.Delete(renamedFilename);
        }

        [TestMethod]
        public void MegaserverEventTimeTable_Constructor()
        {
            WorldBossEventTimeTable mett = new WorldBossEventTimeTable();
            Assert.IsNotNull(mett);
            Assert.IsNotNull(mett.WorldEvents);
        }

        [TestMethod]
        public void MegaserverEventTimeTable_LoadTable_Standard_Success()
        {
            WorldBossEventTimeTable mett = WorldBossEventTimeTable.LoadTable(false);
            Assert.IsNotNull(mett);
            Assert.IsNotNull(mett.WorldEvents);
            Assert.IsTrue(mett.WorldEvents.Count > 0);
        }

        [TestMethod]
        public void MegaserverEventTimeTable_LoadTable_Adjusted_Success()
        {
            WorldBossEventTimeTable mett = WorldBossEventTimeTable.LoadTable(true);
            Assert.IsNotNull(mett);
            Assert.IsNotNull(mett.WorldEvents);
            Assert.IsTrue(mett.WorldEvents.Count > 0);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void MegaserverEventTimeTable_LoadTable_Standard_MissingFile()
        {
            File.Move(WorldBossEventTimeTable.StandardFilename, renamedFilename);
            try
            {
                WorldBossEventTimeTable.LoadTable(false);
            }
            finally
            {
                File.Move(renamedFilename, WorldBossEventTimeTable.StandardFilename);
            }
        }

        [TestMethod]

        [ExpectedException(typeof(FileNotFoundException))]
        public void MegaserverEventTimeTable_LoadTable_Adjusted_MissingFile()
        {
            File.Move(WorldBossEventTimeTable.AdjustedFilename, renamedFilename);
            try
            {
                WorldBossEventTimeTable.LoadTable(true);
            }
            finally
            {
                File.Move(renamedFilename, WorldBossEventTimeTable.AdjustedFilename);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void MegaserverEventTimeTable_LoadTable_Standard_InvalidFile()
        {
            File.Move(WorldBossEventTimeTable.StandardFilename, renamedFilename);
            File.WriteAllText(WorldBossEventTimeTable.StandardFilename, "invalid data");

            try
            {
                WorldBossEventTimeTable.LoadTable(false);
            }
            finally
            {
                File.Delete(WorldBossEventTimeTable.StandardFilename);
                File.Move(renamedFilename, WorldBossEventTimeTable.StandardFilename);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void MegaserverEventTimeTable_LoadTable_Adjusted_InvalidFile()
        {
            File.Move(WorldBossEventTimeTable.AdjustedFilename, renamedFilename);
            File.WriteAllText(WorldBossEventTimeTable.AdjustedFilename, "invalid data");

            try
            {
                WorldBossEventTimeTable.LoadTable(true);
            }
            finally
            {
                File.Delete(WorldBossEventTimeTable.AdjustedFilename);
                File.Move(renamedFilename, WorldBossEventTimeTable.AdjustedFilename);
            }
        }

        [TestMethod]
        public void MegaserverEventTimeTable_CreateTable_Standard_Success()
        {
            File.Move(WorldBossEventTimeTable.StandardFilename, renamedFilename);
            try
            {
                WorldBossEventTimeTable.CreateTable(false);
                Assert.IsTrue(File.Exists(WorldBossEventTimeTable.StandardFilename));
            }
            finally
            {
                if (File.Exists(WorldBossEventTimeTable.StandardFilename))
                    File.Delete(WorldBossEventTimeTable.StandardFilename);
                File.Move(renamedFilename, WorldBossEventTimeTable.StandardFilename);
            }
        }

        [TestMethod]
        public void MegaserverEventTimeTable_CreateTable_Adjusted_Success()
        {
            File.Move(WorldBossEventTimeTable.AdjustedFilename, renamedFilename);
            try
            {
                WorldBossEventTimeTable.CreateTable(true);
                Assert.IsTrue(File.Exists(WorldBossEventTimeTable.AdjustedFilename));
            }
            finally
            {
                if (File.Exists(WorldBossEventTimeTable.AdjustedFilename))
                    File.Delete(WorldBossEventTimeTable.AdjustedFilename);
                File.Move(renamedFilename, WorldBossEventTimeTable.AdjustedFilename);
            }
        }

        [TestMethod]
        public void MegaserverEventTimeTable_LoadTable_Standard_UniqueTimes()
        {
            WorldBossEventTimeTable mett = WorldBossEventTimeTable.LoadTable(false);

            foreach (var worldEvent in mett.WorldEvents)
            {
                Assert.AreEqual(worldEvent.ActiveTimes.Count, worldEvent.ActiveTimes.GroupBy(t => t.XmlTime).Select(at => at.First()).ToList().Count);
            }
        }

        [TestMethod]
        public void MegaserverEventTimeTable_LoadTable_Adjusted_UniqueTimes()
        {
            WorldBossEventTimeTable mett = WorldBossEventTimeTable.LoadTable(true);

            foreach (var worldEvent in mett.WorldEvents)
            {
                Assert.AreEqual(worldEvent.ActiveTimes.Count, worldEvent.ActiveTimes.GroupBy(t => t.XmlTime).Select(at => at.First()).ToList().Count);
            }
        }
    }
}
