﻿using System;
using System.Collections.Generic;
using System.Linq;
using CreateAndFake;
using CreateAndFake.Toolbox.FakerTool;
using CreateAndFake.Toolbox.ValuerTool;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CreateAndFakeTests.Toolbox.ValuerTool
{
    /// <summary>Verifies behavior.</summary>
    [TestClass]
    public sealed class ValuerTests
    {
        /// <summary>Verifies nulls are valid.</summary>
        [TestMethod]
        public void New_NullHintsValid()
        {
            Tools.Asserter.IsNot(null, new Valuer(true, null));
            Tools.Asserter.IsNot(null, new Valuer(false, null));
        }

        /// <summary>Verifies an exception throws when no hint matches.</summary>
        [TestMethod]
        public void GetHashCode_MissingMatchThrows()
        {
            Tools.Asserter.Throws<NotSupportedException>(
                () => new Valuer(false).GetHashCode((object)null));

            Tools.Asserter.Throws<NotSupportedException>(
                () => new Valuer(false).GetHashCode(new object()));
        }

        /// <summary>Verifies multiple items are combined.</summary>
        [TestMethod]
        public void GetHashCode_SupportsMultiple()
        {
            int[] data = new[] { Tools.Randomizer.Create<int>(), Tools.Randomizer.Create<int>() };

            Tools.Asserter.Is(Tools.Valuer.GetHashCode(data), Tools.Valuer.GetHashCode(data[0], data[1]));
        }

        /// <summary>Verifies hints generate the hashes.</summary>
        [TestMethod]
        public void GetHashCode_ValidHint()
        {
            object data = new object();
            int result = Tools.Randomizer.Create<int>();

            Fake<CompareHint> hint = Tools.Faker.Mock<CompareHint>();
            hint.Setup("Supports",
                new[] { data, data, Arg.LambdaAny<ValuerChainer>() },
                Behavior.Returns(true, Times.Once));
            hint.Setup("GetHashCode",
                new[] { data, Arg.LambdaAny<ValuerChainer>() },
                Behavior.Returns(result, Times.Once));

            Tools.Asserter.Is(result, new Valuer(false, hint.Dummy).GetHashCode(data));
            hint.Verify(Times.Exactly(2));
        }

        /// <summary>Verifies an exception throws when no hint matches.</summary>
        [TestMethod]
        public void Compare_MissingMatchThrows()
        {
            Tools.Asserter.Throws<NotSupportedException>(
                () => new Valuer(false).Compare(null, new object()));

            Tools.Asserter.Throws<NotSupportedException>(
                () => new Valuer(false).Compare(new object(), new object()));
        }

        /// <summary>Verifies no differences when the same reference is passed.</summary>
        [TestMethod]
        public void Compare_ReferenceNoDifferences()
        {
            object data = new object();
            Tools.Asserter.IsEmpty(new Valuer(false).Compare(data, data));
        }

        /// <summary>Verifies no differences means equal.</summary>
        [TestMethod]
        public void Equals_NoDifferencesTrue()
        {
            object data1 = new object();
            object data2 = new object();

            Fake<CompareHint> hint = Tools.Faker.Mock<CompareHint>();
            hint.Setup("Supports",
                new[] { data1, data2, Arg.LambdaAny<ValuerChainer>() },
                Behavior.Returns(true, Times.Once));
            hint.Setup("Compare",
                new[] { data1, data2, Arg.LambdaAny<ValuerChainer>() },
                Behavior.Returns(Enumerable.Empty<Difference>(), Times.Once));

            Tools.Asserter.Is(true, new Valuer(false, hint.Dummy).Equals(data1, data2));

            hint.Verify(Times.Exactly(2));
        }

        /// <summary>Verifies differences means unequal.</summary>
        [TestMethod]
        public void Equals_DifferencesFalse()
        {
            object data1 = new object();
            object data2 = new object();

            Fake<CompareHint> hint = Tools.Faker.Mock<CompareHint>();
            hint.Setup("Supports",
                new[] { data1, data2, Arg.LambdaAny<ValuerChainer>() },
                Behavior.Returns(true, Times.Once));
            hint.Setup("Compare",
                new[] { data1, data2, Arg.LambdaAny<ValuerChainer>() },
                Behavior.Returns(Tools.Randomizer.Create<IEnumerable<Difference>>(), Times.Once));

            Tools.Asserter.Is(false, new Valuer(false, hint.Dummy).Equals(data1, data2));

            hint.Verify(Times.Exactly(2));
        }
    }
}
