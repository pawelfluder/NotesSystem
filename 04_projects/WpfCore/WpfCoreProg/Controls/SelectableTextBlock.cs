﻿using System.Windows.Controls;
using System.Windows;

namespace WpfNotesSystemProg3.Controls;

public class SelectableTextBlock : TextBlock
{
    static SelectableTextBlock()
    {
        FocusableProperty.OverrideMetadata(typeof(SelectableTextBlock), new FrameworkPropertyMetadata(true));
        TextEditorWrapper.RegisterCommandHandlers(typeof(SelectableTextBlock), true, true, true);

        // remove the focus rectangle around the control
        FocusVisualStyleProperty.OverrideMetadata(typeof(SelectableTextBlock), new FrameworkPropertyMetadata((object)null));
    }

    private readonly TextEditorWrapper _editor;

    public SelectableTextBlock()
    {
        _editor = TextEditorWrapper.CreateFor(this);
    }
}