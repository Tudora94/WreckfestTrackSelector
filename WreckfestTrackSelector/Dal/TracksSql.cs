using System;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.SqlTypes;
using WreckfestTrackSelector.Models;

namespace WreckfestTrackSelector.Dal
{
    public class TracksSql
    {
        private string datasource = Environment.MachineName;
        private string database = "WreckFestTrackSelector";
        private string username = @"DnDLogin";
        private string password = @"Natural1";
        private SqlConnection conn = new SqlConnection();
        private string _connectionString = "";
        private readonly ILogger<TracksSql> _logger;

        public TracksSql(ILogger<TracksSql> logger)
        {
            this._connectionString = @"Data Source=" + datasource +
    ";Initial Catalog=" + database +
    ";Persist Security Info=True;" +
    "User ID=" + username +
    ";Password=" + password +
    ";TrustServerCertificate=True;";
            _logger = logger;
        }
        public async Task<IEnumerable<Track>> GetTrackListAsync()
        {
            var tracks = new List<Track>();

            string sql = @"
SELECT 
    T.id,
    T.map_name,
    T.thumbnail_location,
    T.map_variant,
    T.track_surface,
    T.track_length,
    T.server_name,
    CAST(CASE WHEN ST.Track_id IS NOT NULL THEN 1 ELSE 0 END AS BIT) AS is_selected,
    ST.Selected_by_browser_id
FROM Tracks AS T
LEFT JOIN SelectedTracks AS ST
    ON T.id = ST.Track_id
    AND ST.bantha_session = @session;
";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@session", 18);

            await conn.OpenAsync();

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                tracks.Add(new Track(
                    reader.GetGuid(0),
                    reader.GetString(2),
                    reader.GetString(1),
                    reader.GetString(3),
                    reader.GetString(4),
                    reader.GetInt32(5),
                    reader.GetBoolean(7),
                    reader.GetString(6),
                    reader.IsDBNull(8) ? null : reader.GetString(8)
                ));
            }

            return tracks;
        }

        public async Task<bool> SelectTrackAsync(string id, string ip, string browser_id, string session_id)
        {
            try
            {
                // First: check if user already selected
                if (await HasUserSelectedAsync(browser_id, session_id))
                    return false; // reject

                string sql = @"
        INSERT INTO SelectedTracks
        (Track_id, selected_by_ip, selected_by_browser_id, selected_at, bantha_session)
        VALUES (@id, @ip, @browser_id, GETDATE(), @session_id);
    ";

                using var conn = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@ip", ip ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@browser_id", browser_id ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@session_id", session_id);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                _logger.LogInformation("Track {TrackId} selected by {BrowserId} from {IP}", id, browser_id, ip);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error selecting track {TrackId} for browser {BrowserId}", id, browser_id);
                return false;
            }
        }


        public async Task<bool> HasUserSelectedAsync(string browserId, string sessionId)
        {
            string sql = @"
        SELECT COUNT(*) 
        FROM SelectedTracks
        WHERE selected_by_browser_id = @browserId
        AND bantha_session = @sessionId;
        ";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@browserId", browserId);
            cmd.Parameters.AddWithValue("@sessionId", sessionId);

            await conn.OpenAsync();
            int count = (int)await cmd.ExecuteScalarAsync();

            return count > 0;
        }

        public async Task<bool> DeselectTrackAsync(string browserId, string sessionId)
        {
            string sql = @"
        DELETE FROM SelectedTracks
        WHERE selected_by_browser_id = @browserId
        AND bantha_session = @sessionId;
    ";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@browserId", browserId);
            cmd.Parameters.AddWithValue("@sessionId", sessionId);

            await conn.OpenAsync();
            int rows = await cmd.ExecuteNonQueryAsync();

            return rows > 0;
        }

        public async Task<IEnumerable<Track>> GetFinalSelectedTracksAsync()
        {
            var list = new List<Track>();

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(@"
        SELECT T.*,
        ST.selected_by_browser_id
        FROM SelectedTracks ST
        JOIN Tracks T ON T.Id = ST.Track_id
        WHERE ST.bantha_session = 18
        ORDER BY ST.selected_at ASC", conn);

            //TODO fix the above to take sessionID

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add(MapTrack(reader));
            }

            return list;
        }

        private Track MapTrack(SqlDataReader reader)
        {
            return new Track(
                reader.GetGuid(reader.GetOrdinal("id")),
                reader.GetString(reader.GetOrdinal("thumbnail_location")),
                reader.GetString(reader.GetOrdinal("map_name")),
                reader.GetString(reader.GetOrdinal("map_variant")),
                reader.GetString(reader.GetOrdinal("track_surface")),
                reader.GetInt32(reader.GetOrdinal("track_length")),
                true,
                reader.GetString(reader.GetOrdinal("server_name")),
                reader.IsDBNull(reader.GetOrdinal("selected_by_browser_id"))
                    ? null
                    : reader.GetString(reader.GetOrdinal("selected_by_browser_id"))
            );
        }

        public async Task<bool> CreatePlaceHolderTracks()
        {
            string sql = @"  DELETE FROM SelectedTracks WHERE bantha_session = -1
        INSERT INTO SelectedTracks
        VALUES('26845B65-A03F-4C58-B557-D338C765B869','1','1',GETDATE(),-1),
        ('BFFD6B74-C679-4540-9B45-F576BF0E5F43','2','2',GETDATE(),-1),
        ('76FFA9C6-2174-47C8-A824-633AB322744D','3','3',GETDATE(),-1),
        ('870E553A-C936-4899-AFAC-1826CA547E3E','4','4',GETDATE(),-1),
        ('19511C83-FE91-4EE9-890C-2DE7A7F1DCEF','5','5',GETDATE(),-1),
        ('CF2F6D51-7CFB-4098-8A00-B86831F7A879','6','6',GETDATE(),-1)
            ";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);

            await conn.OpenAsync();
            int rows = await cmd.ExecuteNonQueryAsync();

            return rows > 0;
        }
            public async Task<IEnumerable<Track>> GetPlaceHolderTracksAsync()
        {
            var list = new List<Track>();

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(@"
        SELECT T.*,
        ST.selected_by_browser_id
        FROM SelectedTracks ST
        JOIN Tracks T ON T.Id = ST.Track_id
        WHERE ST.bantha_session = -1
        ORDER BY ST.selected_at ASC", conn);

            //TODO fix the above to take sessionID

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add(MapTrack(reader));
            }

            return list;
        }
    }
}