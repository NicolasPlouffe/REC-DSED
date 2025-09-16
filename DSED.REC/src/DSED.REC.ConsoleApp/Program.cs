using DSED.REC.Application;
using DSED.REC.DataAccesLayer;
using DSED.REC.Entity;
using DSED.REC.Entity.IDepot;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DSED.REC.ConsoleApp
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    services.AddDbContext<ApplicationDbContext>(option =>
                        option.UseSqlServer(context.Configuration.GetConnectionString("DefaultConnection")));
                    services.AddScoped<ILeadDepot, LeadDepotDepot>();
                    services.AddScoped<LeadServiceBL>(); // ⭐ CORRECTION - Ajouter le service
                    services.AddScoped<IValidator<LeadEntity>, LeadValidator>();
                })
                .Build();
                
            Console.WriteLine("🚀 Démarrage SQL Server 🚀");

            using var scope = host.Services.CreateScope();
            var provider = scope.ServiceProvider;
            var context = provider.GetRequiredService<ApplicationDbContext>();
            
           try
{
    Console.WriteLine("🔄 Test de connexion...");
    
    // Test 1: Connexion basique
    Console.WriteLine("Test 1: Connexion basique...");
    var canConnect = await context.Database.CanConnectAsync();
    Console.WriteLine($"CanConnect: {canConnect}");
    
    if (!canConnect)
    {
        Console.WriteLine("❌ Connexion basique échouée");
        
        // Test 2: Information sur la base
        Console.WriteLine("Test 2: Tentative d'information sur la base...");
        try 
        {
            var dbName = context.Database.GetDbConnection().Database;
            Console.WriteLine($"Nom de la base: {dbName}");
            
            var connectionString = context.Database.GetDbConnection().ConnectionString;
            Console.WriteLine($"Chaîne de connexion: {connectionString}");
        }
        catch (Exception innerEx)
        {
            Console.WriteLine($"Erreur détails base: {innerEx.Message}");
        }
        return;
    }
    
    Console.WriteLine("✅ Connexion établie");
    
    // Test 3: Vérifier l'existence de la table
    Console.WriteLine("Test 3: Vérification de la table...");
    try 
    {
        var count = await context.LeadsDtos.CountAsync();
        Console.WriteLine($"Nombre de leads dans la table: {count}");
    }
    catch (Exception tableEx)
    {
        Console.WriteLine($"Erreur table: {tableEx.Message}");
        Console.WriteLine("La table n'existe probablement pas. Création du schéma...");
        
        await context.Database.EnsureCreatedAsync();
        Console.WriteLine("Schéma créé avec EnsureCreated()");
        
        // Re-test après création
        var countAfter = await context.LeadsDtos.CountAsync();
        Console.WriteLine($"Nombre de leads après création: {countAfter}");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"❌ Erreur de connexion: {ex.Message}");
    if (ex.InnerException != null)
        Console.WriteLine($"   Inner Exception: {ex.InnerException.Message}");
    return;
}


            // Test des opérations CRUD
            var leadService = provider.GetRequiredService<LeadServiceBL>(); // ⭐ CORRECTION - Nom cohérent
            
            // Créer un lead
            Guid id = Guid.NewGuid();
            var nlead = new LeadEntity(
                id,
                firstName: "P!Fou",
                lastName: "Léon", 
                email: "chocol@tine.com"
            );
            
            try
            {
                var addedLead = await leadService.CreateLead(nlead);
                Console.WriteLine($"Lead ajouté (ID : {addedLead.Id})");

                // Si tu as implémenté GetAllLeadsAsync
                // var leads = await leadService.GetAllLeadsAsync();
                // foreach (var l in leads)
                //     Console.WriteLine($"{l.Id} | {l.FirstName} {l.LastName} | {l.Email}");

                Console.WriteLine("Tous les tests CRUD réussis !");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erreur lors des opérations CRUD: {ex.Message}");
            }

            var getLead = await leadService.GetLeadById(id);
            Console.WriteLine(getLead.ToString());
            getLead.Email = "coco@rico.com";
            await leadService.UpdateLead(getLead);
            Console.WriteLine("Lead updated");
            Console.WriteLine(getLead.ToString());
            
            List<LeadEntity> leads = await leadService.GetAllLeadsAsync();

            foreach (var lead in leads)
            {
                Console.WriteLine(lead.ToString());
            }
            
        }
    }
}
