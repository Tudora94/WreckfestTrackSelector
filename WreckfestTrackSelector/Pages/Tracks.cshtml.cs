using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using WreckfestTrackSelector.Dal;
using WreckfestTrackSelector.Hubs;
using WreckfestTrackSelector.Models;

namespace WreckfestTrackSelector.Pages
{
    public class TracksModel : PageModel
    {
        private readonly TracksSql _tracksSql;
        private readonly IHubContext<TrackHub> _hub;
        private readonly ILogger<TracksModel> _logger;

        public TracksModel(TracksSql tracksSql, IHubContext<TrackHub> hub, ILogger<TracksModel> logger)
        {
            _tracksSql = tracksSql;
            _hub = hub;
            _logger = logger;
        }

        public IEnumerable<Track> Tracks { get; set; }

        public async Task OnGetAsync()
        {
            Tracks = await _tracksSql.GetTrackListAsync();
        }

        public async Task<IActionResult> OnPostSelectAsync(Guid trackId)
        {
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
            var browserId = Request.Cookies["browserId"];
            var sessionId = "18"; // or dynamic later

            bool success = await _tracksSql.SelectTrackAsync(
                trackId.ToString(),
                ip,
                browserId,
                sessionId
            );

            if (success)
            {
                // Broadcast to all clients
                await _hub.Clients.All.SendAsync("TrackSelected", trackId.ToString());
            }
            else
            {
                TempData["Error"] = "You have already selected a track this session.";
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeselectAsync()
        {
            var browserId = Request.Cookies["browserId"];
            var sessionId = "18";

            bool success = await _tracksSql.DeselectTrackAsync(browserId, sessionId);

            if (success)
            {
                await _hub.Clients.All.SendAsync("TrackDeselected", browserId);
            }

            return RedirectToPage();
        }

    }
}
