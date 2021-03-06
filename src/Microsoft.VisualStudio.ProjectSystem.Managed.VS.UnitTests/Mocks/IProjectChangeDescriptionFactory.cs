﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using Microsoft.VisualStudio.ProjectSystem.Properties;
using Moq;

namespace Microsoft.VisualStudio.ProjectSystem.VS
{
    internal class IProjectChangeDescriptionFactory
    {
        public static IProjectChangeDescription Create()
        {
            return Mock.Of<IProjectChangeDescription>();
        }

        public static IProjectChangeDescription Implement(IProjectRuleSnapshot after = null,
                                                          IProjectRuleSnapshot before = null,
                                                          IProjectChangeDiff difference = null,
                                                          MockBehavior? mockBehavior = null)
        {
            var behavior = mockBehavior.HasValue ? mockBehavior.Value : MockBehavior.Default;
            var mock = new Mock<IProjectChangeDescription>(behavior);

            if (after != null)
            {
                mock.Setup(x => x.After).Returns(after);
            }

            if (before != null)
            {
                mock.Setup(x => x.Before).Returns(before);
            }

            if (difference != null)
            {
                mock.Setup(x => x.Difference).Returns(difference);
            }

            return mock.Object;
        }

        public static IProjectChangeDescription FromJson(string jsonString)
        {
            var model = new IProjectChangeDescriptionModel();
            return model.FromJson(jsonString);
        }
    }

    internal class IProjectChangeDescriptionModel : JsonModel<IProjectChangeDescription>
    {
        public IProjectRuleSnapshotModel After { get; set; }
        public IProjectRuleSnapshotModel Before { get; set; }
        public IProjectChangeDiffModel Difference { get; set; }

        public override IProjectChangeDescription ToActualModel()
        {
            return new ActualModel
            {
                After = After,
                Before = Before,
                Difference = Difference
            };
        }

        private class ActualModel : IProjectChangeDescription
        {
            public IProjectRuleSnapshot After { get; set; }
            public IProjectRuleSnapshot Before { get; set; }
            public IProjectChangeDiff Difference { get; set; }
        }
    }

}