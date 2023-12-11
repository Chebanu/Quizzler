using FlashCard.Model.Domain;
using FlashCard.Model.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Text.Json;

namespace FlashCard.Model;

/// <summary>
/// Logic for db
/// </summary>
public class FlashCardDbContext : IdentityDbContext<ApplicationUser>
{
	public FlashCardDbContext(DbContextOptions<FlashCardDbContext> options) : base(options)
	{
	}

	public DbSet<Language> Languages { get; set; }
	public DbSet<Level> Levels { get; set; }
	public DbSet<Word> Words { get; set; }
	public DbSet<Translation> Translations { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		foreach (IMutableEntityType entityType in modelBuilder.Model.GetEntityTypes())
		{
			modelBuilder.Entity(entityType.Name).ToTable(entityType.ClrType.Name);
		}

		//initialize primary keys
		modelBuilder.Entity<Language>().HasKey(l => l.LanguageId);
		modelBuilder.Entity<Level>().HasKey(l => l.LevelId);
		modelBuilder.Entity<Word>().HasKey(w => w.WordId);
		modelBuilder.Entity<Translation>().HasKey(t => t.TranslationId);

		//initialize foreign keys
		modelBuilder.Entity<Word>()
			.HasOne(w => w.Language)
			.WithMany()
			.HasForeignKey(w => w.LanguageId);

		modelBuilder.Entity<Word>()
			.HasOne(w => w.Level)
			.WithMany()
			.HasForeignKey(w => w.LevelId);

		modelBuilder.Entity<Translation>()
			.HasOne(t => t.SourceWord)
			.WithMany()
			.HasForeignKey(t => t.SourceWordId)
			.OnDelete(DeleteBehavior.Restrict);


		modelBuilder.Entity<Translation>()
			.HasOne(t => t.TargetWord)
			.WithMany()
			.HasForeignKey(t => t.TargetWordId);

		modelBuilder.Entity<Translation>()
			.HasIndex(w => new { w.SourceWordId, w.TargetWordId })
			.IsUnique();

		//initialize database with some data
		string languageJSON = File.ReadAllText("languages.json");
		List<Language> languages = JsonSerializer.Deserialize<List<Language>>(languageJSON);


		foreach (Language language in languages)
		{
			modelBuilder.Entity<Language>().HasData(language);
		}

		string levelJSON = File.ReadAllText("levels.json");
		List<Level> levels = JsonSerializer.Deserialize<List<Level>>(levelJSON);


		foreach (Level level in levels)
		{
			modelBuilder.Entity<Level>().HasData(level);
		}

		string wordJSON = File.ReadAllText("words.json");
		List<Word> words = JsonSerializer.Deserialize<List<Word>>(wordJSON);


		foreach (Word word in words)
		{
			modelBuilder.Entity<Word>().HasData(word);
		}

		string translationJSON = File.ReadAllText("translations.json");
		List<Translation> translations = JsonSerializer.Deserialize<List<Translation>>(translationJSON);


		foreach (Translation translation in translations)
		{
			modelBuilder.Entity<Translation>().HasData(translation);
		}


		base.OnModelCreating(modelBuilder);
	}
}