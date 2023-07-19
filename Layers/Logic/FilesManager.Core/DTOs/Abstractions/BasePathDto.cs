﻿namespace FilesManager.Core.DTOs.Abstractions
{
    public abstract class BasePathDto
    {
        /// <summary>
        /// Gets the path of the file.
        /// </summary>
        public string Path { get; } = String.Empty;

        /// <summary>
        /// Gets the extension of the file.
        /// </summary>
        public string Extension { get; } = String.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="BasePathDto"/> class.
        /// </summary>
        protected BasePathDto() { }

        /// <inheritdoc cref="BasePathDto()" />
        protected BasePathDto(string path, string extension) : this()
        {
            this.Path = path;
            this.Extension = extension;
        }

        /// <summary>
        /// Checks whether this instance is empty.
        /// </summary>
        public bool IsEmpty()
        {
            return this.Path.Equals(String.Empty) ||
                   this.Extension.Equals(String.Empty);
        }
    }
}
