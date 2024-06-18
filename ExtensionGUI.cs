﻿using DevToys.Api;
using System.ComponentModel.Composition;
using static DevToys.Api.GUI;

namespace DevToysExtensionTemplate;

[Export(typeof(IGuiTool))]
[Name("ExtensionName")] // A unique, internal name of the tool.
[ToolDisplayInformation(
    IconFontName = "FluentSystemIcons", // This font is available by default in DevToys
    IconGlyph = '\uE670', // An icon that represents a pizza
    GroupName = PredefinedCommonToolGroupNames.Converters, // The group in which the tool will appear in the side bar.
    ResourceManagerAssemblyIdentifier = nameof(ResourceAssemblyIdentifier), // The Resource Assembly Identifier to use
    ResourceManagerBaseName =
        "DevToysExtensionTemplate.ExtensionText", // The full name (including namespace) of the resource file containing our localized texts
    ShortDisplayTitleResourceName =
        nameof(ExtensionText.ShortDisplayTitle), // The name of the resource to use for the short display title
    LongDisplayTitleResourceName = nameof(ExtensionText.LongDisplayTitle),// The name of the resource to use for the long display title
    DescriptionResourceName = nameof(ExtensionText.Description), // The name of the resource to use for the description resource name
    AccessibleNameResourceName = nameof(ExtensionText.AccessibleName))] // The name of the resource to use for the accessible name resource name
internal sealed class ExtensionGui : IGuiTool
{
    private readonly UIToolView _view = new UIToolView();
    
    public UIToolView View
    {
        get
        {
            IUIStack verticalStack = Stack()
                .Vertical()
                .WithChildren(
                    Button()
                        .Text(ExtensionText.OpenDialogLabel)
                        .OnClick(OnOpenDialogAsync)
                ); 

            if (_view.RootElement is null)
            {
                _view.WithRootElement(Stack()
                    .Horizontal()
                    .WithChildren(
                        Label().Style(UILabelStyle.Display).Text(ExtensionText.HelloWorldLabel),verticalStack));
                
            }

            return _view;
        }
    }
    public void OnDataReceived(string dataTypeName, object? parsedData)
    {
        throw new NotImplementedException();
    }
    private async ValueTask OnOpenDialogAsync()
    {
        await OpenCustomDialogAsync(dismissible: true);
    }
    private async Task<UIDialog> OpenCustomDialogAsync(bool dismissible = true)
    {
        // Open a dialog
        UIDialog dialog
            = await _view.OpenDialogAsync(
                dialogContent:
                Stack()
                    .Vertical()
                    .WithChildren(
                        Label()
                            .Style(UILabelStyle.Subtitle)
                            .Text(ExtensionText.TitleLabel),
                        Label()
                            .Style(UILabelStyle.Body)
                            .Text(ExtensionText.DescriptionLabel)),
                footerContent:
                Button()
                    .AlignHorizontally(UIHorizontalAlignment.Right)
                    .Text(ExtensionText.CloseLabel)
                    .OnClick(OnCloseDialogButtonClick),
                isDismissible: dismissible);

        void OnCloseDialogButtonClick()
        {
            _view.CurrentOpenedDialog?.Close();
        }

        return dialog;
    }
}