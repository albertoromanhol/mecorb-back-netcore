using AutoMapper;

namespace Analytics.MaquinaCW.Tests.Fixtures
{
    public class MapperFixture
    {
        public IMapper Mapper { get; }

        public MapperFixture()
        {
            var config = new MapperConfiguration(opts =>
            {
                opts.AddProfile(new Application.Mapping.ExemploMap());
                opts.AddProfile(new Application.Mapping.ClientMap());
            });

            Mapper = config.CreateMapper();
        }
    }
}
