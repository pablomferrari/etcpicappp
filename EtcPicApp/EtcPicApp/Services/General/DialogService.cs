using System.Threading.Tasks;
using Acr.UserDialogs;
using EtcPicApp.Contracts.Services.General;

namespace EtcPicApp.Services.General
{
    public class DialogService : IDialogService
    {
        public Task ShowDialog(string message, string title, string buttonLabel)
        {
            return UserDialogs.Instance.AlertAsync(message, title, buttonLabel);
        }

        public void ShowToast(string message)
        {
            UserDialogs.Instance.Toast(message);
        }

        public Task<bool> ShowConfirm(string message, string title, string okText, string cancelText)
        {
            var val = new ConfirmConfig
            {
                Message = message,
                Title = title,
                OkText = okText,
                CancelText = cancelText
            };
            return UserDialogs.Instance.ConfirmAsync(val);
        }

    }
}
