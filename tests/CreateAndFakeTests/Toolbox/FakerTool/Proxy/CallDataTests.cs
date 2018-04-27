﻿using System;
using System.Linq;
using CreateAndFake;
using CreateAndFake.Toolbox.FakerTool;
using CreateAndFake.Toolbox.FakerTool.Proxy;
using CreateAndFakeTests.TestSamples;
using Xunit;

namespace CreateAndFakeTests.Toolbox.FakerTool.Proxy
{
    /// <summary>Verifies behavior.</summary>
    public static class CallDataTests
    {
        /// <summary>Verifies nulls are not accepted.</summary>
        [Fact]
        public static void New_NullsThrow()
        {
            Tools.Asserter.Throws<ArgumentNullException>(() => new CallData(
                null, Tools.Randomizer.Create<Type[]>(), Tools.Randomizer.Create<DataHolderSample[]>(), Tools.Valuer));
            Tools.Asserter.Throws<ArgumentNullException>(() => new CallData(
                Tools.Randomizer.Create<string>(), null, Tools.Randomizer.Create<DataHolderSample[]>(), Tools.Valuer));
            Tools.Asserter.Throws<ArgumentNullException>(() => new CallData(
                Tools.Randomizer.Create<string>(), Tools.Randomizer.Create<Type[]>(), null, Tools.Valuer));
        }

        /// <summary>Verifies a null isn't accepted.</summary>
        [Fact]
        public static void DeepClone_NullThrows()
        {
            Tools.Asserter.Throws<ArgumentNullException>(
                () => Tools.Randomizer.Create<CallData>().DeepClone(null));
        }

        /// <summary>Verifies a null isn't accepted.</summary>
        [Fact]
        public static void MatchesCall_NullThrows()
        {
            Tools.Asserter.Throws<ArgumentNullException>(
                () => Tools.Randomizer.Create<CallData>().MatchesCall(null));
        }

        /// <summary>Verifies no match with different method names.</summary>
        [Theory, RandomData]
        public static void MatchesCall_MethodNameMismatch(DataHolderSample[] data, Type[] generics, string name1)
        {
            string name2 = Tools.Randiffer.Branch(name1);

            Tools.Asserter.Is(false, new CallData(name1, generics, data, Tools.Valuer)
                .MatchesCall(new CallData(name2, generics, data, null)));
        }

        /// <summary>Verifies no match with different method names.</summary>
        [Theory, RandomData]
        public static void MatchesCall_GenericsMismatch(DataHolderSample[] data, string name, Type[] generics1)
        {
            Type[] generics2 = Tools.Randiffer.Branch(generics1);

            Tools.Asserter.Is(false, new CallData(name, generics1, data, Tools.Valuer)
                .MatchesCall(new CallData(name, generics2, data, null)));
        }

        /// <summary>Verifies always a match when using AnyGeneric.</summary>
        [Theory, RandomData]
        public static void MatchesCall_AnyGenericsMatch(DataHolderSample[] data, string name, Type[] generics1)
        {
            Type[] generics2 = generics1.Select(t => typeof(AnyGeneric)).ToArray();

            Tools.Asserter.Is(true, new CallData(name, generics2, data, Tools.Valuer)
                .MatchesCall(new CallData(name, generics1, data, null)));
        }

        /// <summary>Verifies match functionality.</summary>
        [Theory, RandomData]
        public static void MatchesCall_DataMatchBehavior(string name, Type[] generics, DataHolderSample[] data1)
        {
            DataHolderSample[] data2 = data1.Select(d => Tools.Duplicator.Copy(d)).ToArray();

            Tools.Asserter.Is(true, new CallData(name, generics, data1, Tools.Valuer)
                .MatchesCall(new CallData(name, generics, data2, null)));

            Tools.Asserter.Is(false, new CallData(name, generics, data1, null)
                .MatchesCall(new CallData(name, generics, data2, null)));
        }
    }
}