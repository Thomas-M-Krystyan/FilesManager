﻿using FilesManager.UI.Common.Properties;
using FilesManager.UI.Desktop.ViewModels.Layers.Common.Base;

namespace FilesManager.UI.Desktop.ViewModels.Layers.Specific
{
    /// <summary>
    /// View model that contains information about the active layers of the application.
    /// <para>
    ///   Layers can be seen as application modes (e.g., Renaming, Converting, Validating).
    /// </para>
    /// </summary>
    /// <seealso cref="ObservableViewModel"/>
    internal sealed class ActiveLayerViewModel : ObservableViewModel
    {
        #region Texts
        // Layers
        public static string Layer_RenamingButton_Header = Resources.Content_Layer_RenamingButton;
        public static string Layer_RenamingButton_Tooltip = Resources.Tooltip_Layer_RenamingButton;

        public static string Layer_ConvertingButton_Header = Resources.Content_Layer_ConvertingButton;
        public static string Layer_ConvertingButton_Tooltip = Resources.Tooltip_Layer_ConvertingButton;

        public static string Layer_ValidatingButton_Header = Resources.Content_Layer_ValidatingButton;
        public static string Layer_ValidatingButton_Tooltip = Resources.Tooltip_Layer_ValidatingButton;
        #endregion

        #region Properties (binding)
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ActiveLayerViewModel"/> class.
        /// </summary>
        public ActiveLayerViewModel()  // NOTE: Used for Dependency Injection
            : base()
        {
        }
    }
}