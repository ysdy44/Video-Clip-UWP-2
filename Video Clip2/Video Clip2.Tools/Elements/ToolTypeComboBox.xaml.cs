using Video_Clip2.Tools.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using Windows.ApplicationModel.Resources;
using Windows.Globalization;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Video_Clip2.Tools.Elements
{
    internal class ToolTypeComboBoxItem : ComboBoxItem
    {
        //public string Name { get; set; }
        public ToolType Type { get; set; }
        public int Index { get; set; } = -1;
    }

    public sealed partial class ToolTypeComboBox : UserControl
    {

        //@Group
        private readonly IDictionary<ToolType, ToolTypeComboBoxItem> ItemDictionary = new Dictionary<ToolType, ToolTypeComboBoxItem>();

        #region DependencyProperty


        /// <summary> Gets or sets the tool-type. </summary>
        public ToolType ToolType
        {
            get => (ToolType)base.GetValue(ToolTypeProperty);
            set => base.SetValue(ToolTypeProperty, value);
        }
        /// <summary> Identifies the <see cref = "ToolTypeComboBox.ToolType" /> dependency property. </summary>
        public static readonly DependencyProperty ToolTypeProperty = DependencyProperty.Register(nameof(ToolType), typeof(ToolType), typeof(ToolTypeComboBox), new PropertyMetadata(ToolType.None, (sender, e) =>
        {
            ToolTypeComboBox control = (ToolTypeComboBox)sender;

            if (e.NewValue is ToolType value)
            {
                if (control.ItemDictionary.ContainsKey(value))
                {
                    ToolTypeComboBoxItem item = control.ItemDictionary[value];
                    control.ComboBox.SelectedIndex = item.Index;
                }
                else
                {
                    control.ComboBox.SelectedIndex = -1;
                }

                // Tool
                control.Tool = XML.CreateTool(value);
            }
        }
    ));


        /// <summary> Gets or sets the tool. </summary>
        public ITool Tool
        {
            get => (ITool)base.GetValue(ToolProperty);
            set => base.SetValue(ToolProperty, value);
        }
        /// <summary> Identifies the <see cref = "ToolTypeComboBox.Tool" /> dependency property. </summary>
        public static readonly DependencyProperty ToolProperty = DependencyProperty.Register(nameof(Tool), typeof(ITool), typeof(ToolTypeComboBox), new PropertyMetadata(new NoneTool()));


        #endregion

        public ToolTypeComboBox()
        {
            this.InitializeComponent();
            this.InitializeDictionary();
            this.ComboBox.SelectionChanged += (s, e) =>
            {
                if (this.ComboBox.SelectedItem is ToolTypeComboBoxItem item)
                {
                    this.ToolType = item.Type;
                }
            };
        }
    }

    public sealed partial class ToolTypeComboBox : UserControl
    {


        //@Group
        private void InitializeDictionary()
        {
            foreach (object child in this.ComboBox.Items)
            {
                if (child is ToolTypeComboBoxItem item)
                {
                    ToolType type = item.Type;

                    this.ItemDictionary.Add(type, item);
                }
            }
        }

    }
}