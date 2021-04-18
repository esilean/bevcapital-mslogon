using FluentValidation;
using System;

namespace BevCapital.Logon.Domain.Entities
{
    public class AppUser : Entity
    {
        public string Email { get; private set; }
        public string Name { get; private set; }
        public string Password { get; private set; }

        protected AppUser() { }

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="email"></param>
        /// <param name="name"></param>
        private AppUser(string email, string name, DateTime createdAtUtc, DateTime updatedAtUtc)
        {
            Email = email;
            Name = name;
            CreatedAtUtc = createdAtUtc;
            UpdatedAtUtc = updatedAtUtc;

            Validate(this, new AppUserValidator());
        }

        public static AppUser Create(string email, string name)
        {
            return new AppUser(email, name, DateTime.UtcNow, DateTime.UtcNow);
        }

        public static AppUser CreateResponse(string email, string name, 
                                            DateTime createdAtUtc, DateTime updatedAtUtc)
        {
            return new AppUser(email, name, createdAtUtc, updatedAtUtc);
        }

        public void Update(string name)
        {
            Name = name;
        }

        public void SetPassword(string password)
        {
            Password = password;
        }

        internal class AppUserValidator : AbstractValidator<AppUser>
        {
            public AppUserValidator()
            {
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
