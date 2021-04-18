namespace BevCapital.Logon.Application.UseCases.User.Response
{
    public class AppUserOut<TId>
    {
        public TId Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
