using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace WpfCoreProg.Styles
{
    public class RepoItemTemplateSelector : DataTemplateSelector
    {
        DataTemplate stringTemplate; //This would need to be initialized

        //You override this function to select your data template based in the given item
        public override System.Windows.DataTemplate SelectTemplate(object item,
                        System.Windows.DependencyObject container)
        {
            if (item is string)
                return stringTemplate;
            else
                return base.SelectTemplate(item, container);
        }
    }
}
