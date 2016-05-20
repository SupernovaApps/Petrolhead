using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Utils;
using Windows.UI.Popups;

namespace Petrolhead
{
    public sealed class DialogHelper : IDialogHelper
    {
        private static DialogHelper _helper;
        public static DialogHelper Current
        {
            get
            {
                if (_helper == null)
                    _helper = new DialogHelper();
                return _helper;
            }
            private set
            {
                _helper = value;
            }
        }

        private DialogHelper()
        {
            Current = this;
        }

        public async Task<bool> ShowDialogAsync(Exception ex)
        {
          
                if (App.Current.NavigationService.Dispatcher.HasThreadAccess())
                {
                    await App.Current.NavigationService.Dispatcher.DispatchAsync(async () =>
                    {
                        await new MessageDialog(ex.Message, "Whoops!").ShowAsync();
                    });
                }
                else
                    return false;
                
                return true;
           
        }

        public async Task<bool> ShowDialogAsync(string content)
        {
            if (App.Current.NavigationService.Dispatcher.HasThreadAccess())
            {
                await App.Current.NavigationService.Dispatcher.DispatchAsync(async () =>
                {
                    await new MessageDialog(content).ShowAsync();
                });
                return true;
            }
            return false;
        }

        public async Task<bool> ShowDialogAsync(string content, string title)
        {
            if (App.Current.NavigationService.Dispatcher.HasThreadAccess())
            {
                await App.Current.NavigationService.Dispatcher.DispatchAsync(async () =>
                {
                    await new MessageDialog(content).ShowAsync();
                });
                return true;
            }
            return false;
        }

        public async Task<bool> ShowDialogAsync(string content, IList<DialogButton> actions)
        {
            if (App.Current.NavigationService.Dispatcher.HasThreadAccess())
            {
                await App.Current.NavigationService.Dispatcher.DispatchAsync(async () =>
                {
                    if (actions.Count <= 3)
                    {
                        if (actions.Count > 2 && DeviceUtils.CurrentDeviceFamily == DeviceUtils.DeviceFamilies.Mobile)
                            actions.RemoveAt(2);
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException("Button list cannot have more than three items.");
                    }
                        
                    var dialog = new MessageDialog(content);
                    foreach (var btn in actions)
                    {
                        if (string.IsNullOrWhiteSpace(btn.Label))
                            throw new ArgumentOutOfRangeException("All buttons must have a valid label.");

                        if (btn.Action == null)
                            dialog.Commands.Add(new UICommand(btn.Label));
                        else
                            dialog.Commands.Add(new UICommand(btn.Label, (command) =>
                            {
                                btn.Action();
                            }));
                    }
                    await dialog.ShowAsync();
                });
                return true;
            }
            return false;
        }

        public async Task<bool> ShowDialogAsync(string content, string title, IList<DialogButton> actions)
        {
            if (App.Current.NavigationService.Dispatcher.HasThreadAccess())
            {
                await App.Current.NavigationService.Dispatcher.DispatchAsync(async () =>
                {
                    if (actions.Count <= 3)
                    {
                        if (actions.Count > 2 && DeviceUtils.CurrentDeviceFamily == DeviceUtils.DeviceFamilies.Mobile)
                            actions.RemoveAt(2);
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException("Button list cannot have more than three items.");
                    }

                    var dialog = new MessageDialog(content, title);
                    foreach (var btn in actions)
                    {
                        if (string.IsNullOrWhiteSpace(btn.Label))
                            throw new ArgumentOutOfRangeException("All buttons must have a valid label.");

                        if (btn.Action == null)
                            dialog.Commands.Add(new UICommand(btn.Label));
                        else
                            dialog.Commands.Add(new UICommand(btn.Label, (command) =>
                            {
                                btn.Action();
                            }));
                    }
                    await dialog.ShowAsync();
                });
                return true;
            }
            return false;
        }
    }
}
