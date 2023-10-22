﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FilesManager.UI.Common.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("FilesManager.UI.Common.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Process.
        /// </summary>
        public static string Content_ProcessButton {
            get {
                return ResourceManager.GetString("Content_ProcessButton", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Reset.
        /// </summary>
        public static string Content_ResetButton {
            get {
                return ResourceManager.GetString("Content_ResetButton", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The file was&apos;t parsed properly by RegEx in FilePathConverter:.
        /// </summary>
        public static string ERROR_Internal_InvalidFilePathDto {
            get {
                return ResourceManager.GetString("ERROR_Internal_InvalidFilePathDto", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The property name or error message shouldn&apos;t be empty!.
        /// </summary>
        public static string ERROR_Internal_PropertyOrMessageAreEmpty {
            get {
                return ResourceManager.GetString("ERROR_Internal_PropertyOrMessageAreEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The method is used with a wrong event:.
        /// </summary>
        public static string ERROR_Internal_WrongCommandSubscribed {
            get {
                return ResourceManager.GetString("ERROR_Internal_WrongCommandSubscribed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The given value contains illegal characters.
        /// </summary>
        public static string ERROR_Validation_Field_ContainsIllegalCharacter {
            get {
                return ResourceManager.GetString("ERROR_Validation_Field_ContainsIllegalCharacter", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The given value contains not a number.
        /// </summary>
        public static string ERROR_Validation_Field_ContainsNotDigits {
            get {
                return ResourceManager.GetString("ERROR_Validation_Field_ContainsNotDigits", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Some numbers would eventually exceed the max value for &apos;Start number&apos; (65535) if the renaming continue..
        /// </summary>
        public static string ERROR_Validation_Field_ValueWillExceedUshortMax {
            get {
                return ResourceManager.GetString("ERROR_Validation_Field_ValueWillExceedUshortMax", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The file name does not contain preceeding numeric part:.
        /// </summary>
        public static string ERROR_Validation_FileName_HasNoPreceedingNumber {
            get {
                return ResourceManager.GetString("ERROR_Validation_FileName_HasNoPreceedingNumber", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to File loading error.
        /// </summary>
        public static string ERROR_Validation_Files_PathNotRecognized_Header {
            get {
                return ResourceManager.GetString("ERROR_Validation_Files_PathNotRecognized_Header", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unrecognized type of file:.
        /// </summary>
        public static string ERROR_Validation_Files_PathNotRecognized_Text {
            get {
                return ResourceManager.GetString("ERROR_Validation_Files_PathNotRecognized_Text", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to OPTIONS.
        /// </summary>
        public static string Header_General {
            get {
                return ResourceManager.GetString("Header_General", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &quot;Replace name&quot; ([old name] → prefix? + number + postfix?).
        /// </summary>
        public static string Header_Method_IncrementNumber {
            get {
                return ResourceManager.GetString("Header_Method_IncrementNumber", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &quot;Leading zeros&quot;.
        /// </summary>
        public static string Header_Method_LeadingZeros {
            get {
                return ResourceManager.GetString("Header_Method_LeadingZeros", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &quot;Update name&quot; (prepend? + [unchanged name] + append?).
        /// </summary>
        public static string Header_Method_PrependAppend {
            get {
                return ResourceManager.GetString("Header_Method_PrependAppend", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Append:.
        /// </summary>
        public static string Label_Append {
            get {
                return ResourceManager.GetString("Label_Append", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Postfix:.
        /// </summary>
        public static string Label_Postfix {
            get {
                return ResourceManager.GetString("Label_Postfix", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Prefix:.
        /// </summary>
        public static string Label_Prefix {
            get {
                return ResourceManager.GetString("Label_Prefix", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Prepend:.
        /// </summary>
        public static string Label_Prepend {
            get {
                return ResourceManager.GetString("Label_Prepend", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Set:.
        /// </summary>
        public static string Label_Set {
            get {
                return ResourceManager.GetString("Label_Set", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Start number:.
        /// </summary>
        public static string Label_StartNumber {
            get {
                return ResourceManager.GetString("Label_StartNumber", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Operation failed.
        /// </summary>
        public static string RESULT_Operation_Failure {
            get {
                return ResourceManager.GetString("RESULT_Operation_Failure", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Operation successful.
        /// </summary>
        public static string RESULT_Operation_Success_Header {
            get {
                return ResourceManager.GetString("RESULT_Operation_Success_Header", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to All files were renamed!.
        /// </summary>
        public static string RESULT_Operation_Success_Text {
            get {
                return ResourceManager.GetString("RESULT_Operation_Success_Text", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INFO: Optional text to be added at the end of the file name.
        /// </summary>
        public static string Tooltip_Append {
            get {
                return ResourceManager.GetString("Tooltip_Append", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INFO: Drag and drop files here.
        /// </summary>
        public static string Tooltip_FilesList {
            get {
                return ResourceManager.GetString("Tooltip_FilesList", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INFO: The operations on files that can be performed.
        /// </summary>
        public static string Tooltip_General {
            get {
                return ResourceManager.GetString("Tooltip_General", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to EXAMPLE
        ///
        ///INPUT:
        ///- picture20.png
        ///- file42.jpeg
        ///
        ///PARAMETERS:
        ///- Prefix: &quot;test #&quot;
        ///- Start number: 0
        ///- Postfix: &quot;_file&quot;
        ///
        ///RESULT:
        ///- test #0_file.png
        ///- test #1_file.jpeg
        ///
        ///
        ///EXPLANATION: The original file names are completely
        ///replaced by at least one of the provided values. It is po-
        ///ssible to put text before counting numbers, or leave it
        ///blank, to keep only numbers as the new file names.
        ///
        ///PS: You can also use this method to rename your
        ///files in the following way: &quot;image (&quot; + &quot;1&quot; + &quot;)&quot;
        ///resulti [rest of string was truncated]&quot;;.
        /// </summary>
        public static string Tooltip_Method_IncrementNumber {
            get {
                return ResourceManager.GetString("Tooltip_Method_IncrementNumber", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to EXAMPLE
        ///
        ///INPUT:
        ///- 1.jpg
        ///- 15.jpg
        ///
        ///PARAMETERS:
        ///- Set: 2
        ///
        ///RESULT:
        ///- 0001.jpg
        ///- 0015.jpg
        ///
        ///
        ///EXPLANATION: As the first step &quot;n-leading&quot; zeros
        ///were added to the longest file name. Then, as the
        ///second step, names shorter or equal to the longest
        ///possible name were aligned by adding respective
        ///amount of zeroes. This behavior is useful for older
        ///Windows systems sorting behaviors, where files
        ///were ordered lexicographically as following:
        ///
        ///Lexicographically:        Mathematically:
        ///
        ///0.txt          [rest of string was truncated]&quot;;.
        /// </summary>
        public static string Tooltip_Method_LeadingZeros {
            get {
                return ResourceManager.GetString("Tooltip_Method_LeadingZeros", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to EXAMPLE
        ///
        ///INPUT:
        ///- Archive.zip
        ///
        ///PARAMETERS:
        ///- Prepend: &quot;My_[&quot;
        ///- Append: &quot;]_2022&quot;
        ///
        ///RESULT:
        ///- My_[Archive]_2022.zip.
        /// </summary>
        public static string Tooltip_Method_PrependAppend {
            get {
                return ResourceManager.GetString("Tooltip_Method_PrependAppend", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INFO: Optional text to be added after the number.
        /// </summary>
        public static string Tooltip_Postfix {
            get {
                return ResourceManager.GetString("Tooltip_Postfix", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INFO: Optional text to be added before the number.
        /// </summary>
        public static string Tooltip_Prefix {
            get {
                return ResourceManager.GetString("Tooltip_Prefix", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INFO: Optional text to be added at the beginning of the file name.
        /// </summary>
        public static string Tooltip_Prepend {
            get {
                return ResourceManager.GetString("Tooltip_Prepend", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INFO: Starts processing given files based on a selected method.
        /// </summary>
        public static string Tooltip_ProcessButton {
            get {
                return ResourceManager.GetString("Tooltip_ProcessButton", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INFO: Select to activate this method.
        /// </summary>
        public static string Tooltip_RadioButton {
            get {
                return ResourceManager.GetString("Tooltip_RadioButton", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INFO: Clears loaded files and all values from methods.
        /// </summary>
        public static string Tooltip_ResetButton {
            get {
                return ResourceManager.GetString("Tooltip_ResetButton", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INFO: Set the amount of leading zeros before the file name.
        /// </summary>
        public static string Tooltip_Set {
            get {
                return ResourceManager.GetString("Tooltip_Set", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INFO: Rename given files using an incremented start number.
        /// </summary>
        public static string Tooltip_StartNumber {
            get {
                return ResourceManager.GetString("Tooltip_StartNumber", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to TIP: Empty or only whitespaces content will be ignored
        ///
        ///Invalid characters:  \ / : * ? &quot; &lt; &gt; |.
        /// </summary>
        public static string Tooltip_Tip_Content_NonEmptyField {
            get {
                return ResourceManager.GetString("Tooltip_Tip_Content_NonEmptyField", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to TIP: Only positive numbers from the range [0 - 65 535].
        /// </summary>
        public static string Tooltip_Tip_Content_OnlyPositives {
            get {
                return ResourceManager.GetString("Tooltip_Tip_Content_OnlyPositives", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to TIP: Only positive numbers from the range [0 - 7].
        /// </summary>
        public static string Tooltip_Tip_Content_OnlyVerySmallPositives {
            get {
                return ResourceManager.GetString("Tooltip_Tip_Content_OnlyVerySmallPositives", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to File Manager (v0.3.8-alpha).
        /// </summary>
        public static string WindowTitle {
            get {
                return ResourceManager.GetString("WindowTitle", resourceCulture);
            }
        }
    }
}
