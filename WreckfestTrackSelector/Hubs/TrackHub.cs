using Microsoft.AspNetCore.SignalR;

namespace WreckfestTrackSelector.Hubs
{
    public class TrackHub : Hub
    {
        // Called when a track is selected
        public async Task BroadcastTrackSelected(string trackId)
        {
            await Clients.All.SendAsync("TrackSelected", trackId);
        }

        public async Task BroadcastTrackDeselected(string browserId)
        {
            await Clients.All.SendAsync("TrackDeselected", browserId);
        }
    }
}
