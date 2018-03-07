﻿using System.Collections;
using System.Collections.Generic;
using CreateAndFake;
using CreateAndFake.Design.Content;
using CreateAndFake.Toolbox.FakerTool;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CreateAndFakeTests.Design.Content
{
    /// <summary>Verifies behavior.</summary>
    [TestClass]
    public sealed class ValueComparerTests
    {
        /// <summary>Verifies params works as intended.</summary>
        [TestMethod]
        public void GetHashCode_ParamsBehavior()
        {
            int item1 = Tools.Randomizer.Create<int>();
            int item2 = Tools.Randiffer.Branch(item1);
            int item3 = Tools.Randiffer.Branch(item2);
            List<int> collection = new List<int> { item1, item2, item3 };

            Tools.Asserter.Is(
                ValueComparer.Use.GetHashCode(collection),
                ValueComparer.Use.GetHashCode(item1, item2, item3));
        }

        /// <summary>Verifies the type works as intended.</summary>
        [TestMethod]
        public void ValueComparer_ValueEquatableBehavior()
        {
            Fake<IValueEquatable> stub1 = Tools.Faker.Stub<IValueEquatable>();
            Fake<IValueEquatable> stub2 = Tools.Faker.Stub<IValueEquatable>();

            ValueComparer.Use.Equals(stub1.Dummy, stub2.Dummy);
            stub1.Verify(1, m => m.ValuesEqual(stub2.Dummy));
            stub1.VerifyTotalCalls(1);
            stub2.VerifyTotalCalls(0);

            ValueComparer.Use.Equals((object)stub1.Dummy, stub2.Dummy);
            stub1.Verify(2, m => m.ValuesEqual(stub2.Dummy));
            stub1.VerifyTotalCalls(2);
            stub2.VerifyTotalCalls(0);

            ValueComparer.Use.Compare(stub1.Dummy, stub2.Dummy);
            stub1.Verify(1, m => m.GetValueHash());
            stub1.VerifyTotalCalls(3);
            stub2.Verify(1, m => m.GetValueHash());
            stub2.VerifyTotalCalls(1);

            ValueComparer.Use.GetHashCode((object)stub1.Dummy);
            stub1.Verify(2, m => m.GetValueHash());
            stub1.VerifyTotalCalls(4);
        }

        /// <summary>Verifies the type works as intended.</summary>
        [TestMethod]
        public void ValueComparer_ObjectBehavior()
        {
            TestBehavior<int, object>(ValueComparer.Use, ValueComparer.Use);
            TestBehavior<string, object>(ValueComparer.Use, ValueComparer.Use);
        }

        /// <summary>Verifies the type works as intended.</summary>
        [TestMethod]
        public void ValueComparer_IEnumerableBehavior()
        {
            TestBehavior<IEnumerable<int>, object>(ValueComparer.Use, ValueComparer.Use);
            TestBehavior<IEnumerable<int>, IEnumerable>(ValueComparer.Use, ValueComparer.Use);

            TestBehavior<IEnumerable<string>, object>(ValueComparer.Use, ValueComparer.Use);
            TestBehavior<IEnumerable<string>, IEnumerable>(ValueComparer.Use, ValueComparer.Use);
        }

        /// <summary>Verifies the type works as intended.</summary>
        [TestMethod]
        public void ValueComparer_IDictionaryBehavior()
        {
            TestBehavior<IDictionary<int, int>, object>(ValueComparer.Use, ValueComparer.Use);
            TestBehavior<IDictionary<int, int>, IEnumerable>(ValueComparer.Use, ValueComparer.Use);
            TestBehavior<Dictionary<int, int>, IDictionary>(ValueComparer.Use, ValueComparer.Use);

            TestBehavior<IDictionary<string, int>, object>(ValueComparer.Use, ValueComparer.Use);
            TestBehavior<IDictionary<string, int>, IEnumerable>(ValueComparer.Use, ValueComparer.Use);
            TestBehavior<Dictionary<string, int>, IDictionary>(ValueComparer.Use, ValueComparer.Use);

            TestBehavior<IDictionary<int, string>, object>(ValueComparer.Use, ValueComparer.Use);
            TestBehavior<IDictionary<int, string>, IEnumerable>(ValueComparer.Use, ValueComparer.Use);
            TestBehavior<Dictionary<int, string>, IDictionary>(ValueComparer.Use, ValueComparer.Use);

            TestBehavior<IDictionary<string, string>, object>(ValueComparer.Use, ValueComparer.Use);
            TestBehavior<IDictionary<string, string>, IEnumerable>(ValueComparer.Use, ValueComparer.Use);
            TestBehavior<Dictionary<string, string>, IDictionary>(ValueComparer.Use, ValueComparer.Use);
        }

        /// <summary>Verifies instance behavior.</summary>
        private static void TestBehavior<TActual, TComparer>(
            IComparer<TComparer> comparer, IEqualityComparer<TComparer> equalityComparer) where TActual : TComparer
        {
            TActual baseObject = Tools.Randomizer.Create<TActual>();
            TActual equalObject = Tools.Duplicator.Copy(baseObject);
            TActual unequalObject = Tools.Randiffer.Branch(baseObject);

            Tools.Asserter.Is(true, equalityComparer.Equals(baseObject, baseObject));
            Tools.Asserter.Is(true, equalityComparer.Equals(baseObject, equalObject));
            Tools.Asserter.Is(false, equalityComparer.Equals(baseObject, unequalObject));
            Tools.Asserter.Is(false, equalityComparer.Equals(baseObject, default));
            Tools.Asserter.Is(false, equalityComparer.Equals(default, baseObject));
            Tools.Asserter.Is(true, equalityComparer.Equals(default, default));

            Tools.Asserter.Is(0, comparer.Compare(baseObject, baseObject));
            Tools.Asserter.Is(0, comparer.Compare(baseObject, equalObject));
            Tools.Asserter.IsNot(0, comparer.Compare(baseObject, unequalObject));
            Tools.Asserter.IsNot(0, comparer.Compare(baseObject, default));
            Tools.Asserter.IsNot(0, comparer.Compare(default, baseObject));
            Tools.Asserter.Is(0, comparer.Compare(default, default));
        }
    }
}
