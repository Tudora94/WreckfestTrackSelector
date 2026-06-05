using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using WreckfestTrackSelector.Dal;
using WreckfestTrackSelector.Models;

namespace WreckfestTrackSelector.Pages.Admin
{
    public class GenerateModel : PageModel
    {
        private readonly TracksSql _tracksSql;
        private readonly ILogger<GenerateModel> _logger;
        private readonly IWebHostEnvironment _env;

        public string? GeneratedText { get; set; }

        public GenerateModel(TracksSql tracksSql, ILogger<GenerateModel> logger, IWebHostEnvironment env)
        {
            _tracksSql = tracksSql;
            _logger = logger;
            _env = env;
        }

        public async Task OnPostGenerateListAsync()
        {
            try
            {
                var config = new ServerBaseConfig();
                var headerText = config.ToConfigText();

                var tracks = await _tracksSql.GetFinalSelectedTracksAsync();

                var sb = new StringBuilder();
                var configEventList = new StringBuilder();

                int id = 1;

                foreach (var t in tracks)
                {
                    sb.AppendLine($"race {id}");
                    sb.AppendLine($"track={t.map_name}");
                    sb.AppendLine();

                    configEventList.AppendLine($"#Race {id}");
                    configEventList.AppendLine($"#{t.map_name} {t.map_variant}");
                    configEventList.AppendLine($"el_add={t.server_name}");
                    configEventList.AppendLine($"el_gamemode=racing");


                    if (t.server_name == "rt01_1")
                    {
                        configEventList.AppendLine($"el_laps=1");
                        configEventList.AppendLine($"el_car_reset_delay=3");
                    }
                    configEventList.AppendLine();
                    id++;
                }


                // Convert to string
                var text = headerText + Environment.NewLine + configEventList.ToString();

                var folderPath = Path.Combine(_env.WebRootPath, "eventlist");

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                // Build the output path
                var filePath = Path.Combine(folderPath, "server_config_gen.cfg");

                // Write the file
                await System.IO.File.WriteAllTextAsync(filePath, text);

                GeneratedText = sb.ToString();

                _logger.LogInformation("Admin generated eventlist with {Count} tracks", tracks.Count());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating eventlist");
                GeneratedText = "ERROR: Could not generate eventlist. Check logs.";
            }
        }

        public async Task OnPostCreatePlaceholderAsync()
        {
            try
            {
                var config = new ServerBaseConfig();
                var headerText = config.ToConfigText();

                await _tracksSql.CreatePlaceHolderTracks();

                var tracks = await _tracksSql.GetPlaceHolderTracksAsync();

                var sb = new StringBuilder();
                var configEventList = new StringBuilder();

                int id = 1;

                foreach (var t in tracks)
                {
                    sb.AppendLine($"race {id}");
                    sb.AppendLine($"track={t.map_name}");
                    sb.AppendLine();

                    configEventList.AppendLine($"#Race {id}");
                    configEventList.AppendLine($"#{t.map_name} {t.map_variant}");
                    configEventList.AppendLine($"el_add={t.server_name}");
                    configEventList.AppendLine($"el_gamemode=racing");


                    if (t.server_name == "rt01_1")
                    {
                        configEventList.AppendLine($"el_laps=1");
                        configEventList.AppendLine($"el_car_reset_delay=3");
                    }
                    configEventList.AppendLine();
                    id++;
                }


                // Convert to string
                var text = headerText + Environment.NewLine + configEventList.ToString();

                var folderPath = Path.Combine(_env.WebRootPath, "EventList");

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                // Build the output path
                var filePath = Path.Combine(folderPath, "server_config_gen_2.cfg");

                // Write the file
                await System.IO.File.WriteAllTextAsync(filePath, text);

                GeneratedText = sb.ToString();

                _logger.LogInformation("Admin generated eventlist with {Count} tracks", tracks.Count());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating eventlist");
                GeneratedText = "ERROR: Could not generate eventlist. Check logs.";
            }
        }
        // private void (StringBuilder uiStringBuilder, StringBuilder fileBuilder)

    }
}