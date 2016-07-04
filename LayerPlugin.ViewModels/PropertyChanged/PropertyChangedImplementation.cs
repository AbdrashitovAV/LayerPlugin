using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace LayerPlugin.ViewModels.PropertyChanged
{

        public abstract class PropertyChangedImplementation : INotifyPropertyChanged
        {
            protected void RaisePropertyChanged<T>(Expression<Func<T>> raiser)
            {
                var e = PropertyChanged;
                if (e != null)
                {
                    var propName = ((MemberExpression)raiser.Body).Member.Name;
                    e(this, new PropertyChangedEventArgs(propName));
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;
        }

}
