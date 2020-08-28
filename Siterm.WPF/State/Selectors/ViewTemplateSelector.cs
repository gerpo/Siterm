using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Siterm.WPF.State.Selectors
{
    public class ViewTemplateSelector : DataTemplateSelector
    {
        private const string Viewmodel = "ViewModel";
        private const string Model = "Model";

        private readonly Dictionary<string, DataTemplate> _dataTemplates =
            new Dictionary<string, DataTemplate>();

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (!(container is ContentPresenter)) return base.SelectTemplate(item, container);
            if (item == null) return base.SelectTemplate(null, container);

            var type = item.GetType();
            var name = type.Name;
            if (!name.EndsWith(Viewmodel)) return base.SelectTemplate(item, container);

            name = name.Substring(0, name.Length - Model.Length);
            if (_dataTemplates.ContainsKey(name))
                return _dataTemplates[name];

            var match = type.Assembly.GetTypes().FirstOrDefault(t => t.Name == name);
            if (match == null) return base.SelectTemplate(item, container);

            var view = Activator.CreateInstance(match) as DependencyObject;
            if (view == null) return base.SelectTemplate(item, container);

            var factory = new FrameworkElementFactory(match);
            var dataTemplate = new DataTemplate(type)
            {
                VisualTree = factory
            };
            _dataTemplates.Add(name, dataTemplate);
            return dataTemplate;
        }
    }
}