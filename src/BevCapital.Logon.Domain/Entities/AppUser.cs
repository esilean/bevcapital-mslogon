using FluentValidation;
using System;

namespace BevCapital.Logon.Domain.Entities
{
    public class AppUser : Entity<Guid>
    {
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; }
        public DateTime CreatedAt { get; }
        public DateTime UpdatedAt { get; private set; }

        protected AppUser() { }

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="createdAt"></param>
        /// <param name="updatedAt"></param>
        private AppUser(string name, string email, DateTime createdAt, DateTime updatedAt)
        {
            Id = Guid.NewGuid();
            Name = name;
            Email = email;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;

            Validate(this, new AppUserValidator());
        }

        public static AppUser Create(string name, string email)
        {
            return new AppUser(name, email, DateTime.Now, DateTime.Now);
        }

        public void Update(string name, string email)
        {
            Name = name;
            Email = email;
        }

        public void UpdatePassword(string password)
        {
            Password = password;
        }

        internal class AppUserValidator : AbstractValidator<AppUser>
        {
            public AppUserValidator()
            {
                RuleFor(a => a.Id)
                    .NotEmpty()
                    .WithMessage("Invalid id");

                RuleFor(a => a.Name)
                    .NotEmpty()
                    .WithMessage("Invalid name");

                RuleFor(a => a.Email)
                    .NotEmpty()
                    .WithMessage("Invalid email");
            }
        }
    }
}
