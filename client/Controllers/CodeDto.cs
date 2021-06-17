namespace auth01.Controllers
{
    internal class CodeDto
    {
        public string grant_type { get; set; }
        public string code { get; set; }
        public string redirect_uri { get; set; }
    }
}