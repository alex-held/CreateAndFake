﻿using System.Diagnostics.CodeAnalysis;

namespace CreateAndFake.Toolbox.FakerTool
{
    /// <summary>Represents void type for behaviors.</summary>
    [SuppressMessage("Sonar", "S3453:Uncreatable", Justification = "Intended to be uncreatable.")]
    public sealed class VoidType
    {
        /// <summary>Prevents instantiation.</summary>
        private VoidType() { }
    }
}
