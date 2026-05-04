using Group3Flight.Models.DomainModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Group3Flight.Models
{
    internal class ConfigureAirline : IEntityTypeConfiguration<Airline>
    {
        public void Configure(EntityTypeBuilder<Airline> entity)
        {
            // seed initial data
            entity.HasData(
                new Airline { AirlineId = 11, Name = "Spirit Airlines", ImageName = "SpiritAirlines.png" },
                new Airline { AirlineId = 12, Name = "Frontier Airlines", ImageName = "FrontierAirlines.png" },
                new Airline { AirlineId = 13, Name = "Hawaiian Airlines", ImageName = "HawaiianAirlines.png" },
                new Airline { AirlineId = 14, Name = "Allegiant Air", ImageName = "AllegiantAir.png" },
                new Airline { AirlineId = 15, Name = "Sun Country Airlines", ImageName = "SunCountry.png" },
                new Airline { AirlineId = 16, Name = "Silver Airways", ImageName = "SilverAirways.png" }
            );
        }
    }

}
