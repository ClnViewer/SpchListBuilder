using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System.Threading.Tasks;

namespace SpchListBuilder.Pages
{
    public abstract class BasePage : Page
    {
        protected void EventErrorPrint(object o, StringEventArgs e)
        {
            string __title;
            string __message;

            if (e == null)
            {
                __title = Properties.Resources.Error0;
                __message = Properties.Resources._unknown_error;
            }
            else if (e.Exception != null)
            {
                __title = ((String.IsNullOrWhiteSpace(e.Message)) ? Properties.Resources.Error0 : e.Message);
                __message = ((String.IsNullOrWhiteSpace(e.Exception.Message)) ? Properties.Resources._unknown_error : e.Exception.Message);
            }
            else
            {
                __title = Properties.Resources.Error0;
                __message = ((String.IsNullOrWhiteSpace(e.Message)) ? Properties.Resources._unknown_error : e.Message);
            }
            ShowMessageBox(__title, __message);
        }

        protected void ShowMessageBox(string title, string msg)
        {
            Dispatcher.BeginInvoke((Action)(() =>
            {
                this.TryFindParent<MetroWindow>().ShowMessageAsync(title, msg);
            }));
        }

        protected bool TaskCheckReturn<T>(Task<T> t, string head)
        {
            if (t == null)
            {
                ShowMessageBox(head, Properties.Resources.Operation_fault);
                return false;
            }
            else if (t.IsCanceled)
            {
                ShowMessageBox(head, Properties.Resources.Operation_Cancelled);
                return false;
            }
            else if (t.IsFaulted)
            {
                ShowMessageBox(head,
                    ((t.Exception != null) && (t.Exception.Message != null) ?
                    t.Exception.Message : Properties.Resources.Operation_fault)
                );
                return false;
            }
            return true;
        }

    }
}
