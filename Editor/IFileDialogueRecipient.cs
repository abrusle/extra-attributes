namespace Abrusle.ExtraAtributes.Editor
{
    internal interface IFileDialogueRecipient
    {
        /// <summary>
        /// The result of a file dialogue window.
        /// </summary>
        string FileDialogueResult { get; set; }
    }
}