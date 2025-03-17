namespace MyDMS.Domain
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Token { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool Revoked { get; set; }
    }
}
