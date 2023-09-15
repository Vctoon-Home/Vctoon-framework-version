using System.Linq;
using Volo.Abp.Http.Client;
using Volo.Abp.Validation;

namespace VctoonClient.Extensions;

public static class ExceptionExtensions
{
    public static void Notify(this AbpRemoteCallException remoteCallException)
    {
        App.NotificationManager.Show(new Notification(remoteCallException.HttpStatusCode.ToString(), remoteCallException.Message, NotificationType.Error));
    }

    public static void Notify(this AbpValidationException validationException)
    {
        App.NotificationManager.Show(new Notification(validationException.Message, validationException.ValidationErrors.FirstOrDefault()?.ErrorMessage, NotificationType.Error));
    }
}