﻿using System;
using System.Collections.Generic;
using System.Linq;
using CreateAndFake;
using CreateAndFake.Toolbox.FakerTool;
using CreateAndFake.Toolbox.ValuerTool;
using Xunit;

namespace CreateAndFakeTests.Toolbox.ValuerTool
{
    /// <summary>Verifies behavior.</summary>
    public static class ValuerTests
    {
        /// <summary>Verifies null reference exceptions are prevented.</summary>
        [Fact]
        public static void Valuer_GuardsNulls()
        {
            Tools.Tester.PreventsNullRefException(Tools.Valuer);
        }

        /// <summary>Verifies parameters are not mutated.</summary>
        [Fact]
        public static void Valuer_NoParameterMutation()
        {
            Tools.Tester.PreventsParameterMutation(Tools.Valuer);
        }

        /// <summary>Verifies nulls are valid.</summary>
        [Fact]
        public static void New_NullHintsValid()
        {
            Tools.Asserter.IsNot(null, new Valuer(true, null));
            Tools.Asserter.IsNot(null, new Valuer(false, null));
        }

        /// <summary>Verifies an exception throws when no hint matches.</summary>
        [Fact]
        public static void GetHashCode_MissingMatchThrows()
        {
            Tools.Asserter.Throws<NotSupportedException>(
                () => new Valuer(false).GetHashCode((object)null));

            Tools.Asserter.Throws<NotSupportedException>(
                () => new Valuer(false).GetHashCode(new object()));
        }

        /// <summary>Verifies multiple items are combined.</summary>
        [Fact]
        public static void GetHashCode_SupportsMultiple()
        {
            int[] data = new[] { Tools.Randomizer.Create<int>(), Tools.Randomizer.Create<int>() };

            Tools.Asserter.Is(Tools.Valuer.GetHashCode(data), Tools.Valuer.GetHashCode(data[0], data[1]));
        }

        /// <summary>Verifies hints generate the hashes.</summary>
        [Theory, RandomData]
        public static void GetHashCode_ValidHint(object data, int result, Fake<CompareHint> hint)
        {
            hint.Setup("Supports",
                new[] { data, data, Arg.LambdaAny<ValuerChainer>() },
                Behavior.Returns(true, Times.Once));
            hint.Setup("GetHashCode",
                new[] { data, Arg.LambdaAny<ValuerChainer>() },
                Behavior.Returns(result, Times.Once));

            Tools.Asserter.Is(result, new Valuer(false, hint.Dummy).GetHashCode(data));
            hint.VerifyAll(Times.Exactly(2));
        }

        /// <summary>Verifies an exception throws when no hint matches.</summary>
        [Fact]
        public static void Compare_MissingMatchThrows()
        {
            Tools.Asserter.Throws<NotSupportedException>(
                () => new Valuer(false).Compare(null, new object()));

            Tools.Asserter.Throws<NotSupportedException>(
                () => new Valuer(false).Compare(new object(), new object()));
        }

        /// <summary>Verifies no differences when the same reference is passed.</summary>
        [Theory, RandomData]
        public static void Compare_ReferenceNoDifferences(object data)
        {
            Tools.Asserter.IsEmpty(new Valuer(false).Compare(data, data));
        }

        /// <summary>Verifies no differences means equal.</summary>
        [Theory, RandomData]
        public static void Equals_NoDifferencesTrue(object data1, object data2, Fake<CompareHint> hint)
        {
            hint.Setup("Supports",
                new[] { data1, data2, Arg.LambdaAny<ValuerChainer>() },
                Behavior.Returns(true, Times.Once));
            hint.Setup("Compare",
                new[] { data1, data2, Arg.LambdaAny<ValuerChainer>() },
                Behavior.Returns(Enumerable.Empty<Difference>(), Times.Once));

            Tools.Asserter.Is(true, new Valuer(false, hint.Dummy).Equals(data1, data2));

            hint.VerifyAll(Times.Exactly(2));
        }

        /// <summary>Verifies differences means unequal.</summary>
        [Theory, RandomData]
        public static void Equals_DifferencesFalse(object data1, object data2, Fake<CompareHint> hint)
        {
            hint.Setup("Supports",
                new[] { data1, data2, Arg.LambdaAny<ValuerChainer>() },
                Behavior.Returns(true, Times.Once));
            hint.Setup("Compare",
                new[] { data1, data2, Arg.LambdaAny<ValuerChainer>() },
                Behavior.Returns(Tools.Randomizer.Create<IEnumerable<Difference>>(), Times.Once));

            Tools.Asserter.Is(false, new Valuer(false, hint.Dummy).Equals(data1, data2));

            hint.VerifyAll(Times.Exactly(2));
        }

        /// <summary>Verifies an infinite loop exception is caught and given details.</summary>
        [Theory, RandomData]
        public static void Compare_InfiniteLoopDetails(object item1, object item2, Fake<CompareHint> hint)
        {
            hint.Setup("Supports",
                new[] { item1, item2, Arg.LambdaAny<ValuerChainer>() },
                Behavior.Throw<InsufficientExecutionStackException>(Times.Once));

            InsufficientExecutionStackException e = Tools.Asserter.Throws<InsufficientExecutionStackException>(
                () => new Valuer(false, hint.Dummy).Compare(item1, item2));

            Tools.Asserter.Is(true, e.Message.Contains(item1.GetType().Name));
        }

        /// <summary>Verifies an infinite loop exception is caught and given details.</summary>
        [Theory, RandomData]
        public static void GetHashCode_InfiniteLoopDetails(object item, Fake<CompareHint> hint)
        {
            hint.Setup("Supports",
                new[] { item, item, Arg.LambdaAny<ValuerChainer>() },
                Behavior.Throw<InsufficientExecutionStackException>(Times.Once));

            InsufficientExecutionStackException e = Tools.Asserter.Throws<InsufficientExecutionStackException>(
                () => new Valuer(false, hint.Dummy).GetHashCode(item));

            Tools.Asserter.Is(true, e.Message.Contains(item.GetType().Name));
        }
    }
}
