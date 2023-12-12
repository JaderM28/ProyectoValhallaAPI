namespace ProyectoValhallaAPI.Models.Customs
{
    public class AutorizacionResponse
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public bool Resultado { get; set; }
        public string Mensaje { get; set; }
    }
}
