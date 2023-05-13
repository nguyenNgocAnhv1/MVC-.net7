-- retun view...
-- viewbag is more stronger than view
-- temdata
-- config the system cshtml url (program.cs)

==> M02
+ Config 404 error page
+ Mapget Method
+ Constraint url
+ Config attribute route
+ Url Actionlink, asp tag helper
+ Area
+ command line: 
     dotnet tool install -g dotnet-aspnet-codegenerator
     dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design
     dotnet aspnet-codegenerator -h
     dotnet aspnet-codegenerator controller -name Contact -namespace m01_Start.Controllers.Contact -m m01_Start.Models.Contact -udl -dc App.AppDbContext -outDir Areas/Contact/Controllers
     dotnet aspnet-codegenerator area Database
     dotnet aspnet-codegenerator controller -name Blog -namespace m01_Start.Controllers.Blog -m App.Models.Category -udl -dc App.AppDbContext -outDir Areas/Blog/Controllers
     dotnet aspnet-codegenerator controller -name PostController -namespace m01_Start.Controllers.Blog -m App.Models.Post -udl -dc App.AppDbContext -outDir Areas/Blog/Controllers

==> M08 libman
+dotnet tool install -g Microsoft.Web.LibraryManager.Cli
+ libman init
+ Config  by hand made
+ libman restore


==> M08: Use summerload
     + Import lib js and css, by libman
     + Summernote is ease to use, check  the Summernote Home to view more

==> M10: Elfinder
     + Add lib in libman 
      {
      "library": "jqueryui@1.12.1",
      "destination": "wwwroot/lib/jqueryui"
    },
    {
      "library": "elfinder@2.1.57",
      "destination": "wwwroot/lib/elfinder"
    }
    Config some parameter in File manage controller
    If you want to update Elfinder in summernote, you must config againt _Summernote.cshtml in shared folder

