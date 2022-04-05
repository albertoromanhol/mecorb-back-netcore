
namespace Analytics.MaquinaCW.Infrastructure.DbModels
{
    public class ClientDbModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? CodeMetric { get; set; }
        public bool Active { get; set; }        
    }
}
