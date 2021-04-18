using FluentValidation;
using System;

namespace BevCapital.Logon.Domain.Entities
{
    public class AppUser : Entity
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; }

        protected AppUser() { }

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="email"></param>
        private AppUser(string name, string email)
        {
            Id = Guid.NewGuid();
            Name = name;
            Email = email;

            Validate(this, new AppUserValidator());
        }

        public static AppUser Create(string name, string email)
        {
            return new AppUser(name, email);
        }

        public void Update(string name)
        {
            Name = name;
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
