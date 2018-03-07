﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CreateAndFake.Design.Randomization;
using CreateAndFake.Toolbox.RandomizerTool.CreateHints;
using CreateAndFakeTests.TestBases;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CreateAndFakeTests.Toolbox.RandomizerTool.CreateHints
{
    /// <summary>Verifies behavior.</summary>
    [TestClass]
    public sealed class CollectionCreateHintTests : CreateHintTestBase<CollectionCreateHint>
    {
        /// <summary>Potential item types to test with.</summary>
        private static readonly Type[] s_ItemTypes
            = new[] { typeof(string), typeof(object), typeof(int), typeof(double), typeof(KeyValuePair<string, int>) };

        /// <summary>Instance to test with.</summary>
        private static readonly CollectionCreateHint s_TestInstance = new CollectionCreateHint();

        /// <summary>Types that can be created by the hint.</summary>
        private static readonly Type[] s_ValidTypes = CollectionCreateHint.PotentialCollections
            .Concat(new[] { typeof(IEnumerable<>), typeof(IList<>), typeof(ISet<>), typeof(IDictionary<,>) })
            .Concat(new[] { typeof(IReadOnlyCollection<>), typeof(IReadOnlyList<>), typeof(IReadOnlyDictionary<,>) })
            .Concat(new[] { typeof(int[]), typeof(string[]), typeof(object[]) })
            .Select(t => MakeDefined(t)).ToArray();

        /// <summary>Types that can't be created by the hint.</summary>
        private static readonly Type[] s_InvalidTypes
            = new[] { typeof(object), typeof(IEnumerable), typeof(IEnumerable<>) };

        /// <summary>Sets up the tests.</summary>
        public CollectionCreateHintTests() : base(s_TestInstance, s_ValidTypes, s_InvalidTypes) { }

        /// <summary>Populates generics with types if present.</summary>
        /// <param name="type">Potential generic type to define.</param>
        /// <returns>Type ready to be created.</returns>
        private static Type MakeDefined(Type type)
        {
            if (type.IsGenericTypeDefinition)
            {
                IRandom random = new FastRandom();
                return type.MakeGenericType(type.GetGenericArguments().Select(
                    t => random.NextItem(s_ItemTypes)).ToArray());
            }
            else
            {
                return type;
            }
        }
    }
}
