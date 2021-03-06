﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.Threading;

namespace Microsoft.VisualStudio.ProjectSystem.Properties
{
    /// <summary>
    /// A provider for properties that are stored in the source code of the project.
    /// </summary>
    internal abstract class AbstractSourceFilePropertiesProvider : IProjectPropertiesProvider
    {
        private readonly UnconfiguredProject _unconfiguredProject;
        private readonly Workspace _workspace;
        private readonly IProjectThreadingService _threadingService;

        public AbstractSourceFilePropertiesProvider(UnconfiguredProject unconfiguredProject,
                                                    Workspace workspace,
                                                    IProjectThreadingService threadingService)
        {
            Requires.NotNull(unconfiguredProject, nameof(unconfiguredProject));
            Requires.NotNull(workspace, nameof(workspace));
            Requires.NotNull(threadingService, nameof(threadingService));

            _unconfiguredProject = unconfiguredProject;
            _workspace = workspace;
            _threadingService = threadingService;
        }

#pragma warning disable 0067
        public event AsyncEventHandler<ProjectPropertyChangedEventArgs> ProjectPropertyChanging;
        public event AsyncEventHandler<ProjectPropertyChangedEventArgs> ProjectPropertyChangedOnWriter;
        public event AsyncEventHandler<ProjectPropertyChangedEventArgs> ProjectPropertyChanged;
#pragma warning restore 0067

        public string DefaultProjectPath => _unconfiguredProject.FullPath;

        /// <summary>
        /// Gets the properties for a property or item.
        /// </summary>
        public IProjectProperties GetProperties(string file, string itemType, string item)
        {
            return new SourceFileProperties(ProjectPropertiesContext.GetContext(_unconfiguredProject, itemType, item), _workspace, _threadingService);
        }

        /// <summary>
        /// Gets the project-level properties
        /// </summary>
        public IProjectProperties GetCommonProperties()
        {
            return GetProperties(_unconfiguredProject.FullPath, null, null);
        }
        
        /// <summary>
        /// Get the properties for an item.
        /// </summary>
        public IProjectProperties GetItemProperties(string itemType, string item)
        {
            // There aren't any items that are stored in source.
            throw new InvalidOperationException();
        }

        /// <summary>
        /// Get the properties for an item type.
        /// </summary>
        public IProjectProperties GetItemTypeProperties(string itemType)
        {
            // There aren't any items that are stored in source.
            throw new InvalidOperationException();
        }
    }
}
