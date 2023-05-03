using m01_Start.Models;

namespace m01_Start.Services
{
     public class PlanetService : List<PlanetModel>
     {
          public PlanetService()
          {
               Add(new PlanetModel()
               {
                    id =1,
                    name = "Mercury",
                    content = "Mercury (0.307–0.588 AU (45.9–88.0 million km; 28.5–54.7 million mi) from the Sun) is the closest planet to the Sun. The smallest planet in the Solar System (0.055 M), Mercury has no natural satellites. The dominant geological features are impact craters or basins with ejecta blankets, the remains of early volcanic activity including magma flows, and lobed ridges or rupes that were probably produced by a period of contraction early in the planet's history. Mercury's very tenuous atmosphere consists of solar-wind particles trapped by Mercury's magnetic field, as well as atoms blasted off its surface by the solar wind. Its relatively large iron core and thin mantle have not yet been adequately explained. Hypotheses include that its outer layers were stripped off by a giant impact, or that it was prevented from fully accreting by the young Sun's energy"
               });
               Add(new PlanetModel()
               {
                    id = 2,
                    name = "Venus",
                    content = "Venus (0.718–0.728 AU (107.4–108.9 million km; 66.7–67.7 million mi) from the Sun) is close in size to Earth (0.815 M) and, like Earth, has a thick silicate mantle around an iron core, a substantial atmosphere, and evidence of internal geological activity. It is much drier than Earth, and its atmosphere is ninety times as dense. Venus has no natural satellites. It is the hottest planet, with surface temperatures over 400 °C (752 °F), mainly due to the amount of greenhouse gases in the atmosphere. The planet has no magnetic field that would prevent depletion of its substantial atmosphere, which suggests that its atmosphere is being replenished by volcanic eruptions. A relatively young planetary surface displays extensive evidence of volcanic activity, but is devoid of plate tectonics. It may undergo resurfacing episodes on a time scale of 700 million years."
               });
               Add(new PlanetModel()
               {
                    id = 3,
                    name = "Earth",
                    content = "Earth (0.983–1.017 AU (147.1–152.1 million km; 91.4–94.5 million mi) from the Sun) is the largest and densest of the inner planets, the only one known to have current geological activity, and the only place where life is known to exist. Its liquid hydrosphere is unique among the terrestrial planets, and it is the only planet where plate tectonics has been observed. Earth's atmosphere is radically different from those of the other planets, having been altered by the presence of life to contain 21% free oxygen. The planetary magnetosphere shields the surface from solar and cosmic radiation, limiting atmospheric stripping and maintaining habitability. It has one natural satellite, the Moon, the only large satellite of a terrestrial planet in the Solar System."
               });
          }
     }
}