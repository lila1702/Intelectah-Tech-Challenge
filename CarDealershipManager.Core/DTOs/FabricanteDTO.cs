namespace CarDealershipManager.Core.DTOs
{
    public class FabricanteDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string PaisOrigem { get; set; }
        public int AnoFundacao { get; set; }
        public string Website { get; set; }
        public int TotalVeiculos { get; set; }
    }

    public class FabricanteCreateDTO
    {
        public string Nome { get; set; }
        public string PaisOrigem { get; set; }
        public int AnoFundacao { get; set; }
        public string Website { get; set; }
    }

    public class FabricanteUpdateDTO
    {
        public string Nome { get; set; }
        public string PaisOrigem { get; set; }
        public int AnoFundacao { get; set; }
        public string Website { get; set; }
    }
}
