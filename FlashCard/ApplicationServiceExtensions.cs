using FlashCard.Mediator.Words;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FlashCard.Extentions;
public static class ApplicationServiceExtensions
{
	/// <summary>
	/// Extension method for program.cs file
	/// Add Services
	/// </summary>
	/// <param name="services"></param>
	/// <param name="config"></param>
	/// <returns></returns>
	public static IServiceCollection AddApplicationServices(this IServiceCollection services,
		IConfiguration config)
	{
		services.AddEndpointsApiExplorer();
		services.AddSwaggerGen();

		services.AddCors(opt =>
		{
			opt.AddPolicy("CorsPolicy", policy =>
			{
				policy.AllowAnyMethod().AllowAnyHeader().WithOrigins("http://localhost:3000");
			});
		});
		services.AddMediatR(typeof(GetWordsBy.Handler));
		services.AddAutoMapper(typeof(MappingProfiles).Assembly);

		return services;
	}
}