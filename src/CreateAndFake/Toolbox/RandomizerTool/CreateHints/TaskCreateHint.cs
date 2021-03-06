﻿using System;
using System.Linq;
using System.Threading.Tasks;

namespace CreateAndFake.Toolbox.RandomizerTool.CreateHints
{
    /// <summary>Handles generation of injected dummies for the randomizer.</summary>
    public sealed class TaskCreateHint : CreateHint
    {
        /// <inheritdoc/>
        protected internal override (bool, object) TryCreate(Type type, RandomizerChainer randomizer)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (randomizer == null) throw new ArgumentNullException(nameof(randomizer));

            if (type.Inherits<Task>())
            {
                return (true, Create(type, randomizer));
            }
            else
            {
                return (false, null);
            }
        }

        /// <summary>Creates a random instance of the given type.</summary>
        /// <param name="type">Type to generate.</param>
        /// <param name="randomizer">Handles callback behavior for child values.</param>
        /// <returns>The created instance.</returns>
        private static object Create(Type type, RandomizerChainer randomizer)
        {
            if (type.IsGenericType)
            {
                Type content = type.GetGenericArguments().Single();

                return typeof(Task)
                    .GetMethod(nameof(Task.FromResult))
                    .MakeGenericMethod(content)
                    .Invoke(null, new[] { randomizer.Create(content, randomizer.Parent) });
            }
            else
            {
                return Task.FromResult(randomizer.Create<int>());
            }
        }
    }
}
