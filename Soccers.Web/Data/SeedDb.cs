using Microsoft.EntityFrameworkCore;
using Soccers.Common.Enums;
using Soccers.Web.Data.Entities;
using Soccers.Web.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Soccers.Web.Data
{
    public class SeedDb
    {
        private readonly DataContext _dataContext;
        private readonly IImageHelper _imageHelper;
        private readonly IUserHelper _userHelper;
        private readonly Random _random;

        public SeedDb(DataContext dataContext, IImageHelper imageHelper, IUserHelper userHelper)
        {
            _dataContext = dataContext;
            _imageHelper = imageHelper;
            _userHelper = userHelper;
            _random = new Random();
        }
        public async Task SeedAsync()
        {
            await _dataContext.Database.EnsureCreatedAsync();
            await CheckRolesAsync();
            await CheckTeamsAsync();
            await CheckTournamentsAsync();
            await CheckUserAsync("911", "draco", "Orochi", "draco.orochi@jpmail.com", "(21-11: 00) 03 5774 0992", "Castillo Edo, el Palacio Imperial de Tokio", UserType.Admin, "Xamarin.jpg");
            await CheckUsersAsync();
            await CheckPreditionsAsync();
        }

        private async Task CheckUsersAsync(){
            List<Photo> photos = LoadPhotos();
            int i = 0;
            foreach (Photo photo in photos){
                i++;
               await CheckUserAsync($"100{i}", photo.Firstname, photo.Lastname, $"user{i}@yopmail.com", "350 634 2747", "Calle Luna Calle Sol", UserType.User, photo.Image);
            }
        }
        private List<Photo> LoadPhotos()
        {
            return new List<Photo>
            {
                new Photo { Firstname = "Adala", Lastname = "Samir", Image = "Adala.jpg" },
                new Photo { Firstname = "Amalia", Lastname = "Lopez", Image = "Amalia.jpg" },
                new Photo { Firstname = "Camila", Lastname = "Cardona", Image = "Camila.jpg" },
                new Photo { Firstname = "Carolina", Lastname = "Echavarria", Image = "Carolina.jpg" },
                new Photo { Firstname = "Claudia", Lastname = "Sanchez", Image = "Claudia.jpg" },
                new Photo { Firstname = "Juan", Lastname = "Zuluaga", Image = "Zulu.jpg" },
                new Photo { Firstname = "Gilberto", Lastname = "Medez", Image = "Gilberto.jpg" },
                new Photo { Firstname = "Jhon", Lastname = "Smith", Image = "Jhon.jpg" },
                new Photo { Firstname = "Ken", Lastname = "Rogers", Image = "Ken.jpg" },
                new Photo { Firstname = "Laura", Lastname = "Zuluaga", Image = "Laura.jpg" },
                new Photo { Firstname = "Luisa", Lastname = "Zapata", Image = "Luisa.jpg" },
                new Photo { Firstname = "Manuel", Lastname = "Rodriguez", Image = "Manuel.jpg" },
                new Photo { Firstname = "Manuela", Lastname = "Ateortua", Image = "Manuela.jpg" },
                new Photo { Firstname = "Mario", Lastname = "Bedoya", Image = "Mario.jpg" },
                new Photo { Firstname = "Monica", Lastname = "Cano", Image = "Monica.jpg" },
                new Photo { Firstname = "Pedro", Lastname = "Correa", Image = "Pedro.jpg" },
                new Photo { Firstname = "Penelope", Lastname = "Arias", Image = "Penelope.jpg" },
                new Photo { Firstname = "Pepe", Lastname = "Lopez", Image = "Pepe.jpg" },
                new Photo { Firstname = "Raul", Lastname = "Matinez", Image = "Raul.jpg" },
                new Photo { Firstname = "Roberto", Lastname = "Rivas", Image = "Roberto.jpg" },
                new Photo { Firstname = "Rosa", Lastname = "Velasquez", Image = "Rosa.jpg" },
                new Photo { Firstname = "Rosario", Lastname = "Sandoval", Image = "Rosario.jpg" },
                new Photo { Firstname = "Sandra", Lastname = "Machado", Image = "Sandra.jpg" },
                new Photo { Firstname = "Sandro", Lastname = "Ruiz", Image = "Sandro.jpg" },
                new Photo { Firstname = "Teresa", Lastname = "Santamaria", Image = "Teresa.jpg" }
            };
        }

        private async Task<UserEntity> CheckUserAsync(
           string document,
           string firstName,
           string lastName,
           string email,
           string phone,
           string address,
           UserType userType,
           string image)
        {
            try
            {
                UserEntity user = await _userHelper.GetUserAsync(email);
                if (user == null)
                {
                    string path = $"~/images/Users/{image}";//Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot\\images\\Users", image);
                    string imageId = path;//await _blobHelper.UploadBlobAsync(path, "users");

                    int totalTeams = _dataContext.Teams.Count();
                    int random = _random.Next(1, _dataContext.Teams.Count());
                    TeamEntity team = _dataContext.Teams.FirstOrDefault(t => t.Id == random);

                    user = new UserEntity
                    {
                        FirstName = firstName,
                        LastName = lastName,
                        Email = email,
                        UserName = email,
                        PhoneNumber = phone,
                        Address = address,
                        Document = document,
                        Team = team,
                        UserType = userType,
                        ImagePath = imageId
                    };

                    await _userHelper.AddUserAsync(user, "D123456");
                    await _userHelper.AddUserToRoleAsync(user, userType.ToString());

                    //string token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                    //await _userHelper.ConfirmEmailAsync(user, token);
                }

                return user;
            }
            catch (Exception excep)
            {

                var r= excep.Message;
                return null;
            }
            
        }
        private async Task CheckRolesAsync()
        {
            await _userHelper.CheckRoleAsync(UserType.Admin.ToString());
            await _userHelper.CheckRoleAsync(UserType.User.ToString());
        }

        private async Task CheckTeamsAsync()
        {
            if (!_dataContext.Teams.Any())
            {
                await AddTeamAsync("Ajax");
                await AddTeamAsync("America");
                await AddTeamAsync("Argentina");
                await AddTeamAsync("Barcelona");
                await AddTeamAsync("Bayer Leverkusen");
                await AddTeamAsync("Bolivia");
                await AddTeamAsync("Borussia Dortmund");
                await AddTeamAsync("Brasil");
                await AddTeamAsync("Bucaramanga");
                await AddTeamAsync("Canada");
                await AddTeamAsync("Chelsea");
                await AddTeamAsync("Chile");
                await AddTeamAsync("Colombia");
                await AddTeamAsync("Costa Rica");
                await AddTeamAsync("Ecuador");
                await AddTeamAsync("Honduras");
                await AddTeamAsync("Inter Milan");
                await AddTeamAsync("Junior");
                await AddTeamAsync("Juventus");
                await AddTeamAsync("Liverpool");
                await AddTeamAsync("Medellin");
                await AddTeamAsync("Mexico");
                await AddTeamAsync("Millonarios");
                await AddTeamAsync("Nacional");
                await AddTeamAsync("Once Caldas");
                await AddTeamAsync("Panama");
                await AddTeamAsync("Paraguay");
                await AddTeamAsync("Peru");
                await AddTeamAsync("PSG");
                await AddTeamAsync("Real Madrid");
                await AddTeamAsync("Santa Fe");
                await AddTeamAsync("Uruguay");
                await AddTeamAsync("USA");
                await AddTeamAsync("Venezuela");
                await _dataContext.SaveChangesAsync();
            }
        }

        private async Task AddTeamAsync(string name)
        {
            string path = $"~/images/Teams/{name}.jpg";//Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot\\images\\Teams", $"{name}.jpg");
            string imageId = path;//await _blobHelper.UploadBlobAsync(path, "teams");
            _dataContext.Teams.Add(new TeamEntity { Name = name, LogoPath = imageId });
            var _Result = await _dataContext.Teams.Take(4).ToListAsync();
        }

        private void AddTeam(string name)
        {
            _dataContext.Teams.Add(new TeamEntity { Name = name, LogoPath = $"~/images/Teams/{name}.jpg" });
        }

        private async Task CheckTournamentsAsync()
        {
            try
            {
                if (!_dataContext.Tournaments.Any())
                {
                    DateTime startDate = DateTime.Today.AddMonths(2).ToUniversalTime();
                    DateTime endDate = DateTime.Today.AddMonths(3).ToUniversalTime();

                    string imageIdCopaAmerica = await UploadImageTournamentAsync("Copa America 2020.png");
                    string imageIdLigaAguila = await UploadImageTournamentAsync("Liga Aguila 2020-I.png");
                    string imageIdChampions = await UploadImageTournamentAsync("Champions 2020.png");

                    _dataContext.Tournaments.Add(new TournamentEntity
                    {
                        StartDate = startDate,
                        EndDate = endDate,
                        IsActive = true,
                        LogoPath = imageIdCopaAmerica,
                        Name = "Copa America 2020",
                        Groups = new List<GroupEntity>
                    {
                        new GroupEntity
                        {
                             Name = "A",
                             GroupDetails = new List<GroupDetailEntity>
                             {
                                 new GroupDetailEntity { Team = _dataContext.Teams.FirstOrDefault(t => t.Name == "Colombia") },
                                 new GroupDetailEntity { Team = _dataContext.Teams.FirstOrDefault(t => t.Name == "Ecuador") },
                                 new GroupDetailEntity { Team = _dataContext.Teams.FirstOrDefault(t => t.Name == "Panama") },
                                 new GroupDetailEntity { Team = _dataContext.Teams.FirstOrDefault(t => t.Name == "Canada") }
                             },
                             Matches = new List<MatchEntity>
                             {
                                 new MatchEntity
                                 {
                                     Date = startDate.AddHours(14),
                                     Local = _dataContext.Teams.FirstOrDefault(t => t.Name == "Colombia"),
                                     Visitor = _dataContext.Teams.FirstOrDefault(t => t.Name == "Ecuador")
                                 },
                                 new MatchEntity
                                 {
                                     Date = startDate.AddHours(17),
                                     Local = _dataContext.Teams.FirstOrDefault(t => t.Name == "Panama"),
                                     Visitor = _dataContext.Teams.FirstOrDefault(t => t.Name == "Canada")
                                 },
                                 new MatchEntity
                                 {
                                     Date = startDate.AddDays(4).AddHours(14),
                                     Local = _dataContext.Teams.FirstOrDefault(t => t.Name == "Colombia"),
                                     Visitor = _dataContext.Teams.FirstOrDefault(t => t.Name == "Panama")
                                 },
                                 new MatchEntity
                                 {
                                     Date = startDate.AddDays(4).AddHours(17),
                                     Local = _dataContext.Teams.FirstOrDefault(t => t.Name == "Ecuador"),
                                     Visitor = _dataContext.Teams.FirstOrDefault(t => t.Name == "Canada")
                                 },
                                 new MatchEntity
                                 {
                                     Date = startDate.AddDays(9).AddHours(16),
                                     Local = _dataContext.Teams.FirstOrDefault(t => t.Name == "Canada"),
                                     Visitor = _dataContext.Teams.FirstOrDefault(t => t.Name == "Colombia")
                                 },
                                 new MatchEntity
                                 {
                                     Date = startDate.AddDays(9).AddHours(16),
                                     Local = _dataContext.Teams.FirstOrDefault(t => t.Name == "Ecuador"),
                                     Visitor = _dataContext.Teams.FirstOrDefault(t => t.Name == "Panama")
                                 }
                             }
                        },
                        new GroupEntity
                        {
                             Name = "B",
                             GroupDetails = new List<GroupDetailEntity>
                             {
                                 new GroupDetailEntity { Team = _dataContext.Teams.FirstOrDefault(t => t.Name == "Argentina") },
                                 new GroupDetailEntity { Team = _dataContext.Teams.FirstOrDefault(t => t.Name == "Paraguay") },
                                 new GroupDetailEntity { Team = _dataContext.Teams.FirstOrDefault(t => t.Name == "Mexico") },
                                 new GroupDetailEntity { Team = _dataContext.Teams.FirstOrDefault(t => t.Name == "Chile") }
                             },
                             Matches = new List<MatchEntity>
                             {
                                 new MatchEntity
                                 {
                                     Date = startDate.AddDays(1).AddHours(14),
                                     Local = _dataContext.Teams.FirstOrDefault(t => t.Name == "Argentina"),
                                     Visitor = _dataContext.Teams.FirstOrDefault(t => t.Name == "Paraguay")
                                 },
                                 new MatchEntity
                                 {
                                     Date = startDate.AddDays(1).AddHours(17),
                                     Local = _dataContext.Teams.FirstOrDefault(t => t.Name == "Mexico"),
                                     Visitor = _dataContext.Teams.FirstOrDefault(t => t.Name == "Chile")
                                 },
                                 new MatchEntity
                                 {
                                     Date = startDate.AddDays(5).AddHours(14),
                                     Local = _dataContext.Teams.FirstOrDefault(t => t.Name == "Argentina"),
                                     Visitor = _dataContext.Teams.FirstOrDefault(t => t.Name == "Mexico")
                                 },
                                 new MatchEntity
                                 {
                                     Date = startDate.AddDays(5).AddHours(17),
                                     Local = _dataContext.Teams.FirstOrDefault(t => t.Name == "Paraguay"),
                                     Visitor = _dataContext.Teams.FirstOrDefault(t => t.Name == "Chile")
                                 },
                                 new MatchEntity
                                 {
                                     Date = startDate.AddDays(10).AddHours(16),
                                     Local = _dataContext.Teams.FirstOrDefault(t => t.Name == "Chile"),
                                     Visitor = _dataContext.Teams.FirstOrDefault(t => t.Name == "Argentina")
                                 },
                                 new MatchEntity
                                 {
                                     Date = startDate.AddDays(10).AddHours(16),
                                     Local = _dataContext.Teams.FirstOrDefault(t => t.Name == "Paraguay"),
                                     Visitor = _dataContext.Teams.FirstOrDefault(t => t.Name == "Mexico")
                                 }
                             }
                        },
                        new GroupEntity
                        {
                             Name = "C",
                             GroupDetails = new List<GroupDetailEntity>
                             {
                                 new GroupDetailEntity { Team = _dataContext.Teams.FirstOrDefault(t => t.Name == "Brasil") },
                                 new GroupDetailEntity { Team = _dataContext.Teams.FirstOrDefault(t => t.Name == "Venezuela") },
                                 new GroupDetailEntity { Team = _dataContext.Teams.FirstOrDefault(t => t.Name == "USA") },
                                 new GroupDetailEntity { Team = _dataContext.Teams.FirstOrDefault(t => t.Name == "Peru") }
                             },
                             Matches = new List<MatchEntity>
                             {
                                 new MatchEntity
                                 {
                                     Date = startDate.AddDays(2).AddHours(14),
                                     Local = _dataContext.Teams.FirstOrDefault(t => t.Name == "Brasil"),
                                     Visitor = _dataContext.Teams.FirstOrDefault(t => t.Name == "Venezuela")
                                 },
                                 new MatchEntity
                                 {
                                     Date = startDate.AddDays(2).AddHours(17),
                                     Local = _dataContext.Teams.FirstOrDefault(t => t.Name == "USA"),
                                     Visitor = _dataContext.Teams.FirstOrDefault(t => t.Name == "Peru")
                                 },
                                 new MatchEntity
                                 {
                                     Date = startDate.AddDays(6).AddHours(14),
                                     Local = _dataContext.Teams.FirstOrDefault(t => t.Name == "Brasil"),
                                     Visitor = _dataContext.Teams.FirstOrDefault(t => t.Name == "USA")
                                 },
                                 new MatchEntity
                                 {
                                     Date = startDate.AddDays(6).AddHours(17),
                                     Local = _dataContext.Teams.FirstOrDefault(t => t.Name == "Venezuela"),
                                     Visitor = _dataContext.Teams.FirstOrDefault(t => t.Name == "Peru")
                                 },
                                 new MatchEntity
                                 {
                                     Date = startDate.AddDays(11).AddHours(16),
                                     Local = _dataContext.Teams.FirstOrDefault(t => t.Name == "Peru"),
                                     Visitor = _dataContext.Teams.FirstOrDefault(t => t.Name == "Brasil")
                                 },
                                 new MatchEntity
                                 {
                                     Date = startDate.AddDays(11).AddHours(16),
                                     Local = _dataContext.Teams.FirstOrDefault(t => t.Name == "Venezuela"),
                                     Visitor = _dataContext.Teams.FirstOrDefault(t => t.Name == "USA")
                                 }
                             }
                        },
                        new GroupEntity
                        {
                             Name = "D",
                             GroupDetails = new List<GroupDetailEntity>
                             {
                                 new GroupDetailEntity { Team = _dataContext.Teams.FirstOrDefault(t => t.Name == "Uruguay") },
                                 new GroupDetailEntity { Team = _dataContext.Teams.FirstOrDefault(t => t.Name == "Bolivia") },
                                 new GroupDetailEntity { Team = _dataContext.Teams.FirstOrDefault(t => t.Name == "Costa Rica") },
                                 new GroupDetailEntity { Team = _dataContext.Teams.FirstOrDefault(t => t.Name == "Honduras") }
                             },
                             Matches = new List<MatchEntity>
                             {
                                 new MatchEntity
                                 {
                                     Date = startDate.AddDays(3).AddHours(14),
                                     Local = _dataContext.Teams.FirstOrDefault(t => t.Name == "Uruguay"),
                                     Visitor = _dataContext.Teams.FirstOrDefault(t => t.Name == "Bolivia")
                                 },
                                 new MatchEntity
                                 {
                                     Date = startDate.AddDays(3).AddHours(17),
                                     Local = _dataContext.Teams.FirstOrDefault(t => t.Name == "Costa Rica"),
                                     Visitor = _dataContext.Teams.FirstOrDefault(t => t.Name == "Honduras")
                                 },
                                 new MatchEntity
                                 {
                                     Date = startDate.AddDays(7).AddHours(14),
                                     Local = _dataContext.Teams.FirstOrDefault(t => t.Name == "Uruguay"),
                                     Visitor = _dataContext.Teams.FirstOrDefault(t => t.Name == "Costa Rica")
                                 },
                                 new MatchEntity
                                 {
                                     Date = startDate.AddDays(7).AddHours(17),
                                     Local = _dataContext.Teams.FirstOrDefault(t => t.Name == "Bolivia"),
                                     Visitor = _dataContext.Teams.FirstOrDefault(t => t.Name == "Honduras")
                                 },
                                 new MatchEntity
                                 {
                                     Date = startDate.AddDays(12).AddHours(16),
                                     Local = _dataContext.Teams.FirstOrDefault(t => t.Name == "Honduras"),
                                     Visitor = _dataContext.Teams.FirstOrDefault(t => t.Name == "Uruguay")
                                 },
                                 new MatchEntity
                                 {
                                     Date = startDate.AddDays(12).AddHours(16),
                                     Local = _dataContext.Teams.FirstOrDefault(t => t.Name == "Bolivia"),
                                     Visitor = _dataContext.Teams.FirstOrDefault(t => t.Name == "Costa Rica")
                                 }
                             }
                        }
                    }
                    });

                    startDate = DateTime.Today.AddMonths(1).ToUniversalTime();
                    endDate = DateTime.Today.AddMonths(4).ToUniversalTime();

                    _dataContext.Tournaments.Add(new TournamentEntity
                    {
                        StartDate = startDate,
                        EndDate = endDate,
                        IsActive = true,
                        LogoPath = imageIdLigaAguila,
                        Name = "Liga Aguila 2020-I",
                        Groups = new List<GroupEntity>
                    {
                        new GroupEntity
                        {
                             Name = "A",
                             GroupDetails = new List<GroupDetailEntity>
                             {
                                 new GroupDetailEntity { Team = _dataContext.Teams.FirstOrDefault(t => t.Name == "America") },
                                 new GroupDetailEntity { Team = _dataContext.Teams.FirstOrDefault(t => t.Name == "Bucaramanga") },
                                 new GroupDetailEntity { Team = _dataContext.Teams.FirstOrDefault(t => t.Name == "Junior") },
                                 new GroupDetailEntity { Team = _dataContext.Teams.FirstOrDefault(t => t.Name == "Medellin") }
                             },
                             Matches = new List<MatchEntity>
                             {
                                 new MatchEntity
                                 {
                                     Date = startDate.AddHours(14),
                                     Local = _dataContext.Teams.FirstOrDefault(t => t.Name == "America"),
                                     Visitor = _dataContext.Teams.FirstOrDefault(t => t.Name == "Bucaramanga")
                                 },
                                 new MatchEntity
                                 {
                                     Date = startDate.AddHours(17),
                                     Local = _dataContext.Teams.FirstOrDefault(t => t.Name == "Junior"),
                                     Visitor = _dataContext.Teams.FirstOrDefault(t => t.Name == "Medellin")
                                 },
                                 new MatchEntity
                                 {
                                     Date = startDate.AddDays(4).AddHours(14),
                                     Local = _dataContext.Teams.FirstOrDefault(t => t.Name == "America"),
                                     Visitor = _dataContext.Teams.FirstOrDefault(t => t.Name == "Junior")
                                 },
                                 new MatchEntity
                                 {
                                     Date = startDate.AddDays(4).AddHours(17),
                                     Local = _dataContext.Teams.FirstOrDefault(t => t.Name == "Bucaramanga"),
                                     Visitor = _dataContext.Teams.FirstOrDefault(t => t.Name == "Medellin")
                                 },
                                 new MatchEntity
                                 {
                                     Date = startDate.AddDays(9).AddHours(14),
                                     Local = _dataContext.Teams.FirstOrDefault(t => t.Name == "Medellin"),
                                     Visitor = _dataContext.Teams.FirstOrDefault(t => t.Name == "America")
                                 },
                                 new MatchEntity
                                 {
                                     Date = startDate.AddDays(9).AddHours(17),
                                     Local = _dataContext.Teams.FirstOrDefault(t => t.Name == "Bucaramanga"),
                                     Visitor = _dataContext.Teams.FirstOrDefault(t => t.Name == "Junior")
                                 },
                                 new MatchEntity
                                 {
                                     Date = startDate.AddDays(15).AddHours(14),
                                     Local = _dataContext.Teams.FirstOrDefault(t => t.Name == "Bucaramanga"),
                                     Visitor = _dataContext.Teams.FirstOrDefault(t => t.Name == "America")
                                 },
                                 new MatchEntity
                                 {
                                     Date = startDate.AddDays(15).AddHours(17),
                                     Local = _dataContext.Teams.FirstOrDefault(t => t.Name == "Medellin"),
                                     Visitor = _dataContext.Teams.FirstOrDefault(t => t.Name == "Junior")
                                 },
                                 new MatchEntity
                                 {
                                     Date = startDate.AddDays(19).AddHours(14),
                                     Local = _dataContext.Teams.FirstOrDefault(t => t.Name == "Junior"),
                                     Visitor = _dataContext.Teams.FirstOrDefault(t => t.Name == "America")
                                 },
                                 new MatchEntity
                                 {
                                     Date = startDate.AddDays(19).AddHours(17),
                                     Local = _dataContext.Teams.FirstOrDefault(t => t.Name == "Medellin"),
                                     Visitor = _dataContext.Teams.FirstOrDefault(t => t.Name == "Bucaramanga")
                                 },
                                 new MatchEntity
                                 {
                                     Date = startDate.AddDays(19).AddHours(14),
                                     Local = _dataContext.Teams.FirstOrDefault(t => t.Name == "America"),
                                     Visitor = _dataContext.Teams.FirstOrDefault(t => t.Name == "Medellin")
                                 },
                                 new MatchEntity
                                 {
                                     Date = startDate.AddDays(19).AddHours(17),
                                     Local = _dataContext.Teams.FirstOrDefault(t => t.Name == "Junior"),
                                     Visitor = _dataContext.Teams.FirstOrDefault(t => t.Name == "Bucaramanga")
                                 }
                             }
                        },
                        new GroupEntity
                        {
                             Name = "B",
                             GroupDetails = new List<GroupDetailEntity>
                             {
                                 new GroupDetailEntity { Team = _dataContext.Teams.FirstOrDefault(t => t.Name == "Millonarios") },
                                 new GroupDetailEntity { Team = _dataContext.Teams.FirstOrDefault(t => t.Name == "Nacional") },
                                 new GroupDetailEntity { Team = _dataContext.Teams.FirstOrDefault(t => t.Name == "Once Caldas") },
                                 new GroupDetailEntity { Team = _dataContext.Teams.FirstOrDefault(t => t.Name == "Santa Fe") }
                             },
                             Matches = new List<MatchEntity>
                             {
                                 new MatchEntity
                                 {
                                     Date = startDate.AddDays(1).AddHours(14),
                                     Local = _dataContext.Teams.FirstOrDefault(t => t.Name == "Millonarios"),
                                     Visitor = _dataContext.Teams.FirstOrDefault(t => t.Name == "Nacional")
                                 },
                                 new MatchEntity
                                 {
                                     Date = startDate.AddDays(1).AddHours(17),
                                     Local = _dataContext.Teams.FirstOrDefault(t => t.Name == "Once Caldas"),
                                     Visitor = _dataContext.Teams.FirstOrDefault(t => t.Name == "Santa Fe")
                                 },
                                 new MatchEntity
                                 {
                                     Date = startDate.AddDays(5).AddHours(14),
                                     Local = _dataContext.Teams.FirstOrDefault(t => t.Name == "Millonarios"),
                                     Visitor = _dataContext.Teams.FirstOrDefault(t => t.Name == "Once Caldas")
                                 },
                                 new MatchEntity
                                 {
                                     Date = startDate.AddDays(5).AddHours(17),
                                     Local = _dataContext.Teams.FirstOrDefault(t => t.Name == "Nacional"),
                                     Visitor = _dataContext.Teams.FirstOrDefault(t => t.Name == "Santa Fe")
                                 },
                                 new MatchEntity
                                 {
                                     Date = startDate.AddDays(10).AddHours(16),
                                     Local = _dataContext.Teams.FirstOrDefault(t => t.Name == "Santa Fe"),
                                     Visitor = _dataContext.Teams.FirstOrDefault(t => t.Name == "Millonarios")
                                 },
                                 new MatchEntity
                                 {
                                     Date = startDate.AddDays(10).AddHours(16),
                                     Local = _dataContext.Teams.FirstOrDefault(t => t.Name == "Nacional"),
                                     Visitor = _dataContext.Teams.FirstOrDefault(t => t.Name == "Once Caldas")
                                 },
                                 new MatchEntity
                                 {
                                     Date = startDate.AddDays(16).AddHours(14),
                                     Local = _dataContext.Teams.FirstOrDefault(t => t.Name == "Nacional"),
                                     Visitor = _dataContext.Teams.FirstOrDefault(t => t.Name == "Millonarios")
                                 },
                                 new MatchEntity
                                 {
                                     Date = startDate.AddDays(16).AddHours(17),
                                     Local = _dataContext.Teams.FirstOrDefault(t => t.Name == "Santa Fe"),
                                     Visitor = _dataContext.Teams.FirstOrDefault(t => t.Name == "Once Caldas")
                                 },
                                 new MatchEntity
                                 {
                                     Date = startDate.AddDays(20).AddHours(14),
                                     Local = _dataContext.Teams.FirstOrDefault(t => t.Name == "Once Caldas"),
                                     Visitor = _dataContext.Teams.FirstOrDefault(t => t.Name == "Millonarios")
                                 },
                                 new MatchEntity
                                 {
                                     Date = startDate.AddDays(20).AddHours(17),
                                     Local = _dataContext.Teams.FirstOrDefault(t => t.Name == "Santa Fe"),
                                     Visitor = _dataContext.Teams.FirstOrDefault(t => t.Name == "Nacional")
                                 },
                                 new MatchEntity
                                 {
                                     Date = startDate.AddDays(35).AddHours(16),
                                     Local = _dataContext.Teams.FirstOrDefault(t => t.Name == "Millonarios"),
                                     Visitor = _dataContext.Teams.FirstOrDefault(t => t.Name == "Santa Fe")
                                 },
                                 new MatchEntity
                                 {
                                     Date = startDate.AddDays(35).AddHours(16),
                                     Local = _dataContext.Teams.FirstOrDefault(t => t.Name == "Once Caldas"),
                                     Visitor = _dataContext.Teams.FirstOrDefault(t => t.Name == "Nacional")
                                 }
                             }
                        }
                    }
                    });

                    startDate = DateTime.Today.AddMonths(1).ToUniversalTime();
                    endDate = DateTime.Today.AddMonths(2).ToUniversalTime();

                    _dataContext.Tournaments.Add(new TournamentEntity
                    {
                        StartDate = startDate,
                        EndDate = endDate,
                        IsActive = true,
                        LogoPath = imageIdChampions,
                        Name = "Champions 2020",
                        Groups = new List<GroupEntity>
                    {
                        new GroupEntity
                        {
                             Name = "A",
                             GroupDetails = new List<GroupDetailEntity>
                             {
                                 new GroupDetailEntity { Team = _dataContext.Teams.FirstOrDefault(t => t.Name == "Ajax") },
                                 new GroupDetailEntity { Team = _dataContext.Teams.FirstOrDefault(t => t.Name == "Barcelona") }
                             },
                             Matches = new List<MatchEntity>
                             {
                                 new MatchEntity
                                 {
                                     Date = startDate.AddHours(14),
                                     Local = _dataContext.Teams.FirstOrDefault(t => t.Name == "Ajax"),
                                     Visitor = _dataContext.Teams.FirstOrDefault(t => t.Name == "Barcelona")
                                 },
                                 new MatchEntity
                                 {
                                     Date = startDate.AddHours(17),
                                     Local = _dataContext.Teams.FirstOrDefault(t => t.Name == "Barcelona"),
                                     Visitor = _dataContext.Teams.FirstOrDefault(t => t.Name == "Ajax")
                                 }
                             }
                        },
                        new GroupEntity
                        {
                             Name = "B",
                             GroupDetails = new List<GroupDetailEntity>
                             {
                                 new GroupDetailEntity { Team = _dataContext.Teams.FirstOrDefault(t => t.Name == "Bayer Leverkusen") },
                                 new GroupDetailEntity { Team = _dataContext.Teams.FirstOrDefault(t => t.Name == "Chelsea") }
                             },
                             Matches = new List<MatchEntity>
                             {
                                 new MatchEntity
                                 {
                                     Date = startDate.AddDays(1).AddHours(14),
                                     Local = _dataContext.Teams.FirstOrDefault(t => t.Name == "Bayer Leverkusen"),
                                     Visitor = _dataContext.Teams.FirstOrDefault(t => t.Name == "Chelsea")
                                 },
                                 new MatchEntity
                                 {
                                     Date = startDate.AddDays(1).AddHours(17),
                                     Local = _dataContext.Teams.FirstOrDefault(t => t.Name == "Chelsea"),
                                     Visitor = _dataContext.Teams.FirstOrDefault(t => t.Name == "Bayer Leverkusen")
                                 }
                             }
                        },
                        new GroupEntity
                        {
                             Name = "C",
                             GroupDetails = new List<GroupDetailEntity>
                             {
                                 new GroupDetailEntity { Team = _dataContext.Teams.FirstOrDefault(t => t.Name == "Borussia Dortmund") },
                                 new GroupDetailEntity { Team = _dataContext.Teams.FirstOrDefault(t => t.Name == "Inter Milan") }
                             },
                             Matches = new List<MatchEntity>
                             {
                                 new MatchEntity
                                 {
                                     Date = startDate.AddDays(1).AddHours(14),
                                     Local = _dataContext.Teams.FirstOrDefault(t => t.Name == "Borussia Dortmund"),
                                     Visitor = _dataContext.Teams.FirstOrDefault(t => t.Name == "Inter Milan")
                                 },
                                 new MatchEntity
                                 {
                                     Date = startDate.AddDays(1).AddHours(17),
                                     Local = _dataContext.Teams.FirstOrDefault(t => t.Name == "Inter Milan"),
                                     Visitor = _dataContext.Teams.FirstOrDefault(t => t.Name == "Borussia Dortmund")
                                 }
                             }
                        },
                        new GroupEntity
                        {
                             Name = "D",
                             GroupDetails = new List<GroupDetailEntity>
                             {
                                 new GroupDetailEntity { Team = _dataContext.Teams.FirstOrDefault(t => t.Name == "PSG") },
                                 new GroupDetailEntity { Team = _dataContext.Teams.FirstOrDefault(t => t.Name == "Real Madrid") }
                             },
                             Matches = new List<MatchEntity>
                             {
                                 new MatchEntity
                                 {
                                     Date = startDate.AddDays(1).AddHours(14),
                                     Local = _dataContext.Teams.FirstOrDefault(t => t.Name == "PSG"),
                                     Visitor = _dataContext.Teams.FirstOrDefault(t => t.Name == "Real Madrid")
                                 },
                                 new MatchEntity
                                 {
                                     Date = startDate.AddDays(1).AddHours(17),
                                     Local = _dataContext.Teams.FirstOrDefault(t => t.Name == "Real Madrid"),
                                     Visitor = _dataContext.Teams.FirstOrDefault(t => t.Name == "PSG")
                                 }
                             }
                        }
                    }
                    });

                    await _dataContext.SaveChangesAsync();
                }
            }
            catch (Exception excep)
            {

                var r = excep.Message;
            }
        }
        private async Task<string> UploadImageTournamentAsync(string name)
        {
            string path = $"~/images/Tournaments/{name}";//Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot\\images\\Tournaments", name);
            var _Result = await _dataContext.Tournaments.Take(2).ToListAsync();
            return path;//await _blobHelper.UploadBlobAsync(path, "tournaments");

        }

        private class Photo
        {
            public string Firstname { get; set; }

            public string Lastname { get; set; }

            public string Image { get; set; }
        }
        private async Task CheckPreditionsAsync()
        {
            try
            {
                if (!_dataContext.Predictions.Any())
                {
                    var usersList = await _dataContext.Users.ToListAsync();
                    foreach (UserEntity user in usersList)
                    {
                        if (user.UserType == UserType.User)
                        {
                            AddPrediction(user);
                        }
                    }

                    await _dataContext.SaveChangesAsync();
                }
            }
            catch (Exception excep)
            {

                var r= excep.Message;
            }
        }

        private void AddPrediction(UserEntity user)
        {
            Random random = new Random();
            var MatchesList = _dataContext.Matches.ToList();
            foreach (MatchEntity match in MatchesList)
            {
                _dataContext.Predictions.Add(new PredictionEntity
                {
                    GoalsLocal = random.Next(0, 5),
                    GoalsVisitor = random.Next(0, 5),
                    Match = match,
                    User = user
                });
            }
        }
    }
}
