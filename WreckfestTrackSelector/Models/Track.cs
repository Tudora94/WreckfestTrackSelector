using System.Runtime.CompilerServices;

namespace WreckfestTrackSelector.Models
{
    public class Track
    {
        // Backend-only, not shown to users
        public Guid Id { get; set; }
        public string thumbnail_location { get; set; }
        public string map_name { get; set; }
        public string map_variant { get; set; }
        public string track_surface { get; set; }
        public int track_length { get; set; }
        public bool is_selected { get; set; }

        // Backend-only, not shown to users
        public string server_name { get; set; }
        public string? selected_by_browser_id { get; set; }

        public Track(Guid _id, string _location, string _map_name, string _map_variant, string _track_surface, int _track_length, bool _is_selected, string _server_name, string? _selected_by_id)
        {
            this.Id = _id;
            this.thumbnail_location = _location;
            this.map_name = _map_name;
            this.map_variant = _map_variant;
            this.track_surface = _track_surface;
            this.track_length = _track_length;
            this.is_selected = _is_selected;
            this.server_name = _server_name;
            this.selected_by_browser_id = _selected_by_id;
        }
    }
}