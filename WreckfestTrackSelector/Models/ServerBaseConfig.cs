using System.Text;

namespace WreckfestTrackSelector.Models
{
    public class ServerBaseConfig
    {
        public string ServerName { get; set; } = "Bantha_Invitational";
        public string WelcomeMessage { get; set; } = "Test";
        public string Password { get; set; } = "Shoes";
        public int MaxPlayers { get; set; } = 24;
        public int SteamPort { get; set; } = 27015;
        public int GamePort { get; set; } = 33540;
        public int QueryPort { get; set; } = 27016;
        public int ExcludeFromQuickplay { get; set; } = 0;
        public int ClearUsers { get; set; } = 0;
        public int OwnerDisabled { get; set; } = 0;
        public int AdminControl { get; set; } = 1;
        public int LobbyCountdown { get; set; } = 30;
        public int ReadyPlayersRequired { get; set; } = 50;
        public string SessionMode { get; set; } = "24p-lin";
        public string GridOrder { get; set; } = "cup_reverse";
        public int EnableTrackVote { get; set; } = 0;
        public int DisableIdleKick { get; set; } = 0;
        public string Track { get; set; } = "rt01_1";
        public string Gamemode { get; set; } = "racing";
        public int Bots { get; set; } = 24;
        public string AiDifficulty { get; set; } = "expert";
        public int NumTeams { get; set; } = 2;
        public int Laps { get; set; } = 5;
        public int TimeLimit { get; set; } = 5;
        public int EliminationInterval { get; set; } = 0;
        public string VehicleDamage { get; set; } = "realistic";
        public string CarClassRestriction { get; set; } = "";
        public string CarRestriction { get; set; } = "";
        public int SpecialVehiclesDisabled { get; set; } = 0;
        public int CarResetDisabled { get; set; } = 0;
        public int CarResetDelay { get; set; } = 5;
        public int WrongWayLimiterDisabled { get; set; } = 0;
        public string Weather { get; set; } = "";
        public string Frequency { get; set; } = "high";
        public string Log { get; set; } = "log.txt";

        public string ToConfigText()
        {
            var sb = new StringBuilder();

            sb.AppendLine($"server_name={ServerName}");
            sb.AppendLine($"welcome_message={WelcomeMessage}");
            sb.AppendLine($"password={Password}");
            sb.AppendLine($"max_players={MaxPlayers}");
            sb.AppendLine($"steam_port={SteamPort}");
            sb.AppendLine($"game_port={GamePort}");
            sb.AppendLine($"query_port={QueryPort}");
            sb.AppendLine($"exclude_from_quickplay={ExcludeFromQuickplay}");
            sb.AppendLine($"clear_users={ClearUsers}");
            sb.AppendLine($"owner_disabled={OwnerDisabled}");
            sb.AppendLine($"admin_control={AdminControl}");
            sb.AppendLine($"lobby_countdown={LobbyCountdown}");
            sb.AppendLine($"ready_players_required={ReadyPlayersRequired}");
            sb.AppendLine($"session_mode={SessionMode}");
            sb.AppendLine($"grid_order={GridOrder}");
            sb.AppendLine($"enable_track_vote={EnableTrackVote}");
            sb.AppendLine($"disable_idle_kick={DisableIdleKick}");
            sb.AppendLine($"track={Track}");
            sb.AppendLine($"gamemode={Gamemode}");
            sb.AppendLine($"bots={Bots}");
            sb.AppendLine($"ai_difficulty={AiDifficulty}");
            sb.AppendLine($"num_teams={NumTeams}");
            sb.AppendLine($"laps={Laps}");
            sb.AppendLine($"time_limit={TimeLimit}");
            sb.AppendLine($"elimination_interval={EliminationInterval}");
            sb.AppendLine($"vehicle_damage={VehicleDamage}");
            sb.AppendLine($"car_class_restriction={CarClassRestriction}");
            sb.AppendLine($"car_restriction={CarRestriction}");
            sb.AppendLine($"special_vehicles_disabled={SpecialVehiclesDisabled}");
            sb.AppendLine($"car_reset_disabled={CarResetDisabled}");
            sb.AppendLine($"car_reset_delay={CarResetDelay}");
            sb.AppendLine($"wrong_way_limiter_disabled={WrongWayLimiterDisabled}");
            sb.AppendLine($"weather={Weather}");
            sb.AppendLine($"frequency={Frequency}");
            sb.AppendLine($"log={Log}");

            return sb.ToString();
        }
    }
}
