using FluentValidation.Results;
using System.Collections.Generic;

namespace BevCapital.Logon.Domain.Notifications
{
    public interface IAppNotificationHandler
    {
        IReadOnlyCollection<Notification> Notifications { get; }

        bool HasNotifications { get; }

        void AddNotification(string key, string message);

        void AddNotification(Notification notification);

        void AddNotifications(IEnumerable<Notification> notifications);

        void AddNotifications(IReadOnlyCollection<Notification> notifications);

        void AddNotifications(IList<Notification> notifications);

        void AddNotifications(ICollection<Notification> notifications);

        void AddNotifications(ValidationResult validationResult);
    }
}
